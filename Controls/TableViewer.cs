using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Collections;
using System.ComponentModel;
using System.IO;

namespace EerieEdit
{
	class TablesViewer : Control, IEnumerable<Table>
	{
		List<Table> tables = new List<Table>();
		Table draggingTable = null;
		Point dragStartLocation;

		public TablesViewer()
		{
            Pen = Pens.Black;
			SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.Opaque | ControlStyles.OptimizedDoubleBuffer, true);
			Table.ownerTables = this;

			Table.stringFormatHeader = new StringFormat();
			Table.stringFormatHeader.Alignment = StringAlignment.Center;
			Table.stringFormatHeader.LineAlignment = StringAlignment.Near;
		}

		public void AddTable(Table item)
		{
			tables.Add(item);
			item.Rectangle.Width = 100;
			item.Rectangle.Height = 50;
			SetRandomLocation(item);
			Invalidate();
		}
        
		private void SetRandomLocation(Table item)
		{
			const int padding = 25;

			var rect = new Rectangle(padding, padding, Size.Width - padding, Size.Height - padding);
			int tries = 0;
			while (tries++ <= 20)
			{
				bool intersects = false;
				/// Calculeaza o noua pozitie random
				var newPoint = new Point(Program.randomInstance.Next(rect.Left, rect.Right), Program.randomInstance.Next(rect.Top, rect.Bottom));
				var newRect = new Rectangle(newPoint, item.Rectangle.Size);

				foreach (var table in tables)
					if (table.Rectangle.IntersectsWith(newRect))
					{
						intersects = true;
						break;
					}
				if (!rect.Contains(newRect)) continue;
				if (!intersects)
				{
					item.Rectangle = newRect;
					return;
				}
			}
		}

		private void SetGridLocation(Table table)
		{
			var rect = Rectangle.Inflate(this.Bounds, -10, -15);
			const int margin = 15;

			//			int lines = 0, columns = 0;
			int tablesWidth = (tables.Count + 1) * margin;
			tables.ForEach(li => tablesWidth += li.Rectangle.Width);
			if (tablesWidth <= rect.Width)///incap tabelele
			{

			}
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);

			var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.PixelOffsetMode = PixelOffsetMode.HighQuality;

			g.Clear(BackColor);
			tables.ForEach(li => li.Paint(g));
			
            // Draw linked tables
			var visitedTables = new List<Table>();
			using (var pen = new Pen(Color.Black, 2.0f))
				foreach (var item in tables)
					Table.DrawLinksRecursively(g, pen, item, visitedTables);
		}

		/// <summary>
		/// Save the current image to a png file format
		/// </summary>
		/// <param name="stream"></param>
		public void Save(Stream stream)
		{
			using (var bmp = new Bitmap(ClientSize.Width, ClientSize.Height))
			{
				DrawToBitmap(bmp, new Rectangle(Point.Empty, Size));
				bmp.MakeTransparent(BackColor);
				bmp.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
			}
		}

		protected override void OnMouseDown(MouseEventArgs e)
		{
			base.OnMouseDown(e);
			if ((e.Button & MouseButtons.Left) == MouseButtons.Left)
			{
				var obj = GetUnderlyingTable(e.Location);
				if (obj != null && obj.MouseOnHeader(e.Location))
				{
					draggingTable = obj;
					var ps = PointToScreen(new Point(e.X - draggingTable.Rectangle.Left + 1, e.Y - draggingTable.Rectangle.Top + 1));
					Cursor.Clip = new Rectangle(ps.X, ps.Y, Size.Width - draggingTable.Rectangle.Width - 1, Size.Height - draggingTable.Rectangle.Height - 1);
					dragStartLocation = e.Location;
				}
				else if ((e.Button & MouseButtons.Right) == MouseButtons.Right)
				{

				}
			}
		}

		protected override void OnMouseUp(MouseEventArgs e)
		{
			base.OnMouseUp(e);
			draggingTable = null;
			Cursor.Clip = Screen.PrimaryScreen.Bounds;
		}

		protected override void OnMouseMove(MouseEventArgs e)
		{
			base.OnMouseMove(e);

			var obj = GetUnderlyingTable(e.Location);
			Cursor = obj != null && obj.MouseOnHeader(e.Location) ? Cursor = Cursors.SizeAll : Cursor = Cursors.Default;

            if (draggingTable != null)
            {
				draggingTable.AddPosition(e.X - dragStartLocation.X, e.Y - dragStartLocation.Y);
				dragStartLocation = e.Location;
                Invalidate();
			}
		}

		private Table GetUnderlyingTable(Point p)
		{
			foreach (var li in tables)
				if (li.Rectangle.Contains(p))
					return li;
			return null;
		}

		public void Clear()
		{
			tables.Clear();
			Invalidate();
		}

        public StringBuilder GenerateSQL()
        {
            try
            {
                foreach (var table in tables)
                {
                    if (!table.HasPrimaryKey)
                        throw new Exception("Table '" + table.Name + "' doesn't has primary key");
                    if (!table.AllAttributesValide)
                        throw new Exception("Table '" + table.Name + "' contains at least one attribute that doesn't has a value type selected");
                }

                var sb = new StringBuilder();
                foreach (var table in tables)
                    sb.AppendLine(table.GetSQL().ToString());
                return sb;
            }
            catch (Exception e)
            {
                MessageBox.Show(this, "SQL Generation failed because:\n" + e.Message, "ER-Editor", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }
       
        public IEnumerator<Table> GetEnumerator()
        {
            foreach (var table in tables)
                yield return table;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            foreach (var table in tables)
                yield return table;
        }

        [EditorBrowsable(EditorBrowsableState.Always)]
        public Pen Pen { get; set; }
    }

    [TypeConverter(typeof(CompousPenTypeConverter))]
    public struct CompousPen
    {
        int size;
        public int Size { get { return size; } set { size = value; } }

        Color color;
        public Color Color { get { return color; } set { color = value; } }

        public CompousPen(int size, Color color)
        {
            this.color = color;
            this.size = size;
        }

        public override string ToString()
        {
            return string.Format("{1}, {2}", Size, Color);
        }
    }

    public class CompousPenTypeConverter : TypeConverter
    {
        public override bool GetPropertiesSupported(ITypeDescriptorContext context)
        {
            return true;
        }
        public override PropertyDescriptorCollection GetProperties(ITypeDescriptorContext context, object value, Attribute[] attributes)
        {
            return TypeDescriptor.GetProperties(typeof(Pen));
        }
    }
}
