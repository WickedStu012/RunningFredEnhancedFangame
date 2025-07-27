using UnityEngine;

public class FreeSkulliesPanel : MonoBehaviour
{
	public GUI3DSlideTransition transition;

	public GUI3DButton CloseButton;

	public GUI3DButton OpenButton;

	private GUI3DObject guiObject;

	private bool DebugMode;

	private void Awake()
	{
		CloseButton.ClickEvent += Hide;
		OpenButton.ClickEvent += Show;
	}

	public void Show(GUI3DOnClickEvent evt)
	{
		transition.StartIntroTransition();
	}

	public void Hide(GUI3DOnClickEvent evt)
	{
		transition.StartOutroTransition();
	}

	private void OnDisable()
	{
		Hide(null);
	}
}
