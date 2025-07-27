using System.Collections.Generic;
using System.IO;
using System.Security;
using System.Xml.Serialization;
using UnityEngine;

public class GUI3DLocalization : MonoBehaviorSingleton<GUI3DLocalization>
{
	public delegate void RefreshLanguageDlg(string language);

	private const string defaultLanguage = "Global";

	private Dictionary<string, Dictionary<string, GUI3DLocale>> locale = new Dictionary<string, Dictionary<string, GUI3DLocale>>();

	private string currentLanguage = "Global";

	private string[] languages;

	public string[] Languages
	{
		get
		{
			return languages;
		}
	}

	public event RefreshLanguageDlg RefreshLanguageEvent;

	protected override void Initialize()
	{
		currentLanguage = PlayerPrefs.GetString("CurrentLanguage", currentLanguage);
	}

	public override void Awake()
	{
		enableDuplicateInstanceWarning = false;
		base.Awake();
	}

	public void Reset()
	{
		locale.Clear();
	}

	public void SetCurrentLanguage(string language)
	{
		if (!(currentLanguage != language))
		{
			return;
		}
		Reset();
		currentLanguage = language;
		if (Application.isPlaying)
		{
			if (this.RefreshLanguageEvent != null)
			{
				this.RefreshLanguageEvent(language);
			}
			PlayerPrefs.SetString("CurrentLanguage", language);
			PlayerPrefs.Save();
		}
		else
		{
			ForceRefreshText();
		}
	}

