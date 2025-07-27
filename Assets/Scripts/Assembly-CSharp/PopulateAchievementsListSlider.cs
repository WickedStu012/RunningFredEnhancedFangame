using UnityEngine;

public class PopulateAchievementsListSlider : MonoBehaviour
{
	public string ListName;

	public GameObject Prefab;

	private GUI3DListSlider slider;

	private void Awake()
	{
		slider = GetComponent<GUI3DListSlider>();
		Populate();
	}

	public void Populate()
	{
		AchievementItemInfo[] array = ItemsLoader.Load<AchievementItemInfo>(ListName);
		AchievementItemInfo[] array2 = array;
		foreach (AchievementItemInfo item in array2)
		{
			GUI3DObject item2 = CreateItem(item);
			slider.AddItem(item2);
		}
		slider.RelocateItems();
	}

	public GUI3DObject CreateItem(AchievementItemInfo item)
	{
		GameObject gameObject = Object.Instantiate(Prefab) as GameObject;
		AchievementItem component = gameObject.GetComponent<AchievementItem>();
		if (component != null)
		{
			component.ItemInfo = item;
			component.Refresh();
		}
		return gameObject.GetComponent<GUI3DObject>();
	}
}
