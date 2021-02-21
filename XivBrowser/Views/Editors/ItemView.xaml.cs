// © XIV-Tools.
// Licensed under the MIT license.

namespace XIVBrowser.Views
{
	using System.ComponentModel;
	using System.Windows.Controls;
	using System.Windows.Media;
	using LuminaExtensions;
	using LuminaExtensions.Excel;
	using XivBrowser;
	using XIVBrowser.Extensions;
	using XivBrowser.Views.Editors;

	[DocumentEditor(typeof(ItemEx))]
	public partial class ItemView : UserControl, IDocumentEditor, INotifyPropertyChanged
	{
		public ItemView()
		{
			this.InitializeComponent();
			this.DataContext = this;
		}

		public event PropertyChangedEventHandler? PropertyChanged;

		public ItemEx? Item { get; set; }
		public ImageSource? Icon { get; set; }
		public ItemSlots Slot { get; set; }

		public void SetDocument(Document document)
		{
			this.Item = document.Data as ItemEx;

			if (this.Item == null)
				return;

			this.Icon = this.Item.IconFile?.ToImageSource();

			// TODO: if the item fits in more than one slot, let the user select
			// or default to something sensible
			this.Slot = this.Item.FitsInSlots;
		}
	}
}
