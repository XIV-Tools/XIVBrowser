// © XIV-Tools.
// Licensed under the MIT license.

namespace XivBrowser
{
	using System;

	public class Document
	{
		public object Data;
		public string Name;

		public Document(string name, object data)
		{
			this.Name = name;
			this.Data = data;
		}

		public Type DataType => this.Data.GetType();

		public bool CanEdit => false;
	}
}
