using UnityEngine;

public class ShopItem : PageSliderItem
{
	public GUI3DText Name;

	public GUI3DText Price;

	public GUI3DButton Button;

	public GUI3DObject Icon;

	public GUI3DText CountText;

	public GUI3DObject CountContainer;

	public GUI3DObject[] UpgradesOn;

	public GUI3DObject[] UpgradesOff;

	public GUI3DObject SkullyIcon;

	public GUI3DObject SkullyContainer;

	public GUI3DObject DollarContainer;

	public GameObject PurchasedIcon;

	public GUI3DObject ComingSoon;

	public GUI3DObject Survival;

	public GUI3DObject DailyOffer;

	public GUI3DText DailyOfferText1;

	public GUI3DText DailyOfferText2;

	public override void Create(ItemInfo item)
	{
		base.Create(item);
		if (Name != null)
		{
			Name.SetDynamicText(Item.Name);
		}
		if (ComingSoon != null && Item.Enabled)
		{
			ComingSoon.GetComponent<Renderer>().enabled = false;
			GUI3DText componentInChildren = ComingSoon.GetComponentInChildren<GUI3DText>();
			componentInChildren.gameObject.SetActive(false);
		}
		if (Item.Purchased || Store.Instance == null || Store.Instance.GetCurrentSale() == null || Store.Instance.GetCurrentSale().ItemId != Item.Id)
		{
			if (DailyOffer != null)
			{
				DailyOffer.GetComponent<Renderer>().enabled = false;
			}
			if (DailyOfferText1 != null)
			{
				DailyOfferText1.gameObject.SetActive(false);
			}
			if (DailyOfferText2 != null)
			{
				DailyOfferText2.gameObject.SetActive(false);
			}
		}
		else
		{
			if (DailyOffer != null)
			{
				DailyOffer.GetComponent<Renderer>().enabled = true;
			}
			if (DailyOfferText1 != null)
			{
				DailyOfferText1.gameObject.SetActive(true);
			}
			if (DailyOfferText2 != null)
			{
				DailyOfferText2.gameObject.SetActive(true);
			}
			if (DailyOfferText1 != null)
			{
				DailyOfferText1.SetDynamicText(Store.Instance.GetCurrentSale().RealDiscount + "%");
			}
			if (DailyOfferText2 != null)
			{
				DailyOfferText2.gameObject.SetActive(false);
			}
		}
		if (Price != null)
		{
			Price.gameObject.SetActive(true);
			if (!Item.Purchased && Item.Enabled)
			{
				if (PurchasedIcon != null)
				{
					PurchasedIcon.GetComponent<Renderer>().enabled = false;
				}
				if (CountContainer != null && CountText != null)
				{
					if (Item.Consumable)
					{
						CountContainer.GetComponent<Renderer>().enabled = true;
						CountText.gameObject.SetActive(true);
						CountText.SetDynamicText(Item.Count.ToString());
					}
					else
					{
						CountContainer.GetComponent<Renderer>().enabled = false;
						CountText.gameObject.SetActive(false);
					}
				}
				if (Item.Upgradeable > 0)
				{
					for (int i = 0; i < UpgradesOn.Length; i++)
					{
						GUI3DObject gUI3DObject = UpgradesOn[i];
						GUI3DObject gUI3DObject2 = UpgradesOff[i];
						if (i < Item.Upgrades)
						{
							gUI3DObject.GetComponent<Renderer>().enabled = true;
							gUI3DObject2.GetComponent<Renderer>().enabled = false;
						}
						else if (i < Item.Upgradeable)
						{
							gUI3DObject.GetComponent<Renderer>().enabled = false;
							gUI3DObject2.GetComponent<Renderer>().enabled = true;
						}
						else
						{
							gUI3DObject.GetComponent<Renderer>().enabled = false;
							gUI3DObject2.GetComponent<Renderer>().enabled = false;
						}
					}
				}
				else
				{
					for (int j = 0; j < UpgradesOn.Length; j++)
					{
						GUI3DObject gUI3DObject3 = UpgradesOn[j];
						GUI3DObject gUI3DObject4 = UpgradesOff[j];
						gUI3DObject3.GetComponent<Renderer>().enabled = false;
						gUI3DObject4.GetComponent<Renderer>().enabled = false;
					}
				}
				if (Item is MarketItemInfo)
				{
					if (SkullyContainer != null && SkullyIcon != null)
					{
						Object.DestroyImmediate(SkullyIcon.gameObject);
						Object.DestroyImmediate(SkullyContainer.gameObject);
						SkullyIcon = null;
						SkullyContainer = null;
					}
					string text = MonoBehaviorSingleton<GUI3DLocalization>.Instance.GetText("Unsorted", "Skullies", "!BAD_TEXT!");
					if (ConfigParams.IsKongregate())
					{
						Name.SetDynamicText(StringUtil.FormatNumbers(((MarketItemInfo)Item).CountKongregate) + " " + text);
						Price.SetDynamicText(((MarketItemInfo)Item).PriceKreds);
					}
					else
					{
						Name.SetDynamicText(StringUtil.FormatNumbers(((MarketItemInfo)Item).PackCount) + " " + text);
						Price.SetDynamicText(((MarketItemInfo)Item).PriceDollars);
					}
				}
				else if (Item.PriceDollars != null)
				{
					if (ConfigParams.IsKongregate())
					{
						Price.SetDynamicText(Item.PriceKreds);
					}
					else
					{
						Price.SetDynamicText(Item.PriceDollars);
					}
				}
				else
				{
					if (DollarContainer != null)
					{
						Object.DestroyImmediate(DollarContainer.gameObject);
						DollarContainer = null;
					}
					if (Item.Upgradeable > 0)
					{
						switch (Item.Upgrades)
						{
						case 0:
							Price.SetDynamicText(StringUtil.FormatNumbers(Item.Price));
							break;
						case 1:
							Price.SetDynamicText(StringUtil.FormatNumbers(Item.Price1));
							break;
						case 2:
							Price.SetDynamicText(StringUtil.FormatNumbers(Item.Price2));
							break;
						case 3:
							Price.SetDynamicText(StringUtil.FormatNumbers(Item.Price3));
							break;
						case 4:
							Price.SetDynamicText(StringUtil.FormatNumbers(Item.Price4));
							break;
						}
					}
					else
					{
						Price.SetDynamicText(StringUtil.FormatNumbers(Item.GetPrice()));
					}
				}
			}
			else if (Item.Enabled)
			{
				Purchased();
			}
			else
			{
				if (PurchasedIcon != null)
				{
					PurchasedIcon.GetComponent<Renderer>().enabled = false;
				}
				if (SkullyContainer != null && SkullyIcon != null)
				{
					SkullyIcon.GetComponent<Renderer>().enabled = false;
					SkullyContainer.GetComponent<Renderer>().enabled = false;
				}
				if (DollarContainer != null)
				{
					DollarContainer.GetComponent<Renderer>().enabled = false;
				}
				Price.gameObject.SetActive(false);
				if (ComingSoon != null)
				{
					ComingSoon.GetComponent<Renderer>().enabled = true;
				}
			}
		}
		if (Survival != null && Item.Tag == "Survival")
		{
			Survival.CreateOwnMesh = true;
			Survival.CreateMesh();
		}
		if (Item.Picture != string.Empty)
		{
			if (Icon.GetComponent<Renderer>() == null)
			{
				Icon.TextureName = Item.Picture;
				Icon.CreateOwnMesh = true;
				Icon.CreateMesh();
			}
			else
			{
				Icon.RefreshMaterial(Item.Picture);
			}
		}
		if (string.Compare(item.CoinType, "grimmies") == 0)
		{
			if (PlayerAccount.Instance.GetGrimmyIdolPickedCount() >= ConfigParams.IronFredGrimmyGoal)
			{
				if (PurchasedIcon != null)
				{
					PurchasedIcon.GetComponent<Renderer>().enabled = true;
				}
				if (SkullyContainer != null)
				{
					SkullyContainer.GetComponent<Renderer>().enabled = false;
				}
				if (SkullyIcon != null)
				{
					SkullyIcon.GetComponent<Renderer>().enabled = false;
				}
				if (DollarContainer != null)
				{
					DollarContainer.GetComponent<Renderer>().enabled = false;
				}
				Price.gameObject.SetActive(false);
			}
			else
			{
				Price.gameObject.SetActive(true);
				if (SkullyIcon != null)
				{
					SkullyIcon.TextureName = "GrimmyIdol-small";
					SkullyIcon.CreateOwnMesh = true;
					SkullyIcon.CreateMesh();
				}
				if (SkullyContainer != null)
				{
					SkullyContainer.StartSegmentTexName = "goldtag-left";
					SkullyContainer.TextureName = "goldtag-stretch";
					SkullyContainer.EndSegmentTexName = "goldtag-right";
					SkullyContainer.CreateOwnMesh = true;
					SkullyContainer.CreateMesh();
				}
			}
		}
		Store.Instance.RefreshItemsEvent += OnPriceChange;
	}

