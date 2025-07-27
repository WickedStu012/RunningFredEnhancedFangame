using UnityEngine;

public class SetGrimmiesCountOnPopup : MonoBehaviour
{
	public GUI3DText description;

	public GUI3DText hint;

	public GUI3DText hintShadow;

	private GUI3DText grimmiesPicked;

	private void OnEnable()
	{
		grimmiesPicked = GetComponent<GUI3DText>();
		int grimmyIdolPickedCount = PlayerAccount.Instance.GetGrimmyIdolPickedCount();
		if (grimmyIdolPickedCount >= ConfigParams.IronFredGrimmyGoal)
		{
			description.SetDynamicText(MonoBehaviorSingleton<GUI3DLocalization>.Instance.GetText("Unsorted", "IronFredUnlocked", "!BAD_TEXT!"));
			if (hint != null)
			{
				hint.GetComponent<Renderer>().enabled = false;
			}
			if (hintShadow != null)
			{
				hintShadow.GetComponent<Renderer>().enabled = false;
			}
		}
		grimmiesPicked.SetDynamicText(string.Format("{0}/{1}", grimmyIdolPickedCount, ConfigParams.IronFredGrimmyGoal));
	}
}
