using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ColorSwapper.Source;
using Microsoft.Win32;

namespace ColorSwapper
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		private Swapper Swapper { get; set; } = new Swapper();

		public MainWindow()
		{
			InitializeComponent();
		}

		private void ButtonOpenClick(object sender, RoutedEventArgs e)
		{
			OpenFileDialog dialog = new OpenFileDialog()
			{
				Multiselect = true,
				Filter = "PNG|*.png"
			};

			if (dialog.ShowDialog() == true)
			{
				Swapper.LoadBitmaps(dialog.FileNames);
			}
		}

		private void ButtonAddClick(object sender, RoutedEventArgs e)
		{
			try
			{
				int[] from = fromText.Text.Split(',').Select(p => int.Parse(p)).ToArray();
				int[] to = toText.Text.Split(',').Select(p => int.Parse(p)).ToArray();
				Swapper.AddColor(Color.FromArgb(from[0], from[1], from[2], from[3]), Color.FromArgb(to[0], to[1], to[2], to[3]));
			}
			catch (Exception ex)
			{

			}
		}

		private void Button_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				Swapper.Process();
			}
			catch (Exception ex)
			{

			}
		}
	}
}
