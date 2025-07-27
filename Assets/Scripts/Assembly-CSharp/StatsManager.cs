using System;
using System.Collections.Generic;
using UnityEngine;

public class StatsManager : MonoBehaviour
{
	public StatsServiceType sst;

	public string iOSApiKey;

	public string androidApiKey;

	public string webApiKey;

	private static iStatsService ss;

	private static Dictionary<StatVar, TimeEventParams> timedEvents;

	private void Start()
	{
		ss = StatsServiceFactory.Create(sst);
		if (ss != null)
		{
			ss.SetApiKey(androidApiKey);
		}
		onSessionStart();
		UnityEngine.Object.DontDestroyOnLoad(this);
	}

	private void OnApplicationQuit()
	{
		onSessionEnd();
	}

	public static void onSessionStart()
	{
		if (ss != null)
		{
			ss.SessionStart();
		}
	}

	private static void onSessionEnd()
	{
		if (ss != null)
		{
			ss.SessionEnd();
		}
	}

	public static void LogEvent(StatVar sv)
	{
		LogEvent(sv, null, null, null);
	}

	public static void LogEvent(StatVar sv, string param1)
	{
		LogEvent(sv, param1, null, null);
	}

	public static void LogEvent(StatVar sv, string param1, string param2)
	{
		LogEvent(sv, param1, param2, null);
	}

	public static void LogEvent(StatVar sv, string param1, string param2, string param3)
	{
		if (ss != null)
		{
			switch (sv)
			{
			case StatVar.SELECTED_CHARACTER:
				ss.LogEvent("SELECTED_CHARACTER", "Character", param1);
				break;
			case StatVar.START_LEVEL_CASTLE:
				ss.LogEvent("START_LEVEL", "Character", param1, "Castle", param2);
				break;
			case StatVar.START_LEVEL_CAVES:
				ss.LogEvent("START_LEVEL", "Character", param1, "Caves", param2);
				break;
			case StatVar.START_LEVEL_ROOFTOP:
				ss.LogEvent("START_LEVEL", "Character", param1, "Rooftop", param2);
				break;
			case StatVar.START_CHALLENGE:
				ss.LogEvent("START_CHALLENGE", "ChallengeNum", param1);
				break;
			case StatVar.START_ENDLESS_CASTLE:
				ss.LogEvent("START_ENDLESS", "Character", param1, "Castle", param2);
				break;
			case StatVar.START_ENDLESS_CAVES:
				ss.LogEvent("START_ENDLESS", "Character", param1, "Caves", param2);
				break;
			case StatVar.START_ENDLESS_ROOFTOP:
				ss.LogEvent("START_ENDLESS", "Character", param1, "Rooftop", param2);
				break;
			case StatVar.RESTART_LEVEL_CASTLE:
				ss.LogEvent("RESTART_LEVEL", "Castle", param1);
				break;
			case StatVar.RESTART_LEVEL_CAVES:
				ss.LogEvent("RESTART_LEVEL", "Caves", param1);
				break;
			case StatVar.RESTART_LEVEL_ROOFTOP:
				ss.LogEvent("RESTART_LEVEL", "Rooftop", param1);
				break;
			case StatVar.RESTART_ENDLESS_CASTLE:
				ss.LogEvent("RESTART_ENDLESS", "Castle", param1);
				break;
			case StatVar.RESTART_ENDLESS_CAVES:
				ss.LogEvent("RESTART_ENDLESS", "Caves", param1);
				break;
			case StatVar.RESTART_ENDLESS_ROOFTOP:
				ss.LogEvent("RESTART_ENDLESS", "Rooftop", param1);
				break;
			case StatVar.MAIN_MENU_BUTTON:
				ss.LogEvent("MAIN_MENU_BUTTON", "Which", param1);
				break;
			case StatVar.SHOP:
				ss.LogEvent("SHOP", "Time", param1, "From", param2);
				break;
			case StatVar.CHALLENGE_SCREEN:
				ss.LogEvent("CHALLENGE_SCREEN", "WentToCollectables", param1);
				break;
			case StatVar.BUY_SKULLIES:
				ss.LogEvent("BUY_SKULLIES", "Count", param1);
				break;
			case StatVar.BUY_ITEM:
				ss.LogEvent("BUY_ITEM", "ItemType", param1, "Item", param2);
				break;
			case StatVar.BUY_ITEM_UPGRADEABLE:
				ss.LogEvent("BUY_ITEM_UPGRADEABLE", "ItemType", param1, "Item", param2, "Upgrades", param3);
				break;
			case StatVar.BUY_ITEM_CONSUMABLE:
				ss.LogEvent("BUY_ITEM_CONSUMABLE", "ItemType", param1, "Item", param2);
				break;
			case StatVar.TAPJOY:
				ss.LogEvent("TAPJOY");
				break;
			case StatVar.PLAYER_DIE_CASTLE:
				ss.LogEvent("PLAYER_DIE", "Castle", param1, "ChunkNum", param2);
				break;
			case StatVar.PLAYER_DIE_CAVES:
				ss.LogEvent("PLAYER_DIE", "Caves", param1, "ChunkNum", param2);
				break;
			case StatVar.PLAYER_DIE_ROOFTOP:
				ss.LogEvent("PLAYER_DIE", "Rooftop", param1, "ChunkNum", param2);
				break;
			case StatVar.BUY_CONSUMABLE_BY_GROUP:
				ss.LogEvent("BUY_CONSUMABLE_GROUP", "GroupNum", param1, "Item", param2);
				break;
			case StatVar.BUY_VALUE_PACK_BY_GROUP:
				ss.LogEvent("BUY_VALUE_PACK_BY_GROUP", "GroupNum", param1);
				break;
			case StatVar.SUPERFALLINGFRED_POPUP:
				ss.LogEvent("SUPERFALLINGFRED_POPUP", "Action", param1);
				break;
			case StatVar.SKIINGFRED_POPUP:
				ss.LogEvent("SKIINGFRED_POPUP", "Action", param1);
				break;
			case StatVar.START_LEVEL_SECTOR51:
			case StatVar.START_ENDLESS_SECTOR51:
				break;
			}
		}
	}

