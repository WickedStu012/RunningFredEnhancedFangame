using System;
using System.Xml.Serialization;

[Serializable]
public class GiftInfo
{
	[XmlAttribute("name")]
	public string Name { get; set; }

	[XmlAttribute("description")]
	public string Description { get; set; }

	[XmlAttribute("picture")]
	public string Picture { get; set; }

	[XmlAttribute("probability")]
	public int Probability { get; set; }

	[XmlAttribute("id")]
	public int Id { get; set; }
}
