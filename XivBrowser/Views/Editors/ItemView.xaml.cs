// © XIV-Tools.
// Licensed under the MIT license.

namespace XIVBrowser.Views
{
	using System;
	using System.Collections.Generic;
	using System.Collections.ObjectModel;
	using System.Windows.Media;
	using Lumina.Data.Files;
	using LuminaExtensions.Excel;
	using LuminaExtensions.Files;
	using XivBrowser;
	using XIVBrowser.Extensions;
	using XIVBrowser.Services;
	using XivBrowser.Views.Editors;
	using XivToolsWpf.ModelView;

	[DocumentEditor(typeof(ItemViewModel))]
	public partial class ItemView : View, IDocumentEditor
	{
		public ItemView()
		{
			this.InitializeComponent();
			this.MaterialPaths = new ObservableCollection<FileService.SqFileInfo>();
		}

		public ItemViewModel? Item
		{
			get => this.GetValue<ItemViewModel?>();
			set => this.SetValue(value);
		}

		public ImageSource? Icon
		{
			get => this.GetValue<ImageSource?>();
			set => this.SetValue(value);
		}

		public ObservableCollection<FileService.SqFileInfo> MaterialPaths
		{
			get => this.GetValue<ObservableCollection<FileService.SqFileInfo>>();
			set => this.SetValue(value);
		}

		public void SetDocument(Document document)
		{
			this.Item = document.Data as ItemViewModel;

			this.Icon = this.Item?.Icon.ToImageSource();

			string imcFilePath = this.GetImcPath();
			ImcFile? imc = LuminaService.Lumina.GetFile<ImcFile>(imcFilePath);

			if (imc != null)
				this.ImcView.File = imc;

			this.ImcView.Visibility = imc != null ? System.Windows.Visibility.Visible : System.Windows.Visibility.Collapsed;

			IEnumerable<FileService.SqFileInfo>? childFiles = FileService.Search(this.GetDirectory());
			foreach (FileService.SqFileInfo file in childFiles)
			{
				this.MaterialPaths.Add(file);
			}
		}

		private string GetImcPath()
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
	}
}
