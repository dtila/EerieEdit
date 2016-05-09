using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.ComponentModel;

namespace EerieEdit.ERObjects
{
    public enum Multiplicity
    {
        One,
        Many,
        None
    }

	[Serializable]
    public class Link
    {
        /// <summary>
        /// Este obiectul de la care firstLinkAttribute
        /// La legatura Atribut-Entitate, acesta este mereu instanta de Entitate
        /// </summary>
        public ERObject first;
        /// <summary>
        /// Obiuectul asupra caruia s-a inceput creearea legaturii prima data
        /// La legatura Atribut-Entitate, acesta este mereu instanta de Atribut
        /// </summary>
        public ERObject second;
        [Browsable(false)]
        public bool Selected { get; set; }
        
        [Description("Specify the multiplicity between the two objects")]
        [DefaultValue(typeof(Multiplicity), "None")]
        public Multiplicity Multiplicity { get; set; }

        private Point p2, p1;
        public bool hierarhical;
        
        public override string ToString()
        {
            return String.Format("{0} - {1}", first, second); 
        }

        /// <summary>
        /// Creeaza o legatura care trebuie sa fie facuta in urmatorul mod
        /// </summary>
        /// <param name="first">Reprezinta obiectul de la care incepe sa se faca legatura (de obicei este ungerliyingObject)</param>
        /// <param name="second">De regula este firstLinkObject</param>
        /// <param name="continuousLink">True daca este desenata ca linie continua</param>
        public Link(ERObject first, ERObject second, bool hierarhical)
        {
			first.AddLink(this);
			second.AddLink(this);

            this.first = first;
            this.second = second;
            this.hierarhical = hierarhical;

            Multiplicity = Multiplicity.None;

            Recalculate();
        }

        /// <summary>
        /// Get the opposite object that is linked with the function parameter specified
        /// </summary>
        /// <param name="obj">The object that is used to obtain the opposite link</param>
        /// <returns></returns>
		public ERObject GetOtherEnd(ERObject obj)
		{
			if (first == obj)
				return second;
			return first;
		}

        public void Recalculate()
        {
            int dist = int.MaxValue;
            foreach (var ep in first.BoundingRectangle.GetLinkPoints())
                foreach (var ap in second.BoundingRectangle.GetLinkPoints())
                {
                    int d = ep.SquaredDistance(ap);
                    if (ep.SquaredDistance(ap) < dist)
                    {
                        p2 = ep;
                        p1 = ap;
                        dist = d;
                    }
                }
        }

        public bool ContainsPoint(Point p)
        {
			var up = Math.Abs((p2.X - p1.X) * (p1.Y - p.Y) - (p1.X - p.X) * (p2.Y - p1.Y));
			var down = Math.Sqrt(p2.SquaredDistance(p1));
			return (up / down) <= 4.0 && p.X >= Math.Min(p1.X, p2.X) && p.X <= Math.Max(p1.X, p2.X);
        }

        public void Paint(Graphics g)
        {
			using (var pen = new Pen(Color.Black, 2.0f))
			using (var hierarchicalLinkPen = new Pen(Color.Black, 3.0f))
            {
                pen.DashCap = DashCap.Round;
				if (!hierarhical)
					g.DrawLine(pen, p2, p1);
				else
					g.DrawArrow(hierarchicalLinkPen, p1, p2, 10, Math.PI / 3.0);
                
                if (Selected)
                    g.FillRectangles(Brushes.Black, new Rectangle[]
                    {
                        p2.GetAroundRectangle(3),
                        p1.GetAroundRectangle(3),
                    });

                if (Multiplicity == Multiplicity.None)
                    return;

                var middle = new Point((p1.X + p2.X) / 2, (p1.Y + p2.Y) / 2);
                const int multiplicityRectangleHalfSize = 8;

                var rect = new RectangleF(middle.X - multiplicityRectangleHalfSize, middle.Y - multiplicityRectangleHalfSize, 2 * multiplicityRectangleHalfSize, 2 * multiplicityRectangleHalfSize);
                using (var brush = new SolidBrush(first.EditorControl.BackColor))
                {
                    var r = Rectangle.Round(rect);
                    g.FillRectangle(brush, r);
                    g.DrawRectangle(Pens.Black, r);
                }

                var s = Multiplicity == Multiplicity.One ? "1" : "∞";
				using (var font = new Font(first.EditorControl.Font.FontFamily, Multiplicity == Multiplicity.One ? first.EditorControl.Font.SizeInPoints : first.EditorControl.Font.SizeInPoints + 4, GraphicsUnit.Point))
					g.DrawString(s, font, Brushes.Black, rect, MainForm.stringFormat);
            }
        }
    }
}
