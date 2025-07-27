using UnityEngine;

public class GUI3DOnClosePopup : GUI3DEvent
{
	public const string EventName = "GUI3DOnClosePopup";

	public bool Result;

	public GUI3DOnClosePopup()
		: base(null, "GUI3DOnClosePopup")
	{
	}

	public GUI3DOnClosePopup(bool result)
		: base(null, "GUI3DOnClosePopup")
	{
		Result = result;
	}

	public GUI3DOnClosePopup(Object target, bool result)
		: base(target, "GUI3DOnClosePopup")
	{
		Result = result;
	}
}
