using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

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
				Console.WriteLine($"Params: {{path}} {{regex {true}|{false}}} {{base color count}} {{mass type {BlockMassType.Invidual}|{BlockMassType.Mass}}} ");
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

					rawInput = rawInput.Replace("Tomaszon Simplistic Pack", "Tomaszon_Simplistic_Pack");
					string[] input = rawInput.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

					string fileNames = input[0].Replace("Tomaszon_Simplistic_Pack", "Tomaszon Simplistic Pack");
					bool isRegex = bool.Parse(input[1]);

					if (isRegex)
					{
						DirectoryInfo dir = Directory.GetParent(fileNames);
						FileInfo[] files = dir.GetFiles();
						Regex regex = new Regex(Path.GetFileName(fileNames));
						List<FileInfo> filteredFiles = files.Where(f => regex.IsMatch(f.FullName)).ToList();
						filteredFiles.ForEach(f => Process(f.FullName, input[2], input[3]));
					}
					else
					{
						Process(fileNames, input[2], input[3]);
					}
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

		public static void Process(string fileName, string baseColorCountS, string massTypeS)
		{
			try
			{
				int baseColorCount = int.Parse(baseColorCountS);
				BlockMassType massType = (BlockMassType)Enum.Parse(typeof(BlockMassType), massTypeS, true);

				Bitmap originalImage = null;
				using (Bitmap tmp = new Bitmap(fileName))
				{
					originalImage = new Bitmap(tmp);
				}

				Bitmap output = new Processor().ProcessImage(originalImage, baseColorCount, massType);

				output.Save(fileName);

				Console.WriteLine($"Processed: {Path.GetFileName(fileName)}");
				Console.WriteLine();
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
			}
		}
	}
	public enum Command
	{
		Exit
	}
}
