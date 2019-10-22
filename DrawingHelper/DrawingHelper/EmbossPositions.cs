using System.Drawing;

namespace DrawingHelper
{
	public class EmbossPositions
	{
		public Point[] N { get; } = new Point[6];

		public Point[] S { get; } = new Point[6];

		public Point NW { get; }

		public Point NE { get; }

		public Point SW { get; }

		public Point SE { get; }

		public EmbossPositions()
		{
			for (int p = 1; p <= N.Length; p++)
			{
				N[p - 1] = new Point(p, 1);
				S[p - 1] = new Point(p, 6);
			}

			NW = new Point(0, 1);
			NE = new Point(7, 1);
			SW = new Point(0, 6);
			SE = new Point(7, 6);
		}
	}
}
