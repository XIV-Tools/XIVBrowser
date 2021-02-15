// © XIV-Tools.
// Licensed under the MIT license.

namespace XIVBrowser.Views
{
	using System.Collections.Generic;
	using System.Collections.ObjectModel;
	using System.ComponentModel;
	using System.Windows;
	using System.Windows.Controls;
	using Lumina;
	using Lumina.Excel.GeneratedSheets;
	using LuminaExtensions;
	using LuminaExtensions.Excel;
	using XivBrowser;
	using XIVBrowser.Services;

	/// <summary>
	/// Interaction logic for ItemsView.xaml.
	/// </summary>
	public partial class ItemsView : UserControl, INotifyPropertyChanged
	{
		public ItemsView()
		{
			this.InitializeComponent();
			this.DataContext = this;
		}

		public event PropertyChangedEventHandler? PropertyChanged;

		public ObservableCollection<TreeEntry>? Items { get; set; }

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

		private void TreeView_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
		{
			if (this.ItemTree.SelectedItem is ItemTreeEntry itemTree)
			{
				DocumentWell.Open(new Document(itemTree.Item.Name, itemTree.Item));
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

		public class TreeEntry : INotifyPropertyChanged
		{
			public TreeEntry()
			{
				this.Children = new List<TreeEntry>();
			}

			public event PropertyChangedEventHandler? PropertyChanged;

			public List<TreeEntry> Children { get; set; }
			public string? Name { get; set; }
		}
	}
}
