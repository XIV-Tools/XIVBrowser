// © XIV-Tools.
// Licensed under the MIT license.

namespace XivBrowser.Views
{
	using System;
	using System.Collections.Generic;
	using System.Collections.ObjectModel;
	using System.ComponentModel;
	using System.IO;
	using System.Windows;
	using System.Windows.Controls;
	using Lumina.Data.Files;
	using LuminaExtensions;
	using LuminaExtensions.Excel;
	using LuminaExtensions.Files;
	using XIVBrowser.Services;
	using XivToolsWpf.DependencyProperties;

	/// <summary>
	/// Interaction logic for ItemModelView.xaml.
	/// </summary>
	public partial class ItemModelView : UserControl, INotifyPropertyChanged
	{
		public static IBind<ItemSlots> SlotsDp = Binder.Register<ItemSlots, ItemModelView>(nameof(Slot), OnSlotChanged, BindMode.OneWay);
		public static IBind<ItemModelBase?> ModelDp = Binder.Register<ItemModelBase?, ItemModelView>(nameof(Model), OnModelChanged, BindMode.OneWay);

		public ItemModelView()
		{
			this.InitializeComponent();
			this.ContentArea.DataContext = this;
		}

		public event PropertyChangedEventHandler? PropertyChanged;

		public ImcFile? ImcFile { get; private set; }
		public string? MdlPath { get; private set; }

		public ObservableCollection<AvailableRace> AvailableRaces { get; set; } = new ObservableCollection<AvailableRace>();
		public AvailableRace? SelectedRace { get; set; }

		public ItemModelBase? Model
		{
			get => ModelDp.Get(this);
			set => ModelDp.Set(this, value);
		}

		public ItemSlots Slot
		{
			get => SlotsDp.Get(this);
			set => SlotsDp.Set(this, value);
		}

		public ImcFile.ImageChangeData? ImageChangeVariant { get; set; }

		private static void OnModelChanged(ItemModelView sender, ItemModelBase? value)
		{
			if (value == null)
			{
				sender.ImcFile = null;
				return;
			}

			sender.ImcFile = LuminaService.Lumina.GetFile<ImcFile>(value.ImcFilePath);

			/*sender.ImcView.File = sender.ImcFile;
			sender.ImcView.Slot = sender.Slot;
			sender.ImcView.Variant = value.ImcVariant;
			sender.ImcView.CanSelectImcData = sender.Slot == ItemSlots.None;*/

			sender.UpdateMdl();
		}

		private static void OnSlotChanged(ItemModelView sender, ItemSlots value)
		{
			/*sender.ImcView.Slot = value;
			sender.ImcView.CanSelectImcData = value == ItemSlots.None;*/

			sender.UpdateMdl();
		}

		private void UpdateMdl()
		{
			this.MdlPath = null;
			this.ImageChangeVariant = null;

			if (this.ImcFile == null || this.Model == null || this.Slot == ItemSlots.None)
				return;

			this.ImageChangeVariant = this.ImcFile.GetVariant(this.Slot, this.Model.ImcVariant);

			// Check every possible race/type if it produces a valid model path.
			Array allRaces = Enum.GetValues(typeof(RaceTribes));
			Array? allRaceTypes = Enum.GetValues(typeof(RaceTypes));
			foreach (RaceTribes race in allRaces)
			{
				foreach (RaceTypes type in allRaceTypes)
				{
					string path = this.Model.GetModelPath(race, type, this.Slot);
					if (LuminaService.Lumina.FileExists(path))
					{
						AvailableRace option = new AvailableRace(race, type, path);
						this.AvailableRaces.Add(option);
					}
				}
			}

			if (this.AvailableRaces.Count > 0)
				this.SelectedRace = this.AvailableRaces[0];

			////EqdpFile file = LuminaService.Lumina.GetFile<EqdpFile>($"chara/xls/charadb/equipmentdeformerparameter/c{raceTribe}.eqdp");
			////bool f = file != null && file.IsSet(this.Item.ModelSet, slot);
		}

		public class AvailableRace
		{
			public AvailableRace(RaceTribes race, RaceTypes type, string path)
			{
				this.Name = race.ToDisplayName() + " " + type.ToString() + " - " + Path.GetFileName(path);
				this.Key = race.ToKey(type);
				this.Race = race;
				this.Type = type;
				this.MdlPath = path;
			}

			public string Name { get; set; }
			public string Key { get; set; }
			public RaceTribes Race { get; set; }
			public RaceTypes Type { get; set; }
			public string MdlPath { get; set; }
		}
	}
}
