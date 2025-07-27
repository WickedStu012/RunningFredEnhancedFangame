using System.Collections.Generic;

public class GameEventList
{
	private static Dictionary<string, GameEvent> dic = new Dictionary<string, GameEvent>();

	public static void RegisterEvent(GameEvent evn)
	{
		dic.Add(evn.Name, evn);
	}

	public static void UnregisterEvent(GameEvent evn)
	{
		if (dic.ContainsKey(evn.Name))
		{
			dic.Remove(evn.Name);
		}
	}

	public static GameEvent[] GetAllEvents()
	{
		GameEvent[] array = new GameEvent[dic.Count];
		Dictionary<string, GameEvent>.Enumerator enumerator = dic.GetEnumerator();
		int num = 0;
		while (enumerator.MoveNext())
		{
			array[num++] = enumerator.Current.Value;
		}
		return array;
	}
}
