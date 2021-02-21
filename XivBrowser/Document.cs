// © XIV-Tools.
// Licensed under the MIT license.

namespace XivBrowser
{
	using System;
	using System.ComponentModel;

	public class Document : INotifyPropertyChanged
	{
		public object Data;

		public Document(string name, object data)
		{
			this.Name = name;
			this.Data = data;
		}

		public event PropertyChangedEventHandler? PropertyChanged;

		public string Name { get; set; }
		public string? Path { get; set; }
		public Type DataType => this.Data.GetType();
		public bool CanEdit => false;

		public string? Directory => System.IO.Path.GetDirectoryName(this.Path)?.Replace("\\", "/");
	}
}
