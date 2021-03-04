// © XIV-Tools.
// Licensed under the MIT license.

namespace XivBrowser.Views.Editors
{
	using System.Windows.Media;
	using Lumina.Data.Files;
	using LuminaExtensions.Files;
	using XivBrowser.Extensions;

	public class TexViewModel : DocumentViewModel<TexFile, TexView>
	{
		public ImageSource? Image { get; private set; }

		public TexFile.Attribute? Type { get; private set; }
		public TexFile.TextureFormat? Format { get; private set; }
		public ushort? Width { get; private set; }
		public ushort? Height { get; private set; }
		public ushort? Depth { get; private set; }
		public ushort? MipLevels { get; private set; }

		protected override void OnFileChanged(TexFile? file)
		{
			base.OnFileChanged(file);

			this.Image = this.File?.ToImageSource();
			this.Type = this.File?.Header.Type;
			this.Format = this.File?.Header.Format;
			this.Width = this.File?.Header.Width;
			this.Height = this.File?.Header.Height;
			this.Depth = this.File?.Header.Depth;
			this.MipLevels = this.File?.Header.MipLevels;
		}
	}

	public class TexExViewModel : DocumentViewModel<TexFileEx, TexView>
	{
		public ImageSource? Image { get; private set; }

		public TexFile.Attribute? Type { get; private set; }
		public TexFile.TextureFormat? Format { get; private set; }
		public ushort? Width { get; private set; }
		public ushort? Height { get; private set; }
		public ushort? Depth { get; private set; }
		public ushort? MipLevels { get; private set; }

		protected override void OnFileChanged(TexFileEx? file)
		{
			base.OnFileChanged(file);

			this.Image = this.File?.ToImageSource();
			this.Type = this.File?.Header.Type;
			this.Format = this.File?.Header.Format;
			this.Width = this.File?.Header.Width;
			this.Height = this.File?.Header.Height;
			this.Depth = this.File?.Header.Depth;
			this.MipLevels = this.File?.Header.MipLevels;
		}
	}
}
