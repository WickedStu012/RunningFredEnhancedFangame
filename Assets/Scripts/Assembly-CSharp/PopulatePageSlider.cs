using UnityEngine;

public class PopulatePageSlider : MonoBehaviour, IGUI3DInit
{
	public GameObject ShopItemPrefab;

	public GameObject ItemPrefab;

	public GameObject PackItemPrefab;

	public string Type;

	public string TagFilter;

	public string BehaviourType;

	public GUI3DTransition transition;

	public Vector3 ItemScale = Vector3.one;

	private GUI3DObject[] items;

	private GUI3DPageSlider pageSlider;

	private string[] tags;

	private void Awake()
	{
	}

	public void Init()
	{
		Store.Instance.PurchaseSuccessEvent += OnPurchase;
		Populate();
		if (!(transition != null))
		{
			return;
		}
		transition.TransitionStartEvent += delegate
		{
			if (base.gameObject.activeInHierarchy && pageSlider != null)
			{
				pageSlider.ResetVisibility();
			}
		};
	}

	public void Clear(bool clearItems)
	{
		int childCount = base.gameObject.transform.childCount;
		for (int i = 0; i < childCount; i++)
		{
			Transform child = base.gameObject.transform.GetChild(i);
			Object.Destroy(child.gameObject);
		}
		if (clearItems && items != null)
		{
			for (int j = 0; j < items.Length; j++)
			{
				if (items[j] != null)
				{
					pageSlider.RemoveItem(items[j]);
					items[j] = null;
				}
			}
		}
		pageSlider.Clear();
		items = null;
	}

	public void Populate()
	{
		if (TagFilter != string.Empty)
		{
			tags = TagFilter.Split(';');
		}
		pageSlider = GetComponent<GUI3DPageSlider>();
		ItemInfo[] itemsByType = Store.Instance.GetItemsByType(Type);
		if (itemsByType != null)
		{
			items = new GUI3DObject[itemsByType.Length];
			if (itemsByType != null)
			{
				for (int i = 0; i < itemsByType.Length; i++)
				{
					ItemInfo itemInfo = itemsByType[i];
					bool flag = false;
					if (visibilityBehaviourIsShow(itemsByType[i]))
					{
						if (tags != null)
						{
							string[] array = tags;
							foreach (string text in array)
							{
								if (itemInfo.Tag == text)
								{
									flag = true;
									break;
								}
							}
						}
						else
						{
							flag = true;
						}
					}
					if (flag && (!(itemInfo is MarketItemInfo) || !((MarketItemInfo)itemInfo).OneTime || !itemInfo.Purchased))
					{
						if (itemInfo.Purchased && itemInfo.Enabled && BehaviourType != string.Empty && ItemPrefab != null)
						{
							items[i] = CreateItem(itemInfo, BehaviourType, ItemPrefab);
						}
						else if (itemInfo.PriceDollars == null || PackItemPrefab == null)
						{
							items[i] = CreateShopItem(itemInfo);
						}
						else
						{
							items[i] = CreateShopPackItem(itemInfo);
						}
						if (items[i] != null)
						{
							items[i].transform.localScale = ItemScale;
							pageSlider.AddItem(items[i]);
						}
					}
				}
			}
		}
		pageSlider.Refresh();
		pageSlider.ResetVisibility();
	}

	private bool visibilityBehaviourIsShow(ItemInfo itemInfo)
	{
		if (string.Compare(itemInfo.VisibilityBehaviour, "ALWAYS_INVISIBLE") == 0)
		{
			return false;
		}
		return (!itemInfo.Purchased || string.Compare(itemInfo.VisibilityBehaviour, "INVISIBLE_WHEN_IT_WAS_PURCHASED") != 0) && (itemInfo.Purchased || string.Compare(itemInfo.VisibilityBehaviour, "INVISIBLE_WHEN_IT_WAS_NOT_PURCHASED") != 0);
	}

	public virtual GUI3DObject CreateItem(ItemInfo itemInfo, string type, GameObject prefab)
	{
		GameObject gameObject = (GameObject)Object.Instantiate(prefab);
		PageSliderItem pageSliderItem = (PageSliderItem)gameObject.GetComponent(type);
		pageSliderItem.Create(itemInfo);
		return pageSliderItem;
	}

	protected virtual GUI3DObject CreatePageSliderItem<T>(ItemInfo itemInfo, GameObject prefab) where T : PageSliderItem
	{
		GameObject gameObject = Object.Instantiate(prefab) as GameObject;
		T component = gameObject.GetComponent<T>();
		component.Create(itemInfo);
		return component;
	}

	protected GUI3DObject CreateShopItem(ItemInfo itemInfo)
	{
		return CreatePageSliderItem<ShopItem>(itemInfo, ShopItemPrefab);
	}

	protected GUI3DObject CreateShopPackItem(ItemInfo itemInfo)
	{
		return CreatePageSliderItem<ShopItem>(itemInfo, PackItemPrefab);
	}

	private void OnPurchase(ItemInfo item)
	{
		if (item is MarketItemInfo)
		{
			if (((MarketItemInfo)item).OneTime)
			{
				for (int i = 0; i < items.Length; i++)
				{
					if (items[i] is ShopItem && ((ShopItem)items[i]).Item == item)
					{
						GUI3DObject item2 = items[i];
						pageSlider.RemoveItem(item2);
						return;
					}
				}
			}
			else if (((MarketItemInfo)item).Days > 0)
			{
				int days = ((MarketItemInfo)item).Days;
				if (PlayerPrefsWrapper.GetDaysSinceLastTweet() == -1 || PlayerPrefsWrapper.GetDaysSinceLastTweet() <= days)
				{
					for (int j = 0; j < items.Length; j++)
					{
						if (items[j] is ShopItem && ((ShopItem)items[j]).Item == item)
						{
							GUI3DObject item3 = items[j];
							pageSlider.RemoveItem(item3);
							return;
						}
					}
				}
			}
		}
		if (ItemPrefab != null)
		{
			for (int k = 0; k < items.Length; k++)
			{
				if (items[k] is ShopItem && ((ShopItem)items[k]).Item == item)
				{
					GUI3DObject item4 = items[k];
					pageSlider.ReplaceItem(item4, CreateItem(item, BehaviourType, ItemPrefab));
					break;
				}
			}
		}
		else
		{
			if (items == null)
			{
				return;
			}
			for (int l = 0; l < items.Length; l++)
			{
				if (items[l] is ShopItem)
				{
					ShopItem shopItem = (ShopItem)items[l];
					if (shopItem.Item == item)
					{
						shopItem.Purchased();
						break;
					}
				}
			}
		}
	}

	public GUI3DObject GetShopItemById(int itemInfoId)
	{
		for (int i = 0; i < items.Length; i++)
		{
			if (items[i] is ShopItem && ((ShopItem)items[i]).Item.Id == itemInfoId)
			{
				return items[i];
			}
		}
		return null;
	}
}
