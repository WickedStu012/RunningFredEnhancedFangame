using System;
using System.Xml.Serialization;

[Serializable]
public class ChallengeItemInfo : ItemInfo
{
	[XmlAttribute("scene_name")]
	public string SceneName { get; set; }

	[XmlAttribute("collect_picture")]
	public string CollectPicture { get; set; }

	[XmlAttribute("collect_name")]
	public string CollectName { get; set; }
}
