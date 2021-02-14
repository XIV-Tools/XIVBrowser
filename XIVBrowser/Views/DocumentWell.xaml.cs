// © XIV-Tools.
// Licensed under the MIT license.

namespace XIVBrowser.Views
{
	using System.Windows;
	using Dragablz;
	using Wpf.Mv;

	/// <summary>
	/// Interaction logic for DocumentWell.xaml.
	/// </summary>
	public partial class DocumentWell : View
	{
		private readonly InterTabClient tabClient = new InterTabClient();

		public DocumentWell()
		{
			this.InitializeComponent();
		}

		public InterTabClient TabClient => this.tabClient;

		public class InterTabClient : IInterTabClient
		{
			public TabEmptiedResponse TabEmptiedHandler(TabablzControl tabControl, Window window)
			{
				return TabEmptiedResponse.CloseWindowOrLayoutBranch;
			}

			INewTabHost<Window> IInterTabClient.GetNewHost(IInterTabClient interTabClient, object partition, TabablzControl source)
			{
				TornTabWindow view = new TornTabWindow();
				return new NewTabHost<Window>(view, view.InitialTabablzControl);
			}
		}
	}
}
