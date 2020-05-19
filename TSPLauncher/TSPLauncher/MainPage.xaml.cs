using ModelContainer;
using System;
using System.Collections;
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
				var item = args.SelectedItem as Microsoft.UI.Xaml.Controls.NavigationViewItem;
				var tag = item?.Tag?.ToString();
				if (Enum.TryParse(typeof(NavigationPages), tag, out var res))
				{
					switch (res)
					{
						case NavigationPages.AddVersion:
							ModelContainer.Model.A = "asdsadadasdasdasas";
							ModelContainer.Model.InstalledVersions.Add(6);
							break;
						case NavigationPages.Home:
							ContentFrame.Navigate(typeof(HomePage));
							break;
						case NavigationPages.News:
							ContentFrame.Navigate(typeof(NewsPage));
							break;
						case NavigationPages.InstalledVersions:
							navigationView.SelectedItem = (item.MenuItemsSource as IList)?[0];
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
		AddVersion,
		InstalledVersions
	}

	public class Model : ModelBase
	{
		public string A { get { return Get<string>("Alma"); } set { Set(value); } }

		public ObservableCollection<int> InstalledVersions
		{
			get
			{
				return Get(new ObservableCollection<int> { 2, 3 });
			}
			set
			{
				Set(value);
			}
		}
	}

	public class ViewModel : ViewModelBase
	{
		public string AView { get { return Get<string>("A"); } set { Set(value); } }

		public string AMView { get { return Get<string>("A", x => x + " Ft"); } }

		//TODO test different Property and Accessor name null exceptions
		public ObservableCollection<int> InstalledVersions
		{
			get
			{
				return Get<ObservableCollection<int>>();
			}
			set
			{
				Set(value);
			}
		}

		public ViewModel(Model model) : base(model) { }
	}
}