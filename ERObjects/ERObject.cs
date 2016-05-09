//using System;
using System.Collections.Generic;
using System.Drawing;
using EerieEdit.ERObjects;
using System;
using System.Text;
using System.ComponentModel;

namespace EerieEdit.ERObjects
{
	[Serializable]
    [DefaultProperty("Name")]
	public abstract class ERObject
	{
		public const int fillTransparency = 50;
		public const float lineThinkness = 1.8f;

		protected Rectangle Bounds;
		public List<Link> adjacentLinks;

        private bool visible;
        [Description("Specify the visibility of the")]
        [Browsable(false)]
		public bool Visible
		{
			get
			{
				return visible;
			}

			set
			{
				visible = value;
				EditorControl.Invalidate();
			}
		}
        
        [Browsable(false)]
		public IEnumerable<ERObject> Neighbours
		{
			get
			{
				foreach (var link in adjacentLinks)
					yield return link.GetOtherEnd(this);
			}
		}

        [Browsable(false)]
		public IEnumerable<Attribute> Attributes
		{
			get
			{
				return GetNeighbours<Attribute>();
			}
		}

		public bool ChildrenVisible;

		[NonSerialized]
		public DiagramEditor EditorControl;
        
        [Browsable(false)]
		public virtual ERObject ParentObject { get; set; }

		private ERObject parentObject;
		private string name;
        [Description("The name of the selected object")]
		public string Name
		{
			get
			{
				return name;
			}

			set
			{
                EditorControl.Invalidate(DrawingRectangle);
				name = value;
				ComputeBounds();
				EditorControl.Invalidate(DrawingRectangle);
			}
		}
        [Browsable(false)]
		public Rectangle BoundingRectangle
		{
			get { return Bounds; }
			set
			{
				Bounds = value;
				RecalculateLinks();
			}
		}
        
        /// <summary>
        /// Get the display rectangle, inclusive the selected rectangles
        /// </summary>
        [Browsable(false)]
        public Rectangle DrawingRectangle 
        { 
            get 
            {
                return Rectangle.Inflate(Bounds, ExtensionMethods.halfSelectedRectangle * 2 + 1, ExtensionMethods.halfSelectedRectangle * 2 + 1);
            }
        }

        /// <summary>
        /// Invalidate the ERObject with the DrawingRectabgle 
        /// </summary>
        public void Invalidate()
        {
            EditorControl.Invalidate(DrawingRectangle);
        }

		protected ERObject(DiagramEditor editorControl, string name)
		{
			visible = true;
			adjacentLinks = new List<Link>();

			EditorControl = editorControl;
			Name = name;
			Bounds.Location = new Point();

			ChildrenVisible = true;
		}

		public virtual void ComputeBounds()
		{
			using (var g = EditorControl.CreateGraphics())
				Bounds.Size = g.MeasureString(Name, EditorControl.Font).ToSize() + GetPadding();
			RecalculateLinks();
		}

		protected abstract Size GetPadding();

		public override string ToString()
		{
			return Name;
		}

		public bool Intersects(Rectangle rect)
		{
			return Bounds.IntersectsWith(rect);
		}

		public bool Intersects(Point p)
		{
			return Bounds.Contains(p);
		}

		public void SetLocation(int x, int y)
		{
			Bounds = new Rectangle(x, y, Bounds.Width, Bounds.Height);
            EditorControl.ModifyDocument();
			RecalculateLinks();
		}

        /// <summary>
        /// Adjust the bouds ot the object with specified amount
        /// </summary>
		public void Offset(int x, int y)
		{
			Bounds.Offset(x, y);
			RecalculateLinks();
		}

		public Point GetCenter()
		{
			return new Point(Bounds.Left + Bounds.Width / 2, Bounds.Top + Bounds.Height / 2);
		}

		public void AddLink(Link link)
		{
            EditorControl.ModifyDocument();
			adjacentLinks.Add(link);
		}

		private void RecalculateLinks()
		{
			foreach (var link in adjacentLinks)
				link.Recalculate();
		}

		public abstract void DrawBorderShape(Graphics g);
		public abstract void AddLinkedObject(ERObject o);

		public virtual void Paint(Graphics g)
		{
			if (!ChildrenVisible)
			{
				var centerX = Bounds.Right - ExtensionMethods.halfSelectedRectangle * 2;
				var centerY = Bounds.Top - ExtensionMethods.halfSelectedRectangle * 2;
				var size = ExtensionMethods.halfSelectedRectangle;

				g.DrawLine(Pens.Red, centerX - size, centerY, centerX + size, centerY);
				g.DrawLine(Pens.Red, centerX, centerY - size, centerX, centerY + size);
			}
		}

		public IEnumerable<T> GetNeighbours<T>() where T : ERObject
		{
			foreach (var obj in Neighbours)
				if (obj is T)
					yield return obj as T;
		}

		public void ToggleAttributeVisibility()
		{
			ChildrenVisible = !ChildrenVisible;
			if (ChildrenVisible)
				RestoreChildren();
			else
				HideChildren();
		}

        /// <summary>
        /// Retrieve primary key attributes
        /// </summary>
        /// <returns></returns>
		public IEnumerable<ERObjects.Attribute> GetPrimaryKey()
		{
			foreach (var attr in Attributes)
				if (attr.PrimaryKey)
					yield return attr;
		}

		private void HideChildren()
		{
			foreach (var link in adjacentLinks)
			{
				var obj = link.second;
				if (link.first == this && obj is Attribute)
				{
					obj.Visible = false;
					obj.HideChildren();
				}
			}
            EditorControl.ModifyDocument();
		}

		private void RestoreChildren()
		{
			foreach (var link in adjacentLinks)
			{
				var obj = link.second;
				if (link.first == this && obj is Attribute)
				{
					obj.Visible = ChildrenVisible;
					if (ChildrenVisible)
						obj.RestoreChildren();
				}
			}
            EditorControl.ModifyDocument();
		}
	}
}
