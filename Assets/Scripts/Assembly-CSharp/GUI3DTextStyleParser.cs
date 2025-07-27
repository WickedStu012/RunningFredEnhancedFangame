using System.Collections.Generic;
using UnityEngine;

public class GUI3DTextStyleParser : ParserBase
{
	private Dictionary<string, object> property = new Dictionary<string, object>();

	public void Write(string filename)
	{
		Write(filename, property);
	}

	public float GetValueAsFloat(string varName)
	{
		if (!property.ContainsKey(varName.ToLower()))
		{
			Debug.LogError("Error: " + varName + " doesn't exist.");
		}
		return (float)property[varName.ToLower()];
	}

	public int GetValueAsInt(string varName)
	{
		if (!property.ContainsKey(varName.ToLower()))
		{
			Debug.LogError("Error: " + varName + " doesn't exist.");
		}
		return (int)(float)property[varName.ToLower()];
	}

	public string GetValueAsString(string varName)
	{
		if (!property.ContainsKey(varName.ToLower()))
		{
			Debug.LogError("Error: " + varName + " doesn't exist.");
		}
		return (string)property[varName.ToLower()];
	}

	public void SetValue(string varName, int value)
	{
		property[varName.ToLower()] = (float)value;
	}

	public void SetValue(string varName, float value)
	{
		property[varName.ToLower()] = value;
	}

	public void SetValue(string varName, string value)
	{
		property[varName.ToLower()] = value;
	}

	protected override void OnAssign(string varName, bool value)
	{
		property[varName.ToLower()] = value;
	}

	protected override void OnAssign(string varName, string value)
	{
		property[varName.ToLower()] = value;
	}

	protected override void OnAssign(string varName, float value)
	{
		property[varName.ToLower()] = value;
	}

	protected override void OnOpenGroup(string group)
	{
	}

	protected override void OnCloseGroup(string group)
	{
	}

	protected override void OnError(string error)
	{
		Debug.LogError(error);
	}
}
