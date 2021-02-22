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
	using System.Windows;
	using System;
	using Microsoft.Win32;
	using System.Collections.Generic;
	using LuminaExtensions.Converters;
	using System.Text;

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
			Path,
			FileName,
			None,
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

		public Document? Document { get; set; }
		public string? HeaderText { get; set; }
		public UserControl? FileContent { get; set; }
		public string? Error { get; set; }

		public void OpenDocument(Document doc)
		{
			DocumentViewModel? vm = DocumentViewModel.GetViewModelForFile(doc.Data.GetType());

			if (vm == null)
			{
				this.Error = $"View model for type not found: {doc.Data.GetType()}";
				return;
			}

			this.Document = doc;
			this.FileContent = vm.CreateView(doc);
			this.Error = null;

			OnHeaderChanged(this, this.Header);
		}

		private static void OnFileChanged(DocumentView sender, object? value)
		{
			if (value == null)
			{
				sender.FileContent = null;
				return;
			}

			string? name = Path.GetFileNameWithoutExtension(sender.FilePath);
			Document doc = new Document(name ?? string.Empty, value);
			doc.Path = sender.FilePath;

			sender.OpenDocument(doc);
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
			if (mode == HeaderModes.None || sender.Document == null)
			{
				sender.HeaderText = null;
			}
			else if (mode == HeaderModes.FileName)
			{
				sender.HeaderText = sender.Document.Name;
			}
			else
			{
				sender.HeaderText = sender.Document.Path;
			}
		}

		private void OnExportClicked(object sender, RoutedEventArgs e)
		{
			if (this.File == null)
				return;

			if (this.File is FileResource fileResource)
			{
				ConverterBase[] converters = fileResource.GetConverters();

				StringBuilder filterBuilder = new StringBuilder();

				// First filter is always unconverted original file
				filterBuilder.Append(this.File.GetType().Name);
				filterBuilder.Append("|*");
				filterBuilder.Append(Path.GetExtension(this.FilePath));

				foreach (ConverterBase converter in converters)
				{
					filterBuilder.Append("|");
					filterBuilder.Append(converter.Name);
					filterBuilder.Append("|*.");
					filterBuilder.Append(converter.FileExtension);
				}

				SaveFileDialog dlg = new SaveFileDialog();
				dlg.FileName = Path.GetFileName(this.FilePath);
				dlg.Filter = filterBuilder.ToString();

				if (dlg.ShowDialog() != true || dlg.FileName == null)
					return;

				// FilterIndex starts at 1. Thanks Windows.
				if (dlg.FilterIndex == 1)
				{
					fileResource.SaveFile(dlg.FileName);
				}
				else
				{
					int converterIndex = dlg.FilterIndex - 2;
					ConverterBase converter = converters[converterIndex];
					fileResource.ConvertFile(converter, dlg.FileName);
				}
			}
			else
			{
				throw new NotImplementedException($"Unsupported file type for export: {this.File}");
			}
		}
	}
}
