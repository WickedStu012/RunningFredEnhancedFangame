using System;
using System.Xml.Serialization;

namespace BmFont
{
	[Serializable]
	[XmlRoot("font")]
	public class FontFile
	{
		[XmlElement("info")]
		public FontInfo Info { get; set; }

		[XmlElement("common")]
		public FontCommon Common { get; set; }

		[XmlArray("pages")]
		[XmlArrayItem("page")]
		public FontPage[] Pages { get; set; }

		[XmlArrayItem("char")]
		[XmlArray("chars")]
		public FontChar[] Chars { get; set; }

		[XmlArray("kernings")]
		[XmlArrayItem("kerning")]
		public FontKerning[] Kernings { get; set; }
	}
}
