using UnityEngine;

public class SendCode : MonoBehaviour
{
	public CheatConsoleWheel wheel1;

	public CheatConsoleWheel wheel2;

	public CheatConsoleWheel wheel3;

	public CheatConsoleWheel wheel4;

	private GUI3DButton button;

	private int retryCount;

	private void OnEnable()
	{
		if (button == null)
		{
			button = GetComponent<GUI3DButton>();
		}
		button.ReleaseEvent += OnRelease;
		retryCount = 0;
	}

	private void OnDisable()
	{
		if (button == null)
		{
			button = GetComponent<GUI3DButton>();
		}
		button.ReleaseEvent -= OnRelease;
	}

	private void OnRelease(GUI3DOnReleaseEvent evt)
	{
		GUI3DPopupManager.Instance.ShowPopup("Connecting");
		Invoke("getCheat", retryCount);
	}

	private void getCheat()
	{
		string code = string.Format("{0}{1}{2}{3}", wheel1.GetCode(), wheel2.GetCode(), wheel3.GetCode(), wheel4.GetCode());
		CheatConsoleServer.GetCheat(code, getCheatRes);
	}

	private void getCheatRes(bool res, string resStr, int itemId, int itemCount, long cheatId)
	{
		GUI3DPopupManager.Instance.CloseCurrentPopup(GUI3DPopupManager.PopupResult.Cancel);
		string text = string.Empty;
		string icon = string.Empty;
		Debug.Log(string.Format("Cheat. Unlock: {0}", itemId));
		ItemInfo item = Store.Instance.GetItem(itemId);
		if (res)
		{
			if (PlayerPrefsWrapper.HasCheat(cheatId))
			{
				retryCount++;
				GUI3DPopupManager.Instance.ShowPopup("CheatResponseFail", "This cheat was already unlocked in this device!");
				return;
			}
			retryCount = 0;
			switch ((GlobalItemList)itemId)
			{
			case GlobalItemList.SKULLIES_5000:
			case GlobalItemList.SKULLIES_15000:
			case GlobalItemList.SKULLIES_35000:
			case GlobalItemList.SKULLIES_60000:
			case GlobalItemList.SKULLIES_150000:
				SoundManager.PlaySound(8);
				if (item == null)
				{
					Debug.LogError("ii is null");
				}
				PlayerAccount.Instance.AddMoney(item.PackCount);
				text = string.Format("Enjoy your new pack of {0} skullies!", item.PackCount);
				icon = item.Picture;
				PlayerPrefsWrapper.SaveCheat(cheatId);
				break;
			case GlobalItemList.DOUBLE_JUMP:
			case GlobalItemList.RUBBER_BONES:
			case GlobalItemList.FAST_RECOVERY:
				icon = item.Picture;
				text = ((!buyItem(itemId)) ? string.Format("You already have the {0} skill.", item.Name) : string.Format("Enjoy the {0} skill!", item.Name));
				PlayerPrefsWrapper.SaveCheat(cheatId);
				break;
			case GlobalItemList.WALL_BOUNCE:
			case GlobalItemList.SKULLY_MAGNET:
			case GlobalItemList.WALL_GRIP:
			case GlobalItemList.LIFE_SLOT:
			case GlobalItemList.CHICKEN_FLAP:
				icon = item.Picture;
				text = ((!upgradeItem(itemId)) ? string.Format("You already have all the upgrades of {0}.", item.Name) : string.Format("Enjoy the {0} upgrade!", item.Name));
				PlayerPrefsWrapper.SaveCheat(cheatId);
				break;
			case GlobalItemList.DANGER_CAVES:
			case GlobalItemList.ENDLESS_CAVES:
			case GlobalItemList.HIGH_STAKES:
				icon = item.Picture;
				text = ((!buyItem(itemId)) ? string.Format("You already have the {0} world.", item.Name) : string.Format("Enjoy the {0} world!", item.Name));
				PlayerPrefsWrapper.SaveCheat(cheatId);
				break;
			case GlobalItemList.FRED_JUMPER:
			case GlobalItemList.FRED_SADO:
			case GlobalItemList.FRED_KNIGHT:
			case GlobalItemList.FRED_CONVICT:
			case GlobalItemList.FRED_INDY:
			case GlobalItemList.FRED_PERSIA:
			case GlobalItemList.FRED_SOLDIER:
			case GlobalItemList.FRED_EVOLUTION:
			case GlobalItemList.FRED_FEAR:
			case GlobalItemList.FRED_LIFEDOWN:
			case GlobalItemList.FRED_STUPID:
			case GlobalItemList.FRED_NINJA:
			case GlobalItemList.FRED_FOOTBALL:
			case GlobalItemList.FRED_GUMP:
			case GlobalItemList.FRED_PUNK:
			case GlobalItemList.FRED_KILLER:
			case GlobalItemList.FRED_CYBORG:
			case GlobalItemList.FRED_BASEBALL:
			case GlobalItemList.FRED_UNDERWEAR:
			case GlobalItemList.FRED_STRIKER:
			case GlobalItemList.FRED_SPARTAN:
			case GlobalItemList.FRED_VIRTUAL:
			case GlobalItemList.PIP_FRED:
			case GlobalItemList.FRED_PIRATE:
			case GlobalItemList.FRED_DRAGONFLEE:
			case GlobalItemList.FRED_ASSASSIN:
				icon = item.Picture;
				text = ((!buyItem(itemId)) ? string.Format("You already have {0}.", item.Name) : string.Format("Enjoy {0}!", item.Name));
				PlayerPrefsWrapper.SaveCheat(cheatId);
				break;
			case GlobalItemList.PANIC_POWER:
			case GlobalItemList.SHIELD:
			case GlobalItemList.RESURRECT:
			case GlobalItemList.AFTER_BURNER:
			case GlobalItemList.SAFETY_SPRING:
				icon = item.Picture;
				if (buyItemConsumable(itemId, itemCount))
				{
					text = string.Format("Enjoy the pack of {0} {1}!", itemCount, item.Name);
				}
				PlayerPrefsWrapper.SaveCheat(cheatId);
				break;
			case GlobalItemList.VALUE_PACK_1:
				icon = item.Picture;
				Store.Instance.Purchase(Store.Instance.GetItem(1012).Id, false);
				ValuePackManager.Unlock();
				SoundManager.PlaySound(9);
				text = string.Format("Enjoy this amazing Value Pack!");
				PlayerPrefsWrapper.SaveCheat(cheatId);
				break;
			case GlobalItemList.VALUE_PACK_2:
				icon = item.Picture;
				Store.Instance.Purchase(Store.Instance.GetItem(1014).Id, false);
				ValuePack2Manager.Unlock();
				SoundManager.PlaySound(9);
				text = string.Format("Enjoy this amazing Value Pack!");
				PlayerPrefsWrapper.SaveCheat(cheatId);
				break;
			case GlobalItemList.VALUE_PACK_3:
				icon = item.Picture;
				Store.Instance.Purchase(Store.Instance.GetItem(1018).Id, false);
				ValuePack3Manager.Unlock();
				SoundManager.PlaySound(9);
				text = string.Format("Enjoy this amazing Value Pack!");
				PlayerPrefsWrapper.SaveCheat(cheatId);
				break;
			}
			GUI3DPopupManager.Instance.ShowPopup("CheatResponseOK", text, "Congratulations!", icon);
		}
		else
		{
			retryCount++;
			GUI3DPopupManager.Instance.ShowPopup("CheatResponseFail", resStr);
		}
	}

