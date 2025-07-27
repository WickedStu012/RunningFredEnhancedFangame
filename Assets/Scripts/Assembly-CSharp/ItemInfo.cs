using System;
using System.Xml.Serialization;

[Serializable]
public class ItemInfo
{
	public bool Purchased;

	public int Upgrades;

	public int Count;

	public int OrigPrice;

	public int OrigPrice1;

	public int OrigPrice2;

	public int OrigPrice3;

	public int OrigPrice4;

	[XmlAttribute("name")]
	public string Name { get; set; }

	[XmlAttribute("description")]
	public string Description { get; set; }

	[XmlAttribute("price")]
	public int Price { get; set; }

	[XmlAttribute("price1")]
	public int Price1 { get; set; }

	[XmlAttribute("price2")]
	public int Price2 { get; set; }

	[XmlAttribute("price3")]
	public int Price3 { get; set; }

	[XmlAttribute("price4")]
	public int Price4 { get; set; }

	[XmlAttribute("price_dollars")]
	public string PriceDollars { get; set; }

	[XmlAttribute("price_kreds")]
	public string PriceKreds { get; set; }

	[XmlAttribute("tag")]
	public string Tag { get; set; }

	[XmlAttribute("id")]
	public int Id { get; set; }

	[XmlAttribute("type")]
	public string Type { get; set; }

	[XmlAttribute("consumable")]
	public bool Consumable { get; set; }

	[XmlAttribute("upgradeable")]
	public int Upgradeable { get; set; }

	[XmlAttribute("picture")]
	public string Picture { get; set; }

	[XmlAttribute("count")]
	public int PackCount { get; set; }

	[XmlAttribute("enabled")]
	public bool Enabled { get; set; }

	[XmlAttribute("is_mandatory")]
	public int RequieredByLevel { get; set; }

	[XmlAttribute("mandatory_text")]
	public string MandatoryText { get; set; }

	[XmlAttribute("instructions_mobile")]
	public string InstructionsMobile { get; set; }

	[XmlAttribute("instructions_desktop")]
	public string InstructionsDesktop { get; set; }

	[XmlAttribute("visibilityBehaviour")]
	public string VisibilityBehaviour { get; set; }

	[XmlAttribute("coin_type")]
	public string CoinType { get; set; }

	[XmlAttribute("count_kongregate")]
	public int CountKongregate { get; set; }

	public int GetPrice()
	{
		if (Consumable)
		{
			return Price;
		}
		return Price;
	}
}
