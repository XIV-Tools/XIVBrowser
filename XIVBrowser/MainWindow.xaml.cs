﻿// © XIV-Tools.
// Licensed under the MIT license.

namespace XIVBrowser
{
	using System.Windows;
	using LuminaExtensions.Excel;

	/// <summary>
	/// Interaction logic for MainWindow.xaml.
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			this.InitializeComponent();
		}

		private void OnSelectedItemChanged(ItemViewModel item)
		{
			this.ItemView.SetItem(item);
		}
	}
}