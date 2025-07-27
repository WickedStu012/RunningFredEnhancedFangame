using UnityEngine;

public class WingsUnfoldCheckbox : MonoBehaviour
{
	private GUI3DCheckbox checkbox;

	private void OnEnable()
	{
		if (checkbox == null)
		{
			checkbox = GetComponent<GUI3DCheckbox>();
		}
		GameEventDispatcher.AddListener("OnWingsUnfold", OnUnfold);
		GameEventDispatcher.AddListener("OnWingsFold", OnFold);
	}

	private void OnDisable()
	{
		if (checkbox == null)
		{
			checkbox = GetComponent<GUI3DCheckbox>();
		}
		GameEventDispatcher.RemoveListener("OnWingsUnfold", OnUnfold);
		GameEventDispatcher.RemoveListener("OnWingsFold", OnFold);
	}

	private void OnFold(object sender, GameEvent evt)
	{
		Debug.Log("Fold");
	}

	private void OnUnfold(object sender, GameEvent evt)
	{
		Debug.Log("Unfold");
	}
}