	private bool buyItem(int itemId)
	{
		if (!PlayerPrefsWrapper.IsItemPurchased(itemId))
		{
			SoundManager.PlaySound(9);
			Store.Instance.AddItem(itemId);
			return true;
		}
		return false;
	}

	private bool buyItemConsumable(int itemId, int itemCount)
	{
		SoundManager.PlaySound(9);
		Store.Instance.AddItem(itemId, itemCount);
		return true;
	}

	private bool upgradeItem(int itemId)
	{
		if (!PlayerPrefsWrapper.IsItemPurchased(itemId))
		{
			ItemInfo item = Store.Instance.GetItem(itemId);
			item.Upgrades = 1;
			item.Count = 1;
			PlayerPrefsWrapper.PurchaseItem(item);
			SoundManager.PlaySound(9);
			if (PlayerAccount.Instance != null)
			{
				PlayerAccount.Instance.Save(true);
			}
			return true;
		}
		ItemInfo item2 = Store.Instance.GetItem(itemId);
		if (item2.Upgrades < item2.Upgradeable)
		{
			item2.Upgrades++;
			item2.Count = 1;
			PlayerPrefsWrapper.PurchaseItem(item2);
			if (PlayerAccount.Instance != null)
			{
				PlayerAccount.Instance.Save(true);
			}
			SoundManager.PlaySound(9);
			return true;
		}
		return false;
	}
}
