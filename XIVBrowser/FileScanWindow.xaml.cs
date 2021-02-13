// © XIV-Tools.
// Licensed under the MIT license.

namespace XIVBrowser
{
	using System.Collections.Generic;
	using System.Diagnostics;
	using System.IO;
	using System.Runtime.CompilerServices;
	using System.Threading.Tasks;
	using System.Windows;
	using Lumina.Data;
	using Lumina.Data.Files;
	using Lumina.Data.Structs;
	using Lumina.Misc;
	using Newtonsoft.Json;
	using Serilog;
	using Wpf.Mv;
	using XIVBrowser.Services;

	/// <summary>
	/// Interaction logic for FileScanWindow.xaml.
	/// </summary>
	public partial class FileScanWindow : Window
	{
		public FileScanWindow()
		{
			this.DataContext = new Scan();
			this.InitializeComponent();
		}

		private void OnScanClicked(object sender, RoutedEventArgs e)
		{
			Scan scan = this.DataContext as Scan;
			Task.Run(() => scan.Run());
		}

		public class Scan : Bindable
		{
			public Dictionary<ulong, FileService.SqFileInfo> FileLookup = new Dictionary<ulong, FileService.SqFileInfo>();

			private static readonly string[] MatKeys = new List<string>()
			{
				"a", "b", "c", "d", "e", "f", "g",
			}.ToArray();

			public Scan()
			{
				this.Maximum = 100;
				this.Current = 0;
			}

			public double Maximum
			{
				get => (double)this.GetValue();
				set => this.SetValue(value);
			}

			public double Current
			{
				get => (double)this.GetValue();
				set => this.SetValue(value);
			}

			public string Status
			{
				get => (string)this.GetValue();
				set => this.SetValue(value);
			}

			public void Run()
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
									this.FileLookup.Add(hashEntry.Value.hash, new FileService.SqFileInfo(packFile, hashEntry.Value.Offset));
								}
							}

