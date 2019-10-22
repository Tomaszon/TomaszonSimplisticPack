using ColorMine.ColorSpaces;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace DrawingHelper
{
	public class ValueTable
	{
		public Dictionary<EasyColor, int> Values { get; set; } = new Dictionary<EasyColor, int>();

		public EasyColor GetBaseColor()
		{
			return Values.FirstOrDefault(v => v.Value == Values.Max(p => p.Value)).Key;
		}
	}

	public class EasyColor
	{
		private Color Color { get; } = Color.Empty;

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
					EasyColor bc = c.AddLuminance(Properties.Settings.Default.BorderColorDiff);
					EasyColor lc = c.AddLuminance(Properties.Settings.Default.ShineColorDiff);
					EasyColor sc = c.AddLuminance(Properties.Settings.Default.ShadowColorDiff);

					_mtx[x, y] = new ColorMatrixElement() { Location = new Point(x, y), Color = c, BorderColor = bc, ShinColor = lc, ShadowColor = sc };
				}
			}
		}
	}

	public class ColorMatrixElement
	{
		public Point Location { get; set; }

		public EasyColor Color { get; set; }

		public EasyColor BorderColor { get; set; }

		public EasyColor ShinColor { get; set; }

		public EasyColor ShadowColor { get; set; }

		public EasyColor[,] ScaledInnerColorMatrix { get; set; } = new EasyColor[8, 8];

		public Bordering Bordering { get; set; }

		public Embossing Embossing { get; set; }

		public void InitScaledInnerColorMatrix(EasyColor c)
		{
			for (int y = 0; y < ScaledInnerColorMatrix.GetLength(0); y++)
			{
				for (int x = 0; x < ScaledInnerColorMatrix.GetLength(1); x++)
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
			element.Bordering.N = N.Color == baseColor;
			element.Bordering.S = S.Color == baseColor;
			element.Bordering.W = W.Color == baseColor;
			element.Bordering.E = E.Color == baseColor;
			element.Bordering.NW = NW.Color == baseColor;
			element.Bordering.NE = NE.Color == baseColor;
			element.Bordering.SW = SW.Color == baseColor;
			element.Bordering.SE = SE.Color == baseColor;
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
			for (int p = 2; p <= N.Length; p++)
			{
				N[p - 2] = new Point(p, 0);
				S[p - 2] = new Point(p, 8);
				W[p - 2] = new Point(0, p);
				E[p - 2] = new Point(8, p);
			}

			NW = new Point(0, 0);
			NE = new Point(8, 0);
			SW = new Point(0, 8);
			SE = new Point(8, 8);
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
			for (int p = 2; p <= N.Length; p++)
			{
				N[p - 2] = new Point(p, 1);
				S[p - 2] = new Point(p, 7);
			}

			NW = new Point(0, 1);
			NE = new Point(8, 1);
			SW = new Point(0, 7);
			SE = new Point(8, 7);
		}
	}

	public class ImageResizer
	{

	}
}
