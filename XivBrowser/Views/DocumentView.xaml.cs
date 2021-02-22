// © XIV-Tools.
// Licensed under the MIT license.

namespace XivBrowser.Views
{
	using System.ComponentModel;
	using System.IO;
	using System.Windows.Controls;
	using Lumina.Data;
	using XIVBrowser.Services;
	using XivBrowser.Views.Editors;
	using XivToolsWpf.DependencyProperties;

	/// <summary>
	/// Interaction logic for DocumentView.xaml.
	/// </summary>
	public partial class DocumentView : UserControl, INotifyPropertyChanged
	{
		public static IBind<string?> FilePathDp = Binder.Register<string?, DocumentView>(nameof(FilePath), OnPathChanged, BindMode.OneWay);
		public static IBind<object?> FileDp = Binder.Register<object?, DocumentView>(nameof(File), OnFileChanged, BindMode.TwoWay);
		public static IBind<HeaderModes> HeaderDp = Binder.Register<HeaderModes, DocumentView>(nameof(Header), OnHeaderChanged, BindMode.TwoWay);

		public DocumentView()
		{
			this.InitializeComponent();
			this.ContentArea.DataContext = this;
		}

		public event PropertyChangedEventHandler? PropertyChanged;

		public enum HeaderModes
		{
			None,
			FileName,
			Path,
		}

		public object? File
		{
			get => FileDp.Get(this);
			set => FileDp.Set(this, value);
		}

		public string? FilePath
		{
			get => FilePathDp.Get(this);
			set => FilePathDp.Set(this, value);
		}

		public HeaderModes Header
		{
			get => HeaderDp.Get(this);
			set => HeaderDp.Set(this, value);
		}

		public string? HeaderText { get; set; }
		public UserControl? FileContent { get; set; }
		public string? Error { get; set; }

		private static void OnFileChanged(DocumentView sender, object? value)
		{
			if (value == null)
			{
				sender.FileContent = null;
				return;
			}

			DocumentViewModel? vm = DocumentViewModel.GetViewModelForFile(value.GetType());

			if (vm == null)
			{
				sender.Error = $"View model for type not found: {value.GetType()}";
				return;
			}

			string? name = Path.GetFileNameWithoutExtension(sender.FilePath);
			Document doc = new Document(name ?? string.Empty, value);
			doc.Path = sender.FilePath;
			sender.FileContent = vm.CreateView(doc);

			sender.Error = null;
		}

		private static void OnPathChanged(DocumentView sender, string? value)
		{
			sender.File = null;
			OnHeaderChanged(sender, sender.Header);

			if (value == null)
				return;

			FileResource? file = LuminaService.GetFile(value);
			sender.File = file;

			if (file == null)
			{
				sender.Error = $"File not found: {value}";
			}
		}

		private static void OnHeaderChanged(DocumentView sender, HeaderModes mode)
		{
			if (mode == HeaderModes.None)
			{
				sender.HeaderText = null;
			}
			else if (mode == HeaderModes.FileName)
			{
				sender.HeaderText = Path.GetFileName(sender.FilePath);
			}
			else
			{
				sender.HeaderText = sender.FilePath;
			}
		}
	}
}
