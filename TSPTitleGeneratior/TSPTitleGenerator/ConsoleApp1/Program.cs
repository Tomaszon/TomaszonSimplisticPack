using System;

namespace TSPTitleGenerator
{
	internal class Program
	{
		private struct TitleInfos
		{
			public WorldType WorldType { get; set; }

			public TitleType TitleType { get; set; }

			public string Name { get; set; }

			public int Variants { get; set; }

			public bool CTMSupport { get; set; }

			public bool CustomModel { get; set; }
		}

		private static void Main(string[] args)
		{
			while (true)
			{
				try
				{
					TitleInfos titleInfos = new TitleInfos
					{
						WorldType = ReadValue($"{nameof(WorldType)}: {string.Join(" | ", Enum.GetNames(typeof(WorldType)))}",
						() => (WorldType)Enum.Parse(typeof(WorldType), Console.ReadLine(), true)),

						TitleType = ReadValue($"{nameof(TitleType)}: {string.Join(" | ", Enum.GetNames(typeof(TitleType)))}",
						() => (TitleType)Enum.Parse(typeof(TitleType), Console.ReadLine(), true))
					};

					if (titleInfos.TitleType == TitleType.Block)
					{
						titleInfos.Variants = ReadValue("Variants: int", () => int.Parse(Console.ReadLine()));
					}

					titleInfos.Name = ReadValue("Name: string", () => Console.ReadLine());

					if (titleInfos.TitleType == TitleType.Block)
					{
						titleInfos.CustomModel = ReadValue("Custom model: bool", () => bool.Parse(Console.ReadLine()));

						titleInfos.CTMSupport = ReadValue("CTM support: bool", () => bool.Parse(Console.ReadLine()));
					}

					new Generator().Generate(titleInfos.WorldType, titleInfos.TitleType, titleInfos.Name, titleInfos.Variants, titleInfos.CustomModel, titleInfos.CTMSupport, ProgressHandler);
				}
				catch (Exception ex)
				{
					Console.WriteLine(ex.Message);
				}

				while (true)
				{
					Console.WriteLine("Generate new? : y | n");
					string c = Console.ReadLine();

					if(c == "y")
					{
						break;
					}
					else if(c == "n")
					{
						return;
					}
				}
			}
		}

		public static T ReadValue<T>(string text, Func<T> func)
		{
			while (true)
			{
				try
				{
					Console.WriteLine(text);

					return func.Invoke();
				}
				catch { }
			}
		}

		public static void ProgressHandler(string message)
		{
			Console.WriteLine($"\n{message}\n");
		}
	}
}
