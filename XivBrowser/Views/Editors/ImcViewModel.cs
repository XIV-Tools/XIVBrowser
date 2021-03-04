// © XIV-Tools.
// Licensed under the MIT license.

namespace XivBrowser.Views.Editors
{
	using System.Collections.Generic;
	using System.Windows;
	using Lumina.Data.Files;
	using LuminaExtensions;

	public class ImcViewModel : DocumentViewModel<ImcFile, ImcView>
	{
		private ItemSlots slot;
		private ushort variant;

		public int VariantCount { get; set; }
		public bool CanSelectImcData { get; set; } = true;
		public string MaterialKey { get; set; } = string.Empty;

		public List<ItemSlots> ValidSlots { get; } = new ()
		{
			ItemSlots.Head,
			ItemSlots.Body,
			ItemSlots.Hands,
			ItemSlots.Legs,
			ItemSlots.Feet,
			ItemSlots.Ears,
			ItemSlots.Neck,
			ItemSlots.Wrists,
			ItemSlots.RightRing,
			ItemSlots.LeftRing,

			ItemSlots.Weapons,
		};

		public ItemSlots Slot
		{
			get => this.slot;
			set
			{
				this.slot = value;
				this.UpdateVariantCount();
				this.UpdateMaterialId();
			}
		}

		public ushort Variant
		{
			get => this.variant;
			set
			{
				this.variant = value;
				this.UpdateMaterialId();
			}
		}

		protected override void OnViewLoaded()
		{
			base.OnViewLoaded();

			if (this.View == null)
				return;

			ItemModelView? itemModelView = this.View.FindParent<ItemModelView>();

			if (itemModelView == null || itemModelView.Model == null)
				return;

			this.Slot = itemModelView.Slot;
			this.Variant = itemModelView.Model.ImcVariant;
			this.CanSelectImcData = false;
		}

		private void UpdateVariantCount()
		{
			if (this.File == null)
				return;

			if (!this.ValidSlots.Contains(this.Slot))
			{
				// Invalid slot for IMC
				return;
			}

			ImcFile.ImageChangeParts part = this.File.GetPart(this.slot);
			this.VariantCount = part.Variants.Length;
		}

		private void UpdateMaterialId()
		{
			this.MaterialKey = string.Empty;

			if (this.File == null)
				return;

			if (this.variant == 0)
				return;

			if (!this.ValidSlots.Contains(this.Slot))
			{
				// Invalid slot for IMC
				return;
			}

			ImcFile.ImageChangeData imageChangeData;
			if (this.File.TryGetVariant(this.Slot, this.Variant, out imageChangeData))
			{
				this.MaterialKey = imageChangeData.GetMaterialKey();
			}
			else
			{
				this.MaterialKey = string.Empty;
			}
		}
	}
}
