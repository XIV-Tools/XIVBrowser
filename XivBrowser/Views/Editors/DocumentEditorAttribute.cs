// © XIV-Tools.
// Licensed under the MIT license.

namespace XivBrowser.Views.Editors
{
	using System;
	using System.Reflection;

	public class DocumentEditorAttribute : Attribute
	{
		public readonly Type DocumentType;

		public DocumentEditorAttribute(Type documentType)
		{
			this.DocumentType = documentType;
		}

		public static Type? GetEditorForDocument(Type documentType)
		{
			// TODO: cache these for faster lookups
			Type[] types = typeof(DocumentEditorAttribute).Assembly.GetTypes();
			foreach (Type type in types)
			{
				DocumentEditorAttribute? attribute = type.GetCustomAttribute<DocumentEditorAttribute>();
				if (attribute != null && attribute.DocumentType == documentType)
				{
					return type;
				}
			}

			return null;
		}
	}
}
