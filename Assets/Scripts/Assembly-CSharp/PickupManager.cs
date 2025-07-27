using UnityEngine;

internal class PickupManager : MonoBehaviour
{
	private static int goldCoinCount;

	private static int silverCoinCount;

	private void Awake()
	{
		getTotalCoinCount();
	}

	private void getTotalCoinCount()
	{
		PickeableItem[] array = Object.FindObjectsOfType(typeof(PickeableItem)) as PickeableItem[];
		goldCoinCount = 0;
		silverCoinCount = 0;
		for (int i = 0; i < array.Length; i++)
		{
			if (array[i].itemType == PickeableItem.ItemType.GOLD)
			{
				goldCoinCount++;
			}
			else if (array[i].itemType == PickeableItem.ItemType.SILVER)
			{
				silverCoinCount++;
			}
		}
	}

	public static int GetGoldCoinCount()
	{
		return goldCoinCount;
	}

	public static int GetSilverCoinCount()
	{
		return silverCoinCount;
	}
}
