using System.Collections.Generic;
using UnityEngine;

public class SceneParamsManager
{
	private List<object> queue;

	private Dictionary<string, object> dic;

	private bool locked;

	private static SceneParamsManager instance;

	public static SceneParamsManager Instance
	{
		get
		{
			if (instance == null)
			{
				instance = new SceneParamsManager();
			}
			return instance;
		}
	}

	public bool IsEmpty
	{
		get
		{
			return queue.Count == 0;
		}
	}

	private SceneParamsManager()
	{
		queue = new List<object>();
		dic = new Dictionary<string, object>();
	}

	public void Push(object param)
	{
		if (!locked)
		{
			queue.Add(param);
		}
		else
		{
			Debug.Log("Locked: " + param);
		}
	}

	public object Pop()
	{
		object result = queue[queue.Count - 1];
		queue.RemoveAt(queue.Count - 1);
		return result;
	}

	public void Purge()
	{
		queue.Clear();
	}

	public void Lock()
	{
		locked = true;
	}

	public void Unlock()
	{
		locked = false;
	}

	public void SetInt(string key, int val)
	{
		if (dic.ContainsKey(key))
		{
			dic.Remove(key);
		}
		dic.Add(key, val);
	}

	public void SetFloat(string key, float val)
	{
		if (dic.ContainsKey(key))
		{
			dic.Remove(key);
		}
		dic.Add(key, val);
	}

	public void SetString(string key, string val)
	{
		if (dic.ContainsKey(key))
		{
			dic.Remove(key);
		}
		dic.Add(key, val);
	}

	public void SetBool(string key, bool val)
	{
		if (dic.ContainsKey(key))
		{
			dic.Remove(key);
		}
		dic.Add(key, val);
	}

	public void SetObject(string key, object val)
	{
		if (dic.ContainsKey(key))
		{
			dic.Remove(key);
		}
		dic.Add(key, val);
	}

	public int GetInt(string key, int defValue)
	{
		return GetInt(key, defValue, false);
	}

	public int GetInt(string key, int defValue, bool clearValue)
	{
		if (dic.ContainsKey(key))
		{
			int result = (int)dic[key];
			if (clearValue)
			{
				dic.Remove(key);
			}
			return result;
		}
		return defValue;
	}

	public float GetFloat(string key, float defValue)
	{
		return GetFloat(key, defValue, false);
	}

	public float GetFloat(string key, float defValue, bool clearValue)
	{
		if (dic.ContainsKey(key))
		{
			float result = (float)dic[key];
			if (clearValue)
			{
				dic.Remove(key);
			}
			return result;
		}
		return defValue;
	}

	public string GetString(string key, string defValue)
	{
		return GetString(key, defValue, false);
	}

	public string GetString(string key, string defValue, bool clearValue)
	{
		if (dic.ContainsKey(key))
		{
			string result = dic[key] as string;
			if (clearValue)
			{
				dic.Remove(key);
			}
			return result;
		}
		return defValue;
	}

	public bool GetBool(string key, bool defValue)
	{
		return GetBool(key, defValue, false);
	}

	public bool GetBool(string key, bool defValue, bool clearValue)
	{
		if (dic.ContainsKey(key))
		{
			bool result = (bool)dic[key];
			if (clearValue)
			{
				dic.Remove(key);
			}
			return result;
		}
		return defValue;
	}

	public object GetObject(string key, object defValue)
	{
		return GetObject(key, defValue, false);
	}

	public object GetObject(string key, object defValue, bool clearValue)
	{
		if (dic.ContainsKey(key))
		{
			object result = dic[key];
			if (clearValue)
			{
				dic.Remove(key);
			}
			return result;
		}
		return defValue;
	}
}
