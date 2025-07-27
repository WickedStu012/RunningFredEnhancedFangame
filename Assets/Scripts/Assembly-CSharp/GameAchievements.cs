using System.Collections.Generic;
using UnityEngine;

public class GameAchievements : MonoBehaviour
{
	public string[] achievementsIds;

	public string[] achievementsNumericIds;

	private static GameAchievements instance;

	protected Dictionary<string, BeLordAchievementInfo> achList;

	private void Awake()
	{
		instance = this;
		achList = new Dictionary<string, BeLordAchievementInfo>();
		for (int i = 0; i < achievementsIds.Length; i++)
		{
			achList.Add(achievementsIds[i], new BeLordAchievementInfo(achievementsIds[i]));
		}
	}

	private void Start()
	{
	}

	public int GetCount()
	{
		return achList.Count;
	}

	public int GetUnlockedCount()
	{
		int num = 0;
		Dictionary<string, BeLordAchievementInfo>.Enumerator enumerator = achList.GetEnumerator();
		while (enumerator.MoveNext())
		{
			num += (enumerator.Current.Value.completed ? 1 : 0);
		}
		return num;
	}

	public BeLordAchievementInfo GetAchievementById(string id)
	{
		if (achList.ContainsKey(id))
		{
			return achList[id];
		}
		return null;
	}

	public BeLordAchievementInfo GetAchievementByIdx(int idx)
	{
		return GetAchievementById(GetAchievementIdByIdx(idx));
	}

	public string GetAchievementIdByIdx(int idx)
	{
		return achievementsIds[idx];
	}

	public int GetAchievementIdx(string id)
	{
		for (int i = 0; i < achievementsIds.Length; i++)
		{
			if (achievementsIds[i] == id)
			{
				return i;
			}
		}
		return -1;
	}

	public static string GetAchievementNumericId(string strId)
	{
		for (int i = 0; i < instance.achievementsIds.Length; i++)
		{
			Debug.Log("Id: " + strId + " - " + instance.achievementsIds[i]);
			if (string.Compare(instance.achievementsIds[i], strId) == 0)
			{
				Debug.Log("Id: " + strId + " - " + instance.achievementsNumericIds[i]);
				return instance.achievementsNumericIds[i];
			}
		}
		Debug.Log("Id: " + strId + " not found!!!");
		return null;
	}
}
