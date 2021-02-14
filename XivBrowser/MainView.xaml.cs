// © XIV-Tools.
// Licensed under the MIT license.

namespace XIVBrowser
{
	using System.Windows;
	using XivToolsWpf.ModelView;

	public partial class MainView : View
	{
		public MainView()
		{
			this.InitializeComponent();
		}

		private void OnFileScannerMenuClicked(object sender, RoutedEventArgs e)
		{
			new FileScanWindow().ShowDialog();
		}
	}
}
