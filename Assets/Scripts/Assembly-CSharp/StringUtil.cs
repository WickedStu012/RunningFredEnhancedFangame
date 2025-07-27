using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class StringUtil
{
	private static Dictionary<string, Texture2D> stringTex = new Dictionary<string, Texture2D>();

	private static Texture texToDraw;

	public static int Compare(string str1, string str2, bool ignoreCase)
	{
		if (ignoreCase)
		{
			if (str1.ToLower() == str2.ToLower())
			{
				return 0;
			}
			return 1;
		}
		if (str1 == str2)
		{
			return 0;
		}
		return 1;
	}

	public static int Compare(string str1, string str2)
	{
		if (str1.ToLower() == str2.ToLower())
		{
			return 0;
		}
		return 1;
	}

	public static string EncodeTo64(string toEncode)
	{
		byte[] bytes = Encoding.ASCII.GetBytes(toEncode);
		return Convert.ToBase64String(bytes);
	}

	public static string EncodeTo64(byte[] toEncode)
	{
		return Convert.ToBase64String(toEncode);
	}

	public static string DecodeFrom64(string encodedData)
	{
		byte[] bytes = Convert.FromBase64String(encodedData);
		return Encoding.ASCII.GetString(bytes);
	}

	public static byte[] DecodeFrom64ToByteArray(string encodedData)
	{
		if (encodedData != null)
		{
			return Convert.FromBase64String(encodedData);
		}
		return null;
	}

	public static void DrawLabel(Rect rect, StringConsts sc, GUIStyle frontStyle, GUIStyle backStyle)
	{
		DrawLabel(rect, sc, frontStyle, backStyle, false, 1f);
	}

	public static void DrawLabel(Rect rect, StringConsts sc, GUIStyle frontStyle, GUIStyle backStyle, bool disabled)
	{
		DrawLabel(rect, sc, frontStyle, backStyle, disabled, 1f);
	}

	public static void DrawLabel(Rect rect, StringConsts sc, GUIStyle frontStyle, GUIStyle backStyle, float scaleText)
	{
		DrawLabel(rect, sc, frontStyle, backStyle, false, scaleText);
	}

	public static void DrawLabel(Rect rect, StringConsts sc, GUIStyle frontStyle, GUIStyle backStyle, bool disabled, float scaleText)
	{
		if (backStyle != null)
		{
			GUI.Label(new Rect(rect.x + 1f, rect.y + 1f, rect.width, rect.height), Strings.Get(sc), backStyle);
		}
		if (frontStyle != null)
		{
			GUI.Label(new Rect(rect.x, rect.y, rect.width, rect.height), Strings.Get(sc), frontStyle);
		}
	}

	private static void drawLabel(Rect rect, string stringId, GUIStyle frontStyle, GUIStyle backStyle, float scaleText)
	{
		float left = rect.x;
		float top = rect.y;
		texToDraw = stringTex[stringId];
		if (frontStyle.alignment == TextAnchor.MiddleCenter || frontStyle.alignment == TextAnchor.LowerCenter || frontStyle.alignment == TextAnchor.UpperCenter)
		{
			left = rect.x + (rect.width / 2f - (float)(texToDraw.width >> 1));
		}
		else if (frontStyle.alignment == TextAnchor.LowerRight || frontStyle.alignment == TextAnchor.MiddleRight || frontStyle.alignment == TextAnchor.UpperRight)
		{
			left = rect.x + (rect.width - (float)texToDraw.width);
		}
		if (frontStyle.alignment == TextAnchor.MiddleLeft || frontStyle.alignment == TextAnchor.MiddleCenter || frontStyle.alignment == TextAnchor.MiddleRight)
		{
			top = rect.y + (rect.height / 2f - (float)(texToDraw.height >> 1));
		}
		if (scaleText == 1f)
		{
			GUI.DrawTexture(new Rect(left, top, texToDraw.width, texToDraw.height), texToDraw);
		}
		else
		{
			GUI.DrawTexture(new Rect(left, top, (float)texToDraw.width * scaleText, (float)texToDraw.height * scaleText), texToDraw, ScaleMode.StretchToFill);
		}
	}

	public static void DrawDialogLabel(Rect rect, string dlgLineId, GUIStyle frontStyle, GUIStyle backStyle)
	{
		DrawDialogLabel(rect, dlgLineId, frontStyle, backStyle, 1f);
	}

	public static void DrawDialogLabel(Rect rect, string dlgLineId, GUIStyle frontStyle, GUIStyle backStyle, float scaleText)
	{
		if (backStyle != null)
		{
			GUI.Label(new Rect(rect.x + 1f, rect.y + 1f, rect.width, rect.height), dlgLineId, backStyle);
		}
		if (frontStyle != null)
		{
			GUI.Label(new Rect(rect.x, rect.y, rect.width, rect.height), dlgLineId, frontStyle);
		}
	}

	public static void DeleteAllStringTexs()
	{
		if (stringTex.Count > 0)
		{
			stringTex.Clear();
		}
	}

	public static string FormatNumbers(int num)
	{
		if (num <= 999)
		{
			return num.ToString();
		}
		if (num > 999 && num <= 999999)
		{
			int num2 = num % 1000;
			int num3 = num / 1000;
			return string.Format("{0}{1}{2:000}", num3, ConfigParams.decSep, num2);
		}
		int num4 = num % 1000;
		int num5 = num / 1000;
		int num6 = num5 % 1000;
		int num7 = num5 / 1000;
		return string.Format("{0}{1}{2:000}{3}{4:000}", num7, ConfigParams.decSep, num6, ConfigParams.decSep, num4);
	}

	public static string GetTimeNow()
	{
		return string.Format("{0}/{1}/{2}", DateTime.Now.Day, DateTime.Now.Month, DateTime.Now.Year);
	}

	public static string FromDateToString(DateTime date)
	{
		return string.Format("{0}/{1}/{2}", date.Day, date.Month, date.Year);
	}

	public static DateTime FromStringToDate(string date)
	{
		string[] array = date.Split('/');
		if (array != null && array.Length == 3)
		{
			return new DateTime(Convert.ToInt32(array[2]), Convert.ToInt32(array[1]), Convert.ToInt32(array[0]));
		}
		return DateTime.MinValue;
	}

	public static string ASCIIGetString(byte[] bytes)
	{
		return Encoding.ASCII.GetString(bytes);
	}

	public static byte[] ASCIIGetBytes(string str)
	{
		ASCIIEncoding aSCIIEncoding = new ASCIIEncoding();
		return aSCIIEncoding.GetBytes(str);
	}
}
