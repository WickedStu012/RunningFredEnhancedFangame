using System.Text;
using UnityEngine;

public class Hasher
{
	public static byte[] Hash(string str)
	{
		UTF8Encoding uTF8Encoding = new UTF8Encoding();
		byte[] bytes = uTF8Encoding.GetBytes(str);
		byte b = 0;
		byte b2 = 0;
		int num = bytes.Length / 2;
		for (int i = 0; i < num; i++)
		{
			b ^= bytes[i * 2];
			b2 ^= bytes[i * 2 + 1];
		}
		string text = b.ToString();
		num = 3 - text.Length;
		for (int j = 0; j < num; j++)
		{
			text = "0" + text;
		}
		string text2 = b2.ToString();
		num = 3 - text2.Length;
		for (int k = 0; k < num; k++)
		{
			text2 = "0" + text2;
		}
		string text3 = Random.Range(0, 999).ToString();
		num = 3 - text3.Length;
		for (int l = 0; l < num; l++)
		{
			text3 = "0" + text3;
		}
		string text4 = Random.Range(0, 999).ToString();
		num = 3 - text4.Length;
		for (int m = 0; m < num; m++)
		{
			text4 = "0" + text4;
		}
		string s = text3 + text + text4 + text2;
		return uTF8Encoding.GetBytes(s);
	}
}
