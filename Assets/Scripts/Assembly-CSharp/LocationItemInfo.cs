using System;
using System.Xml.Serialization;

[Serializable]
public class LocationItemInfo : ItemInfo
{
	[XmlAttribute("sceneprefix")]
	public string ScenePrefix { get; set; }

	[XmlAttribute("unlock_normal_id")]
	public int UnlockNormalId { get; set; }

	[XmlAttribute("unlock_hard_id")]
	public int UnlockHardId { get; set; }

	[XmlAttribute("unlock_nightmare_id")]
	public int UnlockNightmareId { get; set; }

	[XmlAttribute("market_id")]
	public string MarketId { get; set; }

	[XmlAttribute("android_market_id")]
	public string AndroidMarketId { get; set; }

	[XmlAttribute("mac_market_id")]
	public string MacMarketId { get; set; }
}
