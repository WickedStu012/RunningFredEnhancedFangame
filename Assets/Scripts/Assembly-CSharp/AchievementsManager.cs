using System.Collections.Generic;
using UnityEngine;

public class AchievementsManager : MonoBehaviour
{
	private static AchievementsManager instance;

	private Dictionary<string, string> icons = new Dictionary<string, string>();

	private Dictionary<string, string> names = new Dictionary<string, string>();

	private Dictionary<string, bool> achieved = new Dictionary<string, bool>();

	private float distanceReaper;

	private float startDistanceReaper;

	public static AchievementsManager Instance
	{
		get
		{
			return instance;
		}
	}

	private void Awake()
	{
		names["distancerunner"] = "Distance Runner 1";
		names["distancerunner2"] = "Distance Runner 2";
		names["distancerunner3"] = "Distance Runner 3";
		names["closecall1"] = "Close Call 1";
		names["closecall2"] = "Close Call 2";
		names["closecall3"] = "Close Call 3";
		names["adeptmanorsurvivalist"] = "Adept Manor Survivalist";
		names["expertmanorsurvivalist"] = "Expert Manor Survivalist";
		names["mastermanorsurvivalist"] = "Master Manor Survivalist";
		names["beginnermanorrunner"] = "Beginner Manor Runner";
		names["adeptmanorrunner"] = "Adept Manor Runner";
		names["expertmanorrunner"] = "Expert Manor Runner";
		names["mastermanorrunner"] = "Master Manor Runner";
		names["adeptcavesurvivalist"] = "Adept Cave Survivalist";
		names["expertcavesurvivalist"] = "Expert Cave Survivalist";
		names["mastercavesurvivalist"] = "Master Cave Survivalist";
		names["beginnercavesurvivalist"] = "Beginner Cave Runner";
		names["adeptcaverunner"] = "Adept Cave Runner";
		names["expertcaverunner"] = "Expert Cave Runner";
		names["mastercaverunner"] = "Master Cave Runner";
		icons["distancerunner"] = "distance-runner";
		icons["distancerunner2"] = "distance-runner";
		icons["distancerunner3"] = "distance-runner";
		icons["closecall1"] = "close-call";
		icons["closecall2"] = "close-call";
		icons["closecall3"] = "close-call";
		icons["adeptmanorsurvivalist"] = "distance-runner";
		icons["expertmanorsurvivalist"] = "distance-runner";
		icons["mastermanorsurvivalist"] = "distance-runner";
		icons["beginnermanorrunner"] = "distance-runner";
		icons["adeptmanorrunner"] = "distance-runner";
		icons["expertmanorrunner"] = "distance-runner";
		icons["mastermanorrunner"] = "distance-runner";
		icons["adeptcavesurvivalist"] = "distance-runner";
		icons["expertcavesurvivalist"] = "distance-runner";
		icons["mastercavesurvivalist"] = "distance-runner";
		icons["beginnercavesurvivalist"] = "distance-runner";
		icons["adeptcaverunner"] = "distance-runner";
		icons["expertcaverunner"] = "distance-runner";
		icons["mastercaverunner"] = "distance-runner";
	}

	private void OnEnable()
	{
		instance = this;
	}

	private void OnDisable()
	{
		instance = null;
	}

	public void UnlockAchievement(string achievement)
	{
		achieved[achievement] = true;
		if (!PlayerPrefsWrapper.HasAchievement(achievement))
		{
			achieved[achievement] = true;
			if (icons.ContainsKey(achievement))
			{
				SoundManager.PlaySound(54);
				GUI3DPopupManager.Instance.ShowPopup("AchievementUnlocked", names[achievement], string.Empty, icons[achievement], false);
			}
			else
			{
				Debug.Log("Error: " + achievement + " icon not found.");
			}
			PlayerPrefsWrapper.UnlockAchievement(achievement);
		}
	}

