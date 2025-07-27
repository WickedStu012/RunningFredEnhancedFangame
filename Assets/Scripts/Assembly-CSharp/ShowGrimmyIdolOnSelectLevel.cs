using UnityEngine;

public class ShowGrimmyIdolOnSelectLevel : MonoBehaviour
{
	public int levelLocalNum;

	private GUI3DObject icon;

	private void Start()
	{
	}

	private void OnEnable()
	{
		if (icon == null)
		{
			icon = GetComponent<GUI3DObject>();
		}
		int num = 0;
		if (PlayerPrefsWrapper.GetCurrentChapterId() == Store.Instance.GetItem(1002).Id)
		{
			num = 10;
		}
		else if (PlayerPrefsWrapper.GetCurrentChapterId() == Store.Instance.GetItem(1004).Id)
		{
			num = 20;
		}
		if (PlayerPrefsWrapper.IsGrimmyIdolTaken(num + levelLocalNum))
		{
			icon.RefreshMaterial("GrimmyIdol-64-on");
		}
		else
		{
			icon.RefreshMaterial("GrimmyIdol-64-off");
		}
	}
}