							if (cat2.Index2HashTableEntries != null)
							{
								foreach (KeyValuePair<uint, Index2HashTableEntry> hashEntry in cat2.Index2HashTableEntries)
								{
									SqPack packFile = cat2.DatFiles[hashEntry.Value.DataFileId];
									this.FileLookup.Add(hashEntry.Value.hash, new FileService.SqFileInfo(packFile, hashEntry.Value.Offset));
								}
							}
						}
					}
				}

				Log.Information($"Found {this.FileLookup.Count} file hashes");

				this.ScanForEquipment();
				this.ScanForAccessories();
				this.ScanForHousing();
				this.ScanForWeapons();

				this.Maximum = 0;
				this.Current = 0;
				this.Status = "Done";
			}

			private HashSet<string> ScanForEquipment()
			{
				this.Status = "Scanning for material paths";
				Log.Information($"Scanning for material paths");

				string[] equipmentTypes = new List<string>()
				{
					"met", "top", "glv", "dwn", "sho",
				}.ToArray();

				string[] matKeys = new List<string>()
				{
					"a", "b", "c", "d", "e", "f", "g",
				}.ToArray();

				HashSet<string> paths = new HashSet<string>();

				// This is... a thing.
				// Since I dont have a good way to get materials from items, (possibly Model/SubModel Ids to models to materials?)
				// Lets just slowly crawl through all possible material paths that tex tools generates and check them all.
				this.Maximum = 1000;
				this.Current = 0;

				Parallel.For(0, 1000, (int setId) =>
				{
					this.Current++;
					string set = setId.ToString().PadLeft(4, '0');

					string path = $"chara/equipment/e{set}/e{set}.imc";
					this.CheckPath(path, ref paths);

					for (byte varaintId = 0; varaintId < byte.MaxValue; varaintId++)
					{
						string variant = varaintId.ToString().PadLeft(4, '0');

						for (ushort raceId = 0; raceId <= 18; raceId++)
						{
							string race = raceId.ToString().PadLeft(2, '0');
							for (ushort raceVarId = 0; raceVarId <= 4; raceVarId++)
							{
								string raceVar = raceVarId.ToString().PadLeft(2, '0');

								for (int matKeyId = 0; matKeyId < matKeys.Length; matKeyId++)
								{
									string matKey = matKeys[matKeyId];

									// look for equipment materials
									for (int equipmentTypeId = 0; equipmentTypeId < equipmentTypes.Length; equipmentTypeId++)
									{
										string type = equipmentTypes[equipmentTypeId];
										path = $"chara/equipment/e{set}/material/v{variant}/mt_c{race}{raceVar}e{set}_{type}_{matKey}.mtrl";
										this.CheckPath(path, ref paths);
									}
								}
							}
						}
					}
				});

				this.CheckMaterials(ref paths);

				Log.Information($"Found {paths.Count} files");

				List<string> sortedPaths = new List<string>(paths);
				sortedPaths.Sort();
				string json = JsonConvert.SerializeObject(sortedPaths, Formatting.Indented);
				File.WriteAllText("EquipmentPaths.json", json);
				return paths;
			}

			private HashSet<string> ScanForAccessories()
			{
				this.Maximum = 1000;
				this.Current = 0;
				this.Status = string.Empty;
				HashSet<string> paths = new HashSet<string>();

				string[] accessoryTypes = new List<string>()
				{
					"ear", "nek", "wrs", "rir",
				}.ToArray();

				Parallel.For(0, 1000, (int setId) =>
				{
					this.Current++;

					string set = setId.ToString().PadLeft(4, '0');

					string path = $"chara/accessory/a{set}/a{set}.imc";
					this.CheckPath(path, ref paths);

					for (byte varaintId = 0; varaintId < byte.MaxValue; varaintId++)
					{
						string variant = varaintId.ToString().PadLeft(4, '0');

						for (ushort raceId = 0; raceId <= 18; raceId++)
						{
							string race = raceId.ToString().PadLeft(2, '0');
							for (ushort raceVarId = 0; raceVarId <= 4; raceVarId++)
							{
								string raceVar = raceVarId.ToString().PadLeft(2, '0');

								for (int matKeyId = 0; matKeyId < MatKeys.Length; matKeyId++)
								{
									string matKey = MatKeys[matKeyId];

									// look for accessory materials
									for (int accessoryTypeId = 0; accessoryTypeId < accessoryTypes.Length; accessoryTypeId++)
									{
										string type = accessoryTypes[accessoryTypeId];
										path = $"chara/accessory/a{set}/material/v{variant}/mt_c{race}{raceVar}a{set}_{type}_{matKey}.mtrl";
										this.CheckPath(path, ref paths);
									}
								}
							}
						}
					}
				});

				this.CheckMaterials(ref paths);

				Log.Information($"Found {paths.Count} files");

				List<string> sortedPaths = new List<string>(paths);
				sortedPaths.Sort();
				string json = JsonConvert.SerializeObject(sortedPaths, Formatting.Indented);
				File.WriteAllText("AccessoryPaths.json", json);
				return paths;
			}

			private HashSet<string> ScanForWeapons()
			{
				this.Maximum = 1000 * byte.MaxValue;
				this.Current = 0;
				this.Status = string.Empty;
				HashSet<string> paths = new HashSet<string>();

				Parallel.For(0, 1000, (int setId) =>
				{
					string set = setId.ToString().PadLeft(4, '0');
					for (ushort varaint2Id = 0; varaint2Id < byte.MaxValue; varaint2Id++)
					{
						this.Current++;
						string set2 = varaint2Id.ToString().PadLeft(4, '0');

						string path = $"chara/weapon/w{set}/obj/body/b{set2}/b{set2}.imc";
						if (!this.CheckPath(path, ref paths))
							continue;

						for (byte varaintId = 0; varaintId < byte.MaxValue; varaintId++)
						{
							string variant = varaintId.ToString().PadLeft(4, '0');

							for (ushort raceId = 0; raceId <= 18; raceId++)
							{
								string race = raceId.ToString().PadLeft(2, '0');
								for (ushort raceVarId = 0; raceVarId <= 4; raceVarId++)
								{
									string raceVar = raceVarId.ToString().PadLeft(2, '0');

									for (int matKeyId = 0; matKeyId < MatKeys.Length; matKeyId++)
									{
										string matKey = MatKeys[matKeyId];
										path = $"chara/weapon/w{set}/obj/body/b{set2}/material/v{variant}/mt_w{race}{raceVar}b{set2}_{matKey}.mtrl";
										this.CheckPath(path, ref paths);
									}
								}
							}
						}
					}
				});

				this.CheckMaterials(ref paths);

				Log.Information($"Found {paths.Count} files");

				List<string> sortedPaths = new List<string>(paths);
				sortedPaths.Sort();
				string json = JsonConvert.SerializeObject(sortedPaths, Formatting.Indented);
				File.WriteAllText("WeaponPaths.json", json);
				return paths;
			}

			private HashSet<string> ScanForHousing()
			{
				this.Maximum = 2000;
				this.Current = 0;
				this.Status = string.Empty;
				HashSet<string> paths = new HashSet<string>();

				// look for indoor housing materials
				Parallel.For(0, 1000, (int setId) =>
				{
					this.Current++;
					string set = setId.ToString().PadLeft(4, '0');
					for (int matKeyId = 0; matKeyId < MatKeys.Length; matKeyId++)
					{
						string matKey = MatKeys[matKeyId];
						string path = $"bgcommon/hou/indoor/general/{set}/material/fun_b0_m{set}_0{matKey}.mtrl";
						this.CheckPath(path, ref paths);
					}
				});

				// look for outdoor housing materials
				Parallel.For(0, 1000, (int setId) =>
				{
					this.Current++;
					string set = setId.ToString().PadLeft(4, '0');
					for (int matKeyId = 0; matKeyId < MatKeys.Length; matKeyId++)
					{
						string matKey = MatKeys[matKeyId];
						string path = $"bgcommon/hou/outdoor/general/{set}/material/gar_b0_m{set}_0{matKey}.mtrl";
						this.CheckPath(path, ref paths);
					}
				});

				this.CheckMaterials(ref paths);

				Log.Information($"Found {paths.Count} files");

				List<string> sortedPaths = new List<string>(paths);
				sortedPaths.Sort();
				string json = JsonConvert.SerializeObject(sortedPaths, Formatting.Indented);
				File.WriteAllText("HousingPaths.json", json);
				return paths;
			}

			private void CheckMaterials(ref HashSet<string> paths)
			{
				this.Maximum = paths.Count;
				this.Current = 0;
				this.Status = "Checking materials";

				List<string> currentPaths = new List<string>(paths);

				for (int i = 0; i < currentPaths.Count; i++)
				{
					string path = currentPaths[i];
					this.Current++;

					if (path == null)
						continue;

					if (path.EndsWith(".mtrl"))
					{
						MtrlFile materialFile = LuminaService.Lumina.GetFile<MtrlFile>(path);
						if (materialFile == null)
						{
							paths.Remove(path);
							continue;
						}

						foreach (string texturePath in materialFile.TexturePaths)
						{
							this.CheckPath(texturePath, ref paths);
						}
					}
				}
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			private bool CheckPath(string path, ref HashSet<string> paths)
			{
				FileService.SqFileInfo fileInfo;

				ulong hash = LuminaService.GetFileHash(path);
				if (this.FileLookup.TryGetValue(hash, out fileInfo))
				{
					fileInfo.SetPath(path);
					paths.Add(path);
					this.Status = path;

					return true;
				}

				// Old hasing function never seems to get hit, so skip it i guess?
				/*ulong hash = Crc32.Get(path);
				if (this.FileLookup.TryGetValue(hash, out fileInfo))
				{
					fileInfo.SetPath(path);
					this.Paths.Add(path);
					this.Status = path;
					return;
				}*/

				return false;
			}
		}
	}
}
