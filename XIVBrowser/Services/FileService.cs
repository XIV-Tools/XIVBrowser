// © XIV-Tools.
// Licensed under the MIT license.

namespace XIVBrowser
{
	using System;
	using System.Collections.Generic;
	using System.IO;
	using Lumina.Data;
	using Lumina.Data.Structs;
	using Newtonsoft.Json;
	using Serilog;
	using XIVBrowser.Services;

	public static class FileService
	{
		public static readonly Dictionary<ulong, SqFileInfo> FileLookupHash = new Dictionary<ulong, SqFileInfo>();
		public static readonly Dictionary<string, SqFileInfo> FileLookupPath = new Dictionary<string, SqFileInfo>();

		public static void Load()
		{
			// Get every file in the entire game (as hash -> SqFileInfo)
			foreach (KeyValuePair<string, Repository> repo in LuminaService.Lumina.Repositories)
			{
				foreach (KeyValuePair<byte, List<Category>> category in repo.Value.Categories)
				{
					foreach (Category cat2 in category.Value)
					{
						if (cat2.IndexHashTableEntries != null)
						{
							foreach (KeyValuePair<ulong, IndexHashTableEntry> hashEntry in cat2.IndexHashTableEntries)
							{
								SqPack packFile = cat2.DatFiles[hashEntry.Value.DataFileId];
								FileLookupHash.Add(hashEntry.Value.hash, new FileService.SqFileInfo(packFile, hashEntry.Value.Offset));
							}
						}

						if (cat2.Index2HashTableEntries != null)
						{
							foreach (KeyValuePair<uint, Index2HashTableEntry> hashEntry in cat2.Index2HashTableEntries)
							{
								SqPack packFile = cat2.DatFiles[hashEntry.Value.DataFileId];
								FileLookupHash.Add(hashEntry.Value.hash, new FileService.SqFileInfo(packFile, hashEntry.Value.Offset));
							}
						}
					}
				}
			}

			////LoadPaths(new FileInfo("Data/AccessoryPaths.json"));
			////LoadPaths(new FileInfo("Data/EquipmentPaths.json"));
			////LoadPaths(new FileInfo("Data/HousingPaths.json"));
			////LoadPaths(new FileInfo("Data/WeaponPaths.json"));

			Log.Information($"Loaded {FileLookupPath.Count} file paths out of {FileLookupHash.Count} files.");
		}

		public static IEnumerable<SqFileInfo> Search(string path)
		{
			// TODO: store SqFileInfos in a hierarchal format to make this faster.
			List<SqFileInfo> results = new List<SqFileInfo>();
			foreach (SqFileInfo file in FileLookupPath.Values)
			{
				if (file.Path == null)
					continue;

				if (file.Path.StartsWith(path))
				{
					results.Add(file);
				}
			}

			return results;
		}

		private static void LoadPaths(FileInfo file)
		{
			string json = File.ReadAllText(file.FullName);
			List<string> paths = JsonConvert.DeserializeObject<List<string>>(json);
			foreach (string path in paths)
			{
				if (string.IsNullOrEmpty(path))
					continue;

				SqFileInfo? fileInfo;
				ulong hash = LuminaService.GetFileHash(path);
				if (FileLookupHash.TryGetValue(hash, out fileInfo))
				{
					fileInfo.SetPath(path);
					FileLookupPath.Add(path, fileInfo);
				}
				else
				{
					throw new Exception($"Invalid path in paths data: \"{path}\". Hash not found: \"{hash}\"");
				}
			}
		}

		public record SqFileInfo
		{
			public readonly SqPack Pack;
			public readonly long Offset;

			private SqPackFileInfo? fileInfo;

			public SqFileInfo(SqPack pack, long offset)
			{
				this.Pack = pack;
				this.Offset = offset;
			}

			public string? Path { get; private set; }

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
