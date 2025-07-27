using System;
using System.Collections.Generic;
using UnityEngine;

public class Store : MonoBehaviour
{
	public enum PurchaseResult
	{
		Ok = 0,
		Error = 1,
		Cancel = 2
	}

	public delegate void OnPurchaseEvent(ItemInfo item);

	public delegate void OnRefreshItems();

	public delegate void OnPurchase(PurchaseResult res, string error);

	private const float DISCOUNT = 0.7f;

	private const string PublicKeyIOS = "7f3beabda1e240f7829fc5f9d8fefa67";

	private const string PublicKeyAndroid = "MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAui6J6c8IFYkmtw6NMuBCsgbSqXlX3J+aFeTWNzX6TEY3bkpYallCG7Q6gyWZYcz3VCyKWQaokgd+RVDelDnOVuqCEJEdeVauKVitRpz92npfWzZN51r0SSvgdJZpx5yCx7VaJPPuD4vOLZLDkBM+XJa1yne4tFKOo7QuhCsRzYk9UkLqYu0lmLlLeugggUt7tQ0ekAnWFxNrvBuLjJsq0d4zdy5uM7A/quBMKHJSeQ2reP77NbnJcRLmkTH7bL2iD2QwBn0Di5SXSNKHel7VF1ntiKXH+jnsWe+lBAFS2JgHdD5gv+PuE7jCWTwoi25IlRYRJv7oVedlwTmgj4TSawIDAQAB";

	private const float TotalTimer = 3f;

	private const int RECEIPT_VALIDATION_MAX_NUMBER = 3;

	private const float RECEIPT_VALIDATION_RETRY_TIME = 0.5f;

	private static Store instance;

	private List<ItemInfo> items = new List<ItemInfo>();

	private Dictionary<int, ItemInfo> itemsById = new Dictionary<int, ItemInfo>();

	private Dictionary<string, List<ItemInfo>> itemsByType = new Dictionary<string, List<ItemInfo>>();

	private Dictionary<string, List<ItemInfo>> itemsByTag = new Dictionary<string, List<ItemInfo>>();

	private List<ItemInfo> mandatoryItems = new List<ItemInfo>();

	private Dictionary<string, MarketItemInfo> marketItemsById = new Dictionary<string, MarketItemInfo>();

	private Dictionary<string, LocationItemInfo> locationItemsById = new Dictionary<string, LocationItemInfo>();

	private Dictionary<string, AvatarItemInfo> avatarItemsById = new Dictionary<string, AvatarItemInfo>();

	private OnPurchase onPurchase;

	private string[] productIdArray;

	private bool purchasingOpenURL;

	private float timer;

	private MarketItemInfo marketItem;

	private string lastProductId;

	private int receiptValidateTryNumber;

	private string lastReceipt;

	private int lastQuantity;

	public static bool DebugMode;

	public static Store Instance
	{
		get
		{
			if (instance == null)
			{
				instance = UnityEngine.Object.FindObjectOfType(typeof(Store)) as Store;
				if (instance == null)
				{
					GameObject gameObject = new GameObject();
					instance = gameObject.AddComponent<Store>();
					gameObject.name = "Store";
					instance.Init();
				}
			}
			return instance;
		}
	}

	public event OnPurchaseEvent PurchaseSuccessEvent;

	public event OnPurchaseEvent PurchaseFailedEvent;

	public event OnRefreshItems RefreshItemsEvent;

	private void OnDestroy()
	{
		if (SalesManager.Instance != null)
		{
			SalesManager.Instance.OnSaleRequest -= OnCheckOnSales;
		}
		instance = null;
	}

	private void OnDisable()
	{
	}

	private void Start()
	{
		RefreshItems();
	}

