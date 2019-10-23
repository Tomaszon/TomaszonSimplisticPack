using System;
using System.Drawing;

namespace DrawingHelper
{
	internal class Program
	{
		private static void Main(string[] args)
		{
			while (true)
			{
			Start:
				Console.WriteLine($"Commands: {Command.Exit}");
				Console.WriteLine($"Params: {{path}} {{base color count}} {{mass type {BlockMassType.Invidual}|{BlockMassType.Mass}}} ");
				try
				{
					string rawInput = Console.ReadLine();

					if (Enum.TryParse(rawInput, true, out Command command))
					{
						switch (command)
						{
							case Command.Exit:
								goto End;
							default:
								Console.WriteLine();
								goto Start;
						}
					}

					string[] input = rawInput.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

					string fileName = input[0];
					int baseColorCount = int.Parse(input[1]);
					BlockMassType massType = (BlockMassType)Enum.Parse(typeof(BlockMassType), input[2], true);

					Bitmap originalImage = (Bitmap)Image.FromFile(input[0]);
					Bitmap output = new Processor().ProcessImage(originalImage, baseColorCount, massType);

					output.Save(@"C:\Users\toti9\Documents\GitHub\TomaszonSimplisticPack\DrawingHelper\DrawingHelper\bin\Debug\output.png");

					Console.WriteLine("Processed!");
					Console.WriteLine();
				}
				catch (Exception ex)
				{
					Console.WriteLine(ex.Message);
				}
			}

		End:
			Console.WriteLine("Press to exit");
			Console.ReadKey();
		}
	}

	public enum Command
	{
		Exit
	}
}
