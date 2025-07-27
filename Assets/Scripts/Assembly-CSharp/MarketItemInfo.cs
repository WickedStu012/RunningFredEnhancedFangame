using System;
using System.Xml.Serialization;

[Serializable]
public class MarketItemInfo : ItemInfo
{
	[XmlAttribute("market_id")]
	public string MarketId { get; set; }

	[XmlAttribute("android_market_id")]
	public string AndroidMarketId { get; set; }

	[XmlAttribute("mac_market_id")]
	public string MacMarketId { get; set; }

	[XmlAttribute("onetime")]
	public bool OneTime { get; set; }

	[XmlAttribute("days")]
	public int Days { get; set; }

	[XmlAttribute("url")]
	public string Url { get; set; }
}
