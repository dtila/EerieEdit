using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.ComponentModel;

namespace EerieEdit.ERObjects
{
	[Serializable]
	public class Relationship : ERObject
	{
		private static readonly Color Color = Color.BlueViolet;

		private bool identifying;
        [Description("Specify if the relationship is identifying")]
        [DefaultValue(false)]
		public bool Identifying
		{
			get
			{
				return identifying;
			}

			set
			{
				identifying = value;
				//ComputeBounds();
			}
		}

		/// <summary>
		/// Atributele instanta Relatiei, poate sa fie Entitati sau Atribute
		/// </summary>
		//public List<ERObject> Neighbours = new List<ERObject>();
		public Relationship(DiagramEditor parent, string name, bool identifying)
			: base(parent, name)
		{
			Identifying = identifying;
		}

		public override void AddLinkedObject(ERObject o)
		{
			//  Neighbours.Add(o);
		}

		protected override Size GetPadding()
		{
			var s = new Size(20, 70);
			//if (Identifying)
			//    s += new Size(6, 12);
			return s;
		}

		public override void Paint(Graphics g)
		{
			base.Paint(g);

			if (!Visible)
				return;

			var lines = BoundingRectangle.GetLinkPoints();
			using (var brush = new SolidBrush(Color.FromArgb(ERObject.fillTransparency, Color)))
				g.FillPolygon(brush, lines);

			using (var pen = new Pen(Color, ERObject.lineThinkness))
			{
				g.DrawPolygon(pen, lines);

				if (Identifying)
					g.DrawPolygon(pen, Rectangle.Inflate(Bounds, -3, -6).GetLinkPoints());
			}

			using (var blackBrush = new SolidBrush(EditorControl.ForeColor))
				g.DrawString(Name, EditorControl.Font, blackBrush, Bounds, MainForm.stringFormat);
		}

		public override void DrawBorderShape(Graphics g)
		{
			if (!Visible)
				return;

			using (var pen = new Pen(Color.Black))
				g.DrawPolygon(pen, Bounds.GetLinkPoints());
		}
	}
}
