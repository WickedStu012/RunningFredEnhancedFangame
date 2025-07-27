using UnityEngine;

public class GUI3DOnPageChange : GUI3DEvent
{
	public const string EventName = "GUI3DOnPageChange";

	public int PageNum;

	public GUI3DPage Page;

	public GUI3DOnPageChange()
		: base(null, "GUI3DOnPageChange")
	{
	}

	public GUI3DOnPageChange(Object target)
		: base(target, "GUI3DOnPageChange")
	{
	}
}
