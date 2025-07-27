using UnityEngine;

public class ChangeDifficultyButtonSelectScreen : MonoBehaviour
{
	private GUI3DButton button;

	public GUI3DText buttonText;

	public GUI3DObject buttonIcon;

	private void Awake()
	{
		if (button == null)
		{
			button = GetComponentInChildren<GUI3DButton>();
		}
	}

	private void OnEnable()
	{
		if (button == null)
		{
			button = GetComponentInChildren<GUI3DButton>();
		}
		button.ReleaseEvent += OnRelease;
	}

	private void OnDisable()
	{
		if (button == null)
		{
			button = GetComponentInChildren<GUI3DButton>();
		}
		if (button != null)
		{
			button.ReleaseEvent -= OnRelease;
		}
	}

	private void OnRelease(GUI3DOnReleaseEvent evt)
	{
		GUI3DManager.Instance.Activate("SelectDifficultyEx", true, false);
	}
}
