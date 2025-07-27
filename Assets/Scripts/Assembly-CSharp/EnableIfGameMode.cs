using UnityEngine;

public class EnableIfGameMode : MonoBehaviour
{
	public PlayerAccount.GameMode[] GameModes;

	private GUI3DObject obj;

	private GUI3DText text;

	private void OnEnable()
	{
		obj = GetComponent<GUI3DObject>();
		text = GetComponent<GUI3DText>();
		if (!(obj != null) && !(text != null))
		{
			return;
		}
		PlayerAccount.GameMode[] gameModes = GameModes;
		foreach (PlayerAccount.GameMode gameMode in gameModes)
		{
			if (PlayerAccount.Instance.CurrentGameMode == gameMode)
			{
				return;
			}
		}
		if (obj != null)
		{
			obj.CreateOwnMesh = false;
			if (obj.GetComponent<Renderer>() != null)
			{
				obj.GetComponent<Renderer>().enabled = false;
			}
		}
		if (text != null)
		{
			text.SetDynamicText(string.Empty);
		}
	}
}
