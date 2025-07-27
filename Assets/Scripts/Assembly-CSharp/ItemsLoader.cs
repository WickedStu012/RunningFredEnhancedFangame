using System.IO;
using System.Xml.Serialization;
using UnityEngine;

public class ItemsLoader
{
	public static T[] Load<T>(string filename)
	{
		TextAsset textAsset = Resources.Load(filename) as TextAsset;
		if (textAsset != null)
		{
			return Load<T>(textAsset.bytes);
		}
		Debug.LogError("Couldn't load: " + filename);
		return null;
	}

	public static T[] Load<T>(byte[] bytes)
	{
		MemoryStream memoryStream = new MemoryStream(bytes);
		T[] result = Load<T>(memoryStream);
		memoryStream.Close();
		return result;
	}

	public static T[] Load<T>(MemoryStream ms)
	{
		XmlSerializer xmlSerializer = new XmlSerializer(typeof(ItemsListInfo<T>));
		if (xmlSerializer == null)
		{
			Debug.Log("Couldn't create deserializer...");
			return null;
		}
		if (ms == null)
		{
			Debug.Log("Couldn't create memory stream...");
			return null;
		}
		ItemsListInfo<T> itemsListInfo = (ItemsListInfo<T>)xmlSerializer.Deserialize(ms);
		return itemsListInfo.Items;
	}
}
