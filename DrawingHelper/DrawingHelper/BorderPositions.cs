using System.Drawing;

namespace DrawingHelper
{
	public class BorderPositions
	{
		public Point[] N { get; } = new Point[6];

		public Point[] S { get; } = new Point[6];

		public Point[] W { get; } = new Point[6];

		public Point[] E { get; } = new Point[6];

		public Point NW { get; }

		public Point NE { get; }

		public Point SW { get; }

		public Point SE { get; }

		public BorderPositions()
		{
			for (int p = 1; p <= N.Length; p++)
			{
				N[p - 1] = new Point(p, 0);
				S[p - 1] = new Point(p, 7);
				W[p - 1] = new Point(0, p);
				E[p - 1] = new Point(7, p);
			}

			NW = new Point(0, 0);
			NE = new Point(7, 0);
			SW = new Point(0, 7);
			SE = new Point(7, 7);
		}
	}
}
