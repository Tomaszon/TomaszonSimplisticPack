using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using TSPTitleGenerator.Properties;

namespace TSPTitleGenerator
{
	public class Generator
	{
		private Color _backgroundColor;

		private int _textMaxX = 0;

		private readonly List<Element> _nameElements = new List<Element>();

		private readonly List<Element> _customModelElements = new List<Element>();

		private readonly List<Element> _ctmSupportElements = new List<Element>();

		private readonly List<Element> _variantsElements = new List<Element>();

		private readonly List<Element> _backgroundElements = new List<Element>();

		private Action<string> _progressHandler;

		public void Generate(WorldType worldType, TitleType titleType, string name, int variants, bool customModel, bool ctmSupport, Action<string> progressHandler)
		{
			_progressHandler = progressHandler;

			Init(worldType, titleType, name, variants, customModel, ctmSupport);

			progressHandler?.Invoke("Background elements colored");

			Bitmap image = new Bitmap(Settings.Default.OutputImageSize.Width, Settings.Default.OutputImageSize.Height);

			DrawShapes(image);

			progressHandler?.Invoke("Text wrote");

			image.Save(Path.Combine(Settings.Default.OutputDirectory, $"{name}.png"));

			progressHandler?.Invoke("Successfully generated");
		}

		private void Init(WorldType worldType, TitleType titleType, string name, int variants, bool customModel, bool ctmSupport)
		{
			switch (worldType)
			{
				case WorldType.Overworld:
					_backgroundColor = Settings.Default.OverworldColorCode;
					break;
				case WorldType.Nether:
					_backgroundColor = Settings.Default.NetherColorCode;
					break;
				case WorldType.End:
					_backgroundColor = Settings.Default.EndColorCode;
					break;
			}

			Point nameStartLocation = new Point();
			switch (titleType)
			{
				case TitleType.Block:
					_backgroundElements.Add(ColorBackgroundElement(new Element(Resources.block, Settings.Default.BlockBackgroundLocation)));
					nameStartLocation = Settings.Default.BlockNameLocation;
					break;
				case TitleType.Entity:
					_backgroundElements.Add(ColorBackgroundElement(new Element(Resources.entity, Settings.Default.SingleBackgroundLocation)));
					nameStartLocation = Settings.Default.EntityNameLocation;
					break;
				case TitleType.Item:
					_backgroundElements.Add(ColorBackgroundElement(new Element(Resources.item, Settings.Default.SingleBackgroundLocation)));
					nameStartLocation = Settings.Default.ItemNameLocation;
					break;
			}

			LoadTextElements(_nameElements, name, nameStartLocation);
			if (titleType == TitleType.Block)
			{
				LoadTextElements(_variantsElements, variants.ToString(), Settings.Default.VariantsLocation);
				LoadTextElements(_customModelElements, customModel ? "yes" : "no", Settings.Default.CustomMoldeLocation);
				LoadTextElements(_ctmSupportElements, ctmSupport ? "yes" : "no", Settings.Default.CTMSupportLocation);
			}

			int currentX = _backgroundElements.First().Location.X + _backgroundElements.First().Image.Width;
			while (currentX < _textMaxX)
			{
				_backgroundElements.Add(ColorBackgroundElement(new Element(titleType == TitleType.Block ? Resources.block_background : Resources.single_background, new Point(currentX, _backgroundElements.First().Location.Y))));
				currentX++;
			}

			switch (titleType)
			{
				case TitleType.Block:
					_backgroundElements.Add(ColorBackgroundElement(new Element(Resources.block_end, new Point(_textMaxX, Settings.Default.BlockBackgroundLocation.Y))));
					break;
				default:
					_backgroundElements.Add(ColorBackgroundElement(new Element(Resources.single_end, new Point(_textMaxX, Settings.Default.SingleBackgroundLocation.Y))));
					break;
			}
		}

		private void DrawShapes(Bitmap image)
		{
			using (Graphics g = Graphics.FromImage(image))
			{
				g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
				g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.Half;

				foreach (var e in _backgroundElements)
				{
					g.DrawImage(e.Image, new Rectangle(e.Location, new Size(e.Size.Width, e.Size.Height)));
				}

				_progressHandler?.Invoke("Background drawed");

				Array.ForEach(new[] { _nameElements, _customModelElements, _ctmSupportElements, _variantsElements }, l =>
				{
					foreach (var e in l)
					{
						g.DrawImage(e.Image, new Rectangle(e.Location, new Size(e.Size.Width * 2, e.Size.Height * 2)));
					}
				});
			}

		}

		public Element ColorBackgroundElement(Element element)
		{
			for (int x = 0; x < element.Image.Width; x++)
			{
				for (int y = 0; y < element.Image.Height; y++)
				{
					if (element.Image.GetPixel(x, y).ToArgb() == Color.White.ToArgb())
					{
						element.Image.SetPixel(x, y, _backgroundColor);
					}
				}
			}

			return element;
		}

		private void LoadTextElements(List<Element> list, string content, Point startLocation)
		{
			foreach (var c in content)
			{
				string character = c == ' ' ? "_space" : c.ToString();
				if (int.TryParse(character, out int _))
				{
					character = $"_{character}";
				}

				Point nexLoc = list.Count == 0 ? startLocation : Point.Add(list.Last().Location, new Size(list.Last().Image.Size.Width * 2, 0));

				list.Add(new Element((Bitmap)Resources.ResourceManager.GetObject(character), nexLoc));
			}

			_textMaxX = _textMaxX < list.Last().Location.X + list.Last().Image.Width * 2 ? list.Last().Location.X + list.Last().Image.Width * 2 : _textMaxX;
		}
	}

	public enum WorldType
	{
		Overworld,
		Nether,
		End
	}

	public enum TitleType
	{
		Block,
		Entity,
		Item
	}

	public class Element
	{
		public Size Size { get; set; }

		public Bitmap Image { get; set; }

		public Point Location { get; set; }

		public Element(Bitmap image, Point location)
		{
			Image = image;
			Size = image.Size;
			Location = location;
		}
	}
}
