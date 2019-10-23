using System.Collections.Generic;
using System.Drawing;

namespace DrawingHelper
{
	public class Processor
	{
		public Bitmap ProcessImage(Bitmap bitmap, int baseColorCount, BlockMassType massType)
		{
			BorderPositions borderPositions = new BorderPositions();
			EmbossPositions embossPositions = new EmbossPositions();

			List<EasyColor> baseColors = new ValueTable().GetBaseColor(bitmap, baseColorCount);

			ColorMatrix cmtx = new ColorMatrix();
			cmtx.CreateFromBitmap(bitmap);

			ShapeCreator shapeCreator = new ShapeCreator();
			for (int i = 0; i < cmtx.Length; i++)
			{

				ColorMatrixElement element = cmtx.GetElement(i);
				cmtx.GetSurroundingElements(element, massType,
					out ColorMatrixElement N, out ColorMatrixElement S, out ColorMatrixElement W, out ColorMatrixElement E,
					out ColorMatrixElement NW, out ColorMatrixElement NE, out ColorMatrixElement SW, out ColorMatrixElement SE);

				shapeCreator.DetermineBorder(element, baseColors, N, S, W, E, NW, NE, SW, SE);
				shapeCreator.DetermineEmboss(element);
				shapeCreator.SetEmbossColors(embossPositions, element);
				shapeCreator.SetBorderColors(borderPositions, element);
			}

			ImageHandler imageHandler = new ImageHandler();

			return imageHandler.PaintImage(cmtx);
		}
	}
}
