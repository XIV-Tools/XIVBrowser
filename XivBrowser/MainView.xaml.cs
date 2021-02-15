// © XIV-Tools.
// Licensed under the MIT license.

namespace XIVBrowser
{
	using System.Windows;
	using System.Windows.Controls;

	public partial class MainView : UserControl
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
