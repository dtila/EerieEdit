using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using EerieEdit.ERObjects;

namespace EerieEdit
{
	class Table
	{
		const int itemsSpacing = 18;
		public static TablesViewer ownerTables = null;
		public static StringFormat stringFormatHeader = null;

		/// <summary>
		/// The ER object that has been created this table
		/// </summary>
		ERObject ownerObject;
		public Rectangle Rectangle;

        internal Dictionary<ERObject, Table> primaryKeysLinkedTables = new Dictionary<ERObject, Table>();
        List<ERObject> items = new List<ERObject>();

		public bool HasPrimaryKey
		{
			get
			{
				return primaryKeysLinkedTables.Count > 0;
			}
		}

		public bool AllAttributesValide
		{
			get
			{
				foreach (var primary in primaryKeysLinkedTables)
				{
					var attrbute = primary.Key as ERObjects.Attribute;
					if (attrbute != null && string.IsNullOrEmpty(attrbute.AttributeValueType))
						return false;
				}
				foreach (var item in items)
				{
					var attribute = item as ERObjects.Attribute;
					if (attribute != null && string.IsNullOrEmpty(attribute.AttributeValueType))
						return false;
				}
				return true;
			}
		}

		public string Name
		{
			get { return ownerObject.Name; }
			set { ownerObject.Name = value; }
		}

		public Table(ERObject ownerObject)
		{
			this.ownerObject = ownerObject;
            ComputeWidth(ownerObject);
            Rectangle.Width = 100;
		}

		public void AddPosition(int x, int y)
		{
			Rectangle.X += x;
			Rectangle.Y += y;
		}

		/// <summary>
		/// Delete link to a table in both directions
		/// </summary>
		/// <param name="item"></param>
		public void DeleteLinkedTable(Table item)
		{
			primaryKeysLinkedTables.Remove(item.ownerObject);
			item.primaryKeysLinkedTables.Remove(ownerObject);
		}
		/// <summary>
		/// Verifica daca mousele este deasupra instantei de tabel. Este folosit pentru schmbarea cursorului la Move sau la Normal
		/// </summary>
		/// <param name="p">Pozitia mousele relativ la Control</param>
		/// <returns>True daca mousele e in zona de header, altfel false</returns>
		public bool MouseOnHeader(Point p)
		{
			return p.X >= Rectangle.Left && p.X <= Rectangle.Right && p.Y >= Rectangle.Top && p.Y <= Rectangle.Top + itemsSpacing;
		}

		/// <summary>
		/// Add a primary key linked with parameter linkedTable. The function add linked objects in both directions
		/// </summary>
		/// <param name="ownerObject"></param>
		/// <param name="linkedTable"></param>
		public void AddPrimaryItem(ERObject ownerObject, Table linkedTable)
		{
			if (!primaryKeysLinkedTables.ContainsKey(ownerObject))
				primaryKeysLinkedTables.Add(ownerObject, linkedTable);

			if (linkedTable != null)
				if (linkedTable.primaryKeysLinkedTables.ContainsKey(this.ownerObject))
					linkedTable.primaryKeysLinkedTables[this.ownerObject] = this;
				else
					linkedTable.primaryKeysLinkedTables.Add(this.ownerObject, this);

			ComputeWidth(ownerObject);
		}

        public void SetPrimaryItem(ERObject erObject, Table table)
        {
            primaryKeysLinkedTables[erObject] = table;
            ComputeWidth(erObject);
        }

		public void AddItem(ERObject item)
		{
			items.Add(item);
			ComputeWidth(item);
		}

        private void ComputeWidth(ERObject item)
        {
			Rectangle.Height = Math.Max(itemsSpacing, (primaryKeysLinkedTables.Count + items.Count + 1) * itemsSpacing);
            using (var g = ownerTables.CreateGraphics())
            {
                using (var boldFont = new Font(ownerTables.Font, FontStyle.Bold))
                    Rectangle.Width = Math.Max(Rectangle.Width, (int)g.MeasureString(item.ToString(), boldFont).Width + itemsSpacing);
            }
        }

