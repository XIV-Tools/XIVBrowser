// © XIV-Tools.
// Licensed under the MIT license.

namespace XivBrowser.Views.Editors
{
	using LuminaExtensions.Files;

	public class MtrlViewModel : DocumentViewModel<MtrlFile, MtrlView>
	{
		protected override void OnFileChanged(MtrlFile? file)
		{
			base.OnFileChanged(file);
		}
	}
}
