using System;
using System.Xml.Serialization;

[Serializable]
public class AvatarItemInfo : ItemInfo
{
	[XmlAttribute("avatar_prefab")]
	public string AvatarPrefab { get; set; }

	[XmlAttribute("market_id")]
	public string MarketId { get; set; }

	[XmlAttribute("android_market_id")]
	public string AndroidMarketId { get; set; }

	[XmlAttribute("mac_market_id")]
	public string MacMarketId { get; set; }
}
