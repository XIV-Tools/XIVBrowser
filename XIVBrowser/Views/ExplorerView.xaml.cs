// © XIV-Tools.
// Licensed under the MIT license.

namespace XIVBrowser.Views
{
	using System.Collections.Generic;
	using System.Collections.ObjectModel;
	using System.ComponentModel;
	using System.Windows.Controls;

	/// <summary>
	/// Interaction logic for ExplorerView.xaml.
	/// </summary>
	public partial class ExplorerView : UserControl, INotifyPropertyChanged
	{
		public ExplorerView()
		{
			this.InitializeComponent();

			this.Refresh();
		}

		public event PropertyChangedEventHandler? PropertyChanged;

		public ObservableCollection<FileService.SqFileInfo>? Files { get; set; }
		public int FilesWithPaths { get; set; }
		public int TotalFiles { get; set; }

		public void Refresh()
		{
			ObservableCollection<FileService.SqFileInfo> collection = new ObservableCollection<FileService.SqFileInfo>();
			foreach (FileService.SqFileInfo file in FileService.FileLookupPath.Values)
			{
				if (file.Path == null)
					continue;

				collection.Add(file);
			}

			this.Files = collection;
			this.FilesWithPaths = this.Files.Count;
			this.TotalFiles = FileService.FileLookupHash.Count;
		}
	}
}
