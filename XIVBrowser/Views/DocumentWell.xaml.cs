// © XIV-Tools.
// Licensed under the MIT license.

namespace XIVBrowser.Views
{
	using System.Windows;
	using System.Windows.Controls;
	using Dragablz;
	using XivBrowser;
	using XivBrowser.Views;
	using XivToolsWpf.Windows;

	/// <summary>
	/// Interaction logic for DocumentWell.xaml.
	/// </summary>
	public partial class DocumentWell : UserControl
	{
		private static DocumentWell? instance;
		private readonly InterTabClient tabClient = new InterTabClient();

		public DocumentWell()
		{
			instance = this;
			this.InitializeComponent();
		}

		public InterTabClient TabClient => this.tabClient;

		public static void Open(Document document)
		{
			if (instance == null)
				return;

			DocumentView documentView = new DocumentView();
			documentView.Header = DocumentView.HeaderModes.Path;

			ScrollViewer scroll = new ScrollViewer();
			scroll.Content = documentView;
			scroll.HorizontalScrollBarVisibility = ScrollBarVisibility.Disabled;
			scroll.VerticalScrollBarVisibility = ScrollBarVisibility.Visible;

			TabItem tab = new TabItem();
			tab.Header = document.Name;
			tab.Content = scroll;
			instance.InitialTabablzControl.Items.Add(tab);
			tab.IsSelected = true;

			documentView.OpenDocument(document);
		}

		private void OnTabClosing(ItemActionCallbackArgs<TabablzControl> args)
		{
		}

		public class InterTabClient : IInterTabClient
		{
			public TabEmptiedResponse TabEmptiedHandler(TabablzControl tabControl, Window window)
			{
				if (window is StyledWindow styledWindow)
				{
					if (styledWindow.Content is TornTabView)
					{
						return TabEmptiedResponse.CloseWindowOrLayoutBranch;
					}

					return TabEmptiedResponse.DoNothing;
				}

				return TabEmptiedResponse.CloseWindowOrLayoutBranch;
			}

			INewTabHost<Window> IInterTabClient.GetNewHost(IInterTabClient interTabClient, object partition, TabablzControl source)
			{
				StyledWindow window = StyledWindow.Create<TornTabView>();
				window.OverlapTitleBar = true;
				return new NewTabHost<Window>(window, ((TornTabView)window.Content).InitialTabablzControl);
			}
		}
	}
}
