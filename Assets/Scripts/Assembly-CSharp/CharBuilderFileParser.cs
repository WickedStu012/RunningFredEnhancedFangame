using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class CharBuilderFileParser
{
	private enum State
	{
		NONE = 0,
		CHAR = 1,
		BASE = 2,
		MAT = 3,
		PART = 4,
		HIDEPART = 5,
		COMMON = 6
	}

	private const string CHAR_FILE = "Characters/characters";

	public static List<CharDef> ParseFile()
	{
		TextAsset textAsset = Resources.Load("Characters/characters") as TextAsset;
		if (textAsset == null || textAsset.bytes.Length == 0)
		{
			Debug.LogError(string.Format("[ERROR]Cannot find the character def file."));
			return null;
		}
		return ParseFromArray(textAsset.bytes);
	}

	public static List<CharDef> ParseFromArray(byte[] data)
	{
		MemoryStream memoryStream = new MemoryStream(data);
		List<CharDef> result = ParseFromStream(memoryStream);
		memoryStream.Close();
		return result;
	}

	public static List<CharDef> ParseFromStream(Stream stream)
	{
		string matGoreName = string.Empty;
		List<string> list = new List<string>();
		List<string> list2 = new List<string>();
		List<string> list3 = new List<string>();
		List<string> list4 = new List<string>();
		State state = State.NONE;
		TextReader textReader = new StreamReader(stream);
		string text = textReader.ReadLine();
		List<CharDef> list5 = new List<CharDef>();
		CharDef charDef = null;
		while (text != null)
		{
			text = text.Trim();
			string[] array = text.Split(':');
			switch (state)
			{
			case State.NONE:
				if (text.StartsWith("Common"))
				{
					state = State.COMMON;
				}
				else if (text.StartsWith("Character:"))
				{
					charDef = new CharDef();
					list5.Add(charDef);
					charDef.charName = array[1];
					charDef.matGoreName = matGoreName;
					charDef.skinned = list;
					charDef.charred = list2;
					charDef.bones = list3;
					charDef.gore = list4;
					state = State.CHAR;
				}
				break;
			case State.COMMON:
				if (text.StartsWith("MaterialGore:"))
				{
					matGoreName = array[1];
				}
				else if (text.StartsWith("CharredPart:"))
				{
					list2.Add(array[1]);
				}
				else if (text.StartsWith("SkinnedPart:"))
				{
					list.Add(array[1]);
				}
				else if (text.StartsWith("BonesPart:"))
				{
					list3.Add(array[1]);
				}
				else if (text.StartsWith("GorePart:"))
				{
					list4.Add(array[1]);
				}
				else if (text.StartsWith("EndCommon"))
				{
					state = State.NONE;
				}
				break;
			case State.CHAR:
				if (text.StartsWith("Base:"))
				{
					charDef.basePrefab = array[1];
				}
				else if (text.StartsWith("Material:"))
				{
					charDef.matName = array[1];
				}
				else if (text.StartsWith("Part:"))
				{
					charDef.parts.Add(array[1]);
				}
				else if (text.StartsWith("Hide:"))
				{
					charDef.hideParts.Add(array[1]);
				}
				else if (text.StartsWith("EndCharacter"))
				{
					state = State.NONE;
				}
				break;
			}
			text = textReader.ReadLine();
		}
		return list5;
	}
}