	private void OnEnable()
	{
		if (Item.Purchased || Store.Instance == null || Store.Instance.GetCurrentSale() == null || Store.Instance.GetCurrentSale().ItemId != Item.Id)
		{
			if (DailyOffer != null)
			{
				DailyOffer.GetComponent<Renderer>().enabled = false;
			}
			if (DailyOfferText1 != null)
			{
				DailyOfferText1.gameObject.SetActive(false);
			}
			if (DailyOfferText2 != null)
			{
				DailyOfferText2.gameObject.SetActive(false);
			}
		}
		else
		{
			if (DailyOfferText1 != null)
			{
				DailyOfferText1.gameObject.SetActive(true);
			}
			if (DailyOfferText2 != null)
			{
				DailyOfferText2.gameObject.SetActive(true);
			}
			if (DailyOffer != null)
			{
				DailyOffer.GetComponent<Renderer>().enabled = true;
			}
			if (DailyOfferText1 != null)
			{
				DailyOfferText1.SetDynamicText(Store.Instance.GetCurrentSale().RealDiscount + "%");
			}
			if (DailyOfferText2 != null)
			{
				DailyOfferText2.SetDynamicText("off!");
			}
		}
		if (!Item.Purchased && PurchasedIcon != null)
		{
			PurchasedIcon.GetComponent<Renderer>().enabled = false;
		}
		if (CountContainer != null && CountText != null)
		{
			if (Item.Consumable)
			{
				CountContainer.GetComponent<Renderer>().enabled = true;
				CountText.gameObject.SetActive(true);
				CountText.SetDynamicText(Item.Count.ToString());
			}
			else
			{
				CountContainer.GetComponent<Renderer>().enabled = false;
				CountText.gameObject.SetActive(false);
			}
		}
		RefreshItems();
	}

