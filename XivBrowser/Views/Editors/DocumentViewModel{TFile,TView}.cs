// © XIV-Tools.
// Licensed under the MIT license.

namespace XivBrowser.Views.Editors
{
	using System;
	using System.Windows;
	using System.Windows.Controls;
	using XivToolsWpf.DependencyProperties;

	public abstract class DocumentViewModel<TFile, TView> : DocumentViewModel
		where TView : UserControl
	{
		private TFile? file;

		public TFile? File
		{
			get => this.file;
			set
			{
				this.file = value;
				this.OnFileChanged(value);
			}
		}

		public Document? Document { get; set; }
		public TView? View { get; private set; }

		public override sealed bool CanEditFile(Type fileType)
		{
			return fileType == typeof(TFile);
		}

		public override sealed UserControl CreateView(Document document)
		{
			if (document.Data is TFile file)
			{
				this.Document = document;
				this.File = file;
			}
			else
			{
				throw new Exception($"Incoorect document file type: {document.Data} for view model file type: {typeof(TFile)}");
			}

			this.View = Activator.CreateInstance<TView>();
			this.View.DataContext = this;
			this.View.Loaded += this.OnViewLoaded;
			return this.View;
		}

		protected virtual void OnFileChanged(TFile? file)
		{
		}

		protected virtual void OnViewLoaded()
		{
		}

		private void OnViewLoaded(object sender, RoutedEventArgs e)
		{
			this.OnViewLoaded();
		}
	}
}
