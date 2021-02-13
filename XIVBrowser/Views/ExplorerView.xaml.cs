// © XIV-Tools.
// Licensed under the MIT license.

namespace XIVBrowser.Views
{
	using System.Collections.Generic;
	using System.Collections.ObjectModel;
	using System.Windows;
	using Wpf.Mv;

	/// <summary>
	/// Interaction logic for ExplorerView.xaml.
	/// </summary>
	public partial class ExplorerView : View
	{
		public ExplorerView()
		{
			this.InitializeComponent();
		}

		public ObservableCollection<FileService.SqFileInfo> Files
		{
			get => (ObservableCollection<FileService.SqFileInfo>)this.GetValue();
			set => this.SetValue(value);
		}

		public int FilesWithPaths
		{
			get => (int)this.GetValue();
			set => this.SetValue(value);
		}

		public int TotalFiles
		{
			get => (int)this.GetValue();
			set => this.SetValue(value);
		}

		private void OnScanClicked(object sender, RoutedEventArgs e)
		{
			FileScanWindow window = new FileScanWindow();
			window.Show();
		}

		/*public async void Refresh()
		{
			await FileService.ScanAsync(false, p);

			ObservableCollection<FileService.SqFileInfo> collection = new ObservableCollection<FileService.SqFileInfo>();
			foreach (KeyValuePair<ulong, FileService.SqFileInfo> keyVal in FileService.FileLookup)
			{
				if (keyVal.Value.Path == null)
					continue;

				collection.Add(keyVal.Value);
			}

			this.Files = collection;
			this.FilesWithPaths = this.Files.Count;
			this.TotalFiles = FileService.FileLookup.Count;
		}*/
	}
}