	private void Init()
	{
		PlayerPrefsWrapper.OnGameDataChangeEvent += OnGameDataChange;
		items.Clear();
		itemsByTag.Clear();
		itemsByType.Clear();
		ItemInfo[] itemsArray = ItemsLoader.Load<ItemInfo>("Shop/itemlist");
		AddItems(itemsArray);
		itemsArray = ItemsLoader.Load<AvatarItemInfo>("Shop/avatarList");
		AddItems(itemsArray);
		itemsArray = ItemsLoader.Load<MarketItemInfo>("Shop/skulliesList");
		AddItems(itemsArray);
		itemsArray = ItemsLoader.Load<LocationItemInfo>("Shop/chaptersList");
		AddItems(itemsArray);
		itemsArray = ItemsLoader.Load<ChallengeItemInfo>("Shop/challengesList");
		AddItems(itemsArray);
		foreach (ItemInfo item in items)
		{
			itemsById[item.Id] = item;
			if (item.RequieredByLevel != 0)
			{
				mandatoryItems.Add(item);
			}
		}
		if (PlayerAccount.Instance != null)
		{
			List<string> list = new List<string>();
			string text = string.Empty;
			foreach (ItemInfo item2 in items)
			{
				if (!(item2 is MarketItemInfo))
				{
					OnSale currentSale = SalesManager.Instance.CurrentSale;
					if (currentSale != null && (currentSale.ItemId == -1 || item2.Id == currentSale.ItemId))
					{
						item2.Price = (int)((float)item2.Price * currentSale.Discount);
						item2.Price1 = (int)((float)item2.Price1 * currentSale.Discount);
						item2.Price2 = (int)((float)item2.Price2 * currentSale.Discount);
						item2.Price3 = (int)((float)item2.Price3 * currentSale.Discount);
						item2.Price4 = (int)((float)item2.Price4 * currentSale.Discount);
					}
					if (PlayerAccount.Instance.IsDiscountActive())
					{
						item2.Price = (int)((float)item2.Price * 0.7f);
						item2.Price1 = (int)((float)item2.Price1 * 0.7f);
						item2.Price2 = (int)((float)item2.Price2 * 0.7f);
						item2.Price3 = (int)((float)item2.Price3 * 0.7f);
						item2.Price4 = (int)((float)item2.Price4 * 0.7f);
					}
				}
				else
				{
					MarketItemInfo marketItemInfo = (MarketItemInfo)item2;
					if (marketItemInfo.Tag == "Skullies")
					{
						marketItemsById[marketItemInfo.MarketId] = marketItemInfo;
						text = ((!(text == string.Empty)) ? (text + ", " + marketItemInfo.AndroidMarketId) : marketItemInfo.AndroidMarketId);
						list.Add(marketItemInfo.AndroidMarketId);
						if (Application.platform == RuntimePlatform.IPhonePlayer)
						{
							marketItemInfo.PriceDollars = PlayerPrefs.GetString(marketItemInfo.MarketId + "CurrencySymbol", "...") + PlayerPrefs.GetString(marketItemInfo.MarketId + "Price", string.Empty);
						}
					}
				}
				LocationItemInfo locationItemInfo = item2 as LocationItemInfo;
				if (locationItemInfo != null && locationItemInfo.MarketId != null)
				{
					locationItemsById[locationItemInfo.MarketId] = locationItemInfo;
					text = ((!(text == string.Empty)) ? (text + ", " + locationItemInfo.AndroidMarketId) : locationItemInfo.AndroidMarketId);
					list.Add(locationItemInfo.AndroidMarketId);
					if (Application.platform == RuntimePlatform.IPhonePlayer)
					{
						string text2 = PlayerPrefs.GetString(locationItemInfo.MarketId + "CurrencySymbol", "$");
						string text3 = PlayerPrefs.GetString(locationItemInfo.MarketId + "Price", string.Empty);
						if (text3 != string.Empty)
						{
							locationItemInfo.PriceDollars = text2 + text3;
						}
					}
					else if (Application.platform == RuntimePlatform.OSXPlayer)
					{
						string text4 = PlayerPrefs.GetString(locationItemInfo.MacMarketId + "CurrencySymbol", "$");
						string text5 = PlayerPrefs.GetString(locationItemInfo.MacMarketId + "Price", string.Empty);
						if (text5 != string.Empty)
						{
							locationItemInfo.PriceDollars = text4 + text5;
						}
					}
				}
				else
				{
					AvatarItemInfo avatarItemInfo = item2 as AvatarItemInfo;
					if (avatarItemInfo != null && avatarItemInfo.MarketId != null)
					{
						avatarItemsById[avatarItemInfo.MarketId] = avatarItemInfo;
						text = ((!(text == string.Empty)) ? (text + ", " + avatarItemInfo.AndroidMarketId) : avatarItemInfo.AndroidMarketId);
						list.Add(avatarItemInfo.AndroidMarketId);
						if (Application.platform == RuntimePlatform.IPhonePlayer)
						{
							string text6 = PlayerPrefs.GetString(avatarItemInfo.MarketId + "CurrencySymbol", "$");
							string text7 = PlayerPrefs.GetString(avatarItemInfo.MarketId + "Price", string.Empty);
							if (text7 != string.Empty)
							{
								avatarItemInfo.PriceDollars = text6 + text7;
							}
						}
						else if (Application.platform == RuntimePlatform.OSXPlayer)
						{
							string text8 = PlayerPrefs.GetString(avatarItemInfo.MacMarketId + "CurrencySymbol", "$");
							string text9 = PlayerPrefs.GetString(avatarItemInfo.MacMarketId + "Price", string.Empty);
							if (text9 != string.Empty)
							{
								avatarItemInfo.PriceDollars = text8 + text9;
							}
						}
					}
				}
				if (IsItemPurchased(item2))
				{
					item2.Purchased = true;
				}
			}
			productIdArray = new string[list.Count];
			for (int i = 0; i < list.Count; i++)
			{
				productIdArray[i] = list[i];
			}
		}
		CheckOnSales();
	}

	public void InitBeLordInApp()
	{
		if (DebugMode)
		{
			Debug.Log("Store.InitBeLordInApp()");
		}
		BeLordInApp.Instance.Init("MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAui6J6c8IFYkmtw6NMuBCsgbSqXlX3J+aFeTWNzX6TEY3bkpYallCG7Q6gyWZYcz3VCyKWQaokgd+RVDelDnOVuqCEJEdeVauKVitRpz92npfWzZN51r0SSvgdJZpx5yCx7VaJPPuD4vOLZLDkBM+XJa1yne4tFKOo7QuhCsRzYk9UkLqYu0lmLlLeugggUt7tQ0ekAnWFxNrvBuLjJsq0d4zdy5uM7A/quBMKHJSeQ2reP77NbnJcRLmkTH7bL2iD2QwBn0Di5SXSNKHel7VF1ntiKXH+jnsWe+lBAFS2JgHdD5gv+PuE7jCWTwoi25IlRYRJv7oVedlwTmgj4TSawIDAQAB");
	}

