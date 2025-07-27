using UnityEngine;

public class LevelRandomPattern
{
	public LevelRandomUnit[][] pattern;

	public LevelRandomPattern(SettingId sid)
	{
		switch (sid)
		{
		case SettingId.CASTLE:
			pattern = CastlePattern.pattern;
			break;
		case SettingId.CAVE:
			pattern = CavePattern.pattern;
			break;
		case SettingId.ROOFTOP:
			pattern = RooftopPattern.pattern;
			break;
		}
		for (int i = 0; i < pattern.Length; i++)
		{
			float num = 0f;
			for (int j = 0; j < pattern[i].Length; j++)
			{
				pattern[i][j].probAccum = pattern[i][j].prob + num;
				num += pattern[i][j].prob;
			}
		}
	}

	public LevelRandomUnitType GetRandomUnitType(int lvlNum)
	{
		if (lvlNum >= pattern.Length)
		{
			lvlNum = pattern.Length - 1;
		}
		float num = Random.Range(0f, 1f);
		for (int i = 0; i < pattern[lvlNum].Length; i++)
		{
			if (num <= pattern[lvlNum][i].probAccum)
			{
				return pattern[lvlNum][i].unitType;
			}
		}
		Debug.Log("@GetRandomUnitType returns the last element.");
		return pattern[lvlNum][pattern[lvlNum].Length - 1].unitType;
	}
}
