using UnityEngine;

public class GUI3DEvent
{
	public Object Target;

	public string Name;

	public GUI3DEvent()
	{
	}

	public GUI3DEvent(Object target, string name)
	{
		Target = target;
		Name = name;
	}
}