	public void SetPrice(string price)
	{
		if (Price != null)
		{
			Price.SetDynamicText(price);
		}
	}

	public void Purchased()
	{
		if (Item.Purchased || Store.Instance == null || Store.Instance.GetCurrentSale() == null || Store.Instance.GetCurrentSale().ItemId != Item.Id)
		{
			if (DailyOffer != null)
			{
				DailyOffer.GetComponent<Renderer>().enabled = false;
			}
			if (DailyOfferText1 != null)
			{
				DailyOfferText1.gameObject.SetActive(false);
			}
			if (DailyOfferText2 != null)
			{
				DailyOfferText2.gameObject.SetActive(false);
			}
		}
		else
		{
			if (DailyOfferText1 != null)
			{
				DailyOfferText1.gameObject.SetActive(true);
			}
			if (DailyOfferText2 != null)
			{
				DailyOfferText2.gameObject.SetActive(true);
			}
			if (DailyOffer != null)
			{
				DailyOffer.GetComponent<Renderer>().enabled = true;
			}
			if (DailyOfferText1 != null)
			{
				DailyOfferText1.SetDynamicText(Store.Instance.GetCurrentSale().RealDiscount + "%");
			}
			if (DailyOfferText2 != null)
			{
				DailyOfferText2.SetDynamicText("off!");
			}
		}
		if (Item.Purchased)
		{
			if (SkullyContainer != null && SkullyIcon != null)
			{
				SkullyIcon.GetComponent<Renderer>().enabled = false;
				SkullyContainer.GetComponent<Renderer>().enabled = false;
			}
			if (DollarContainer != null)
			{
				DollarContainer.GetComponent<Renderer>().enabled = false;
			}
			Price.gameObject.SetActive(false);
			if (PurchasedIcon != null)
			{
				PurchasedIcon.GetComponent<Renderer>().enabled = true;
			}
		}
		if (Item.Upgradeable > 0)
		{
			if (!Item.Purchased)
			{
				switch (Item.Upgrades)
				{
				case 0:
					Price.SetDynamicText(StringUtil.FormatNumbers(Item.GetPrice()));
					break;
				case 1:
					Price.SetDynamicText(StringUtil.FormatNumbers(Item.Price1));
					break;
				case 2:
					Price.SetDynamicText(StringUtil.FormatNumbers(Item.Price2));
					break;
				case 3:
					Price.SetDynamicText(StringUtil.FormatNumbers(Item.Price3));
					break;
				case 4:
					Price.SetDynamicText(StringUtil.FormatNumbers(Item.Price4));
					break;
				}
			}
			for (int i = 0; i < UpgradesOn.Length; i++)
			{
				GUI3DObject gUI3DObject = UpgradesOn[i];
				GUI3DObject gUI3DObject2 = UpgradesOff[i];
				if (gUI3DObject != null && gUI3DObject.GetComponent<Renderer>() != null && gUI3DObject2 != null && gUI3DObject2.GetComponent<Renderer>() != null)
				{
					if (i < Item.Upgrades)
					{
						gUI3DObject.GetComponent<Renderer>().enabled = true;
						gUI3DObject2.GetComponent<Renderer>().enabled = false;
					}
					else if (i < Item.Upgradeable)
					{
						gUI3DObject.GetComponent<Renderer>().enabled = false;
						gUI3DObject2.GetComponent<Renderer>().enabled = true;
					}
					else
					{
						gUI3DObject.GetComponent<Renderer>().enabled = false;
						gUI3DObject2.GetComponent<Renderer>().enabled = false;
					}
				}
			}
		}
		else
		{
			for (int j = 0; j < UpgradesOn.Length; j++)
			{
				GUI3DObject gUI3DObject3 = UpgradesOn[j];
				GUI3DObject gUI3DObject4 = UpgradesOff[j];
				if (gUI3DObject3 != null && gUI3DObject3.GetComponent<Renderer>() != null)
				{
					gUI3DObject3.GetComponent<Renderer>().enabled = false;
				}
				if (gUI3DObject4 != null && gUI3DObject4.GetComponent<Renderer>() != null)
				{
					gUI3DObject4.GetComponent<Renderer>().enabled = false;
				}
			}
		}
		if (CountContainer != null && CountText != null)
		{
			if (Item.Consumable)
			{
				CountContainer.GetComponent<Renderer>().enabled = true;
				CountText.gameObject.SetActive(true);
				CountText.SetDynamicText(Item.Count.ToString());
			}
			else
			{
				CountContainer.GetComponent<Renderer>().enabled = false;
				CountText.gameObject.SetActive(false);
			}
		}
	}

