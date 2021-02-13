// © XIV-Tools.
// Licensed under the MIT license.

namespace XIVBrowser.Extensions
{
	using Lumina.Excel.GeneratedSheets;

	public static class ItemExtensions
	{
		public static (ushort modelSet, ushort modelBase, ushort modelVariant) GetMainModel(this Item self)
		{
			bool isWeapon = self.IsWeapon();
			ulong val = self.ModelMain;

			if (val == 0)
				return (0, 0, 0);

			if (isWeapon)
			{
				return ((ushort)val, (ushort)(val >> 16), (ushort)(val >> 32));
			}
			else
			{
				return (0, (ushort)val, (ushort)(val >> 16));
			}
		}

		public static (ushort modelSet, ushort modelBase, ushort modelVariant) GetSubModel(this Item self)
		{
			bool isWeapon = self.IsWeapon();
			ulong val = self.ModelSub;

			if (val == 0)
				return (0, 0, 0);

			if (isWeapon)
			{
				return ((ushort)val, (ushort)(val >> 16), (ushort)(val >> 32));
			}
			else
			{
				return (0, (ushort)val, (ushort)(val >> 16));
			}
		}

		public static bool IsWeapon(this Item self)
		{
			return self.EquipSlotCategory.Value.MainHand != 0 || self.EquipSlotCategory.Value.OffHand != 0;
		}
	}
}
