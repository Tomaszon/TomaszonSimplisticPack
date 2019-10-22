using ColorMine.ColorSpaces;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace DrawingHelper
{
	public static class Processor
	{
		public static Bitmap ProcessImage(Bitmap bitmap)
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

	public class ValueTableEntry
	{
		public EasyColor Color { get; set; }

		public int Count { get; set; }
	}

	public class ValueTable
	{
		public List<ValueTableEntry> Values { get; set; } = new List<ValueTableEntry>();

		public EasyColor GetBaseColor(Bitmap bitmap)
		{
			for (int y = 0; y < bitmap.Height; y++)
			{
				for (int x = 0; x < bitmap.Width; x++)
				{
					Color c = bitmap.GetPixel(x, y);
					if (!Values.Exists(t => t.Color.Color.ToArgb() == c.ToArgb()))
					{
						Values.Add(new ValueTableEntry() { Color = new EasyColor(c), Count = 1 });
					}
					else
					{
						Values.Find(p => p.Color.Color.ToArgb() == c.ToArgb()).Count++;
					}
				}
			}

			return Values.FirstOrDefault(v => v.Count == Values.Max(p => p.Count)).Color;
		}
	}

	public class EasyColor
	{
		public Color Color { get; } = Color.Empty;

		public EasyColor(Color color) : this(color.A, color.R, color.G, color.B) { }

		public EasyColor(int a, int r, int g, int b)
		{
			Color = Color.FromArgb(a, r, g, b);
		}

		public static bool operator ==(EasyColor self, EasyColor other)
		{
			if (self is null && !(other is null) || !(self is null) && other is null)
			{
				return false;
			}

			if (self is null && other is null)
			{
				return true;
			}

			return self.Color.ToArgb() == other.Color.ToArgb();
		}

		public static bool operator !=(EasyColor self, EasyColor other)
		{
			return !(self == other);
		}

		public override string ToString()
		{
			return $"A:{Color.A} R:{Color.R} G:{Color.G} B:{Color.B}";
		}

		public EasyColor AddLuminance(double luminance)
		{
			IRgb rgb = new Rgb() { R = Color.R, G = Color.G, B = Color.B };
			Lch lch = rgb.To<Lch>();
			double newLuminance = lch.L + luminance;
			lch.L = newLuminance < 0 ? 0 : newLuminance > 100 ? 100 : newLuminance;
			rgb = lch.ToRgb();
			Color newColor = Color.FromArgb(Color.A, (int)rgb.R, (int)rgb.G, (int)rgb.B);
			return new EasyColor(newColor);
		}

		public override bool Equals(object obj)
		{
			return base.Equals(obj);
		}

		public override int GetHashCode()
		{
			return base.GetHashCode();
		}
	}

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
					EasyColor bc = c.AddLuminance(Properties.Settings.Default.BorderColorDiff);
					EasyColor lc = c.AddLuminance(Properties.Settings.Default.ShineColorDiff);
					EasyColor sc = c.AddLuminance(Properties.Settings.Default.ShadowColorDiff);

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

	public class Bordering
	{
		public bool N { get; set; }

		public bool S { get; set; }

		public bool W { get; set; }

		public bool E { get; set; }

		public bool NW { get; set; }

		public bool NE { get; set; }

		public bool SW { get; set; }

		public bool SE { get; set; }
	}

	public class Embossing
	{
		public bool N { get; set; }

		public bool S { get; set; }

		public bool NW { get; set; }

		public bool NE { get; set; }

		public bool SW { get; set; }

		public bool SE { get; set; }
	}

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
			element.Bordering.NW = element.Color != baseColor && (NW is null || NW.Color == baseColor);
			element.Bordering.NE = element.Color != baseColor && (NE is null || NE.Color == baseColor);
			element.Bordering.SW = element.Color != baseColor && (SW is null || SW.Color == baseColor);
			element.Bordering.SE = element.Color != baseColor && (SE is null || SE.Color == baseColor);
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

	public class BorderPositions
	{
		public Point[] N { get; } = new Point[6];

		public Point[] S { get; } = new Point[6];

		public Point[] W { get; } = new Point[6];

		public Point[] E { get; } = new Point[6];

		public Point NW { get; }

		public Point NE { get; }

		public Point SW { get; }

		public Point SE { get; }

		public BorderPositions()
		{
			for (int p = 1; p <= N.Length; p++)
			{
				N[p - 1] = new Point(p, 0);
				S[p - 1] = new Point(p, 7);
				W[p - 1] = new Point(0, p);
				E[p - 1] = new Point(7, p);
			}

			NW = new Point(0, 0);
			NE = new Point(7, 0);
			SW = new Point(0, 7);
			SE = new Point(7, 7);
		}
	}

	public class EmbossPositions
	{
		public Point[] N { get; } = new Point[6];

		public Point[] S { get; } = new Point[6];

		public Point NW { get; }

		public Point NE { get; }

		public Point SW { get; }

		public Point SE { get; }

		public EmbossPositions()
		{
			for (int p = 1; p <= N.Length; p++)
			{
				N[p - 1] = new Point(p, 1);
				S[p - 1] = new Point(p, 6);
			}

			NW = new Point(0, 1);
			NE = new Point(7, 1);
			SW = new Point(0, 6);
			SE = new Point(7, 6);
		}
	}

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
