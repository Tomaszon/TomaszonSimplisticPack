using ModelContainer;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace TSPLauncher
{
	/// <summary>
	/// An empty page that can be used on its own or navigated to within a Frame.
	/// </summary>
	public sealed partial class MainPage : Page
	{
		public ModelContainer<ViewModel, Model> ModelContainer { get; set; } = new ModelContainer<ViewModel, Model>();

		public MainPage()
		{
			InitializeComponent();
		}

		private void Page_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
		{
			//ModelContainer.Model.InstalledVersions = new List<int> { 4, 5, 7, 8, 6 };
			//ModelContainer.ViewModel.A = "asd";
		}

		private void ContentFrame_NavigationFailed(object sender, Windows.UI.Xaml.Navigation.NavigationFailedEventArgs e)
		{
			throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
		}

		private void ContentFrame_Navigated(object sender, Windows.UI.Xaml.Navigation.NavigationEventArgs e)
		{

		}

		private void NavigationView_SelectionChanged(Microsoft.UI.Xaml.Controls.NavigationView sender, Microsoft.UI.Xaml.Controls.NavigationViewSelectionChangedEventArgs args)
		{
			if (args.IsSettingsSelected)
			{
				//ContentFrame.Navigate(typeof(SettingsPage))
			}
			else
			{
				var tag = (args.SelectedItem as Microsoft.UI.Xaml.Controls.NavigationViewItem)?.Tag?.ToString();
				if (Enum.TryParse(typeof(E.NavigationPages), tag, out var res))
				{
					switch (res)
					{
						case E.NavigationPages.AddVersion:
							ModelContainer.Model.A = "asdsadadasdasdasas";
							ModelContainer.ViewModel.InstalledVersions.Add(6);
							break;
						case E.NavigationPages.Home:
							ContentFrame.Navigate(typeof(HomePage));
							break;
						case E.NavigationPages.News:
							ContentFrame.Navigate(typeof(NewsPage));
							break;
						default:

							break;
					}
				}
			}
		}
	}

	public class Model : ModelBase
	{
		public string A { get { return Get<string>("Alma"); } set { Set(value); } }

		public List<int> InstalledVersions
		{
			get
			{
				return Get<List<int>>(new List<int>(new[] { 2, 3 }));
			}
			set
			{
				Set(value);
			}
		}
	}

	public class ViewModel : ViewModelBase
	{
		public string A { get { return Get<string>(); } set { Set(value); } }

		//TODO test different Property and Accessor name null exceptions
		public List<int> InstalledVersions
		{
			get
			{
				return Get<List<int>>();
			}
			set
			{
				Set(value);
			}
		}

		public ViewModel(Model model) : base(model) { }
	}
}

namespace E
{
	public enum NavigationPages
	{
		Home,
		News,
		AddVersion
	}

}
