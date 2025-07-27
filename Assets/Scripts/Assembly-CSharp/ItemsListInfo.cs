using System;
using System.Xml.Serialization;

[Serializable]
[XmlRoot("itemslist")]
public class ItemsListInfo<T>
{
	[XmlArrayItem("item")]
	[XmlArray("items")]
	public T[] Items { get; set; }
}
