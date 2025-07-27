using System;
using System.Xml.Serialization;

[Serializable]
[XmlRoot("locales")]
public class GUI3DLocale
{
	[XmlAttribute("id")]
	public string Id { get; set; }

	[XmlAttribute("text")]
	public string Text { get; set; }

	[XmlAttribute("scale_x")]
	public float ScaleX { get; set; }

	[XmlAttribute("scale_y")]
	public float ScaleY { get; set; }

	[XmlAttribute("pos_x")]
	public float PosX { get; set; }

	[XmlAttribute("pos_y")]
	public float PosY { get; set; }

	[XmlAttribute("volatile")]
	public bool Volatile { get; set; }
}
