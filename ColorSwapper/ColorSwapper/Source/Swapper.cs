using System;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;

namespace ColorSwapper.Source
{
	public class Swapper
	{
		public EntryCollection EntryCollection { get; set; } = new EntryCollection();

		public ObservableCollection<BitmapEntry> Bitmaps { get; set; } = new ObservableCollection<BitmapEntry>();

		public void LoadBitmaps(string[] files)
		{
			Array.ForEach(files, f =>
			{
				if (Bitmaps.ToList().Exists(t => t.Name == f))
				{
					Bitmaps.Remove(Bitmaps.First(t => t.Name == f));
				}
				Bitmaps.Add(new BitmapEntry() { Name = f, Modified = new Bitmap(new Bitmap(f)), Original = new Bitmap(new Bitmap(f)) });
			});
		}

		public void AddColor(Color from, Color to)
		{
			EntryCollection.AddColor(from, to);
		}

		private void CheckBitmap(string bmpName, Bitmap b)
		{
			for (int x = 0; x < b.Width; x++)
			{
				for (int y = 0; y < b.Height; y++)
				{
					if (EntryCollection.ContainsFrom(b.GetPixel(x, y), out FromToColor fromToColor))
					{
						EntryCollection.AddPoint(bmpName, b, new Point(x, y), fromToColor);
					}
				}
			}
		}

		private void SwapColors(string bmpName, Bitmap b)
		{
			EntryCollection.Points.Where(e =>
				e.BitmapName == bmpName).ToList().ForEach(p =>
					b.SetPixel(p.Location.X, p.Location.Y, p.FromToColor.To));
		}

		private void SaveChanges(BitmapEntry bitmapEntry)
		{
			bitmapEntry.Modified.Save(bitmapEntry.Name);
		}

		public void Process()
		{
			Clear();
			foreach (BitmapEntry b in Bitmaps)
			{
				CheckBitmap(b.Name, b.Original);
				SwapColors(b.Name, b.Modified);
			}
		}

		public void Save()
		{
			Bitmaps.Where(b =>
				EntryCollection.Points.FirstOrDefault(p =>
					p.BitmapName == b.Name) != null).ToList().ForEach(b => SaveChanges(b));
		}

		public void Clear()
		{
			EntryCollection.Points.Clear();
		}
	}
}
