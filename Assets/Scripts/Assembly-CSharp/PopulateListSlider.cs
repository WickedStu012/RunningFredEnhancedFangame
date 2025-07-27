using UnityEngine;

public class PopulateListSlider : MonoBehaviour
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
		ChallengeItemInfo[] array = ItemsLoader.Load<ChallengeItemInfo>(ListName);
		ChallengeItemInfo[] array2 = array;
		foreach (ChallengeItemInfo item in array2)
		{
			GUI3DObject item2 = CreateItem(item);
			slider.AddItem(item2);
		}
		slider.RelocateItems();
	}

	public GUI3DObject CreateItem(ChallengeItemInfo item)
	{
		GameObject gameObject = Object.Instantiate(Prefab) as GameObject;
		gameObject.layer = base.gameObject.layer;
		ChallengeItem component = gameObject.GetComponent<ChallengeItem>();
		if (component != null)
		{
			component.ItemInfo = item;
			component.Refresh();
		}
		else
		{
			CollectableItem component2 = gameObject.GetComponent<CollectableItem>();
			component2.ItemInfo = item;
			component2.Refresh();
		}
		return gameObject.GetComponent<GUI3DObject>();
	}
}
