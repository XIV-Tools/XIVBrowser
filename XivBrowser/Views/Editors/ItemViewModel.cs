// © XIV-Tools.
// Licensed under the MIT license.

namespace XivBrowser.Views.Editors
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using System.Windows.Media;
	using LuminaExtensions;
	using LuminaExtensions.Excel;
	using XivBrowser.Extensions;

	public class ItemViewModel : DocumentViewModel<ItemEx, ItemView>
	{
		public ImageSource? Icon { get; set; }
		public ItemSlots Slot { get; set; }

		protected override void OnFileChanged(ItemEx file)
		{
			base.OnFileChanged(file);

			if (this.File == null)
				return;

			this.Icon = this.File.IconFile?.ToImageSource();

			// TODO: if the item fits in more than one slot, let the user select
			// or default to something sensible
			this.Slot = this.File.FitsInSlots;
		}
	}
}
