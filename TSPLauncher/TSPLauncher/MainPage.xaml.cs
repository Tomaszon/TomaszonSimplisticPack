using Microsoft.UI.Xaml.Controls;
using ModelContainer;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace TSPLauncher
{
	/// <summary>
	/// An empty page that can be used on its own or navigated to within a Frame.
	/// </summary>
	public sealed partial class MainPage : Windows.UI.Xaml.Controls.Page
	{
		public ModelContainer<ViewModel, Model> ModelContainer { get; set; } = new ModelContainer<ViewModel, Model>();

		public MainPage()
		{
			InitializeComponent();
		}

		private void Page_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
		{
			//ModelContainer.Model.InstalledVersions = new ObservableCollection<int> { 4, 5, 7, 8, 6 };
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
				var item = args.SelectedItem as NavigationViewItem;
				var tag = item?.Tag?.ToString();
				if (Enum.TryParse(typeof(NavigationPages), tag, out var res))
				{
					switch (res)
					{
						case NavigationPages.AddVersion:
							ModelContainer.Model.InstalledVersions.Add(6);
							break;
						case NavigationPages.Home:
							ContentFrame.Navigate(typeof(HomePage));
							break;
						case NavigationPages.News:
							ContentFrame.Navigate(typeof(NewsPage));
							break;
						default:

							break;
					}
				}
			}
		}
	}

	public enum NavigationPages
	{
		Home,
		News,
		AddVersion
	}

	public class Model : ModelBase
	{
		public string A { get { return Get("Alma"); } set { Set(value); } }

		public List<int> InstalledVersions
		{
			get { return Get(new List<int> { 2, 3 }); }
		}
	}


	public class ViewModel : ViewModelBase
	{
		public ObservableCollection<NavigationViewItem> InstalledVersionsMenuItems
		{
			get
			{
				return Get<ObservableCollection<int>, ObservableCollection<NavigationViewItem>>(transform: x =>
					new ObservableCollection<NavigationViewItem>(x.Select(e =>
						new NavigationViewItem()
						{
							Content = e,
							Icon = new Windows.UI.Xaml.Controls.SymbolIcon(Windows.UI.Xaml.Controls.Symbol.Page2)
						})), nameof(Model.InstalledVersions));
			}
		}

		public ViewModel(Model model) : base(model) { }
	}
}