	private void Update()
	{
		int distance = DistanceManager.Instance.Distance;
		int num = PlayerPrefsWrapper.GetTotalMeters() + distance;
		if (num >= 500000 && !achieved.ContainsKey("distancerunner3"))
		{
			Instance.UnlockAchievement("distancerunner3");
		}
		else if (num >= 50000 && !achieved.ContainsKey("distancerunner2"))
		{
			Instance.UnlockAchievement("distancerunner2");
		}
		else if (num >= 10000 && !achieved.ContainsKey("distancerunner"))
		{
			Instance.UnlockAchievement("distancerunner");
		}
		if (ReaperManager.Instance != null)
		{
			if (ReaperManager.Instance.IsReaperVisible)
			{
				distanceReaper = (float)distance - startDistanceReaper;
				if (distanceReaper >= 500f)
				{
					if (!achieved.ContainsKey("closecall3"))
					{
						Instance.UnlockAchievement("closecall3");
					}
				}
				else if (distanceReaper >= 250f)
				{
					if (!achieved.ContainsKey("closecall2"))
					{
						Instance.UnlockAchievement("closecall2");
					}
				}
				else if (distanceReaper >= 100f && !achieved.ContainsKey("closecall1"))
				{
					Instance.UnlockAchievement("closecall1");
				}
			}
			else
			{
				startDistanceReaper = distance;
			}
		}
		if (PlayerAccount.Instance.CurrentGameMode != PlayerAccount.GameMode.Survival)
		{
			return;
		}
		if (PlayerAccount.Instance.CurrentLevel == "Castle-Random")
		{
			if (PlayerAccount.Instance.CurrentLevelNum == 1)
			{
				if (distance >= 3000 && !achieved.ContainsKey("beginnermanorrunner"))
				{
					Instance.UnlockAchievement("beginnermanorrunner");
				}
				else if (distance >= DistanceManager.Instance.DistanceToUnlockNormalLevel && !achieved.ContainsKey("adeptmanorsurvivalist"))
				{
					Instance.UnlockAchievement("adeptmanorsurvivalist");
				}
			}
			else if (PlayerAccount.Instance.CurrentLevelNum == 2)
			{
				if (distance >= 2000 && !achieved.ContainsKey("adeptmanorrunner"))
				{
					Instance.UnlockAchievement("adeptmanorrunner");
				}
				else if (distance >= DistanceManager.Instance.DistanceToUnlockHardLevel && !achieved.ContainsKey("expertmanorsurvivalist"))
				{
					Instance.UnlockAchievement("expertmanorsurvivalist");
				}
			}
			else if (PlayerAccount.Instance.CurrentLevelNum == 3)
			{
				if (distance >= 1500 && !achieved.ContainsKey("expertmanorrunner"))
				{
					Instance.UnlockAchievement("expertmanorrunner");
				}
				else if (distance >= DistanceManager.Instance.DistanceToUnlockNightmareLevel && !achieved.ContainsKey("mastermanorsurvivalist"))
				{
					Instance.UnlockAchievement("mastermanorsurvivalist");
				}
			}
			else if (PlayerAccount.Instance.CurrentLevelNum == 4 && distance >= 500 && !achieved.ContainsKey("mastermanorrunner"))
			{
				Instance.UnlockAchievement("mastermanorrunner");
			}
		}
		else
		{
			if (!(PlayerAccount.Instance.CurrentLevel == "Caves-Random"))
			{
				return;
			}
			if (PlayerAccount.Instance.CurrentLevelNum == 1)
			{
				if (distance >= 3000 && !achieved.ContainsKey("beginnercavesurvivalist"))
				{
					Instance.UnlockAchievement("beginnercavesurvivalist");
				}
				else if (distance >= DistanceManager.Instance.DistanceToUnlockNormalLevel && !achieved.ContainsKey("adeptcavesurvivalist"))
				{
					Instance.UnlockAchievement("adeptcavesurvivalist");
				}
			}
			else if (PlayerAccount.Instance.CurrentLevelNum == 2)
			{
				if (distance >= 2000 && !achieved.ContainsKey("adeptcaverunner"))
				{
					Instance.UnlockAchievement("adeptcaverunner");
				}
				else if (distance >= DistanceManager.Instance.DistanceToUnlockHardLevel && !achieved.ContainsKey("expertcavesurvivalist"))
				{
					Instance.UnlockAchievement("expertcavesurvivalist");
				}
			}
			else if (PlayerAccount.Instance.CurrentLevelNum == 3)
			{
				if (distance >= 1500 && !achieved.ContainsKey("expertcaverunner"))
				{
					Instance.UnlockAchievement("expertcaverunner");
				}
				else if (distance >= DistanceManager.Instance.DistanceToUnlockNightmareLevel && !achieved.ContainsKey("mastercavesurvivalist"))
				{
					Instance.UnlockAchievement("mastercavesurvivalist");
				}
			}
			else if (PlayerAccount.Instance.CurrentLevelNum == 4 && distance >= 500 && !achieved.ContainsKey("mastercaverunner"))
			{
				Instance.UnlockAchievement("mastercaverunner");
			}
		}
	}
}
