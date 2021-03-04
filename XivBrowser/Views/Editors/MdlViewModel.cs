// © XIV-Tools.
// Licensed under the MIT license.

namespace XivBrowser.Views.Editors
{
	using System;
	using System.Collections.ObjectModel;
	using System.IO;
	using System.Linq;
	using System.Windows;
	using Lumina.Data.Files;
	using LuminaExtensions.Files;

	public class MdlViewModel : DocumentViewModel<MdlFile, MdlView>
	{
		public string? MaterialSetKey { get; set; }
		public ObservableCollection<MaterialOption> Materials { get; set; } = new ObservableCollection<MaterialOption>();
		public MaterialOption? SelectedMaterial { get; set; }

		protected override void OnFileChanged(MdlFile? file)
		{
			base.OnFileChanged(file);

			this.Materials.Clear();

			if (file == null)
				return;

			////this.OnViewLoaded();
		}

		protected override void OnViewLoaded()
		{
			base.OnViewLoaded();

			// If this mdlview is hosted within an ItemModelView then get the material
			// Id from the Imc that it defines.
			ItemModelView? itemModelView = this.View?.FindParent<ItemModelView>();
			if (itemModelView != null)
			{
				this.MaterialSetKey = itemModelView.ImageChangeVariant?.GetMaterialKey();
			}
			else
			{
				// Otherwise present a list of all possible material sets
				// TODO: get a list, and show it on the view.
				throw new NotImplementedException();
			}

			if (this.File == null || this.Document == null || this.Document.Directory == null)
				throw new NotSupportedException("Cannot view mdl materials without document path");

			string dir = this.Document.Directory;
			dir = dir.Replace("model", "material");
			dir = dir + "/" + this.MaterialSetKey;

			foreach (string materialPath in this.File.Materials)
			{
				string fullPath = dir + materialPath;
				this.Materials.Add(new MaterialOption(materialPath, fullPath));
			}

			this.SelectedMaterial = this.Materials?.FirstOrDefault();
		}

		public class MaterialOption
		{
			public MaterialOption(string path, string fullPath)
			{
				this.Path = path;
				this.FullPath = fullPath;
			}

			public string Path { get; set; }
			public string FullPath { get; set; }
		}
	}
}