	private void OnGameDataChange()
	{
		foreach (ItemInfo item in items)
		{
			item.Upgrades = PlayerPrefsWrapper.ItemUpgrades(item.Id);
			item.Count = PlayerPrefsWrapper.ItemCount(item.Id);
			item.Purchased = IsItemPurchased(item);
		}
	}

	private void OnProductInfo(List<BeLordProductInfo> products)
	{
		if (DebugMode)
		{
			Debug.Log("Store.OnProductInfo() products is null" + (products == null));
		}
		if (products != null)
		{
			foreach (BeLordProductInfo product in products)
			{
				PlayerPrefs.SetString(product.Id + "CurrencySymbol", product.CurrencySymbol);
				PlayerPrefs.SetString(product.Id + "Price", product.Price);
				if (marketItemsById.ContainsKey(product.Id))
				{
					marketItemsById[product.Id].PriceDollars = product.CurrencySymbol + product.Price;
				}
				else if (locationItemsById.ContainsKey(product.Id))
				{
					locationItemsById[product.Id].PriceDollars = product.CurrencySymbol + product.Price;
				}
				else if (avatarItemsById.ContainsKey(product.Id))
				{
					avatarItemsById[product.Id].PriceDollars = product.CurrencySymbol + product.Price;
				}
			}
		}
		if (this.RefreshItemsEvent != null)
		{
			this.RefreshItemsEvent();
		}
	}

	private void OnRequestProductError(string error)
	{
		if (DebugMode)
		{
			Debug.Log("Store.OnRequestProductError() error: " + error);
		}
		Debug.Log(error);
	}

	private void AddItems(ItemInfo[] itemsArray)
	{
		foreach (ItemInfo itemInfo in itemsArray)
		{
			if (!(itemInfo is MarketItemInfo) || !(((MarketItemInfo)itemInfo).Tag == "Tweeter") || ((PlayerPrefsWrapper.GetDaysSinceLastTweet() == -1 || PlayerPrefsWrapper.GetDaysSinceLastTweet() >= ((MarketItemInfo)itemInfo).Days) && (!(BeLordSocial.Instance != null) || BeLordSocial.Instance.IsSocialApiAvailable())))
			{
				itemInfo.Upgrades = PlayerPrefsWrapper.ItemUpgrades(itemInfo.Id);
				itemInfo.Count = PlayerPrefsWrapper.ItemCount(itemInfo.Id);
				itemInfo.OrigPrice = itemInfo.GetPrice();
				itemInfo.OrigPrice1 = itemInfo.Price1;
				itemInfo.OrigPrice2 = itemInfo.Price2;
				itemInfo.OrigPrice3 = itemInfo.Price3;
				itemInfo.OrigPrice4 = itemInfo.Price4;
				items.Add(itemInfo);
				if (!itemsByTag.ContainsKey(itemInfo.Tag))
				{
					itemsByTag[itemInfo.Tag] = new List<ItemInfo>();
				}
				itemsByTag[itemInfo.Tag].Add(itemInfo);
				if (!itemsByType.ContainsKey(itemInfo.Type))
				{
					itemsByType[itemInfo.Type] = new List<ItemInfo>();
				}
				itemsByType[itemInfo.Type].Add(itemInfo);
			}
		}
	}

	private void OnEnable()
	{
		if (!(SalesManager.Instance == null))
		{
			SalesManager.Instance.OnSaleRequest += OnCheckOnSales;
		}
	}

