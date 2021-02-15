// © XIV-Tools.
// Licensed under the MIT license.

namespace XIVBrowser
{
	using System.Windows.Controls;
	using static XIVBrowser.Views.DocumentWell;

	public partial class TornTabView : UserControl
	{
		private readonly InterTabClient tabClient = new InterTabClient();

		public TornTabView()
		{
			this.InitializeComponent();
		}

		public InterTabClient TabClient => this.tabClient;
	}
}
