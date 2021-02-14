// © XIV-Tools.
// Licensed under the MIT license.

namespace XIVBrowser.Views
{
	using System.Collections.Generic;
	using System.Collections.ObjectModel;
	using System.Windows;
	using Lumina;
	using Lumina.Excel.GeneratedSheets;
	using LuminaExtensions;
	using LuminaExtensions.Excel;
	using XIVBrowser.Services;
	using XivToolsWpf.ModelView;

	/// <summary>
	/// Interaction logic for ItemsView.xaml.
	/// </summary>
	public partial class ItemsView : View
	{
		public ItemsView()
		{
			this.InitializeComponent();
		}

		public delegate void SelectionChangedDelegate(ItemViewModel item);

		public event SelectionChangedDelegate? SelectedItemChanged;

		public ObservableCollection<TreeEntry>? Items
		{
			get => this.GetValue<ObservableCollection<TreeEntry>?>();
			set => this.SetValue(value);
		}

		public void Refresh()
		{
			ExcelSheetViewModel<ItemViewModel, Item> itemsSheet = LuminaService.Lumina.GetExcelSheet<ItemViewModel, Item>();

			Dictionary<ItemSlots, TreeEntry> categoryLookup = new Dictionary<ItemSlots, TreeEntry>();

			foreach (ItemViewModel item in itemsSheet)
			{
				if (item.ModelBase == 0 && item.SubModelBase == 0)
					continue;

				TreeEntry? category;
				if (!categoryLookup.TryGetValue(item.FitsInSlots, out category))
				{
					category = new TreeEntry();
					category.Name = item.FitsInSlots.ToString();
					categoryLookup.Add(item.FitsInSlots, category);
				}

				category.Children.Add(new ItemTreeEntry(item));
			}

			this.Items = new ObservableCollection<TreeEntry>(categoryLookup.Values);
		}

		private void OnViewLoaded(object sender, System.Windows.RoutedEventArgs e)
		{
			this.Refresh();
		}

		private void OnSelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
		{
			if (e.NewValue is ItemTreeEntry itemTree)
			{
				this.SelectedItemChanged?.Invoke(itemTree.Item);
			}
		}

		public class ItemTreeEntry : TreeEntry
		{
			public readonly ItemViewModel Item;

			public ItemTreeEntry(ItemViewModel item)
			{
				this.Item = item;
			}

			public new string Name => this.Item.Name;
		}

		public class TreeEntry : Bindable
		{
			public TreeEntry()
			{
				this.Children = new List<TreeEntry>();
			}

			public List<TreeEntry> Children
			{
				get => this.GetValue<List<TreeEntry>>();
				set => this.SetValue(value);
			}

			public string? Name
			{
				get => this.GetValue<string?>();
				set => this.SetValue(value);
			}
		}
	}
}
