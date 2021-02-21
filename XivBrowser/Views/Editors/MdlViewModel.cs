// © XIV-Tools.
// Licensed under the MIT license.

namespace XivBrowser.Views.Editors
{
	using System;
	using System.IO;
	using System.Linq;
	using System.Windows;
	using LuminaExtensions.Files;

	public class MdlViewModel : DocumentViewModel<MdlFile, MdlView>
	{
		private string? materialPath;

		public string? MaterialFullPath { get; set; }
		public byte? MaterialSetId { get; set; }

		public string? MaterialPath
		{
			get => this.materialPath;
			set
			{
				this.materialPath = value;
				this.UpdateMaterialPath();
			}
		}

		protected override void OnViewLoaded()
		{
			base.OnViewLoaded();

			// If this mdlview is hosted within an ItemModelView then get the material
			// Id from the Imc that it defines.
			ItemModelView? itemModelView = this.View?.FindParent<ItemModelView>();
			if (itemModelView != null)
			{
				this.MaterialSetId = itemModelView.ImageChangeVariant?.MaterialId;
			}
			else
			{
				// Otherwise present a list of all possible material sets
				// TODO: get a list, and show it on the view.
				throw new NotImplementedException();
			}

			this.MaterialPath = this.File?.Materials.FirstOrDefault();
		}

		private void UpdateMaterialPath()
		{
			if (this.Document == null || this.Document.Directory == null || this.MaterialSetId == null)
				return;

			string dir = this.Document.Directory;
			dir = dir.Replace("model", "material");
			string? str = (this.MaterialSetId - 1).ToString();
			string matKey = "v" + str!.PadLeft(4, '0');

			this.MaterialFullPath = dir + "/" + matKey + this.MaterialPath;
		}
	}
}
