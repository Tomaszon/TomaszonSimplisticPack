using System.Drawing;

namespace DrawingHelper
{
	public class ColorMatrixElement
	{
		public Point Location { get; set; }

		public EasyColor Color { get; set; }

		public EasyColor BorderColor { get; set; }

		public EasyColor ShineColor { get; set; }

		public EasyColor ShadowColor { get; set; }

		public EasyColor[,] ScaledInnerColorMatrix { get; set; } = new EasyColor[8, 8];

		public Bordering Bordering { get; set; } = new Bordering();

		public Embossing Embossing { get; set; } = new Embossing();

		public ColorMatrixElement(EasyColor c)
		{
			for (int y = 0; y < ScaledInnerColorMatrix.GetLength(1); y++)
			{
				for (int x = 0; x < ScaledInnerColorMatrix.GetLength(0); x++)
				{
					ScaledInnerColorMatrix[x, y] = c;
				}
			}
		}
	}
}
