using UnityEngine;

public class DescriptionSetter : MonoBehaviour
{
	public GUI3DText Description;

	private GUI3DPageSlider pageSlider;

	private LocationItemInfo itemInfo;

	private void OnEnable()
	{
		if (pageSlider == null)
		{
			pageSlider = GetComponentInChildren<GUI3DPageSlider>();
		}
		pageSlider.PageChangeEvent += OnPageChange;
		if (itemInfo == null && PlayerAccount.Instance.CurrentChapterInfo != null)
		{
			itemInfo = PlayerAccount.Instance.CurrentChapterInfo;
			if (Description != null)
			{
				Description.SetDynamicText(itemInfo.Description);
			}
		}
	}

	private void OnDisable()
	{
		if (pageSlider == null)
		{
			pageSlider = GetComponentInChildren<GUI3DPageSlider>();
		}
		pageSlider.PageChangeEvent -= OnPageChange;
	}

	private void OnPageChange(GUI3DOnPageChange evt)
	{
		GUI3DObject[] items = evt.Page.GetItems();
		if (items != null && items.Length != 0)
		{
			itemInfo = null;
			GUI3DObject gUI3DObject = items[0];
			if (gUI3DObject is LevelItem)
			{
				itemInfo = (LocationItemInfo)((LevelItem)gUI3DObject).Item;
			}
			else if (gUI3DObject is ShopItem)
			{
				itemInfo = (LocationItemInfo)((ShopItem)gUI3DObject).Item;
			}
			if (itemInfo != null && Description != null)
			{
				Description.SetDynamicText(itemInfo.Description);
			}
		}
	}
}
