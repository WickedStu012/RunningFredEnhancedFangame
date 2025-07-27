using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using UnityEngine;

internal class Strings
{
	private static ArrayList _strs;

	private static bool _loaded;

	private static Dictionary<string, string> values;

	public static void Clear()
	{
		_strs = null;
		values = null;
		_loaded = false;
	}

	public static void LoadFromFile(string filename)
	{
		FileStream fileStream = File.Open(filename, FileMode.Open);
		_strs = BuildStringTable(fileStream);
		fileStream.Close();
	}

	public static void LoadFromByteArray(byte[] xmlData)
	{
		MemoryStream memoryStream = new MemoryStream(xmlData);
		_strs = BuildStringTable(memoryStream);
		memoryStream.Close();
	}

	public static void LoadFromResources(string strFile)
	{
		TextAsset textAsset = Resources.Load(strFile) as TextAsset;
		if (textAsset == null || textAsset.bytes.Length == 0)
		{
			Debug.Log(string.Format("[ERROR][STRING LOADING] Cannot load {0}", strFile));
		}
		else
		{
			LoadFromByteArray(textAsset.bytes);
		}
	}

	public static ArrayList BuildStringTable(Stream stream)
	{
		values = new Dictionary<string, string>();
		XmlTextReader xmlTextReader = new XmlTextReader(stream);
		ArrayList arrayList = new ArrayList();
		Hashtable hashtable = new Hashtable();
		while (xmlTextReader.Read())
		{
			string name = xmlTextReader.Name;
			switch (xmlTextReader.NodeType)
			{
			case XmlNodeType.Element:
				switch (name)
				{
				default:
				{
		
					{
						break;
					}
					if (xmlTextReader.HasAttributes)
					{
						hashtable.Clear();
						for (int i = 0; i < xmlTextReader.AttributeCount; i++)
						{
							xmlTextReader.MoveToAttribute(i);
							hashtable.Add(xmlTextReader.Name, xmlTextReader.Value);
						}
					}
					arrayList.Add(hashtable["value"].ToString().Replace("\\n", "\n"));
					values[hashtable["id"].ToString()] = hashtable["value"].ToString().Replace("\\n", "\n");
					break;
				}
				case "strings":
					break;
				}
				break;
			}
		}
		xmlTextReader.Close();
		_loaded = true;
		return arrayList;
	}

	public static string Get<T>(T stringId)
	{
		if (_strs == null)
		{
			Debug.Log("Warning. Strings not loaded yet. (trying to get one)");
			return string.Empty;
		}
		try
		{
			return (string)_strs[Convert.ToInt32(stringId)];
		}
		catch
		{
			return string.Empty;
		}
	}

	public static void AddNewValue(string id, string val)
	{
		if (values == null)
		{
			values = new Dictionary<string, string>();
		}
		if (_strs == null)
		{
			_strs = new ArrayList();
		}
		values[id] = val;
		_strs.Add(val);
	}

	public static void RemoveValue(string id)
	{
		if (values == null || _strs == null)
		{
			return;
		}
		values.Remove(id);
		foreach (string key in values.Keys)
		{
			_strs.Add(values[key]);
		}
	}

	public static bool ContainsId(string id)
	{
		if (values == null)
		{
			Debug.Log("Warning. Strings not loaded yet. (trying to get one)");
			return false;
		}
		if (values.ContainsKey(id))
		{
			return true;
		}
		return false;
	}

	public static string GetFromDictionary(string id)
	{
		if (ContainsId(id))
		{
			return values[id];
		}
		return string.Empty;
	}

	public static bool IsLoaded()
	{
		return _loaded;
	}

	public static bool SaveFromDictionaryToResources(Dictionary<string, string> values, string filename)
	{
		string text = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>\n";
		text += "<strings>\n";
		foreach (string key in values.Keys)
		{
			string text2 = text;
			text = text2 + "\t<string id=\"" + key + "\" value=\"" + values[key] + "\"/>\n";
		}
		text += "</strings>\n";
		FileStream fileStream = File.Open("Assets/Resources/" + filename, FileMode.Create);
		StreamWriter streamWriter = new StreamWriter(fileStream);
		streamWriter.Write(text);
		streamWriter.Close();
		fileStream.Close();
		return true;
	}

	public static bool SaveToResources(string filename)
	{
		return SaveFromDictionaryToResources(values, filename);
	}
}
