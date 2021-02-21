// © XIV-Tools.
// Licensed under the MIT license.

namespace XIVBrowser.Services
{
	using System;
	using System.Collections.Generic;
	using System.IO;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using Lumina.Data;
	using Lumina.Data.Files;
	using LuminaExtensions.Files;
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

		public static FileResource GetFile(string path)
		{
			// Special case to handle custom file types that lumina doesn't have built-in
			string extension = Path.GetExtension(path);
			switch (extension)
			{
				case ".mtrl": return Lumina.GetFile<MtrlFile>(path);
				case ".mdl": return Lumina.GetFile<MdlFile>(path);
				case ".eqdp": return Lumina.GetFile<EqdpFile>(path);
				case ".tex": return Lumina.GetFile<TexFile>(path);
			}

			return Lumina.GetFile(path);
		}
	}
}
