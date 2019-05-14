using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ColorSwapper.Source
{
	public class SwapClass
	{
		public List<Tuple<Color, Color>> Colors { get; set; } = new List<Tuple<Color, Color>>();

		public List<Tuple<Bitmap, int, Point>> Points { get; set; } = new List<Tuple<Bitmap, int, Point>>();

		public void AddColor(Color from, Color to)
		{
			if (!Colors.Exists(t => t.Item1.ToArgb() == from.ToArgb()))
			{
				Colors.Add(new Tuple<Color, Color>(from, to));
			}
		}

		public void AddPoint(Bitmap b, int index, Point point)
		{
			Points.Add(new Tuple<Bitmap, int, Point>(b, index, point));
		}

		public bool ContainsFrom(Color color, out int index)
		{
			index = Colors.FindIndex(t => t.Item1.ToArgb() == color.ToArgb());

			return index != -1;
		}
	}
}
