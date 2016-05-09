using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.ComponentModel;

namespace EerieEdit.ERObjects
{
	[Serializable]
	public class Attribute : ERObject
	{
		private static readonly Color Color = Color.Green;
		private static readonly Color PrimaryAttributeColor = Color.Chocolate;
		private static readonly Color PrimaryAutoIncrementColor = Color.CadetBlue;

		/// <summary>
		/// Lista atributelor, poate sa fie alte Atribute sau Relatie
		/// </summary>
		//private List<ERObject> attributes = new List<ERObject>();

		private AttributeType type;
        [DefaultValue(typeof(AttributeType), "Simple")]
        [Description("Specify the type of selected attribute.")]
		public AttributeType Type
		{
			get
			{
				return type;
			}
			set
			{
				type = value;
				//ComputeBounds();
			}
		}

        private string valueType = string.Empty;
        
        [DisplayName("ValueType")]
        [Description("Value of the attribute\nThis value will be used on SQL code generation")]
        public string AttributeValueType
        {
            get
            {
                return valueType;
            }
            set
            {
                valueType = value;
                //ComputeBounds();
            }
        }

        bool autoIncrement;
        [Description("Specify if the attrinute is auto increment, primary key.")]
        [DefaultValue(false)]
        public bool AutoIncrement 
        {
            get { return autoIncrement; }
            set
            {
                if (value && !PrimaryKey)
                    PrimaryKey = true;
                autoIncrement = value;
				var ent = ParentObject as Entity;
				if (ent != null)
					ent.UpdateStrength();
            }
        }
		
        [Browsable(false)]
        public bool IsCompound
		{
			get
			{
				return adjacentLinks.Count > 1;
			}
		}

        [Description("Specify if the value of attribute can be null")]
        [DefaultValue(false)]
        public bool CanBeNull { get; set; }
        
        [Description("Specify if the attribute is the primary key of the entity.")]
        [DefaultValue(false)]
        public bool PrimaryKey
		{
			get
			{
				return primaryKey;
			}

			set
			{
				primaryKey = value;
				var ent = ParentObject as Entity;
				if (ent != null)
					ent.UpdateStrength();
			}
		}
		private bool primaryKey;

		public override ERObject ParentObject
		{
			get
			{
				return base.ParentObject;
			}
			set
			{
				var oldParent = ParentObject;
				base.ParentObject = value;
				var ent = oldParent as Entity;
				if (ent != null)
					ent.UpdateStrength();
				ent = ParentObject as Entity;
				if (ent != null)
					ent.UpdateStrength();
			}
		}
		public Attribute(DiagramEditor parent, string name, bool primary, bool canBeNull, bool autoIncrement, AttributeType type, string valueType)
			: base(parent, name)
		{
			PrimaryKey = primary;
			Type = type;
            AttributeValueType = string.Empty;
            CanBeNull = canBeNull;
            AutoIncrement = autoIncrement;
            this.valueType = valueType;
		}

		public override void AddLinkedObject(ERObject o)
		{
			//  attributes.Add(o);
		}

		protected override Size GetPadding()
		{
			var s = new Size(25, 12);
			//if (Type == AttributeType.Multivalued)
			//    s += new Size(6, 6);
			return s;
		}

		public override void Paint(Graphics g)
		{
			if (!Visible)
				return;

			base.Paint(g);

			Color color = PrimaryKey ? AutoIncrement ? PrimaryAutoIncrementColor : PrimaryAttributeColor : Color;

			using (var brush = new SolidBrush(Color.FromArgb(ERObject.fillTransparency, color)))
				g.FillEllipse(brush, Bounds);

			using (Pen pen = new Pen(color, ERObject.lineThinkness))
			{
				if (Type == AttributeType.Derived)
					pen.DashStyle = DashStyle.Dash;

				g.DrawEllipse(pen, Bounds);

				if (Type == AttributeType.Multivalued)
					g.DrawEllipse(pen, Rectangle.Inflate(Bounds, -3, -3));
			}
			using (var blackBrush = new SolidBrush(EditorControl.ForeColor))
				if (PrimaryKey)
					using (var font = new Font(EditorControl.Font, FontStyle.Underline | FontStyle.Bold))
						g.DrawString(Name, font, blackBrush, Bounds, MainForm.stringFormat);
				else
                    g.DrawString(Name, EditorControl.Font, blackBrush, Bounds, MainForm.stringFormat);
		}

		public override void DrawBorderShape(Graphics g)
		{
			if (!Visible)
				return;

			using (var pen = new Pen(Color.Black))
			{
				if (Type == AttributeType.Derived)
					pen.DashStyle = DashStyle.Dash;
				g.DrawEllipse(pen, Bounds);
			}
		}
        public override string ToString()
        {
            if (string.IsNullOrEmpty(AttributeValueType))
                return base.ToString();
            return string.Format("{0}: {1}", Name, AttributeValueType.ToString());
        }
	}

    public enum AttributeType
    {
        Simple,
        Multivalued,
        Derived
    };
}
