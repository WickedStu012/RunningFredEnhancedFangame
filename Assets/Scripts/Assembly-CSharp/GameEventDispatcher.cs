using System.Collections.Generic;
using UnityEngine;

public class GameEventDispatcher : MonoBehaviour
{
	private static Dictionary<string, List<OnGameEvent>> eventCallbacks = new Dictionary<string, List<OnGameEvent>>();

	private static bool dispatching = false;

	private static List<KeyValuePair<string, OnGameEvent>> purge = new List<KeyValuePair<string, OnGameEvent>>();

	private void OnDisable()
	{
	}

	public static void ClearSceneListeners()
	{
		eventCallbacks.Clear();
	}

	public static void AddListener(string evt, OnGameEvent callback)
	{
		if (!eventCallbacks.ContainsKey(evt))
		{
			eventCallbacks.Add(evt, new List<OnGameEvent>());
		}
		if (!eventCallbacks[evt].Contains(callback))
		{
			eventCallbacks[evt].Add(callback);
		}
	}

	public static void RemoveListener(string evtName, OnGameEvent callback)
	{
		if (!dispatching)
		{
			if (eventCallbacks.ContainsKey(evtName))
			{
				eventCallbacks[evtName].Remove(callback);
			}
		}
		else
		{
			purge.Add(new KeyValuePair<string, OnGameEvent>(evtName, callback));
		}
	}

	public static void RemoveListenerNow(string evtName, OnGameEvent callback)
	{
		if (eventCallbacks.ContainsKey(evtName))
		{
			eventCallbacks[evtName].Remove(callback);
		}
	}

	public static void Dispatch(object sender, GameEvent evt)
	{
		dispatching = true;
		if (eventCallbacks.ContainsKey(evt.Name))
		{
			for (int i = 0; i < eventCallbacks[evt.Name].Count; i++)
			{
				eventCallbacks[evt.Name][i](sender, evt);
			}
		}
		if (purge.Count != 0)
		{
			foreach (KeyValuePair<string, OnGameEvent> item in purge)
			{
				eventCallbacks[item.Key].Remove(item.Value);
			}
			purge.Clear();
		}
		dispatching = false;
	}
}
