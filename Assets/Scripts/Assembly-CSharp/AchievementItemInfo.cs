using System;
using System.Xml.Serialization;

[Serializable]
public class AchievementItemInfo : ItemInfo
{
	[XmlAttribute("achievement_id")]
	public string AchievementId { get; set; }
}
