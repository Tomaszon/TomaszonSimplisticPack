using System.Drawing;

namespace ColorSwapper.Source
{
	public class PointEntry
	{
		public string BitmapName { get; set; }

		public Point Location { get; set; }

		public FromToColor FromToColor { get; set; }

		public Bitmap Bitmap { get; set; }
	}
}
