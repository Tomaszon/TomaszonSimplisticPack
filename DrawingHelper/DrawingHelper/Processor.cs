using System.Drawing;

namespace DrawingHelper
{
	public class Processor
	{
		public Bitmap ProcessImage(Bitmap bitmap)
		{
			BorderPositions borderPositions = new BorderPositions();
			EmbossPositions embossPositions = new EmbossPositions();

			EasyColor baseColor = new ValueTable().GetBaseColor(bitmap);

			ColorMatrix cmtx = new ColorMatrix();
			cmtx.CreateFromBitmap(bitmap, baseColor);

			ShapeCreator shapeCreator = new ShapeCreator();
			for (int i = 0; i < cmtx.Length; i++)
			{

				ColorMatrixElement element = cmtx.GetElement(i);
				cmtx.GetSurroundingElements(element,
					out ColorMatrixElement N, out ColorMatrixElement S, out ColorMatrixElement W, out ColorMatrixElement E,
					out ColorMatrixElement NW, out ColorMatrixElement NE, out ColorMatrixElement SW, out ColorMatrixElement SE);

				shapeCreator.DetermineBorder(element, baseColor, N, S, W, E, NW, NE, SW, SE);
				shapeCreator.DetermineEmboss(element);
				shapeCreator.SetEmbossColors(embossPositions, element);
				shapeCreator.SetBorderColors(borderPositions, element);
			}

			ImageHandler imageHandler = new ImageHandler();

			return imageHandler.PaintImage(cmtx);
		}
	}
}
