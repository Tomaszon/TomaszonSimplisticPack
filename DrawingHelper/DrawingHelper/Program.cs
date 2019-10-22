using System;
using System.Drawing;

namespace DrawingHelper
{
	internal class Program
	{
		private static void Main(string[] args)
		{
			string fileName = "test.png";//Console.ReadLine();

			Bitmap input = (Bitmap)Bitmap.FromFile(fileName);

			Bitmap output = Processor.ProcessImage(input);

			output.Save(@"C:\Users\toti9\Documents\GitHub\TomaszonSimplisticPack\DrawingHelper\DrawingHelper\bin\Debug\output.png");

			//Console.ReadKey();
		}
	}
}