	public void RefreshItems()
	{
		if (SalesManager.Instance == null)
		{
			return;
		}
		if (PlayerAccount.Instance != null && PlayerAccount.Instance.IsDiscountActive())
		{
			for (int i = 0; i < items.Count; i++)
			{
				ItemInfo itemInfo = items[i];
				if (!(itemInfo is MarketItemInfo))
				{
					OnSale currentSale = SalesManager.Instance.CurrentSale;
					if (currentSale != null && (currentSale.ItemId == -1 || itemInfo.Id == currentSale.ItemId))
					{
						itemInfo.Price = (int)((float)itemInfo.OrigPrice * currentSale.Discount);
						itemInfo.Price1 = (int)((float)itemInfo.OrigPrice1 * currentSale.Discount);
						itemInfo.Price2 = (int)((float)itemInfo.OrigPrice2 * currentSale.Discount);
						itemInfo.Price3 = (int)((float)itemInfo.OrigPrice3 * currentSale.Discount);
						itemInfo.Price4 = (int)((float)itemInfo.OrigPrice4 * currentSale.Discount);
					}
					itemInfo.Price = (int)((float)itemInfo.GetPrice() * 0.7f);
					itemInfo.Price1 = (int)((float)itemInfo.Price1 * 0.7f);
					itemInfo.Price2 = (int)((float)itemInfo.Price2 * 0.7f);
					itemInfo.Price3 = (int)((float)itemInfo.Price3 * 0.7f);
					itemInfo.Price4 = (int)((float)itemInfo.Price4 * 0.7f);
				}
			}
		}
		else
		{
			for (int j = 0; j < items.Count; j++)
			{
				ItemInfo itemInfo2 = items[j];
				if (!(itemInfo2 is MarketItemInfo))
				{
					OnSale currentSale2 = SalesManager.Instance.CurrentSale;
					if (currentSale2 != null && (currentSale2.ItemId == -1 || itemInfo2.Id == currentSale2.ItemId))
					{
						itemInfo2.Price = (int)((float)itemInfo2.OrigPrice * currentSale2.Discount);
						itemInfo2.Price1 = (int)((float)itemInfo2.OrigPrice1 * currentSale2.Discount);
						itemInfo2.Price2 = (int)((float)itemInfo2.OrigPrice2 * currentSale2.Discount);
						itemInfo2.Price3 = (int)((float)itemInfo2.OrigPrice3 * currentSale2.Discount);
						itemInfo2.Price4 = (int)((float)itemInfo2.OrigPrice4 * currentSale2.Discount);
					}
					else
					{
						itemInfo2.Price = itemInfo2.OrigPrice;
						itemInfo2.Price1 = itemInfo2.OrigPrice1;
						itemInfo2.Price2 = itemInfo2.OrigPrice2;
						itemInfo2.Price3 = itemInfo2.OrigPrice3;
						itemInfo2.Price4 = itemInfo2.OrigPrice4;
					}
				}
			}
		}
		if (this.RefreshItemsEvent != null)
		{
			this.RefreshItemsEvent();
		}
	}

	public bool CheckMoney(int id)
	{
		ItemInfo itemInfo = itemsById[id];
		if (itemInfo is MarketItemInfo)
		{
			return true;
		}
		int itemPrice = GetItemPrice(itemInfo);
		if (PlayerAccount.Instance != null && itemPrice <= PlayerAccount.Instance.RetrieveMoney())
		{
			return true;
		}
		return false;
	}

	public int GetItemPrice(int id)
	{
		ItemInfo itemInfo = itemsById[id];
		if (itemInfo != null)
		{
			return GetItemPrice(itemInfo);
		}
		return 0;
	}

	public int GetItemPrice(ItemInfo item)
	{
		if (item != null)
		{
			if (item.Upgradeable > 0)
			{
				switch (item.Upgrades)
				{
				case 0:
					return item.GetPrice();
				case 1:
					return item.Price1;
				case 2:
					return item.Price2;
				case 3:
					return item.Price3;
				case 4:
					return item.Price4;
				}
			}
			return item.Price;
		}
		return 0;
	}

	public bool Purchase(int id, OnPurchase onPurchase)
	{
		this.onPurchase = onPurchase;
		return Purchase(id);
	}

