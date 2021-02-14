// © XIV-Tools.
// Licensed under the MIT license.

namespace XIVBrowser
{
	using System.Windows;

	/// <summary>
	/// Interaction logic for MainWindow.xaml.
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			this.InitializeComponent();
		}

		private void OnFileScannerMenuClicked(object sender, RoutedEventArgs e)
		{
			new FileScanWindow().ShowDialog();
		}
	}
}
