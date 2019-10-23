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
		public void CreateFromBitmap(Bitmap b)
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

		public void GetSurroundingElements(ColorMatrixElement center, BlockMassType massType,
			out ColorMatrixElement N, out ColorMatrixElement S, out ColorMatrixElement W, out ColorMatrixElement E,
			out ColorMatrixElement NW, out ColorMatrixElement NE, out ColorMatrixElement SW, out ColorMatrixElement SE)
		{
			int x = center.Location.X;
			int y = center.Location.Y;

			N = Get(x, y - 1, massType);
			S = Get(x, y + 1, massType);
			W = Get(x - 1, y, massType);
			E = Get(x + 1, y, massType);

			NW = Get(x - 1, y - 1, massType);
			NE = Get(x + 1, y - 1, massType);
			SW = Get(x - 1, y + 1, massType);
			SE = Get(x + 1, y + 1, massType);
		}

		private ColorMatrixElement Get(int x, int y, BlockMassType massType)
		{
			try
			{
				return _mtx[x, y];
			}
			catch (IndexOutOfRangeException)
			{
				if (massType == BlockMassType.Invidual)
				{
					return null;
				}
				else
				{
					return _mtx[(x + _mtx.GetLength(1)) % _mtx.GetLength(1), (y + _mtx.GetLength(0)) % _mtx.GetLength(0)];
				}
			}
		}
	}
}
