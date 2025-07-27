using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

internal class FredCharacterDamage
{
	private enum State
	{
		NONE = 0,
		PART = 1
	}

	public int originalWidth = 512;

	public int originalHeight = 512;

	public Dictionary<string, int[]> goreTexData;

	public FredCharacterDamage(int width, int height)
	{
		goreTexData = new Dictionary<string, int[]>();
		goreTexData.Add("eye1", new int[4] { 86, 40, 43, 41 });
		goreTexData.Add("eye2", new int[4] { 133, 41, 42, 40 });
		goreTexData.Add("backeyes", new int[4] { 45, 192, 90, 25 });
		goreTexData.Add("nose", new int[4] { 120, 78, 22, 19 });
		goreTexData.Add("mouth1", new int[4] { 90, 96, 81, 29 });
		goreTexData.Add("mouth2", new int[4] { 66, 221, 64, 29 });
		goreTexData.Add("mouth3", new int[4] { 67, 175, 64, 16 });
		goreTexData.Add("mouth4", new int[4] { 198, 179, 61, 26 });
		goreTexData.Add("mouth5", new int[4] { 137, 174, 60, 17 });
		goreTexData.Add("rhead1", new int[4] { 19, 21, 62, 55 });
		goreTexData.Add("rhead2", new int[4] { 19, 77, 66, 59 });
		goreTexData.Add("rhead3", new int[4] { 0, 20, 19, 91 });
		goreTexData.Add("lhead1", new int[4] { 180, 16, 69, 56 });
		goreTexData.Add("lhead2", new int[4] { 176, 74, 72, 60 });
		goreTexData.Add("lhead3", new int[4] { 248, 30, 16, 88 });
		goreTexData.Add("headoff1", new int[4] { 137, 193, 56, 58 });
		goreTexData.Add("headoff2", new int[4] { 196, 209, 69, 44 });
		goreTexData.Add("neck", new int[4] { 1, 138, 264, 35 });
		goreTexData.Add("pelvis", new int[4] { 265, 0, 167, 26 });
		goreTexData.Add("rlegoff1", new int[4] { 281, 30, 58, 42 });
		goreTexData.Add("rlegoff2", new int[4] { 264, 79, 82, 20 });
		goreTexData.Add("rlegoff3", new int[4] { 266, 120, 80, 30 });
		goreTexData.Add("rlegoff4", new int[4] { 265, 182, 80, 27 });
		goreTexData.Add("llegoff1", new int[4] { 357, 30, 59, 42 });
		goreTexData.Add("llegoff2", new int[4] { 351, 79, 81, 20 });
		goreTexData.Add("llegoff3", new int[4] { 351, 120, 80, 30 });
		goreTexData.Add("llegoff4", new int[4] { 352, 182, 80, 27 });
		goreTexData.Add("lfoot1", new int[4] { 436, 1, 49, 70 });
		goreTexData.Add("lfoot2", new int[4] { 438, 77, 70, 26 });
		goreTexData.Add("rfoot1", new int[4] { 462, 106, 49, 70 });
		goreTexData.Add("rfoot2", new int[4] { 438, 180, 70, 27 });
		goreTexData.Add("chest", new int[4] { 56, 257, 96, 140 });
		goreTexData.Add("back", new int[4] { 222, 261, 119, 136 });
		goreTexData.Add("rarmoff", new int[4] { 0, 257, 51, 69 });
		goreTexData.Add("side1", new int[4] { 0, 332, 53, 63 });
		goreTexData.Add("larmoff", new int[4] { 155, 261, 65, 66 });
		goreTexData.Add("side2", new int[4] { 157, 332, 63, 63 });
		goreTexData.Add("middle", new int[4] { 0, 400, 345, 15 });
		goreTexData.Add("rarm1", new int[4] { 0, 421, 13, 60 });
		goreTexData.Add("rarm2", new int[4] { 19, 426, 60, 52 });
		goreTexData.Add("rarm3", new int[4] { 95, 427, 10, 53 });
		goreTexData.Add("rarm4", new int[4] { 110, 424, 39, 37 });
		goreTexData.Add("rarm5", new int[4] { 117, 463, 34, 30 });
		goreTexData.Add("rarm6", new int[4] { 0, 480, 32, 32 });
		goreTexData.Add("rarm7", new int[4] { 37, 483, 27, 28 });
		goreTexData.Add("rarm8", new int[4] { 93, 482, 26, 28 });
		goreTexData.Add("larm1", new int[4] { 173, 421, 13, 60 });
		goreTexData.Add("larm2", new int[4] { 191, 426, 61, 52 });
		goreTexData.Add("larm3", new int[4] { 268, 427, 10, 53 });
		goreTexData.Add("larm4", new int[4] { 283, 424, 39, 37 });
		goreTexData.Add("larm5", new int[4] { 291, 463, 33, 29 });
		goreTexData.Add("larm6", new int[4] { 173, 480, 32, 32 });
		goreTexData.Add("larm7", new int[4] { 210, 483, 27, 27 });
		goreTexData.Add("larm8", new int[4] { 266, 482, 26, 28 });
		goreTexData.Add("ingle1", new int[4] { 325, 26, 48, 12 });
		goreTexData.Add("ingle2", new int[4] { 340, 40, 17, 37 });
		goreTexData.Add("rleg2", new int[4] { 267, 101, 37, 19 });
		goreTexData.Add("rleg3", new int[4] { 308, 100, 35, 18 });
		goreTexData.Add("rleg4", new int[4] { 265, 148, 39, 32 });
		goreTexData.Add("rleg5", new int[4] { 307, 150, 36, 32 });
		goreTexData.Add("lleg2", new int[4] { 351, 100, 37, 20 });
		goreTexData.Add("lleg3", new int[4] { 393, 102, 34, 18 });
		goreTexData.Add("lleg4", new int[4] { 352, 151, 39, 30 });
		goreTexData.Add("lleg5", new int[4] { 393, 150, 36, 29 });
		Dictionary<string, int[]>.Enumerator enumerator = goreTexData.GetEnumerator();
		float num = (float)width / (float)originalWidth;
		float num2 = (float)height / (float)originalHeight;
		while (enumerator.MoveNext())
		{
			enumerator.Current.Value[2] = (int)((float)enumerator.Current.Value[2] * num);
			enumerator.Current.Value[3] = (int)((float)enumerator.Current.Value[3] * num2);
			enumerator.Current.Value[0] = (int)((float)enumerator.Current.Value[0] * num);
			enumerator.Current.Value[1] = height - (int)((float)enumerator.Current.Value[1] * num2) - enumerator.Current.Value[3];
		}
	}

