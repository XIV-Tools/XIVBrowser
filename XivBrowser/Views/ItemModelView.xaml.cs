// © XIV-Tools.
// Licensed under the MIT license.

namespace XivBrowser.Views
{
	using System.Collections.Generic;
	using System.ComponentModel;
	using System.Windows;
	using System.Windows.Controls;
	using Lumina.Data.Files;
	using LuminaExtensions;
	using LuminaExtensions.Excel;
	using XIVBrowser.Services;
	using XivToolsWpf.DependencyProperties;

	/// <summary>
	/// Interaction logic for ItemModelView.xaml.
	/// </summary>
	public partial class ItemModelView : UserControl, INotifyPropertyChanged
	{
		public static IBind<ItemSlots> SlotsDp = Binder.Register<ItemSlots, ItemModelView>(nameof(Slot), OnSlotChanged, BindMode.OneWay);
		public static IBind<ItemModelBase?> ModelDp = Binder.Register<ItemModelBase?, ItemModelView>(nameof(Model), OnModelChanged, BindMode.OneWay);

		public ItemModelView()
		{
			this.InitializeComponent();
		}

		public event PropertyChangedEventHandler? PropertyChanged;

		public ImcFile? ImcFile { get; private set; }

		public ItemModelBase? Model
		{
			get => ModelDp.Get(this);
			set => ModelDp.Set(this, value);
		}

		public ItemSlots Slot
		{
			get => SlotsDp.Get(this);
			set => SlotsDp.Set(this, value);
		}

		private static void OnModelChanged(ItemModelView sender, ItemModelBase? value)
		{
			if (value == null)
			{
				sender.ImcFile = null;
				return;
			}

			sender.ImcFile = LuminaService.Lumina.GetFile<ImcFile>(value.ImcFilePath);

			sender.ImcView.File = sender.ImcFile;
			sender.ImcView.Slot = sender.Slot;
			sender.ImcView.Variant = value.ImcVariant;
			sender.ImcView.CanSelectImcData = sender.Slot == ItemSlots.None;
		}

		private static void OnSlotChanged(ItemModelView sender, ItemSlots value)
		{
			sender.ImcView.Slot = value;
			sender.ImcView.CanSelectImcData = value == ItemSlots.None;
		}

		/*private void Do()
		{
			ImcFile.ImageChangeData imageChangeData = this.ImcFile.GetVariant(this.Slot, this.Model.ImcVariant);

			foreach (ushort raceTribeId in RaceTribeExtensions.All)
			{
				// Why bother checking the eqdp file for racial support? we could just see if the material file exists within the file system.
				////EqdpFile file = LuminaService.Lumina.GetFile<EqdpFile>($"chara/xls/charadb/equipmentdeformerparameter/c{raceTribe}.eqdp");
				////bool f = file != null && file.IsSet(this.Item.ModelSet, slot);

				// TODO: get the materials from the mdl file.
				List<char> materialVariants = new List<char>() { 'a', 'b', 'c', 'd', 'e', 'f', 'g', };

				foreach (char materialVariant in materialVariants)
				{
					string materialPath = this.GetMaterialPath(imageChangeData.MaterialId, raceTribeId, materialVariant);
					MtrlFile? materialFile = LuminaService.Lumina.GetFile<MtrlFile>(materialPath);
				}
			}
		}

		private string GetModelPath(ushort raceTribeId)
		{
			if (this.Item == null)
				throw new Exception("No Item in item view");

			string raceTribeIdStr4 = raceTribeId.ToString().PadLeft(4, '0');
			string slotKey = this.Item.FitsInSlots.ToAbbreviation();

			if (this.Item.IsEquipment)
				return $"chara/equipment/e{this.Item.SetId}/model/c{raceTribeIdStr4}e{this.Item.SetId}_{slotKey}.mdl";
		}

		private string GetMaterialPath(byte materialId, ushort raceTribeId, char materialVariant)
		{
			if (this.Item == null)
				throw new Exception("No Item in item view");

			ushort primaryId = this.Item.ModelSet;

			string primaryIdStr4 = primaryId.ToString().PadLeft(4, '0');
			string raceTribeIdStr4 = raceTribeId.ToString().PadLeft(4, '0');
			string materialIdStr4 = (materialId - 1).ToString().PadLeft(4, '0');
			string slotKey = this.Item.FitsInSlots.ToAbbreviation();

			if (this.Item.IsEquipment)
				return $"chara/equipment/e{primaryIdStr4}/material/v{materialIdStr4}/mt_c{raceTribeIdStr4}e{primaryIdStr4}_{slotKey}_{materialVariant}.mtrl";

			if (this.Item.IsAccessory)
				return $"chara/accessory/a{primaryIdStr4}/a{primaryIdStr4}.imc";

			if (this.Item.IsWeapon)
			{
				throw new NotImplementedException();
				ushort secondaryId = 0000; // ??
				string secondaryIdStr4 = secondaryId.ToString().PadLeft(4, '0');
				return $"chara/weapon/w{primaryIdStr4}/obj/body/b{secondaryIdStr4}/b{secondaryIdStr4}.imc";
			}

			throw new NotSupportedException($"No material path generator for item type: {this.Item}");
		}*/
	}
}
