// © XIV-Tools.
// Licensed under the MIT license.

namespace XIVBrowser.Views
{
	using System;
	using System.ComponentModel;
	using System.Windows.Controls;
	using System.Windows.Media;
	using Lumina.Data.Files;
	using LuminaExtensions;
	using LuminaExtensions.Excel;
	using LuminaExtensions.Files;
	using XivBrowser;
	using XIVBrowser.Extensions;
	using XIVBrowser.Services;
	using XivBrowser.Views.Editors;

	[DocumentEditor(typeof(ItemViewModel))]
	public partial class ItemView : UserControl, IDocumentEditor, INotifyPropertyChanged
	{
		public ItemView()
		{
			this.InitializeComponent();
			this.DataContext = this;
		}

		public event PropertyChangedEventHandler? PropertyChanged;

		public ItemViewModel? Item { get; set; }
		public ImageSource? Icon { get; set; }

		public void SetDocument(Document document)
		{
			this.Item = document.Data as ItemViewModel;

			if (this.Item == null)
				return;

			this.Icon = this.Item.Icon.ToImageSource();

			// TODO: if the item fits in more than one slot, let the user select
			// or default to something sensible
			ItemSlots slot = this.Item.FitsInSlots;

			string imcFilePath = this.GetImcPath();
			ImcFile? imc = LuminaService.Lumina.GetFile<ImcFile>(imcFilePath);
			this.ImcView.Visibility = imc != null ? System.Windows.Visibility.Visible : System.Windows.Visibility.Collapsed;
			if (imc != null)
			{
				this.ImcView.File = imc;
				this.ImcView.Slot = slot;
				this.ImcView.Variant = this.Item.ModelVariant;
				this.ImcView.CanSelectImcData = false;

				ImcFile.ImageChangeData imageChangeData = imc.GetVariant(slot, this.Item.ModelVariant);
				string materialPath = this.GetMaterialPath(imageChangeData.MaterialId);

				// Why bother checking the eqdp file for racial support? we could just see if the file exists within the file system
				// and use that?
				// Since we have to get all the eqdp files by race anyway...
				EqdpFile file = LuminaService.Lumina.GetFile<EqdpFile>("chara/xls/charadb/equipmentdeformerparameter/c0101.eqdp");
				bool f = file.IsSet(this.Item.ModelSet, slot);
			}
		}

		private string GetImcPath()
		{
			if (this.Item == null)
				throw new Exception("No Item in item view");

			ushort primaryId = this.Item.ModelSet;
			string primaryIdStr4 = primaryId.ToString().PadLeft(4, '0');
			string primaryIdStr6 = primaryId.ToString().PadLeft(6, '0');

			ushort secondaryId = 0000;
			string secondaryIdStr4 = secondaryId.ToString().PadLeft(4, '0');

			if (this.Item.IsEquipment)
				return $"chara/equipment/e{primaryIdStr4}/e{primaryIdStr4}.imc";

			if (this.Item.IsAccessory)
				return $"chara/accessory/a{primaryIdStr4}/a{primaryIdStr4}.imc";

			if (this.Item.IsWeapon)
				return $"chara/weapon/w{primaryIdStr4}/obj/body/b{secondaryIdStr4}/b{secondaryIdStr4}.imc";

			throw new NotSupportedException($"No IMC path generator for item type: {this.Item}");
		}

		private string GetMaterialPath(byte materialId)
		{
			if (this.Item == null)
				throw new Exception("No Item in item view");

			ushort primaryId = this.Item.ModelSet;
			string primaryIdStr4 = primaryId.ToString().PadLeft(4, '0');
			string primaryIdStr6 = primaryId.ToString().PadLeft(6, '0');

			string materialIsStr4 = (materialId - 1).ToString().PadLeft(4, '0');

			string slotKey = this.Item.FitsInSlots.ToAbbreviation();
			string materialVariant = "a";

			if (this.Item.IsEquipment)
				return $"chara/equipment/e{primaryIdStr4}/material/v{materialIsStr4}/mt_c0101e{primaryIdStr4}_{slotKey}_{materialVariant}.mtrl";

			if (this.Item.IsAccessory)
				return $"chara/accessory/a{primaryIdStr4}/a{primaryIdStr4}.imc";

			if (this.Item.IsWeapon)
			{
				throw new NotImplementedException();
				ushort secondaryId = 0000;
				string secondaryIdStr4 = secondaryId.ToString().PadLeft(4, '0');
				return $"chara/weapon/w{primaryIdStr4}/obj/body/b{secondaryIdStr4}/b{secondaryIdStr4}.imc";
			}

			throw new NotSupportedException($"No material path generator for item type: {this.Item}");
		}
	}
}