	public bool Purchase(int id, bool callInAppPurchaseAPI)
	{
		ItemInfo itemInfo = itemsById[id];
		if ((itemInfo is MarketItemInfo || (itemInfo is LocationItemInfo && (itemInfo as LocationItemInfo).AndroidMarketId != null) || (itemInfo is AvatarItemInfo && (itemInfo as AvatarItemInfo).AndroidMarketId != null)) && !IsItemPurchased(itemInfo))
		{
			marketItem = itemInfo as MarketItemInfo;
			if (marketItem != null)
			{
				if (marketItem.Tag == "Skullies")
				{
					string empty = string.Empty;
					empty = marketItem.AndroidMarketId;
					if (Application.isEditor || !callInAppPurchaseAPI || ConfigParams.IsDemoVersion)
					{
						PurchaseRes(empty, string.Empty, 1);
					}
					else
					{
						receiptValidateTryNumber = 0;
						BeLordInApp.Instance.Buy(empty, 1, PurchaseRes, PurchaseError, PurchaseCancel);
					}
				}
				else if (marketItem.Tag == "OpenURL")
				{
					PlayerPrefsWrapper.SetPendingPayment(marketItem.PackCount);
					Application.OpenURL(marketItem.Url);
					purchasingOpenURL = true;
					PlayerPrefsWrapper.PurchaseItem(itemInfo);
					itemInfo.Purchased = true;
					timer = 3f;
					if (this.PurchaseSuccessEvent != null)
					{
						this.PurchaseSuccessEvent(itemInfo);
					}
				}
				else if (marketItem.Tag == "Tweeter")
				{
					ServerDateBackEnd.Instance.GetServerDate(OnCheckDate);
				}
			}
			else
			{
				LocationItemInfo locationItemInfo = itemInfo as LocationItemInfo;
				Debug.Log(string.Format("locItemInfo: {0}", locationItemInfo));
				if (locationItemInfo != null && locationItemInfo.AndroidMarketId != null)
				{
					string empty2 = string.Empty;
					empty2 = locationItemInfo.AndroidMarketId;
					if (Application.isEditor || !callInAppPurchaseAPI || ConfigParams.IsDemoVersion)
					{
						PurchaseResPack(empty2, string.Empty, 1);
					}
					else
					{
						receiptValidateTryNumber = 0;
						BeLordInApp.Instance.Buy(empty2, 1, PurchaseResPack, PurchaseError, PurchaseCancel);
					}
				}
				else
				{
					AvatarItemInfo avatarItemInfo = itemInfo as AvatarItemInfo;
					Debug.Log(string.Format("avaItemInfo: {0}", avatarItemInfo));
					if (avatarItemInfo != null && avatarItemInfo.AndroidMarketId != null)
					{
						string empty3 = string.Empty;
						empty3 = avatarItemInfo.AndroidMarketId;
						if (Application.isEditor || !callInAppPurchaseAPI || ConfigParams.IsDemoVersion)
						{
							PurchaseResPack(empty3, string.Empty, 1);
						}
						else
						{
							receiptValidateTryNumber = 0;
							BeLordInApp.Instance.Buy(empty3, 1, PurchaseResPack, PurchaseError, PurchaseCancel);
						}
					}
				}
			}
			return true;
		}
		if (CheckMoney(id) && !IsItemPurchased(itemInfo))
		{
			if (PlayerAccount.Instance != null)
			{
				if (itemInfo.Id != 0)
				{
					StatsManager.LogEvent(StatVar.BUY_ITEM, itemInfo.Type, itemInfo.Name);
					if (itemInfo.Upgradeable > 0)
					{
						StatsManager.LogEvent(StatVar.BUY_ITEM_UPGRADEABLE, itemInfo.Type, itemInfo.Name, itemInfo.Upgrades.ToString());
					}
					if (itemInfo.Consumable)
					{
						StatsManager.LogEvent(StatVar.BUY_ITEM_CONSUMABLE, itemInfo.Type, itemInfo.Name);
					}
				}
				itemInfo.Upgrades++;
				if (itemInfo.Consumable)
				{
					itemInfo.Count += itemInfo.PackCount;
					StatsManager.LogEvent(StatVar.BUY_CONSUMABLE_BY_GROUP, DeviceGroup.GetGroupNum().ToString(), itemInfo.Name);
				}
				PlayerPrefsWrapper.PurchaseItem(itemInfo);
				if (itemInfo.Upgrades >= itemInfo.Upgradeable && !itemInfo.Consumable)
				{
					itemInfo.Purchased = true;
				}
				if (itemInfo.Upgradeable > 0)
				{
					switch (itemInfo.Upgrades)
					{
					case 1:
						if (itemInfo.GetPrice() <= PlayerAccount.Instance.RetrieveMoney())
						{
							PlayerAccount.Instance.SubMoney(itemInfo.GetPrice());
						}
						break;
					case 2:
						if (itemInfo.Price1 <= PlayerAccount.Instance.RetrieveMoney())
						{
							PlayerAccount.Instance.SubMoney(itemInfo.Price1);
						}
						break;
					case 3:
						if (itemInfo.Price2 <= PlayerAccount.Instance.RetrieveMoney())
						{
							PlayerAccount.Instance.SubMoney(itemInfo.Price2);
						}
						break;
					case 4:
						if (itemInfo.Price3 <= PlayerAccount.Instance.RetrieveMoney())
						{
							PlayerAccount.Instance.SubMoney(itemInfo.Price3);
						}
						break;
					case 5:
						if (itemInfo.Price4 <= PlayerAccount.Instance.RetrieveMoney())
						{
							PlayerAccount.Instance.SubMoney(itemInfo.Price4);
						}
						break;
					}
				}
				else
				{
					PlayerAccount.Instance.SubMoney(itemInfo.Price);
				}
			}
			if (this.PurchaseSuccessEvent != null)
			{
				this.PurchaseSuccessEvent(itemInfo);
			}
			if (PlayerAccount.Instance != null && PlayerAccount.Instance.IsDiscountActive())
			{
				PlayerAccount.Instance.DeactivateDiscount();
				RefreshItems();
			}
			SoundManager.PlaySound(8);
			if (PlayerAccount.Instance != null)
			{
				PlayerAccount.Instance.Save(false);
			}
			return true;
		}
		onPurchase = null;
		if (this.PurchaseFailedEvent != null)
		{
			this.PurchaseFailedEvent(itemInfo);
		}
		return false;
	}

	public bool Purchase(int id)
	{
		return Purchase(id, true);
	}

	private void OnCheckDate(DateTime date)
	{
		DateTime dateTime = new DateTime(DateTime.UtcNow.Ticks, DateTimeKind.Utc);
		if (date == DateTime.MinValue || Mathf.Abs((dateTime - date).Days) <= 2)
		{
			if (!ConfigParams.isKindle)
			{
				Debug.Log("Checking if Twitter is logged in...");
				if (!BeLordSocial.Instance.IsTwitterLoggedIn())
				{
					Debug.Log("Twitter is logged in...");
					BeLordSocial.twitterLogin += TwitterPost;
					BeLordSocial.twitterLoginFailed += OnTwitterLoginFailed;
					Debug.Log("Showing Twitter OAuth login dialog...");
					BeLordSocial.Instance.ShowTwitterOauthLoginDialog();
				}
				else
				{
					Debug.Log("Twitter already logged in...");
					TwitterPost();
				}
			}
		}
		else
		{
			PurchaseError("It's too soon to tweet now, try again later!");
		}
	}

