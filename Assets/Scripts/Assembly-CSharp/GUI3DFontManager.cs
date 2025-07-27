using System.Collections.Generic;
using BmFont;

public class GUI3DFontManager
{
	private static GUI3DFontManager instance;

	private Dictionary<string, FontDesc> fontDesc = new Dictionary<string, FontDesc>();

	public static GUI3DFontManager Instance
	{
		get
		{
			if (instance == null)
			{
				instance = new GUI3DFontManager();
			}
			return instance;
		}
	}

	public static void Release()
	{
		instance = null;
	}

	public FontDesc LoadFont(string font)
	{
		if (font == null || font == string.Empty)
		{
			return null;
		}
		if (this.fontDesc.ContainsKey(font))
		{
			return this.fontDesc[font];
		}
		FontFile fontFile = FontLoader.Load(font);
		if (fontFile != null)
		{
			FontDesc fontDesc = new FontDesc();
			fontDesc.fontFile = fontFile;
			int num = 0;
			for (int i = 0; i < fontFile.Chars.Length; i++)
			{
				if (fontFile.Chars[i].ID > num)
				{
					num = fontFile.Chars[i].ID;
				}
			}
			fontDesc.Chars = new FontChar[num + 1];
			for (int j = 0; j < fontFile.Chars.Length; j++)
			{
				fontDesc.Chars[fontFile.Chars[j].ID] = fontFile.Chars[j];
			}
			this.fontDesc[font] = fontDesc;
			return fontDesc;
		}
		return null;
	}
}
