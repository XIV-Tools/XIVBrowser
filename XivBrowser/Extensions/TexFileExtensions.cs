// © XIV-Tools.
// Licensed under the MIT license.

namespace XivBrowser.Extensions
{
	using System;
	using System.Windows.Media;
	using System.Windows.Media.Imaging;
	using Lumina.Data.Files;

	public static class TexFileExtensions
	{
		public static ImageSource ToImageSource(this TexFile self)
		{
			if (self == null)
				throw new NullReferenceException();

			BitmapSource bmp = BitmapSource.Create(self.Header.Width, self.Header.Height, 96, 96, PixelFormats.Bgra32, null, self.ImageData, self.Header.Width * 4);
			bmp.Freeze();

			return bmp;
		}
	}
}
