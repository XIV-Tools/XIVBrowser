// © XIV-Tools.
// Licensed under the MIT license.

namespace XIVBrowser.Views
{
	using System.Collections.Generic;
	using System.Collections.ObjectModel;
	using XivToolsWpf.ModelView;

	/// <summary>
	/// Interaction logic for ExplorerView.xaml.
	/// </summary>
	public partial class ExplorerView : View
	{
		public ExplorerView()
		{
			this.InitializeComponent();

			this.Refresh();
		}

		public ObservableCollection<FileService.SqFileInfo>? Files
		{
			get => this.GetValue<ObservableCollection<FileService.SqFileInfo>?>();
			set => this.SetValue(value);
		}

		public int FilesWithPaths
		{
			get => this.GetValue<int>();
			set => this.SetValue(value);
		}

		public int TotalFiles
		{
			get => this.GetValue<int>();
			set => this.SetValue(value);
		}

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