	private void OnPriceChange()
	{
		RefreshItems();
	}

	private void RefreshItems()
	{
		if (Item.Purchased || Store.Instance == null || Store.Instance.GetCurrentSale() == null || Store.Instance.GetCurrentSale().ItemId != Item.Id)
		{
			if (DailyOffer != null)
			{
				DailyOffer.GetComponent<Renderer>().enabled = false;
			}
			if (DailyOfferText1 != null)
			{
				DailyOfferText1.gameObject.SetActive(false);
			}
			if (DailyOfferText2 != null)
			{
				DailyOfferText2.gameObject.SetActive(false);
			}
		}
		else
		{
			if (DailyOfferText1 != null)
			{
				DailyOfferText1.gameObject.SetActive(true);
			}
			if (DailyOfferText2 != null)
			{
				DailyOfferText2.gameObject.SetActive(true);
			}
			if (DailyOffer != null)
			{
				DailyOffer.GetComponent<Renderer>().enabled = true;
			}
			if (DailyOfferText1 != null)
			{
				DailyOfferText1.SetDynamicText(Store.Instance.GetCurrentSale().RealDiscount + "%");
			}
			if (DailyOfferText2 != null)
			{
				DailyOfferText2.SetDynamicText("off!");
			}
		}
		if (Item is MarketItemInfo && Price != null)
		{
			if (ConfigParams.IsKongregate())
			{
				Price.SetDynamicText(((MarketItemInfo)Item).PriceKreds);
			}
			else
			{
				Price.SetDynamicText(((MarketItemInfo)Item).PriceDollars);
			}
		}
		if (Item.Upgradeable > 0)
		{
			if (!Item.Purchased)
			{
				switch (Item.Upgrades)
				{
				case 0:
					Price.SetDynamicText(StringUtil.FormatNumbers(Item.GetPrice()));
					break;
				case 1:
					Price.SetDynamicText(StringUtil.FormatNumbers(Item.Price1));
					break;
				case 2:
					Price.SetDynamicText(StringUtil.FormatNumbers(Item.Price2));
					break;
				case 3:
					Price.SetDynamicText(StringUtil.FormatNumbers(Item.Price3));
					break;
				case 4:
					Price.SetDynamicText(StringUtil.FormatNumbers(Item.Price4));
					break;
				}
			}
			for (int i = 0; i < UpgradesOn.Length; i++)
			{
				GUI3DObject gUI3DObject = UpgradesOn[i];
				GUI3DObject gUI3DObject2 = UpgradesOff[i];
				if (i < Item.Upgrades)
				{
					gUI3DObject.GetComponent<Renderer>().enabled = true;
					gUI3DObject2.GetComponent<Renderer>().enabled = false;
				}
				else if (i < Item.Upgradeable)
				{
					gUI3DObject.GetComponent<Renderer>().enabled = false;
					gUI3DObject2.GetComponent<Renderer>().enabled = true;
				}
				else
				{
					gUI3DObject.GetComponent<Renderer>().enabled = false;
					gUI3DObject2.GetComponent<Renderer>().enabled = false;
				}
			}
		}
		else
		{
			if (!(Item is MarketItemInfo) && !Item.Purchased && Item.Enabled)
			{
				if (PurchasedIcon != null)
				{
					PurchasedIcon.GetComponent<Renderer>().enabled = false;
				}
				if (SkullyContainer != null && SkullyIcon != null)
				{
					SkullyIcon.GetComponent<Renderer>().enabled = true;
					SkullyContainer.GetComponent<Renderer>().enabled = true;
				}
				if (Item.PriceDollars != null)
				{
					if (ConfigParams.IsKongregate())
					{
						Price.SetDynamicText(Item.PriceKreds);
					}
					else
					{
						Price.SetDynamicText(Item.PriceDollars);
					}
				}
				else
				{
					Price.SetDynamicText(StringUtil.FormatNumbers(Item.Price));
				}
			}
			for (int j = 0; j < UpgradesOn.Length; j++)
			{
				GUI3DObject gUI3DObject3 = UpgradesOn[j];
				GUI3DObject gUI3DObject4 = UpgradesOff[j];
				gUI3DObject3.GetComponent<Renderer>().enabled = false;
				gUI3DObject4.GetComponent<Renderer>().enabled = false;
			}
			if (CountContainer != null && CountText != null)
			{
				if (Item.Consumable)
				{
					CountContainer.GetComponent<Renderer>().enabled = true;
					CountText.gameObject.SetActive(true);
					CountText.SetDynamicText(Item.Count.ToString());
				}
				else
				{
					CountContainer.GetComponent<Renderer>().enabled = false;
					CountText.gameObject.SetActive(false);
				}
			}
		}
		if (string.Compare(Item.CoinType, "grimmies") != 0)
		{
			return;
		}
		if (PlayerAccount.Instance.GetGrimmyIdolPickedCount() >= ConfigParams.IronFredGrimmyGoal)
		{
			if (PurchasedIcon != null)
			{
				PurchasedIcon.GetComponent<Renderer>().enabled = true;
			}
			if (SkullyContainer != null)
			{
				SkullyContainer.GetComponent<Renderer>().enabled = false;
			}
			if (SkullyIcon != null)
			{
				SkullyIcon.GetComponent<Renderer>().enabled = false;
			}
			if (DollarContainer != null)
			{
				DollarContainer.GetComponent<Renderer>().enabled = false;
			}
			Price.gameObject.SetActive(false);
			return;
		}
		Price.gameObject.SetActive(true);
		if (PurchasedIcon != null)
		{
			PurchasedIcon.GetComponent<Renderer>().enabled = false;
		}
		if (SkullyContainer != null)
		{
			SkullyContainer.GetComponent<Renderer>().enabled = true;
		}
		if (SkullyIcon != null)
		{
			SkullyIcon.GetComponent<Renderer>().enabled = true;
		}
		if (DollarContainer != null)
		{
			DollarContainer.GetComponent<Renderer>().enabled = true;
		}
		Price.SetDynamicText(ConfigParams.IronFredGrimmyGoal.ToString());
	}
}
