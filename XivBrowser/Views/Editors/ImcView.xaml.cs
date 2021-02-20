// © XIV-Tools.
// Licensed under the MIT license.

namespace XivBrowser.Views.Editors
{
	using System;
	using System.Collections.Generic;
	using System.ComponentModel;
	using System.Windows;
	using System.Windows.Controls;
	using Lumina.Data.Files;
	using LuminaExtensions;

	/// <summary>
	/// Interaction logic for ImcView.xaml.
	/// </summary>
	[DocumentEditor(typeof(ImcFile))]
	public partial class ImcView : UserControl, IDocumentEditor, INotifyPropertyChanged
	{
		private ImcFile? file;

		public ImcView()
		{
			this.InitializeComponent();
			this.ContentArea.DataContext = this;

			this.PropertyChanged += this.OnSelfPropertyChanged;
		}

		public event PropertyChangedEventHandler? PropertyChanged;

		public ImcFile? File
		{
			get => this.file;
			set
			{
				if (value != null)
				{
					this.VariantCount = value.Count;
				}

				this.file = value;
			}
		}

		public ushort VariantCount { get; set; }
		public bool CanSelectImcData { get; set; } = true;
		public ItemSlots Slot { get; set; }
		public ushort Variant { get; set; }
		public byte MaterialId { get; set; }

		public List<ItemSlots> ValidSlots { get; } = new ()
		{
			ItemSlots.Head,
			ItemSlots.Body,
			ItemSlots.Hands,
			ItemSlots.Legs,
			ItemSlots.Feet,
			ItemSlots.Ears,
			ItemSlots.Neck,
			ItemSlots.Wrists,
			ItemSlots.RightRing,
			ItemSlots.LeftRing,
		};

		public void SetDocument(Document document)
		{
			if (document.Data is ImcFile imc)
			{
				this.File = imc;
			}
			else
			{
				throw new Exception($"Attempt to use ImcEditor for wrong document type: {document.Data}");
			}
		}

		private void OnSelfPropertyChanged(object? sender, PropertyChangedEventArgs e)
		{
			if (this.File == null)
				return;

			if (e.PropertyName == nameof(ImcView.Slot) || e.PropertyName == nameof(ImcView.Variant))
			{
				if (!this.ValidSlots.Contains(this.Slot))
				{
					// Invalid slot for IMC
					return;
				}

				ImcFile.ImageChangeData imageChangeData = this.File.GetVariant(this.Slot, this.Variant);
				this.MaterialId = imageChangeData.MaterialId;
			}
		}
	}
}
