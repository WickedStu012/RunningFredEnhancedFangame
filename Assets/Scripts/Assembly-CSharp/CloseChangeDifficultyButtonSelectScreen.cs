using UnityEngine;

public class CloseChangeDifficultyButtonSelectScreen : MonoBehaviour
{
	private GUI3DButton button;

	private void Awake()
	{
		if (button == null)
		{
			button = GetComponentInChildren<GUI3DButton>();
		}
	}

	private void Update()
	{
		if (Input.GetKeyUp(KeyCode.Escape))
		{
			GUI3DManager.Instance.Activate("SelectChapterSurvivalEx", true, true);
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
		GUI3DManager.Instance.Activate("SelectChapterSurvivalEx", true, true);
	}
}
