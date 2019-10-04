using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;

namespace ColorSwapper.Source
{
	public class EntryCollection
	{
		public ObservableCollection<FromToColor> Colors { get; set; } = new ObservableCollection<FromToColor>();

		public ObservableCollection<PointEntry> Points { get; set; } = new ObservableCollection<PointEntry>();

		public void AddColor(Color from, Color to)
		{
			if (Colors.ToList().Exists(t => t.From.ToArgb() == from.ToArgb()))
			{
				Colors.Remove(Colors.First(t => t.From.ToArgb() == from.ToArgb()));
			}
			Colors.Add(new FromToColor() { From = from, To = to });
		}

		public void AddPoint(string bmpName, Bitmap b, Point point, FromToColor fromToColor)
		{
			Points.Add(new PointEntry() { BitmapName = bmpName, Bitmap = b, FromToColor = fromToColor, Location = point });
		}

		public bool ContainsFrom(Color color, out FromToColor fromToColor)
		{
			fromToColor = Colors.ToList().Find(t => t.From.ToArgb() == color.ToArgb());

			return fromToColor != null;
		}
	}
}