	public static void LogEventTimed(StatVar sv)
	{
		LogEventTimed(sv, null, null);
	}

	public static void LogEventTimed(StatVar sv, string param1)
	{
		LogEventTimed(sv, param1, null);
	}

	public static void LogEventTimed(StatVar sv, string param1, string param2)
	{
		if (ss != null)
		{
			if (timedEvents == null)
			{
				timedEvents = new Dictionary<StatVar, TimeEventParams>();
			}
			if (timedEvents.ContainsKey(sv))
			{
				timedEvents[sv] = new TimeEventParams(DateTime.Now, param1, param2);
			}
			else
			{
				timedEvents.Add(sv, new TimeEventParams(DateTime.Now, param1, param2));
			}
		}
	}

	public static void LogEventEndTimed(StatVar sv)
	{
		if (ss != null && timedEvents != null && timedEvents.ContainsKey(sv))
		{
			TimeSpan timeSpan = DateTime.Now - timedEvents[sv].time;
			string param = timedEvents[sv].param1;
			string param2 = timedEvents[sv].param2;
			timedEvents.Remove(sv);
			int num = (int)timeSpan.TotalSeconds;
			if (sv == StatVar.SHOP)
			{
				num = (int)((timeSpan.TotalSeconds + 5.0) / 5.0) * 5;
			}
			if (param == null)
			{
				LogEvent(sv, num.ToString());
			}
			else if (param != null && param2 == null)
			{
				LogEvent(sv, num.ToString(), param);
			}
			else if (param != null && param2 != null)
			{
				LogEvent(sv, num.ToString(), param, param2);
			}
		}
	}
}
