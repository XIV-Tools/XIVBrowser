// © XIV-Tools.
// Licensed under the MIT license.

namespace XivBrowser.Views.Editors
{
	using System;
	using System.ComponentModel;
	using System.Windows.Controls;
	using LuminaExtensions.Files;

	/// <summary>
	/// Interaction logic for MdlView.xaml.
	/// </summary>
	[DocumentEditor(typeof(MdlFile))]
	public partial class MdlView : UserControl, IDocumentEditor, INotifyPropertyChanged
	{
		public MdlView()
		{
			this.InitializeComponent();
			this.ContentArea.DataContext = this;
		}

		public event PropertyChangedEventHandler? PropertyChanged;

		public MdlFile? File { get; set; }

		public void SetDocument(Document document)
		{
			if (document.Data is MdlFile mdl)
			{
				this.File = mdl;
			}
			else
			{
				throw new Exception($"Attempt to use MdlView for wrong document type: {document.Data}");
			}
		}
	}
}
