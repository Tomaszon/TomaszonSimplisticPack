using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ColorSwapper.Source
{
	public class Swapper
	{
		public SwapClass SwapClass { get; set; } = new SwapClass();

		public ObservableCollection<Tuple<string, Bitmap, Bitmap>> Bitmaps { get; set; } = new ObservableCollection<Tuple<string, Bitmap, Bitmap>>();

		public void LoadBitmaps(string[] files)
		{
			Array.ForEach(files, f =>
			{
				if (Bitmaps.ToList().Exists(t => t.Item1 == f))
				{
					Bitmaps.Remove(Bitmaps.First(t => t.Item1 == f));
				}
				Bitmaps.Add(new Tuple<string, Bitmap, Bitmap>(f, new Bitmap(new Bitmap(f)), null));
			});
		}

		public void AddColor(Color from, Color to)
		{
			SwapClass.AddColor(from, to);
		}

		private void CheckBitmap(Bitmap b)
		{
			SwapClass.Points.Clear();
			for (int x = 0; x < b.Width; x++)
			{
				for (int y = 0; y < b.Height; y++)
				{
					if (SwapClass.ContainsFrom(b.GetPixel(x, y), out int index))
					{
						SwapClass.AddPoint(b, index, new Point(x, y));
					}
				}
			}
		}

		private void SwapColors(Bitmap b)
		{
			SwapClass.Points.Where(e => e.Item1 == b).ToList().ForEach(p => b.SetPixel(p.Item3.X, p.Item3.Y, SwapClass.Colors[p.Item2].To));
		}

		private void SaveChanges(Tuple<string, Bitmap, Bitmap> bitmap)
		{
			bitmap.Item2.Save(bitmap.Item1);
		}

		public void Process()
		{
			foreach (var b in Bitmaps)
			{
				CheckBitmap(b.Item2);
				SwapColors(b.Item2);
			}
		}

		public void Save()
		{
			foreach (var b in Bitmaps)
			{
				SaveChanges(b);
			}
		}

		public void Clear()
		{
			SwapClass.Points.Clear();
			SwapClass.Points.Clear();
		}
	}
}
