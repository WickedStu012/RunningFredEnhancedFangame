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
		
		// Parse the gore texture data from the text file instead of using hardcoded values
		parseGoreTexData("CharacterDamage", width, height);
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
		int originalWidth = 1024;
		int originalHeight = 1024;
		
		while (text != null)
		{
			text = text.Trim();
			string[] array = text.Split(';');
			switch (state)
			{
			case State.NONE:
			{
				// Parse "width:512;height:512" format
				string[] widthPart = array[0].Split(':');
				string[] heightPart = array[1].Split(':');
				originalWidth = Convert.ToInt32(widthPart[1]);
				originalHeight = Convert.ToInt32(heightPart[1]);
				state = State.PART;
				break;
			}
			case State.PART:
			{
				try
				{
					string[] array2 = array[1].Split(',');
					int x = Convert.ToInt32(array2[0]);
					int y = Convert.ToInt32(array2[1]);
					int w = Convert.ToInt32(array2[2]);
					int h = Convert.ToInt32(array2[3]);
					
					// Scale coordinates to match the actual texture size
					float scaleX = (float)width / (float)originalWidth;
					float scaleY = (float)height / (float)originalHeight;
					
					int scaledX = (int)(x * scaleX);
					int scaledW = (int)(w * scaleX);
					int scaledH = (int)(h * scaleY);
					
					// Fix Y coordinate flipping - the original coordinates are top-down, Unity uses bottom-up
					// Use the original formula but ensure proper positioning
					int scaledY = height - (int)(y * scaleY) - scaledH;
					
					// Ensure coordinates stay within bounds
					scaledX = Mathf.Clamp(scaledX, 0, width - 1);
					scaledY = Mathf.Clamp(scaledY, 0, height - 1);
					scaledW = Mathf.Clamp(scaledW, 1, width - scaledX);
					scaledH = Mathf.Clamp(scaledH, 1, height - scaledY);
					
					goreTexData.Add(array[0], new int[4] { scaledX, scaledY, scaledW, scaledH });
				}
				catch (Exception ex)
				{
					Debug.LogError(string.Format("Error parsing gore part {0}: {1}", array[0], ex.Message));
				}
				break;
			}
			}
			text = textReader.ReadLine();
		}
		textReader.Close();
	}
}