		/// <summary>
		/// Visist recursively linked tables, and store them into visitedTables list
		/// </summary>
		/// <param name="visitedTables"></param>
		/// <returns>True if the table must be drawed</returns>
		public static void DrawLinksRecursively(Graphics g, Pen p, Table table, List<Table> visitedTables)
		{
			if (visitedTables.Contains(table))
				return;

			foreach (var t in table.primaryKeysLinkedTables)
			{
				visitedTables.Add(table);
				DrawLinksRecursively(g, p, t.Value, visitedTables);
				DrawLink(g, p, table, t.Value);
			}
		}

		static void DrawLink(Graphics g, Pen pen, Table firstTable, Table secondTable)
		{
			int max = int.MaxValue;
			Point p1 = Point.Empty, p2 = Point.Empty;
			if (firstTable == secondTable)
				return;

			for (int i = 0; i < firstTable.primaryKeysLinkedTables.Count; i++)
			{
				if (!firstTable.IsLinkedWithAtPosition(secondTable, i))
					continue;

				foreach (var fp in firstTable.GetRectangleForPrimaryKey(i).GetHorizontalLinkPoints())
					for (int j = 0; j < secondTable.primaryKeysLinkedTables.Count; j++)
					{
						foreach (var sp in secondTable.GetRectangleForPrimaryKey(j).GetHorizontalLinkPoints())
						{
							var d = fp.SquaredDistance(sp);
							if (d < max)
							{
								p1 = fp;
								p2 = sp;
								max = d;
							}
						}
					}
			}
			g.DrawLine(pen, p1, p2);
		}

		public void Paint(Graphics g)
		{
			using (var brush = new LinearGradientBrush(Rectangle, ProfessionalColors.ButtonPressedHighlight, Color.WhiteSmoke, LinearGradientMode.Horizontal))
				g.FillRectangle(brush, Rectangle);
			using (var pen = new Pen(Color.Black))
			{
				g.DrawRectangle(pen, Rectangle);
				g.FillRectangle(Brushes.RoyalBlue, Rectangle.Left, Rectangle.Top, Rectangle.Width + 1, itemsSpacing);
				using (var font = new Font(ownerTables.Font, FontStyle.Bold))
					g.DrawString(Name, font, Brushes.Black, Rectangle, stringFormatHeader);

				// draw primary keys
				int lines = 0;
				using (var font = new Font(ownerTables.Font.FontFamily, ownerTables.Font.Size, FontStyle.Bold))
					foreach (var item in primaryKeysLinkedTables)
					{
						g.DrawLine(pen, Rectangle.Left, Rectangle.Top + (lines + 1) * itemsSpacing, Rectangle.Right, Rectangle.Top + (lines + 1) * itemsSpacing);
						g.DrawString(item.Key.ToString(), font, Brushes.Black, Rectangle.FromLTRB(Rectangle.Left + 10, Rectangle.Top + (lines + 1) * itemsSpacing + 2, Rectangle.Right, Rectangle.Bottom));
						lines++;
					}

				//draw other keys
				for (int i = 0; i < items.Count; i++, lines++)
				{
					g.DrawLine(pen, Rectangle.Left, Rectangle.Top + (lines + 1) * itemsSpacing, Rectangle.Right, Rectangle.Top + (lines + 1) * itemsSpacing);
					g.DrawString(items[i].ToString(), ownerTables.Font, Brushes.Black, Rectangle.FromLTRB(Rectangle.Left + 10, Rectangle.Top + (lines + 1) * itemsSpacing + 2, Rectangle.Right, Rectangle.Bottom));
				}
			}
		}

		/// Static methods
		public static Table FromEntity(Entity e)
		{
			var table = new Table(e);
			foreach (var li in e.adjacentLinks)
			{
				var otherEndObject = li.GetOtherEnd(e);
				if (otherEndObject is EerieEdit.ERObjects.Attribute)
					table.AddItem((otherEndObject as EerieEdit.ERObjects.Attribute));
			}
			return table;
		}

		public Rectangle GetRectangleForPrimaryKey(int primaryKeyPosition)
		{
			if (primaryKeyPosition >= this.primaryKeysLinkedTables.Count)
				throw new ArgumentOutOfRangeException("primaryKeyCount");
			return new Rectangle(Rectangle.X, Rectangle.Y + (itemsSpacing * (primaryKeyPosition + 1)), Rectangle.Width, itemsSpacing);
		}

		public bool IsLinkedWith(Table table)
		{
			return this.primaryKeysLinkedTables.ContainsValue(table);
		}