	private void parseGoreTexData(string goreTexSrc, int width, int height)
	{
		TextAsset textAsset = Resources.Load(string.Format("Gore/Textures/{0}", goreTexSrc)) as TextAsset;
		if (textAsset == null || textAsset.bytes.Length == 0)
		{
			Debug.LogError(string.Format("[ERROR]Cannot find the gore def file: {0}", goreTexSrc));
			return;
		}
		MemoryStream memoryStream = new MemoryStream(textAsset.bytes);
		parseGoreTexData(memoryStream, width, height);
		memoryStream.Close();
	}

	private void parseGoreTexData(Stream stream, int width, int height)
	{
		State state = State.NONE;
		TextReader textReader = new StreamReader(stream);
		string text = textReader.ReadLine();
		Dictionary<string, float[]> dictionary = new Dictionary<string, float[]>();
		int num = 1024;
		int num2 = 1024;
		while (text != null)
		{
			text = text.Trim();
			string[] array = text.Split(';');
			switch (state)
			{
			case State.NONE:
			{
				string[] array3 = array[0].Split(':');
				string[] array4 = array[0].Split(':');
				num = Convert.ToInt32(array3[1]);
				num2 = Convert.ToInt32(array4[1]);
				state = State.PART;
				break;
			}
			case State.PART:
			{
				string[] array2 = array[1].Split(',');
				dictionary.Add(value: new float[4]
				{
					Convert.ToSingle(array2[0]) / (float)num,
					Convert.ToSingle(array2[1]) / (float)num2,
					Convert.ToSingle(array2[2]) / (float)num,
					Convert.ToSingle(array2[3]) / (float)num2
				}, key: array[0]);
				break;
			}
			}
			text = textReader.ReadLine();
		}
	}
}
