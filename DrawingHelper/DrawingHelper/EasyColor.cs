using ColorMine.ColorSpaces;
using System.Drawing;

namespace DrawingHelper
{
	public class EasyColor
	{
		public Color Color { get; } = Color.Empty;

		public EasyColor(Color color) : this(color.A, color.R, color.G, color.B) { }

		public EasyColor(int a, int r, int g, int b)
		{
			Color = Color.FromArgb(a, r, g, b);
		}

		public int ToArgb()
		{
			return Color.ToArgb();
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

			return self.ToArgb() == other.Color.ToArgb();
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
}
