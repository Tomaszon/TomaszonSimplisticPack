using System.Drawing;

namespace DrawingHelper
{
	public class ImageHandler
	{
		public Bitmap PaintImage(ColorMatrix colorMatrix)
		{
			Bitmap b = new Bitmap(64, 64);

			for (int i = 0; i < colorMatrix.Length; i++)
			{
				ColorMatrixElement element = colorMatrix.GetElement(i);
				for (int y = 0; y < element.ScaledInnerColorMatrix.GetLength(1); y++)
				{
					for (int x = 0; x < element.ScaledInnerColorMatrix.GetLength(0); x++)
					{
						b.SetPixel(element.Location.X * 8 + x, element.Location.Y * 8 + y, element.ScaledInnerColorMatrix[x, y].Color);
					}
				}
			}

			return b;
		}
	}
}
