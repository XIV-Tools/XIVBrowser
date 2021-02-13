// © XIV-Tools.
// Licensed under the MIT license.

namespace XIVBrowser
{
	using System;
	using System.Collections.Generic;
	using System.Diagnostics;
	using System.IO;
	using System.Threading.Tasks;
	using Lumina.Data;
	using Lumina.Data.Structs;
	using Lumina.Misc;
	using Newtonsoft.Json;
	using Serilog;
	using XIVBrowser.Services;

	public static class FileService
	{
		public class SqFileInfo
		{
			public readonly SqPack Pack;
			public readonly long Offset;

			private SqPackFileInfo? fileInfo;

			public SqFileInfo(SqPack pack, long offset)
			{
				this.Pack = pack;
				this.Offset = offset;
			}

			public string Path { get; private set; }

			public FileType Type => this.FileInfo.Type;

			public SqPackFileInfo FileInfo
			{
				get
				{
					if (this.fileInfo == null)
						this.fileInfo = this.Pack.GetFileMetadata(this.Offset);

					return (SqPackFileInfo)this.fileInfo;
				}
			}

			public void SetPath(string path)
			{
				if (this.Path != null && this.Path != path)
					throw new Exception($"Attempt to change path of file from {this.Path} to {path}");

				this.Path = path;
			}

			public void Export(FileInfo to)
			{
				FileResource dat = this.Pack.ReadFile<FileResource>(this.Offset);
				dat.SaveFile(to.FullName);
			}
		}
	}
}
