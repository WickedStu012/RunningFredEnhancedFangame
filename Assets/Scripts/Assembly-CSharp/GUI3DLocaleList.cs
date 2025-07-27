using System;
using System.Xml.Serialization;

[Serializable]
[XmlRoot("localization")]
public class GUI3DLocaleList
{
	[XmlArray("locales")]
	[XmlArrayItem("locale")]
	public GUI3DLocale[] Locales { get; set; }
}