	private void TwitterPost()
	{
		if (!ConfigParams.isKindle)
		{
			BeLordSocial.twitterLogin -= TwitterPost;
			BeLordSocial.twitterLoginFailed -= OnTwitterLoginFailed;
			BeLordSocial.twitterPost += OnTwitterPost;
			BeLordSocial.twitterPostFailed += OnTwitterPostFailed;
			Debug.Log("Posting on twitter...");
			switch (UnityEngine.Random.Range(0, 5))
			{
			case 0:
				BeLordSocial.Instance.PostTwitterStatusUpdate("Have you tried Running Fred? It's awesome! https://play.google.com/store/apps/details?id=com.dedalord.runningfred");
				break;
			case 1:
				BeLordSocial.Instance.PostTwitterStatusUpdate("Hands down, the best and bloodiest runner in the store - Running Fred! https://play.google.com/store/apps/details?id=com.dedalord.runningfred");
				break;
			case 2:
				BeLordSocial.Instance.PostTwitterStatusUpdate("Running Fred rocks! Give it a try: https://play.google.com/store/apps/details?id=com.dedalord.runningfred");
				break;
			case 3:
				BeLordSocial.Instance.PostTwitterStatusUpdate("Can't put this game down! You gotta try Running Fred. https://play.google.com/store/apps/details?id=com.dedalord.runningfred");
				break;
			case 4:
				BeLordSocial.Instance.PostTwitterStatusUpdate("The best and bloodiest runner in the store - Running Fred! https://play.google.com/store/apps/details?id=com.dedalord.runningfred");
				break;
			}
		}
	}

	private void OnTwitterLoginFailed(string error)
	{
		BeLordSocial.twitterLogin -= TwitterPost;
		BeLordSocial.twitterLoginFailed -= OnTwitterLoginFailed;
		Debug.Log("Login on twitter failed...");
		PurchaseError(error);
	}

	private void OnTwitterPost()
	{
		BeLordSocial.twitterPost -= OnTwitterPost;
		BeLordSocial.twitterPostFailed -= OnTwitterPostFailed;
		PurchaseRes(marketItem.MarketId, string.Empty, 1);
		if (this.PurchaseSuccessEvent != null)
		{
			this.PurchaseSuccessEvent(marketItem);
		}
		PlayerPrefsWrapper.SetTweetDate();
		Debug.Log("Posting on twitter successful...");
	}

	private void OnTwitterPostFailed(string error)
	{
		BeLordSocial.twitterPost -= OnTwitterPost;
		BeLordSocial.twitterPostFailed -= OnTwitterPostFailed;
		PurchaseError(error);
		Debug.Log("Posting on twitter failed...");
	}

	public void ConsumeItem(int id)
	{
		ItemInfo itemInfo = itemsById[id];
		itemInfo.Count = PlayerPrefsWrapper.ConsumeItem(itemInfo);
		if (this.RefreshItemsEvent != null)
		{
			this.RefreshItemsEvent();
		}
	}

	public void AddItem(int id)
	{
		ItemInfo itemInfo = itemsById[id];
		itemInfo.Count = PlayerPrefsWrapper.AddItem(itemInfo);
		if (this.RefreshItemsEvent != null)
		{
			this.RefreshItemsEvent();
		}
	}

	public void AddItem(int id, int itemCount)
	{
		ItemInfo itemInfo = itemsById[id];
		itemInfo.Count = PlayerPrefsWrapper.AddItem(itemInfo, itemCount);
		if (this.RefreshItemsEvent != null)
		{
			this.RefreshItemsEvent();
		}
	}

	public ItemInfo[] GetItems()
	{
		return items.ToArray();
	}

	public ItemInfo[] GetItemsByType(string type)
	{
		if (!itemsByType.ContainsKey(type))
		{
			return null;
		}
		return itemsByType[type].ToArray();
	}

	public ItemInfo[] GetItemsByTag(string tag)
	{
		if (!itemsByTag.ContainsKey(tag))
		{
			return null;
		}
		return itemsByTag[tag].ToArray();
	}

	public bool HasPurchasedItem(int id)
	{
		return itemsById[id].Purchased;
	}

	public ItemInfo GetItem(int id)
	{
		if (itemsById.ContainsKey(id))
		{
			return itemsById[id];
		}
		return null;
	}

	public MarketItemInfo GetItemByMarketId(string marketId)
	{
		if (!marketItemsById.ContainsKey(marketId))
		{
			return null;
		}
		return marketItemsById[marketId];
	}

	private bool IsItemPurchased(ItemInfo item)
	{
		if (PlayerAccount.Instance == null)
		{
			return true;
		}
		if (PlayerAccount.Instance.UnlockEverything)
		{
			return true;
		}
		if (PlayerAccount.Instance.UnlockLevels && item.Type == "chapter")
		{
			return true;
		}
		if (item.Upgrades >= item.Upgradeable && !item.Consumable && PlayerPrefsWrapper.IsItemPurchased(item.Id))
		{
			return true;
		}
		return false;
	}

	public ItemInfo GetMandatoryItem(string chapter, int level)
	{
		if (PlayerAccount.Instance == null)
		{
			return null;
		}
		int num = PlayerAccount.Instance.GetChapterOrder(chapter) * 10 + level;
		foreach (ItemInfo mandatoryItem in mandatoryItems)
		{
			if (!mandatoryItem.Purchased && (mandatoryItem.Upgradeable <= 0 || mandatoryItem.Upgrades <= 0) && mandatoryItem.RequieredByLevel <= num)
			{
				return mandatoryItem;
			}
		}
		return null;
	}

