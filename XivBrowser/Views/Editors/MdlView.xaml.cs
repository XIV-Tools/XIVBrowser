﻿// © XIV-Tools.
// Licensed under the MIT license.

namespace XivBrowser.Views.Editors
{
	using System;
	using System.ComponentModel;
	using System.Linq;
	using System.Windows.Controls;
	using Lumina.Data.Files;
	using LuminaExtensions.Files;
	using XIVBrowser.Services;
	using XivToolsWpf.DependencyProperties;

	/// <summary>
	/// Interaction logic for MdlView.xaml.
	/// </summary>
	public partial class MdlView : UserControl, INotifyPropertyChanged
	{
		public static IBind<string?> MdlPathDp = Binder.Register<string?, MdlView>(nameof(MdlPath), OnMdlPathChanged, BindMode.OneWay);

		private MdlFile? file;
		private string? materialPath;

		public MdlView()
		{
			this.InitializeComponent();
			this.ContentArea.DataContext = this;
		}

		public event PropertyChangedEventHandler? PropertyChanged;

		public MdlFile? File
		{
			get => this.file;
			set
			{
				this.file = value;
				this.MaterialPath = this.file?.Materials.FirstOrDefault();
			}
		}

		public string? MdlPath
		{
			get => MdlPathDp.Get(this);
			set => MdlPathDp.Set(this, value);
		}

		public string? MaterialPath
		{
			get => this.materialPath;
			set
			{
				this.materialPath = value;
			}
		}

		public MtrlFile? MaterialFile { get; set; }

		private static void OnMdlPathChanged(MdlView sender, string? value)
		{
			if (value == null)
			{
				sender.File = null;
				return;
			}

			sender.File = LuminaService.Lumina.GetFile<MdlFile>(value);
		}
	}
}
