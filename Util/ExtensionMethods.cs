using System.Drawing;
using System;

namespace EerieEdit
{
	public delegate void Action<T1, T2>(T1 t, T2 t2);

	public static class ExtensionMethods
	{
		public const int halfSelectedRectangle = 3;

		public static Rectangle[] GetSelectionHandles(this Rectangle rect)
		{
			return new Rectangle[]
            {
                new Rectangle(rect.Left - halfSelectedRectangle, rect.Top - halfSelectedRectangle, halfSelectedRectangle * 2, halfSelectedRectangle * 2),
                new Rectangle(rect.Right - halfSelectedRectangle, rect.Top - halfSelectedRectangle, halfSelectedRectangle * 2, halfSelectedRectangle * 2),
                new Rectangle(rect.Left - halfSelectedRectangle, rect.Bottom - halfSelectedRectangle, halfSelectedRectangle * 2, halfSelectedRectangle *2),
                new Rectangle(rect.Right - halfSelectedRectangle, rect.Bottom - halfSelectedRectangle, halfSelectedRectangle * 2, halfSelectedRectangle * 2)
            };
		}
		public static Point[] GetLinkPoints(this Rectangle rect)
		{
			return new Point[]
            {
                new Point(rect.Left + rect.Width / 2, rect.Top),
                new Point(rect.Left, rect.Top + rect.Height / 2),
                new Point(rect.Left + rect.Width / 2, rect.Bottom),
                new Point(rect.Right, rect.Top + rect.Height / 2),
            };
		}

        public static Point[] GetHorizontalLinkPoints(this Rectangle rect)
        {
            return new Point[]
            {
                new Point(rect.Left, rect.Top + rect.Height / 2),
                new Point(rect.Right, rect.Top + rect.Height / 2),
            };
        }

        public static Rectangle GetAroundRectangle(this Point p, int size)
        {
            return Rectangle.FromLTRB(p.X - size, p.Y - size, p.X + size, p.Y + size);
        }

		public static int SquaredDistance(this Point p1, Point p2)
		{
			int d1 = p1.X - p2.X;
			int d2 = p1.Y - p2.Y;
			return d1 * d1 + d2 * d2;
		}

		public static void DrawArrow(this Graphics g, Pen pen, Point pt1, Point pt2, int width, double alpha)
		{
			g.DrawLine(pen, pt1, pt2);

			var v = new Point(pt2.X - pt1.X, pt2.Y - pt1.Y);
			var len = Math.Sqrt(v.X * v.X + v.Y * v.Y);
			var distance = width / (Math.Tan(alpha / 2.0) * len);
			var basePoint = new Point(pt2.X - (int) (distance * v.X), pt2.Y - (int) (distance * v.Y));
			var normal = new Point(-v.Y, v.X);
			var distNormal = width / (2.0 * len);
			var left = new Point(basePoint.X + (int) (distNormal * normal.X), basePoint.Y + (int) (distNormal * normal.Y));
			var right = new Point(basePoint.X - (int) (distNormal * normal.X), basePoint.Y - (int) (distNormal * normal.Y));

			g.DrawLine(pen, pt2, left);
			g.DrawLine(pen, pt2, right);
		}
	}
}

namespace System.Runtime.CompilerServices
{
	[AttributeUsageAttribute(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Method)]
	public sealed class ExtensionAttribute : Attribute
	{
		public ExtensionAttribute() : base() { }
	}
}
