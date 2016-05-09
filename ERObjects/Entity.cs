using System;
using System.Collections.Generic;
using System.Drawing;
using System.ComponentModel;

namespace EerieEdit.ERObjects
{
	public enum EntityType { Strong, Weak }

	[Serializable]
	public class Entity : ERObject
	{
		private static readonly Color Color = Color.Red;

		EntityType type;
		[Browsable(false)]
		public EntityType Type
		{
			get
			{
				return type;
			}

			set
			{
				type = value;
				Invalidate();
				//ComputeBounds();
			}
		}

		//public List<ERObject> Neighbours = new List<ERObject>();

		public Entity(DiagramEditor parent, string name)
			: base(parent, name)
		{
			Type = EntityType.Weak;
		}

		public override void AddLinkedObject(ERObject o)
		{
			//  Neighbours.Add(o);
		}

		protected override Size GetPadding()
		{
			var s = new Size(25, 12);
			return s;
		}

		public override void Paint(Graphics g)
		{
			if (!Visible)
				return;

			base.Paint(g);

			using (var brush = new SolidBrush(Color.FromArgb(ERObject.fillTransparency, Color)))
				g.FillRectangle(brush, Bounds);
			using (Pen pen = new Pen(Color, ERObject.lineThinkness))
			{
				g.DrawRectangle(pen, Bounds);
				if (Type == EntityType.Weak)
					g.DrawRectangle(pen, Rectangle.Inflate(Bounds, -3, -3));
			}
			using (var blackBrush = new SolidBrush(EditorControl.ForeColor))
				g.DrawString(Name, EditorControl.Font, blackBrush, Bounds, MainForm.stringFormat);
		}

		public override void DrawBorderShape(Graphics g)
		{
			if (Visible)
				using (var pen = new Pen(Color.Black))
					g.DrawRectangle(pen, Bounds);
		}

		public void UpdateStrength()
		{
			var strong = false;
			foreach (var att in this.Attributes)
				if (att.PrimaryKey && !att.AutoIncrement)
				{
					strong = true;
					break;
				}

			if (strong)
				Type = EntityType.Strong;
			else
				Type = EntityType.Weak;
		}
	}
}
