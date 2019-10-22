using System;
using System.Drawing;

namespace DrawingHelper
{
	internal class Program
	{
		private static void Main(string[] args)
		{
			Console.WriteLine("Params: {path}");
			string fileName = Console.ReadLine();

			Bitmap input = (Bitmap)Image.FromFile(fileName);
			try
			{
				Bitmap output = new Processor().ProcessImage(input);

				output.Save(@"C:\Users\toti9\Documents\GitHub\TomaszonSimplisticPack\DrawingHelper\DrawingHelper\bin\Debug\output.png");

				Console.WriteLine("Processed!");
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
			}

			Console.WriteLine("Press to exit");
			Console.ReadKey();
		}
	}
}
