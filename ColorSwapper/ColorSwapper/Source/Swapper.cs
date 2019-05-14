using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ColorSwapper.Source
{
	public class Swapper
	{
		public SwapClass SwapClass { get; set; } = new SwapClass();

		public List<Tuple<string, Bitmap>> Bitmaps { get; set; } = new List<Tuple<string, Bitmap>>();

		public void LoadBitmaps(string[] files)
		{
			Array.ForEach(files, f => Bitmaps.Add(new Tuple<string, Bitmap>(f, new Bitmap(new Bitmap(f)))));
		}

		public void AddColor(Color from, Color to)
		{
			SwapClass.AddColor(from, to);
		}

		private void CheckBitmap(Bitmap b)
		{
			for (int x = 0; x < b.Width; x++)
			{
				for (int y = 0; y < b.Height; y++)
				{
					if (SwapClass.ContainsFrom(b.GetPixel(x, y), out int index))
					{
						SwapClass.AddPoint(index, new Point(x, y));
					}
				}
			}
		}

		private void SwapColors(Bitmap b)
		{
			SwapClass.Points.ForEach(p => b.SetPixel(p.Item2.X, p.Item2.Y, SwapClass.Colors[p.Item1].Item2));
		}

		private void SaveChanges(Tuple<string, Bitmap> bitmap)
		{
			bitmap.Item2.Save(bitmap.Item1);
		}

		public void Process()
		{
			Bitmaps.ForEach(b => CheckBitmap(b.Item2));

			Bitmaps.ForEach(b => SwapColors(b.Item2));

			Bitmaps.ForEach(b => SaveChanges(b));
		}
	}
}
