using System.IO;
using System.Xml.Serialization;
using UnityEngine;

namespace BmFont
{
	public class FontLoader
	{
		public static FontFile Load(string filename)
		{
			XmlSerializer xmlSerializer = new XmlSerializer(typeof(FontFile));
			if (xmlSerializer == null)
			{
				Debug.Log("Couldn't create deserializer...");
				return null;
			}
			TextAsset textAsset = Resources.Load(filename, typeof(TextAsset)) as TextAsset;
			if (textAsset != null)
			{
				MemoryStream memoryStream = new MemoryStream(textAsset.bytes);
				if (memoryStream == null)
				{
					return null;
				}
				FontFile result = (FontFile)xmlSerializer.Deserialize(memoryStream);
				memoryStream.Close();
				return result;
			}
			return null;
		}
	}
}
