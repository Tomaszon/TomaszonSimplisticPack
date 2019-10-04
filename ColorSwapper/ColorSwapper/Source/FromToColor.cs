using System.Collections.Generic;
using System.Drawing;

namespace ColorSwapper.Source
{
	public class FromToColor
	{
		public Color From { get; set; }

		public Color To { get; set; }

		public string FromFormatted { get { return $"{From.A}, {From.R}, {From.G}, {From.B}"; } }

		public string ToFormatted { get { return $"{To.A}, {To.R}, {To.G}, {To.B}"; } }

		public string Formatted { get { return $"{FromFormatted} => {ToFormatted}"; } }

		public static bool operator ==(FromToColor self, FromToColor other)
		{
			return self.Formatted == other.Formatted;
		}

		public static bool operator !=(FromToColor self, FromToColor other)
		{
			return !(self == other);
		}
	}
}
