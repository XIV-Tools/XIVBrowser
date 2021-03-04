// © XIV-Tools.
// Licensed under the MIT license.

namespace XIVBrowser
{
	using System.Windows;
	using XivToolsWpf;
	using XivToolsWpf.Windows;

	/// <summary>
	/// Interaction logic for App.xaml.
	/// </summary>
	public partial class App : Application
	{
		protected override void OnStartup(StartupEventArgs e)
		{
			Themes.Apply(Brightness.Dark, Colors.DeepOrange);

			LogService.Create();

			this.MainWindow = StyledWindow.Show<MainView>();
			this.MainWindow.SizeToContent = SizeToContent.Manual;
			this.MainWindow.Width = 1024;
			this.MainWindow.Height = 720;

			base.OnStartup(e);
		}
	}
}