	public bool HasAllMandatories()
	{
		foreach (ItemInfo mandatoryItem in mandatoryItems)
		{
			if ((!mandatoryItem.Purchased && mandatoryItem.Upgradeable == 0) || (mandatoryItem.Upgradeable > 0 && mandatoryItem.Upgrades == 0))
			{
				return false;
			}
		}
		return true;
	}

	public OnSale GetCurrentSale()
	{
		return SalesManager.Instance.CurrentSale;
	}

	private void CheckOnSales()
	{
		if (!(SalesManager.Instance == null))
		{
			SalesManager.Instance.GetCurrentSale();
		}
	}

	private void OnCheckOnSales(OnSale sale)
	{
		OnSale currentSale = SalesManager.Instance.CurrentSale;
		if (currentSale != null)
		{
			RefreshItems();
		}
		SalesManager.Instance.OnSaleRequest -= OnCheckOnSales;
	}

	private void PurchaseRetry()
	{
		PurchaseRes(lastProductId, lastReceipt, lastQuantity);
	}

	private void PurchaseRes(string productId, string receipt, int quantity)
	{
		Debug.Log(string.Format("Store.PurchaseRes. productId: {0} receipt: {1}", productId, receipt));
		lastProductId = productId;
		lastReceipt = receipt;
		lastQuantity = quantity;
		if (Application.isEditor || ConfigParams.IsDemoVersion)
		{
			onValidateReceiptPurchaseRes(true, true);
		}
		else
		{
			onValidateReceiptPurchaseRes(true, true);
		}
	}

	private void onValidateReceiptPurchaseRes(bool res, bool receiptIsValid)
	{
		if (!res)
		{
			receiptValidateTryNumber++;
			if (receiptValidateTryNumber < 3)
			{
				Debug.Log(string.Format("Purchase receipt validation retry: {0}", receiptValidateTryNumber));
				Invoke("PurchaseRetry", 0.5f);
			}
			else
			{
				onPurchase(PurchaseResult.Error, "Receipt verification failed");
			}
		}
		else if (receiptIsValid)
		{
			MarketItemInfo marketItemInfo = null;
			foreach (ItemInfo item in items)
			{
				MarketItemInfo marketItemInfo2 = item as MarketItemInfo;
				if (marketItemInfo2 != null)
				{
					string empty = string.Empty;
					empty = marketItemInfo2.AndroidMarketId;
					if (empty == lastProductId)
					{
						marketItemInfo = marketItemInfo2;
						break;
					}
				}
			}
			if (marketItemInfo != null)
			{
				if (ConfigParams.IsKongregate())
				{
					StatsManager.LogEvent(StatVar.BUY_SKULLIES, marketItemInfo.CountKongregate.ToString());
				}
				else
				{
					StatsManager.LogEvent(StatVar.BUY_SKULLIES, marketItemInfo.PackCount.ToString());
				}
				PlayerPrefsWrapper.ClearPendings();
				if (ConfigParams.IsKongregate())
				{
					PlayerAccount.Instance.AddMoney(marketItemInfo.CountKongregate);
				}
				else
				{
					PlayerAccount.Instance.AddMoney(marketItemInfo.PackCount);
				}
				PlayerAccount.Instance.Save(true);
				SoundManager.PlaySound(8);
				PlayerPrefs.SetInt("iap", 1);
				if (onPurchase != null)
				{
					onPurchase(PurchaseResult.Ok, string.Empty);
				}
			}
			else
			{
				onPurchase(PurchaseResult.Error, string.Empty);
			}
			onPurchase = null;
		}
		else
		{
			onPurchase(PurchaseResult.Error, "Receipt verification failed");
		}
	}

	private void PurchasePackRetry()
	{
		PurchaseResPack(lastProductId, lastReceipt, lastQuantity);
	}

	private void PurchaseResPack(string productId, string receipt, int quantity)
	{
		lastProductId = productId;
		lastReceipt = receipt;
		lastQuantity = quantity;
		if (Application.isEditor || ConfigParams.IsDemoVersion)
		{
			onValidateReceiptPurchaseResPack(true, true);
		}
		else
		{
			onValidateReceiptPurchaseResPack(true, true);
		}
	}

