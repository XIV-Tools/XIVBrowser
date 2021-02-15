// © XIV-Tools.
// Licensed under the MIT license.

namespace XivBrowser.Views.Editors
{
	using System;
	using System.Windows.Controls;
	using Lumina.Data.Files;
	using XivToolsWpf.ModelView;

	/// <summary>
	/// Interaction logic for ImcView.xaml.
	/// </summary>
	[DocumentEditor(typeof(ImcFile))]
	public partial class ImcView : View, IDocumentEditor
	{
		public ImcView()
		{
			this.InitializeComponent();
		}

		public ushort VariantCount
		{
			get => this.GetValue<ushort>();
			set => this.SetValue(value);
		}

		public ImcFile File
		{
			get => this.GetValue<ImcFile>();
			set
			{
				this.SetValue(value);
				this.VariantCount = value.Count;
			}
		}

		public void SetDocument(Document document)
		{
			if (document.Data is ImcFile imc)
			{
				this.File = imc;
			}
			else
			{
				throw new Exception($"Attempt to use ImcEditor for wrong document type: {document.Data}");
			}
		}
	}
}
