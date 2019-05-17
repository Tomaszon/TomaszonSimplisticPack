using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ColorSwapper.Source
{
	public class SwapClass
	{
		public ObservableCollection<FromToColor> Colors { get; set; } = new ObservableCollection<FromToColor>();

		public List<Tuple<Bitmap, int, Point>> Points { get; set; } = new List<Tuple<Bitmap, int, Point>>();

		public void AddColor(Color from, Color to)
		{
			if (Colors.ToList().Exists(t => t.From.ToArgb() == from.ToArgb()))
			{
				Colors.Remove(Colors.First(t => t.From.ToArgb() == from.ToArgb()));
			}
			Colors.Add(new FromToColor() { From = from, To = to });
		}

		public void AddPoint(Bitmap b, int index, Point point)
		{
			Points.Add(new Tuple<Bitmap, int, Point>(b, index, point));
		}

		public bool ContainsFrom(Color color, out int index)
		{
			index = Colors.ToList().FindIndex(t => t.From.ToArgb() == color.ToArgb());

			return index != -1;
		}
	}

	public class FromToColor
	{
		public Color From { get; set; }

		public Color To { get; set; }

		public string FromFormatted { get { return $"{From.A}, {From.R}, {From.G}, {From.B}"; } }

		public string ToFormatted { get { return $"{To.A}, {To.R}, {To.G}, {To.B}"; } }

		public string Formatted { get { return $"{FromFormatted} => {ToFormatted}"; } }
	}
}
