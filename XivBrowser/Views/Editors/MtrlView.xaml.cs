// © XIV-Tools.
// Licensed under the MIT license.

namespace XivBrowser.Views.Editors
{
	using System;
	using System.ComponentModel;
	using System.Windows.Controls;
	using LuminaExtensions.Files;
	using XivToolsWpf.DependencyProperties;

	/// <summary>
	/// Interaction logic for MtrlView.xaml.
	/// </summary>
	public partial class MtrlView : UserControl, INotifyPropertyChanged
	{
		public static IBind<MtrlFile?> FileDp = Binder.Register<MtrlFile?, MtrlView>(nameof(File), BindMode.TwoWay);

		public MtrlView()
		{
			this.InitializeComponent();
		}

		public event PropertyChangedEventHandler? PropertyChanged;

		public MtrlFile? File
		{
			get => FileDp.Get(this);
			set => FileDp.Set(this, value);
		}
	}
}
