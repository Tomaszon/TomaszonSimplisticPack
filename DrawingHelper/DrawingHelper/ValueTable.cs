using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace DrawingHelper
{
	public class ValueTable
	{
		public List<ValueTableEntry> Values { get; set; } = new List<ValueTableEntry>();

		public List<EasyColor> GetBaseColor(Bitmap bitmap, int topCount)
		{
			for (int y = 0; y < bitmap.Height; y++)
			{
				for (int x = 0; x < bitmap.Width; x++)
				{
					Color c = bitmap.GetPixel(x, y);
					if (!Values.Exists(t => t.Color.ToArgb() == c.ToArgb()))
					{
						Values.Add(new ValueTableEntry() { Color = new EasyColor(c), Count = 1 });
					}
					else
					{
						Values.Find(p => p.Color.ToArgb() == c.ToArgb()).Count++;
					}
				}
			}
			Values = Values.OrderByDescending(p => p.Count).ToList();

			return Values.Take(topCount).Select(e => e.Color).ToList();
		}
	}
}
