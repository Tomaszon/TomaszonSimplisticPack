using System;
using System.Drawing;

namespace DrawingHelper
{
	public class ShapeCreator
	{
		public void DetermineBorder(ColorMatrixElement element, EasyColor baseColor,
			ColorMatrixElement N, ColorMatrixElement S, ColorMatrixElement W, ColorMatrixElement E,
			ColorMatrixElement NW, ColorMatrixElement NE, ColorMatrixElement SW, ColorMatrixElement SE)
		{
			element.Bordering.N = element.Color != baseColor && (N is null || N.Color == baseColor);
			element.Bordering.S = element.Color != baseColor && (S is null || S.Color == baseColor);
			element.Bordering.W = element.Color != baseColor && (W is null || W.Color == baseColor);
			element.Bordering.E = element.Color != baseColor && (E is null || E.Color == baseColor);
			element.Bordering.NW = element.Color != baseColor && (element.Bordering.N || element.Bordering.W || NW is null || NW.Color == baseColor);
			element.Bordering.NE = element.Color != baseColor && (element.Bordering.N || element.Bordering.E || NE is null || NE.Color == baseColor);
			element.Bordering.SW = element.Color != baseColor && (element.Bordering.S || element.Bordering.W || SW is null || SW.Color == baseColor);
			element.Bordering.SE = element.Color != baseColor && (element.Bordering.S || element.Bordering.E || SE is null || SE.Color == baseColor);
		}

		public void SetBorderColors(BorderPositions borderPositions, ColorMatrixElement element)
		{
			if (element.Bordering.N)
			{
				SetInnerColors(element.BorderColor, element, borderPositions.N);
			}
			if (element.Bordering.S)
			{
				SetInnerColors(element.BorderColor, element, borderPositions.S);
			}
			if (element.Bordering.W)
			{
				SetInnerColors(element.BorderColor, element, borderPositions.W);
			}
			if (element.Bordering.E)
			{
				SetInnerColors(element.BorderColor, element, borderPositions.E);
			}
			if (element.Bordering.NW)
			{
				SetInnerColors(element.BorderColor, element, borderPositions.NW);
			}
			if (element.Bordering.NE)
			{
				SetInnerColors(element.BorderColor, element, borderPositions.NE);
			}
			if (element.Bordering.SW)
			{
				SetInnerColors(element.BorderColor, element, borderPositions.SW);
			}
			if (element.Bordering.SE)
			{
				SetInnerColors(element.BorderColor, element, borderPositions.SE);
			}
		}

		private void SetInnerColors(EasyColor swapColor, ColorMatrixElement element, params Point[] points)
		{
			Array.ForEach(points, e => element.ScaledInnerColorMatrix[e.X, e.Y] = swapColor);
		}

		public void DetermineEmboss(ColorMatrixElement element)
		{
			element.Embossing.N = element.Bordering.N;
			element.Embossing.S = element.Bordering.S;
			element.Embossing.NW = element.Bordering.NW;
			element.Embossing.NE = element.Bordering.NE;
			element.Embossing.SW = element.Bordering.SW;
			element.Embossing.SE = element.Bordering.SE;
		}

		public void SetEmbossColors(EmbossPositions embossPositions, ColorMatrixElement element)
		{
			if (element.Embossing.N)
			{
				SetInnerColors(element.ShineColor, element, embossPositions.N);
			}
			if (element.Embossing.S)
			{
				SetInnerColors(element.ShadowColor, element, embossPositions.S);
			}
			if (element.Embossing.NW)
			{
				SetInnerColors(element.ShineColor, element, embossPositions.NW);
			}
			if (element.Embossing.NE)
			{
				SetInnerColors(element.ShineColor, element, embossPositions.NE);
			}
			if (element.Embossing.SW)
			{
				SetInnerColors(element.ShadowColor, element, embossPositions.SW);
			}
			if (element.Embossing.SE)
			{
				SetInnerColors(element.ShadowColor, element, embossPositions.SE);
			}
		}
	}
}
