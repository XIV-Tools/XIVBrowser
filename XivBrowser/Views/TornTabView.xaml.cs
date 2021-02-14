// © XIV-Tools.
// Licensed under the MIT license.

namespace XIVBrowser
{
	using System.Windows;
	using XivToolsWpf.ModelView;
	using static XIVBrowser.Views.DocumentWell;

	public partial class TornTabView : View
	{
		private readonly InterTabClient tabClient = new InterTabClient();

		public TornTabView()
		{
			this.InitializeComponent();
		}

		public InterTabClient TabClient => this.tabClient;
	}
}
