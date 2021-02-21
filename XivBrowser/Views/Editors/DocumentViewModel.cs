// © XIV-Tools.
// Licensed under the MIT license.

namespace XivBrowser.Views.Editors
{
	using System;
	using System.ComponentModel;
	using System.Windows.Controls;

	public abstract class DocumentViewModel : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler? PropertyChanged;

		public static DocumentViewModel? GetViewModelForFile(Type file)
		{
			foreach (Type t in typeof(DocumentViewModel).Assembly.GetTypes())
			{
				if (t.IsAbstract)
					continue;

				if (!t.IsSubclassOf(typeof(DocumentViewModel)))
					continue;

				DocumentViewModel? vm = (DocumentViewModel?)Activator.CreateInstance(t);

				if (vm == null)
					continue;

				if (vm.CanEditFile(file))
				{
					return vm;
				}
			}

			return null;
		}

		public abstract bool CanEditFile(Type fileType);
		public abstract UserControl CreateView(Document document);
	}
}