		public bool IsLinkedWithAtPosition(Table table, int linkedPosition)
		{
			var i = 0;
			foreach (var item in primaryKeysLinkedTables)
			{
				if (item.Value == table && i == linkedPosition)
					return true;
				i++;
			}
			return false;
		}

        #region SQL Code generation
        StringBuilder GetSqlServerForAttribute(ERObjects.Attribute attribute)
        {
            var sb = new StringBuilder("\t ");
            if (attribute.Name.Contains(" "))
                sb.AppendFormat("[{0}]", attribute.Name);
            else
                sb.Append(attribute.Name);

            sb.Append(' ' + attribute.AttributeValueType.ToString().ToLower());
            if (!(ownerObject is Entity))
                sb.Append(attribute.CanBeNull ? " null" : " not null");

            if (ownerObject is Relationship)
            {
                if (primaryKeysLinkedTables.ContainsKey(attribute))
                {
                    var objName = primaryKeysLinkedTables[attribute].ownerObject.Name;
                    sb.Append(" foreign key references ");
                    if (objName.Contains(" "))
                        sb.AppendFormat("[{0}]", objName);
                    else
                        sb.Append(objName);

                    sb.Append('(');
                    if (attribute.Name.Contains(" "))
                        sb.AppendFormat("[{0}]", attribute.Name);
                    else
                        sb.Append(attribute.Name);
                    sb.Append(')');
                }
            }

            if (ownerObject is Entity)
            {
                if (primaryKeysLinkedTables.ContainsKey(attribute))
                {
                    if (attribute.PrimaryKey)
                    {
						sb.Append(" primary key");
                        if (attribute.AutoIncrement)
                            sb.Append(" identity");
                    }
                    else
                        sb.Append(attribute.CanBeNull ? " null" : " not null");
                }
            }
            sb.Append(',');
            return sb;
        }

        StringBuilder GetMySQLForAttribute(ERObjects.Attribute attribute)
        {
            var sb = new StringBuilder("\t ");
            if(attribute.Name.Contains(" "))
                sb.AppendFormat("[{0}]", attribute.Name);
            else
                sb.Append(attribute.Name);

            sb.Append(' ' + attribute.AttributeValueType.ToString().ToLower());
            if (!(ownerObject is Entity))
                sb.Append(attribute.CanBeNull ? " null" : " not null");


            if (ownerObject is Relationship)
            {
                if (primaryKeysLinkedTables.ContainsKey(attribute))
                {
                    var objName = primaryKeysLinkedTables[attribute].ownerObject.Name;
                    sb.Append(" foreign key references ");
                    if (objName.Contains(" "))
                        sb.AppendFormat("[{0}]", objName);
                    else
                        sb.Append(objName);

                    sb.Append('(');
                    if (attribute.Name.Contains(" "))
                        sb.AppendFormat("[{0}]", attribute.Name);
                    else
                        sb.Append(attribute.Name);
                    sb.Append(')');
                }
            }

            if (ownerObject is Entity)
            {
                if (primaryKeysLinkedTables.ContainsKey(attribute))
                {
                    if (attribute.PrimaryKey)
                    {
                        if (attribute.AutoIncrement)
                            sb.Append(" auto_increment");
                        sb.Append(" primary key");
                    }
                    else
                        sb.Append(attribute.CanBeNull ? " null" : " not null");
                }
            }
            sb.Append(',');
            return sb;
        }

        string GetSqlForAttrinute(ERObjects.Attribute attribute)
        {
            return GetSqlServerForAttribute(attribute).ToString();        
        }

        public StringBuilder GetSQL()
        {
            var sb = new StringBuilder("create table ");
            if(Name.Contains(" "))
                sb.AppendFormat("[{0}]", Name);
            else
                sb.Append(Name);

            sb.AppendLine(Environment.NewLine + "(");
            foreach (var item in primaryKeysLinkedTables)
                sb.AppendLine(GetSqlForAttrinute(item.Key as ERObjects.Attribute));

            foreach (var item in items)
                sb.AppendLine(GetSqlForAttrinute(item as ERObjects.Attribute));

            sb.Remove(sb.Length - Environment.NewLine.Length - 1 , 3);            
            sb.AppendLine(Environment.NewLine + ")");
            return sb;
        }
        #endregion
    }
}
