// © XIV-Tools.
// Licensed under the MIT license.

namespace XIVBrowser.Views
{
	using System;
	using System.Windows;
	using System.Windows.Controls;
	using Dragablz;
	using XivBrowser;
	using XivBrowser.Views.Editors;
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

			DocumentViewModel? vm = DocumentViewModel.GetViewModelForFile(document.DataType);

			if (vm == null)
				return;

			UserControl? view = vm.CreateView(document);

			if (view == null)
				return;

			TabItem tab = new TabItem();
			tab.Header = document.Name;
			tab.Content = view;
			instance.InitialTabablzControl.Items.Add(tab);
			tab.IsSelected = true;
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
