// © XIV-Tools.
// Licensed under the MIT license.

namespace XivBrowser.Views.Editors
{
	using System.Collections.Generic;
	using Lumina.Data.Files;
	using LuminaExtensions;

	public class ImcViewModel : DocumentViewModel<ImcFile, ImcView>
	{
		private ItemSlots slot;
		private ushort variant;

		public ushort VariantCount { get; set; }
		public bool CanSelectImcData { get; set; } = true;
		public byte MaterialId { get; set; }

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
		};

		public ItemSlots Slot
		{
			get => this.slot;
			set
			{
				this.slot = value;
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

		private void UpdateMaterialId()
		{
			if (this.File == null)
				return;

			if (!this.ValidSlots.Contains(this.Slot))
			{
				// Invalid slot for IMC
				return;
			}

			ImcFile.ImageChangeData imageChangeData;
			if (this.File.TryGetVariant(this.Slot, this.Variant, out imageChangeData))
			{
				this.MaterialId = imageChangeData.MaterialId;
			}
			else
			{
				this.MaterialId = 0;
			}
		}
	}
}
