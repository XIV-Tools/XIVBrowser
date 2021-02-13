// © XIV-Tools.
// Licensed under the MIT license.

namespace XIVBrowser.Services
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;

	using LuminaMain = Lumina.Lumina;

	public static class LuminaService
	{
		// TODO: not this
		public static LuminaMain Lumina = new LuminaMain("C:\\Games\\FINAL FANTASY XIV Online\\game\\sqpack");

		public static ulong GetFileHash(string path)
		{
			try
			{
				return LuminaMain.GetFileHash(path);
			}
			catch (Exception)
			{
				return 0;
			}
		}
	}
}
