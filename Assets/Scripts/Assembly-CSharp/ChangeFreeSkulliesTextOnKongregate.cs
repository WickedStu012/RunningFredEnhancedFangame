using UnityEngine;

public class ChangeFreeSkulliesTextOnKongregate : MonoBehaviour
{
	private void Start()
	{
		if (ConfigParams.IsKongregate())
		{
			GetComponent<GUI3DText>().SetDynamicText(MonoBehaviorSingleton<GUI3DLocalization>.Instance.GetText("Unsorted", "FreeKreds", "!BAD_TEXT!"));
		}
	}
}
