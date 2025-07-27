using UnityEngine;

public class WriteCheatButton : MonoBehaviour
{
	public CheatConsoleWheel wheel1;

	public CheatConsoleWheel wheel2;

	public CheatConsoleWheel wheel3;

	public CheatConsoleWheel wheel4;

	private GUI3DButton button;

	private void OnEnable()
	{
		if (button == null)
		{
			button = GetComponent<GUI3DButton>();
		}
		button.ReleaseEvent += OnRelease;
	}

	private void OnDisable()
	{
		if (button == null)
		{
			button = GetComponent<GUI3DButton>();
		}
		button.ReleaseEvent -= OnRelease;
	}

	private void OnRelease(GUI3DOnReleaseEvent evt)
	{
		string text = string.Format("{0}{1}{2}{3}", wheel1.GetCode(), wheel2.GetCode(), wheel3.GetCode(), wheel4.GetCode());
		int num = int.Parse(CheatConsoleServerTest.Instance.itemId);
		int num2 = int.Parse(CheatConsoleServerTest.Instance.itemCount);
		int num3 = int.Parse(CheatConsoleServerTest.Instance.itemUseCount);
		int num4 = int.Parse(CheatConsoleServerTest.Instance.version);
		Debug.Log(string.Format("The code is: {0} iid: {1} icnt: {2} iucnt: {3} ver: {4}", text, num, num2, num3, num4));
		CheatConsoleServer.WriteCheat(text, num, num2, num3, num4, writeCheatRes);
	}

	private void writeCheatRes(bool res, string resStr)
	{
		Debug.Log(string.Format("Response: {0}", res));
	}
}
