// © XIV-Tools.
// Licensed under the MIT license.

namespace XIVBrowser.Views
{
	using System;
	using Lumina.Data.Files;
	using LuminaExtensions.Excel;
	using XIVBrowser.Services;
	using XivToolsWpf.ModelView;

	/// <summary>
	/// Interaction logic for ItemView.xaml.
	/// </summary>
	public partial class ItemView : View
	{
		public ItemView()
		{
			this.InitializeComponent();
		}

		public string? FilePath
		{
			get => this.GetValue<string?>();
			set => this.SetValue(value);
		}

		public ItemViewModel? Item
		{
			get => this.GetValue<ItemViewModel?>();
			set => this.SetValue(value);
		}

		public void SetItem(ItemViewModel item)
		{
			this.Item = item;
			this.FilePath = this.GetDirectory();

			string imcPath = this.GetImcDirectory();
			////ImcFile imc = LuminaService.Lumina.GetFile<ImcFile>(imcPath);
		}

		private string GetDirectory()
		{
			if (this.Item == null)
				throw new Exception("No Item in item view");

			ushort primaryId = this.Item.ModelBase;
			string primaryIdStr4 = primaryId.ToString().PadLeft(4, '0');
			string primaryIdStr6 = primaryId.ToString().PadLeft(6, '0');

			ushort secondaryId = 0000;
			string secondaryIdStr4 = secondaryId.ToString().PadLeft(4, '0');

			if (this.Item.IsEquipment)
				return $"chara/equipment/e{primaryIdStr4}/";

			if (this.Item.IsAccessory)
				return $"chara/accessory/a{primaryIdStr4}/";

			if (this.Item.IsWeapon)
				return $"chara/weapon/w{primaryIdStr4}/obj/body/b{secondaryIdStr4}/";

			throw new Exception("Unknown item type directory");

			// Monster - chara/monster/m0000/obj/body/b000/
			// Demihuman - chara/demihuman/d0000/obj/equipment/e0000/
			// UI / Paintings - ui/icon/000000/
			// Furnature - bgcommon/hou/indoor/general/0000/
			// Furnature - bgcommon/hou/outdoor/general/0000/
			// Human Body - chara/human/c0000/obj/body/b0000/
			// Human Face - chara/human/c0000/obj/face/f0000/
			// Human Tail - chara/human/c0000/obj/tail/t0000/
			// Human Hair - chara/human/c0000/obj/hair/h0000/
			// Human Ear - chara/human/c0000/obj/zear/z0000/
			// Decal Face Paint- chara/common/texture/decal_face/
			// Decal Equipment - chara/common/texture/decal_equip
		}

		private string GetImcDirectory()
		{
			if (this.Item == null)
				throw new Exception("No Item in item view");

			ushort primaryId = this.Item.ModelBase;
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

			throw new Exception("Unknown item type directory");
		}
	}
}
