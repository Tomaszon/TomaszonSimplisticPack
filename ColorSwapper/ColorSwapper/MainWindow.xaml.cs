using ColorSwapper.Source;
using Microsoft.Win32;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

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
			Swapper.AddColor(Color.FromArgb(255, 245, 204, 39), Color.Black);
			listColors.ItemsSource = Swapper.EntryCollection.Colors;
			listImages.ItemsSource = Swapper.Bitmaps;
			listImages.DisplayMemberPath = nameof(BitmapEntry.Name);
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

				//fromText.Text = "";
				//toText.Text = "";
			}
			catch (Exception ex)
			{

			}
		}

		private void ButtonProcess_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				Swapper.Process();
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
		}

		private void ButtonClear_Click(object sender, RoutedEventArgs e)
		{
			Swapper.Clear();
		}

		private void FromText_TextChanged(object sender, TextChangedEventArgs e)
		{
			try
			{
				Color c = ParseColor(fromText.Text);
				//fromRec.Fill = new System.Windows.Media.SolidColorBrush(new System.Windows.Media.Color() { A = c.A, R = c.R, G = c.G, B = c.B });
			}
			catch
			{
				//fromRec.Fill = System.Windows.Media.Brushes.Transparent;
			}
		}

		private void ToText_TextChanged(object sender, TextChangedEventArgs e)
		{
			try
			{
				Color c = ParseColor(toText.Text);
				//toRec.Fill = new System.Windows.Media.SolidColorBrush(new System.Windows.Media.Color() { A = c.A, R = c.R, G = c.G, B = c.B });
			}
			catch
			{
				//toRec.Fill = System.Windows.Media.Brushes.Transparent;
			}
		}

		private Color ParseColor(string text)
		{
			int[] colorARGB = text.Split(',').Select(p => int.Parse(p)).ToArray();
			return Color.FromArgb(colorARGB[0], colorARGB[1], colorARGB[2], colorARGB[3]);
		}

		private void ListColors_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			try
			{
				//var arr = listBox.SelectedValue.ToString().Split('>').Select(p => p.Trim('=')).ToArray();
				//fromText.Text = arr[0];
				//toText.Text = arr[1];
			}
			catch (Exception ex)
			{
				//fromText.Text = "";
				//toText.Text = "";
			}
		}

		private void ListImages_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			try
			{
				imgFrom.Source = Convert(Swapper.Bitmaps.FirstOrDefault(t => t == listImages.SelectedValue).Original);
				imgTo.Source = Convert(Swapper.Bitmaps.FirstOrDefault(t => t == listImages.SelectedValue).Modified);
				//fromText.Text = arr[0];
				//toText.Text = arr[1];
			}
			catch (Exception ex)
			{
				//fromText.Text = "";
				//toText.Text = "";
			}
		}

		private void Save_Click(object sender, RoutedEventArgs e)
		{
			Swapper.Save(false);
		}

		private System.Windows.Media.ImageSource Convert(System.Drawing.Image image)
		{
			using (MemoryStream ms = new MemoryStream())
			{
				image.Save(ms, ImageFormat.Bmp);
				ms.Seek(0, SeekOrigin.Begin);

				System.Windows.Media.Imaging.BitmapImage bitmapImage = new System.Windows.Media.Imaging.BitmapImage();
				bitmapImage.BeginInit();
				bitmapImage.CacheOption = System.Windows.Media.Imaging.BitmapCacheOption.OnLoad;
				bitmapImage.StreamSource = ms;
				bitmapImage.EndInit();

				return bitmapImage;
			}
		}

		private void ButtonRestore_Click(object sender, RoutedEventArgs e)
		{
			Swapper.Save(true);
		}
	}
}
