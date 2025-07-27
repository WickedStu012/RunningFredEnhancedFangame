using UnityEngine;

public class ButtonChangeLevel : MonoBehaviour
{
	public SndIdMenu Click = SndIdMenu.SND_MENU_CLICK;

	private GUI3DInteractiveObject button;

	private void OnEnable()
	{
		if (button == null)
		{
			button = GetComponent<GUI3DInteractiveObject>();
		}
		button.ReleaseEvent += OnRelease;
	}

	private void OnDisable()
	{
		if (button == null)
		{
			button = GetComponent<GUI3DInteractiveObject>();
		}
		button.ReleaseEvent -= OnRelease;
	}

	private void OnRelease(GUI3DOnReleaseEvent evt)
	{
		if (PlayerAccount.Instance.CurrentGameMode == PlayerAccount.GameMode.Adventure)
		{
			GUI3DManager.Instance.Activate("SelectChapterAdventure", true, true);
		}
		else
		{
			GUI3DManager.Instance.Activate("SelectChapterSurvival", true, true);
		}
	}
}
