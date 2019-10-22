using System;
using System.Drawing;

namespace DrawingHelper
{
	public class ColorMatrix
	{
		private readonly ColorMatrixElement[,] _mtx = new ColorMatrixElement[8, 8];

		public int Length => _mtx.Length;

		public ColorMatrixElement GetElement(int index) => _mtx[index / _mtx.GetLength(0), index % _mtx.GetLength(0)];

		/// <summary>
		/// Create from 8x8 bitmap
		/// </summary>
		public void CreateFromBitmap(Bitmap b, EasyColor baseColor)
		{
			if (b.Size.Height != 8 || b.Size.Width != 8)
			{
				throw new ArgumentException("Image size must be 8x8");
			}

			for (int y = 0; y < b.Height; y++)
			{
				for (int x = 0; x < b.Width; x++)
				{
					EasyColor c = new EasyColor(b.GetPixel(x, y));
					EasyColor bc = c.AddLuminance(Properties.Settings.Default.BorderColorDiffBase);
					EasyColor lc = c.AddLuminance(Properties.Settings.Default.ShineColorDiffBase);
					EasyColor sc = c.AddLuminance(Properties.Settings.Default.ShadowColorDiffBase);

					_mtx[x, y] = new ColorMatrixElement(c) { Location = new Point(x, y), Color = c, BorderColor = bc, ShineColor = lc, ShadowColor = sc };
				}
			}
		}

		public void GetSurroundingElements(ColorMatrixElement center, out ColorMatrixElement N, out ColorMatrixElement S,
			out ColorMatrixElement W, out ColorMatrixElement E, out ColorMatrixElement NW,
			out ColorMatrixElement NE, out ColorMatrixElement SW, out ColorMatrixElement SE)
		{
			int x = center.Location.X;
			int y = center.Location.Y;

			N = Get(x, y - 1);
			S = Get(x, y + 1);
			W = Get(x - 1, y);
			E = Get(x + 1, y);

			NW = Get(x - 1, y - 1);
			NE = Get(x + 1, y - 1);
			SW = Get(x - 1, y + 1);
			SE = Get(x + 1, y + 1);
		}

		private ColorMatrixElement Get(int x, int y)
		{
			try
			{
				return _mtx[x, y];
			}
			catch (IndexOutOfRangeException)
			{
				return null;
			}
		}
	}
}
