using UnityEngine;

public class SelectChapterButton : MonoBehaviour
{
	public string GUIToSwitch;

	public string GUIToSwitchIfSurvival;

	private GUI3DButton button;

	private GUI3DPanel panel;

	private GUI3DTransition transition;

	private LevelItem item;

	private void OnEnable()
	{
		if (item == null)
		{
			item = GetComponentInChildren<LevelItem>();
		}
		if (button == null)
		{
			button = GetComponentInChildren<GUI3DButton>();
		}
		button.ClickEvent += OnClick;
	}

	private void OnDisable()
	{
		if (button == null)
		{
			button = GetComponentInChildren<GUI3DButton>();
		}
		if (button != null)
		{
			button.ClickEvent -= OnClick;
		}
	}

	private void OnClick(GUI3DOnClickEvent evt)
	{
		PlayerAccount.Instance.SelectChapter((LocationItemInfo)item.Item);
		if (panel == null)
		{
			panel = button.GetPanel();
		}
		if (panel != null)
		{
			if (transition == null)
			{
				transition = panel.GetComponent<GUI3DTransition>();
			}
			if (transition != null)
			{
				transition.TransitionEndEvent += OnTransitionEnd;
				transition.StartTransition();
			}
			else
			{
				GUI3DManager.Instance.Activate(GUIToSwitch, true, false);
			}
		}
	}

	private void OnTransitionEnd(GUI3DOnTransitionEndEvent evt)
	{
		transition.TransitionEndEvent -= OnTransitionEnd;
		if (item.Tag == "Survival")
		{
			GUI3DManager.Instance.Activate(GUIToSwitchIfSurvival, true, false);
		}
		else
		{
			GUI3DManager.Instance.Activate(GUIToSwitch, true, false);
		}
	}
}
