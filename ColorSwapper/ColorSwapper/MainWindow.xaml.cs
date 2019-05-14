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
				Swapper.AddColor(ParseColor(fromText.Text), ParseColor(toText.Text));
				listBox.Items.Clear();
				Swapper.SwapClass.Colors.ForEach(t => listBox.Items.Add($"{t.Item1.A}, {t.Item1.R}, {t.Item1.G}, {t.Item1.B} => {t.Item2.A}, {t.Item2.R}, {t.Item2.G}, {t.Item2.B}"));

				fromText.Text = "";
				toText.Text = "";
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

		private void ButtonClear_Click(object sender, RoutedEventArgs e)
		{
			Swapper.Clear();
			listBox.Items.Clear();
		}

		private void FromText_TextChanged(object sender, TextChangedEventArgs e)
		{
			try
			{
				Color c = ParseColor(fromText.Text);
				fromRec.Fill = new System.Windows.Media.SolidColorBrush(new System.Windows.Media.Color() { A = c.A, R = c.R, G = c.G, B = c.B });
			}
			catch
			{
				fromRec.Fill = System.Windows.Media.Brushes.Transparent;
			}
		}

		private void ToText_TextChanged(object sender, TextChangedEventArgs e)
		{
			try
			{
				Color c = ParseColor(toText.Text);
				toRec.Fill = new System.Windows.Media.SolidColorBrush(new System.Windows.Media.Color() { A = c.A, R = c.R, G = c.G, B = c.B });
			}
			catch
			{
				toRec.Fill = System.Windows.Media.Brushes.Transparent;
			}
		}

		private Color ParseColor(string text)
		{
			int[] colorARGB = text.Split(',').Select(p => int.Parse(p)).ToArray();
			return Color.FromArgb(colorARGB[0], colorARGB[1], colorARGB[2], colorARGB[3]);
		}

		private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			try
			{
				var arr = listBox.SelectedValue.ToString().Split('>').Select(p => p.Trim('=')).ToArray();
				fromText.Text = arr[0];
				toText.Text = arr[1];
			}
			catch (Exception ex)
			{
				fromText.Text = "";
				toText.Text = "";
			}
		}
	}
}