	public void ForceRefreshText()
	{
		GUI3DText[] componentsInChildren = GetComponentsInChildren<GUI3DText>(true);
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			if (componentsInChildren[i].UseCustomXML)
			{
				MonoBehaviorSingleton<GUI3DLocalization>.Instance.Load(componentsInChildren[i].CustomXML, true);
			}
			else
			{
				MonoBehaviorSingleton<GUI3DLocalization>.Instance.Load(componentsInChildren[i].LocaleXML, true);
			}
			componentsInChildren[i].RefreshText();
			if (componentsInChildren[i].DynamicText)
			{
				componentsInChildren[i].ForceSetText(componentsInChildren[i].DynamicTextFormat);
			}
		}
	}

	public string GetCurrentLanguage()
	{
		return currentLanguage;
	}

	public void SetTextValues(string xml, string textId, string text, Vector2 localPos, Vector2 localScale, bool isVolatile)
	{
		if (!locale.ContainsKey(xml))
		{
			locale[xml] = new Dictionary<string, GUI3DLocale>();
		}
		if (!locale[xml].ContainsKey(textId))
		{
			locale[xml][textId] = new GUI3DLocale();
		}
		locale[xml][textId].Text = text;
		locale[xml][textId].Id = textId;
		locale[xml][textId].PosX = localPos.x;
		locale[xml][textId].PosY = localPos.y;
		locale[xml][textId].ScaleX = localScale.x;
		locale[xml][textId].ScaleY = localScale.y;
		locale[xml][textId].Volatile = isVolatile;
	}

	public void SetTextValues(GUI3DText t)
	{
		string localeXML = t.LocaleXML;
		string uniqueId = t.GetUniqueId();
		if (!string.IsNullOrEmpty(localeXML))
		{
			if (!locale.ContainsKey(localeXML))
			{
				locale[localeXML] = new Dictionary<string, GUI3DLocale>();
			}
			SetTextValues(localeXML, uniqueId, t.GetText(), t.transform.localPosition, t.transform.localScale, !t.UseCustomXML);
		}
	}

	public string GetText(string xml, int id, string defaultText)
	{
		Load(xml);
		if (locale.ContainsKey(xml) && locale[xml].ContainsKey(id.ToString()))
		{
			return locale[xml][id.ToString()].Text;
		}
		return defaultText;
	}

	public string GetText(string xml, string id, string defaultText)
	{
		Load(xml);
		if (locale.ContainsKey(xml) && locale[xml].ContainsKey(id))
		{
			return locale[xml][id].Text;
		}
		return defaultText;
	}

	public Vector2 GetTextPos(string xml, int id, Vector2 defaultPosition)
	{
		Load(xml);
		if (locale.ContainsKey(xml) && locale[xml].ContainsKey(id.ToString()))
		{
			return new Vector2(locale[xml][id.ToString()].PosX, locale[xml][id.ToString()].PosY);
		}
		return defaultPosition;
	}

	public Vector2 GetTextPos(string xml, string id, Vector2 defaultPosition)
	{
		Load(xml);
		if (locale.ContainsKey(xml) && locale[xml].ContainsKey(id))
		{
			return new Vector2(locale[xml][id].PosX, locale[xml][id].PosY);
		}
		return defaultPosition;
	}

	public Vector2 GetTextScale(string xml, int id, Vector2 defaultScale)
	{
		Load(xml);
		if (locale.ContainsKey(xml) && locale[xml].ContainsKey(id.ToString()))
		{
			return new Vector2(locale[xml][id.ToString()].ScaleX, locale[xml][id.ToString()].ScaleY);
		}
		return defaultScale;
	}

	public Vector2 GetTextScale(string xml, string id, Vector2 defaultScale)
	{
		Load(xml);
		if (locale.ContainsKey(xml) && locale[xml].ContainsKey(id))
		{
			return new Vector2(locale[xml][id].ScaleX, locale[xml][id].ScaleY);
		}
		return defaultScale;
	}

	public void Save(string xmlName)
	{
		if (Application.isPlaying || string.IsNullOrEmpty(xmlName))
		{
			return;
		}
		string text = currentLanguage;
		SaveInLanguage(xmlName, currentLanguage);
		GUI3DLocale[] array = new GUI3DLocale[locale[xmlName].Count];
		locale[xmlName].Values.CopyTo(array, 0);
		Reset();
		for (int i = 0; i < languages.Length; i++)
		{
			if (languages[i] == text)
			{
				continue;
			}
			Load(xmlName, languages[i], true);
			bool flag = false;
			GUI3DLocale[] array2 = array;
			foreach (GUI3DLocale gUI3DLocale in array2)
			{
				if (!locale[xmlName].ContainsKey(gUI3DLocale.Id))
				{
					Debug.Log(gUI3DLocale.Id + " was added to " + languages[i]);
					gUI3DLocale.Text = "[" + gUI3DLocale.Text + "]";
					locale[xmlName][gUI3DLocale.Id] = gUI3DLocale;
					flag = true;
				}
			}
			if (flag)
			{
				SaveInLanguage(xmlName, currentLanguage);
			}
		}
		Reset();
		Load(xmlName, text);
	}

	public void SaveInLanguage(string xmlName, string language)
	{
		if (Application.isPlaying || string.IsNullOrEmpty(xmlName))
		{
			return;
		}
		Load(xmlName);
		if (!locale.ContainsKey(xmlName))
		{
			return;
		}
		string text = "<?xml version=\"1.0\"?>\n<localization>\n\t<locales>\n";
		foreach (GUI3DLocale value in locale[xmlName].Values)
		{
			string text2 = SecurityElement.Escape(value.Text);
			text2 = text2.Replace("\n", "&#10;");
			text += "\t\t<locale ";
			text = text + "id = \"" + value.Id + "\"\n";
			text = text + "\t\t\ttext = \"" + text2 + "\"\n";
			if (value.Volatile)
			{
				if (value.PosX != 0f || value.PosY != 0f)
				{
					string text3 = text;
					text = text3 + "\t\t\tpos_x = \"" + value.PosX + "\" ";
					text3 = text;
					text = text3 + "pos_y = \"" + value.PosY + "\"\n";
				}
				if (value.ScaleX != 0f || value.ScaleY != 0f)
				{
					string text3 = text;
					text = text3 + "\t\t\tscale_x = \"" + value.ScaleX + "\" ";
					text3 = text;
					text = text3 + "scale_y = \"" + value.ScaleY + "\"\n";
				}
				text = text + "\t\t\tvolatile = \"" + value.Volatile.ToString().ToLower() + "\"\n";
			}
			text += "\t\t/>\n";
		}
		text += "\t</locales>\n";
		text += "</localization>\n";
		if (Application.isEditor && !Application.isPlaying && !Directory.Exists(("Assets/Resources/Localization/" + language).Replace('/', Path.DirectorySeparatorChar)))
		{
			Directory.CreateDirectory(("Assets/Resources/Localization/" + language).Replace('/', Path.DirectorySeparatorChar));
		}
		FileStream fileStream = new FileStream(("Assets/Resources/Localization/" + language + "/" + xmlName + ".txt").Replace('/', Path.DirectorySeparatorChar), FileMode.Create);
		StreamWriter streamWriter = new StreamWriter(fileStream);
		streamWriter.Write(text);
		streamWriter.Close();
		fileStream.Close();
	}

	public void Load(string xmlName, bool force = false)
	{
		Load(xmlName, currentLanguage, force);
	}

	public void Load(string xmlName, string language, bool force = false)
	{
		if (string.IsNullOrEmpty(xmlName) || (!force && locale.ContainsKey(xmlName) && currentLanguage == language))
		{
			return;
		}
		if (currentLanguage != language)
		{
			locale.Clear();
		}
		currentLanguage = language;
		GUI3DLocale[] array = LoadLocalesList(xmlName, language);
		if (array == null)
		{
			return;
		}
		locale[xmlName] = new Dictionary<string, GUI3DLocale>();
		GUI3DLocale[] array2 = array;
		foreach (GUI3DLocale gUI3DLocale in array2)
		{
			locale[xmlName][gUI3DLocale.Id] = gUI3DLocale;
			if (gUI3DLocale.Id.Contains("."))
			{
				gUI3DLocale.Volatile = true;
			}
		}
	}

	public void PurgeXML(GUI3D gui)
	{
		Reset();
		GUI3DText[] componentsInChildren = gui.GetComponentsInChildren<GUI3DText>(true);
		Dictionary<string, List<string>> dictionary = new Dictionary<string, List<string>>();
		string text = currentLanguage;
		GUI3DText[] array = componentsInChildren;
		foreach (GUI3DText gUI3DText in array)
		{
			string text2 = (gUI3DText.UseCustomXML ? gUI3DText.CustomXML : gUI3DText.LocaleXML);
			if (string.IsNullOrEmpty(text2))
			{
				continue;
			}
			Load(text2);
			if (locale.ContainsKey(text2))
			{
				string uniqueId = gUI3DText.GetUniqueId();
				if (!dictionary.ContainsKey(text2))
				{
					dictionary[text2] = new List<string>();
				}
				dictionary[text2].Add(uniqueId);
			}
		}
		for (int j = 0; j < languages.Length; j++)
		{
			foreach (string key in dictionary.Keys)
			{
				Load(key, languages[j]);
			}
			PurgeXMLForLanguage(gui, languages[j], dictionary);
		}
		Reset();
		currentLanguage = text;
	}

	public void PurgeXMLForLanguage(GUI3D gui, string language, Dictionary<string, List<string>> ids)
	{
		Dictionary<string, List<string>> dictionary = new Dictionary<string, List<string>>();
		foreach (string key in ids.Keys)
		{
			Dictionary<string, GUI3DLocale> dictionary2 = locale[key];
			foreach (string key2 in dictionary2.Keys)
			{
				if (!ids[key].Contains(key2) && dictionary2[key2].Volatile)
				{
					if (!dictionary.ContainsKey(key))
					{
						dictionary[key] = new List<string>();
					}
					dictionary[key].Add(key2);
				}
			}
		}
		foreach (string key3 in dictionary.Keys)
		{
			if (dictionary[key3].Count <= 0)
			{
				continue;
			}
			foreach (string item in dictionary[key3])
			{
				locale[key3].Remove(item);
				Debug.Log("Removed " + item + " from " + language + " from " + key3);
			}
			SaveInLanguage(key3, language);
		}
	}

	private GUI3DLocale[] LoadLocalesList(string filename, string language)
	{
		if (filename.Contains("."))
		{
			int length = filename.IndexOf('.');
			filename = filename.Substring(0, length);
		}
		byte[] array = null;
		TextAsset textAsset = Resources.Load("Localization/" + language + "/" + filename, typeof(TextAsset)) as TextAsset;
		if (textAsset != null)
		{
			array = textAsset.bytes;
		}
		if (array != null && array.Length > 0)
		{
			return LoadLocalesList(array);
		}
		Debug.LogError("Couldn't load: " + language + "/" + filename);
		return null;
	}

	private GUI3DLocale[] LoadLocalesList(byte[] bytes)
	{
		MemoryStream memoryStream = new MemoryStream(bytes);
		GUI3DLocale[] result = LoadLocalesList(memoryStream);
		memoryStream.Close();
		return result;
	}

	private GUI3DLocale[] LoadLocalesList(Stream ms)
	{
		XmlSerializer xmlSerializer = new XmlSerializer(typeof(GUI3DLocaleList));
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
		GUI3DLocaleList gUI3DLocaleList = (GUI3DLocaleList)xmlSerializer.Deserialize(ms);
		return gUI3DLocaleList.Locales;
	}
}