	private void onValidateReceiptPurchaseResPack(bool res, bool receiptIsValid)
	{
		Debug.Log(string.Format("Receipt validation: {0}", receiptIsValid));
		if (!res)
		{
			receiptValidateTryNumber++;
			if (receiptValidateTryNumber < 3)
			{
				Debug.Log(string.Format("Purchase receipt validation retry: {0}", receiptValidateTryNumber));
				Invoke("PurchasePackRetry", 0.5f);
			}
			else
			{
				onPurchase(PurchaseResult.Error, "Receipt verification failed");
			}
		}
		else if (receiptIsValid)
		{
			LocationItemInfo locationItemInfo = null;
			AvatarItemInfo avatarItemInfo = null;
			foreach (ItemInfo item in items)
			{
				LocationItemInfo locationItemInfo2 = item as LocationItemInfo;
				if (locationItemInfo2 != null)
				{
					string empty = string.Empty;
					empty = locationItemInfo2.AndroidMarketId;
					if (empty == lastProductId)
					{
						Debug.Log(string.Format("onValidateReceiptPurchaseResPack: marketId: {0} lastProductId: {1}", empty, lastProductId));
						locationItemInfo = locationItemInfo2;
						break;
					}
					continue;
				}
				avatarItemInfo = item as AvatarItemInfo;
				if (avatarItemInfo != null)
				{
					string empty2 = string.Empty;
					empty2 = avatarItemInfo.AndroidMarketId;
					if (empty2 == lastProductId)
					{
						Debug.Log(string.Format("onValidateReceiptPurchaseResPack: marketId: {0} lastProductId: {1}", empty2, lastProductId));
						avatarItemInfo = avatarItemInfo;
						break;
					}
				}
			}
			if (locationItemInfo != null)
			{
				PlayerPrefsWrapper.ClearPendings();
				locationItemInfo.Purchased = true;
				PlayerPrefsWrapper.PurchaseItem(locationItemInfo);
				PlayerAccount.Instance.Save(true);
				SoundManager.PlaySound(8);
				if (onPurchase != null)
				{
					Debug.Log(string.Format("PurchaseResult.Ok"));
					onPurchase(PurchaseResult.Ok, string.Empty);
				}
			}
			else if (avatarItemInfo != null)
			{
				PlayerPrefsWrapper.ClearPendings();
				avatarItemInfo.Purchased = true;
				PlayerPrefsWrapper.PurchaseItem(avatarItemInfo);
				PlayerAccount.Instance.Save(true);
				SoundManager.PlaySound(8);
				if (onPurchase != null)
				{
					Debug.Log(string.Format("PurchaseResult.Ok"));
					onPurchase(PurchaseResult.Ok, string.Empty);
				}
			}
			else
			{
				Debug.Log(string.Format("PurchaseResult.Error"));
				onPurchase(PurchaseResult.Error, string.Empty);
			}
			onPurchase = null;
		}
		else
		{
			Debug.Log(string.Format("PurchaseResult.Error. Receipt verification failed"));
			onPurchase(PurchaseResult.Error, "Receipt verification failed");
		}
	}

	private void PurchaseError(string error)
	{
		Debug.Log("Store.PurchaseError");
		if (onPurchase != null)
		{
			onPurchase(PurchaseResult.Error, error);
		}
	}

	private void PurchaseCancel(string error)
	{
		Debug.Log("Store.PurchaseCancel");
		if (onPurchase != null)
		{
			onPurchase(PurchaseResult.Cancel, string.Empty);
		}
	}

	public bool CanPurchaseItems()
	{
		if (PlayerAccount.Instance == null)
		{
			return false;
		}
		foreach (ItemInfo item in items)
		{
			if (!(item is MarketItemInfo) && !IsItemPurchased(item))
			{
				int itemPrice = GetItemPrice(item);
				if (itemPrice != 0 && itemPrice <= PlayerAccount.Instance.RetrieveMoney())
				{
					return true;
				}
			}
		}
		return false;
	}

	public int CanPurchaseItemsCount()
	{
		if (PlayerAccount.Instance == null)
		{
			return 0;
		}
		int num = 0;
		foreach (ItemInfo item in items)
		{
			if (!(item is MarketItemInfo) && !IsItemPurchased(item) && item.Enabled && (!(item.Type != "avatar") || !(item.Type != "chapter") || !(item.Type != "skill") || !(item.Type != "power")))
			{
				int itemPrice = GetItemPrice(item);
				if (itemPrice != 0 && itemPrice <= PlayerAccount.Instance.RetrieveMoney())
				{
					num++;
				}
			}
		}
		return num;
	}

	public int CanPurchaseItemsCount(string type)
	{
		if (PlayerAccount.Instance == null)
		{
			return 0;
		}
		int num = 0;
		foreach (ItemInfo item in items)
		{
			if (!(item is MarketItemInfo) && !item.Purchased && !(item.Type != type) && item.Enabled)
			{
				int itemPrice = GetItemPrice(item);
				if (itemPrice != 0 && itemPrice <= PlayerAccount.Instance.RetrieveMoney())
				{
					num++;
				}
			}
		}
		return num;
	}

	public int CanPurchaseItemsCount(string type, string tag)
	{
		if (PlayerAccount.Instance == null)
		{
			return 0;
		}
		int num = 0;
		foreach (ItemInfo item in items)
		{
			if (!(item is MarketItemInfo) && !item.Purchased && !(item.Type != type) && !(item.Tag != tag) && item.Enabled)
			{
				int itemPrice = GetItemPrice(item);
				if (itemPrice != 0 && itemPrice <= PlayerAccount.Instance.RetrieveMoney())
				{
					num++;
				}
			}
		}
		return num;
	}

	private void Update()
	{
		if (purchasingOpenURL)
		{
			timer -= GUI3DManager.Instance.DeltaTime;
			if (timer <= 0f)
			{
				PurchaseRes(marketItem.MarketId, string.Empty, 1);
				purchasingOpenURL = false;
				PlayerAccount.Instance.Save(true);
			}
		}
	}
}
