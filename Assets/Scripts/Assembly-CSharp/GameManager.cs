using System;
using UnityEngine;

public class GameManager
{
	public static float ActionSpeed = 1f;

	public static float ReplaySpeed = 1f;

	public static GameState CurrentGameState;

	private static float accumTime;

	private static bool fredIsDead;

	private static bool checkDeadEvents;

	private static bool resurrectDialog;

	private static bool fading;

	public static void OnGameStart()
	{
	}

	public static void OnGameEnd()
	{
	}

	public static void OnGameUpdate()
	{
		if (!checkDeadEvents)
		{
			return;
		}
		accumTime += Time.deltaTime;
		if (accumTime >= 0.5f && accumTime < 1.5f)
		{
			if (CharHelper.GetProps().Lives > 0 && (PlayerAccount.Instance == null || PlayerAccount.Instance.CurrentGameMode != PlayerAccount.GameMode.Survival) && !fading)
			{
				CameraFade.Instance.FadeOut(0.5f);
				fading = true;
			}
		}
		else if (accumTime > 1.5f)
		{
			if (CharHelper.GetProps().Lives > 0 && (PlayerAccount.Instance == null || PlayerAccount.Instance.CurrentGameMode != PlayerAccount.GameMode.Survival))
			{
				CharHelper.GetProps().Lives--;
				CheckPointManager.RespawnOnLastCheckPoint();
				CameraFade.Instance.FadeIn(1f);
				InputManager.ResetSuperSprintTimer();
				GameEventDispatcher.Dispatch(null, new OnPlayerRespawningNow());
				GC.Collect();
				fading = false;
				fredIsDead = false;
			}
			else
			{
				GameEventDispatcher.Dispatch(null, new OnPlayerDead());
			}
			checkDeadEvents = false;
		}
	}

	public static void OnLevelStart()
	{
		Profile.CheckPerformance();
		GameEventDispatcher.Dispatch(null, new OnLevelLoaded());
		CharHelper.GetCharStateMachine().LoadAndAttachExtras();
		GameEventDispatcher.AddListener("PlayerDieFalling", OnPlayerDieFalling);
		GameEventDispatcher.AddListener("PlayerHittedByArrow", OnPlayerHittedByArrow);
		GameEventDispatcher.AddListener("PlayerDieOnImpact", OnPlayerDieOnImpact);
		GameEventDispatcher.AddListener("PlayerExploted", OnPlayerExploted);
		GameEventDispatcher.AddListener("PlayerFrozen", OnPlayerFrozen);
		GameEventDispatcher.AddListener("PlayerWasBurnt", OnPlayerWasBurnt);
		GameEventDispatcher.AddListener("PlayerWasCrushed", OnPlayerWasCrushed);
		GameEventDispatcher.AddListener("PlayerWasPierced", OnPlayerWasPierced);
		GameEventDispatcher.AddListener("OnPlayerRespawningNow", OnPlayerRespawnNow);
		GameEventDispatcher.AddListener("OnPauseEvent", OnPauseAndResume);
		GameEventDispatcher.AddListener("OnLevelComplete", OnLevelEnd);
		fredIsDead = false;
	}

	private static void OnLevelEnd(object sender, GameEvent e)
	{
	}

	public static void OnLevelUnLoad()
	{
		GameEventDispatcher.Dispatch(null, new OnLevelUnLoad());
	}

	private static void OnPlayerDieFalling(object sender, GameEvent e)
	{
		if (!fredIsDead)
		{
			onPlayerDie();
		}
	}

	private static void OnPlayerHittedByArrow(object sender, GameEvent e)
	{
		if (!fredIsDead)
		{
			onPlayerDie();
		}
	}

	private static void OnPlayerDieOnImpact(object sender, GameEvent e)
	{
		if (!fredIsDead)
		{
			onPlayerDie();
		}
	}

	private static void OnPlayerExploted(object sender, GameEvent e)
	{
		if (!fredIsDead)
		{
			onPlayerDie();
		}
	}

	private static void OnPlayerFrozen(object sender, GameEvent e)
	{
		if (!fredIsDead)
		{
			onPlayerDie();
		}
	}

	private static void OnPlayerWasBurnt(object sender, GameEvent e)
	{
		if (!fredIsDead)
		{
			onPlayerDie();
		}
	}

	private static void OnPlayerWasCrushed(object sender, GameEvent e)
	{
		if (!fredIsDead)
		{
			onPlayerDie();
		}
	}

	private static void OnPlayerWasPierced(object sender, GameEvent e)
	{
		if (!fredIsDead)
		{
			onPlayerDie();
		}
	}

	private static void OnPlayerRespawnNow(object sender, GameEvent e)
	{
		ChunkRelocator.SetChangeVisibility(true);
	}

	private static void OnPauseAndResume(object sender, GameEvent e)
	{
		OnPauseEvent onPauseEvent = e as OnPauseEvent;
		if (onPauseEvent != null)
		{
			if (onPauseEvent.Paused)
			{
				SoundManager.PauseAll(true);
			}
			else
			{
				SoundManager.PauseAll(false);
			}
		}
	}

	public static bool IsFredDead()
	{
		return fredIsDead;
	}

	private static void onPlayerDie()
	{
		fredIsDead = true;
		if (JetpackMeter.Instance != null)
		{
			JetpackMeter.Instance.StopSound();
		}
		if ((PlayerAccount.Instance.CurrentGameMode == PlayerAccount.GameMode.Adventure || PlayerAccount.Instance.CurrentGameMode == PlayerAccount.GameMode.Challenge) && CharHelper.GetProps().Lives > 0)
		{
			accumTime = 0f;
			checkDeadEvents = true;
			GameEventDispatcher.Dispatch(null, new OnHideGUIEvent());
			ChunkRelocator.SetChangeVisibility(false);
			if (CharHelper.GetProps().Lives > 0)
			{
				GameEventDispatcher.Dispatch(null, new OnPlayerWillRespawn());
			}
			return;
		}
		bool flag = false;
		if (PlayerAccount.Instance.CurrentGameMode == PlayerAccount.GameMode.Challenge)
		{
			ChallengeItemInfo[] array = ItemsLoader.Load<ChallengeItemInfo>("Shop/challengeslist");
			for (int i = 0; i < array.Length; i++)
			{
				if (string.Compare(array[i].SceneName, PlayerAccount.Instance.CurrentChallenge, false) == 0)
				{
					flag = true;
					break;
				}
			}
		}
		if (PlayerAccount.Instance.CurrentGameMode == PlayerAccount.GameMode.Challenge && flag)
		{
			ChunkRelocator.SetChangeVisibility(false);
			GameEventDispatcher.Dispatch(null, new OnPlayerDead());
		}
		else
		{
			resurrectDialog = true;
			GUI3DPopupManager.Instance.ShowPopup("ConsumingItemPopupResurrect", onResurrectPopupClose);
		}
	}

	private static void onResurrectPopupClose(GUI3DPopupManager.PopupResult popupRes)
	{
		if (popupRes == GUI3DPopupManager.PopupResult.No || popupRes == GUI3DPopupManager.PopupResult.Cancel)
		{
			fredIsDead = true;
			accumTime = 0f;
			checkDeadEvents = true;
			GameEventDispatcher.Dispatch(null, new OnHideGUIEvent());
			ChunkRelocator.SetChangeVisibility(false);
			return;
		}
		ItemInfo item = Store.Instance.GetItem(120);
		if (CharHelper.GetProps().freeResurectByTapjoy == ResurrectStatus.ACTIVATED)
		{
			if (PlayerAccount.Instance.CurrentGameMode == PlayerAccount.GameMode.Adventure || PlayerAccount.Instance.CurrentGameMode == PlayerAccount.GameMode.Challenge)
			{
				CheckPointManager.RespawnOnLastCheckPoint();
				CameraFade.Instance.FadeIn(1f);
				GameEventDispatcher.Dispatch(null, new OnPlayerRespawningNow());
				fading = false;
				fredIsDead = false;
				CharHelper.GetProps().Lives = 0;
			}
			else
			{
				ResetGameForEndless(false);
			}
			CharHelper.GetProps().freeResurectByTapjoy = ResurrectStatus.USED;
			return;
		}
		if (item.Count == 0)
		{
			if (CharHelper.GetProps().freeResurectByTapjoy == ResurrectStatus.READY && BeLordTapJoy.IsReadyToUse)
			{
				GUI3DPopupManager.Instance.ShowPopup("ShopItemBuyResurrects", item.Description, item.Name, item.Picture, onResurrectBuy);
			}
			else
			{
				GUI3DPopupManager.Instance.ShowPopup("ShopItemBuy", item.Description, item.Name, item.Picture, onResurrectBuy);
			}
			BuyItemPopup buyItemPopup = (BuyItemPopup)GUI3DPopupManager.Instance.CurrentPopup;
			if (buyItemPopup != null)
			{
				buyItemPopup.Price.SetDynamicText(item.Price.ToString());
				buyItemPopup.YourSkullies.SetDynamicText(StringUtil.FormatNumbers(PlayerAccount.Instance.RetrieveMoney()));
				buyItemPopup.SetOkText(MonoBehaviorSingleton<GUI3DLocalization>.Instance.GetText("Unsorted", "GetItNow", "!BAD_TEXT!"));
			}
			return;
		}
		Store.Instance.ConsumeItem(item.Id);
		if (PlayerAccount.Instance.CurrentGameMode == PlayerAccount.GameMode.Adventure || PlayerAccount.Instance.CurrentGameMode == PlayerAccount.GameMode.Challenge)
		{
			CheckPointManager.RespawnOnLastCheckPoint();
			CameraFade.Instance.FadeIn(1f);
			GameEventDispatcher.Dispatch(null, new OnPlayerRespawningNow());
			fading = false;
			fredIsDead = false;
			ItemInfo item2 = Store.Instance.GetItem(111);
			if (item2 != null && item2.Upgrades > 0)
			{
				GameEventDispatcher.Dispatch(null, new OnAdventureResurrect());
			}
			else
			{
				CharHelper.GetProps().Lives = 0;
			}
		}
		else
		{
			ResetGameForEndless(false);
		}
	}

	private static void onResurrectBuy(GUI3DPopupManager.PopupResult result)
	{
		if (CharHelper.GetProps().freeResurectByTapjoy == ResurrectStatus.ACTIVATED)
		{
			return;
		}
		ItemInfo item = Store.Instance.GetItem(120);
		if (result == GUI3DPopupManager.PopupResult.Yes)
		{
			if (!Store.Instance.CheckMoney(item.Id))
			{
				ChunkRelocator.SetChangeVisibility(false);
				GUI3DPopupManager.Instance.ShowPopup("NotEnoughMoney", OnCloseNotEnoughMoney);
				return;
			}
			Store.Instance.Purchase(item.Id);
		}
		GUI3DPopupManager.Instance.ShowPopup("ConsumingItemPopupResurrect", onResurrectPopupClose);
	}

	public static void resurrectByTapjoy()
	{
		onResurrectPopupClose(GUI3DPopupManager.PopupResult.Yes);
	}

	public static void openResurrectPostVideoPopup()
	{
		GUI3DPopupManager.Instance.ShowPopup("ResurrectPostVideo", resurrectByTapjoyContinue);
	}

	public static void resurrectByTapjoyContinue(GUI3DPopupManager.PopupResult result)
	{
		resurrectByTapjoy();
	}

	private static void OnCloseNotEnoughMoney(GUI3DPopupManager.PopupResult result)
	{
		if (result == GUI3DPopupManager.PopupResult.Yes)
		{
			if (resurrectDialog)
			{
				resurrectDialog = false;
				SceneParamsManager.Instance.Push("resurrect");
			}
			else if (!GUI3DManager.Instance.IsActive("StoreGUI"))
			{
				if (PlayerAccount.Instance.CurrentGameMode == PlayerAccount.GameMode.Adventure)
				{
					SceneParamsManager.Instance.Push("SelectChapterAdventureEx");
				}
				else
				{
					SceneParamsManager.Instance.Push("SelectChapterSurvivalEx");
				}
			}
			GUI3D gUI3D = GUI3DManager.Instance.Activate("StoreGUI", true, true);
			GUI3DTabControl componentInChildren = gUI3D.GetComponentInChildren<GUI3DTabControl>();
			if (componentInChildren != null)
			{
				componentInChildren.SwitchToTab("Tab5");
			}
			else
			{
				Debug.Log("Can't find TabControl");
			}
		}
		else
		{
			GameEventDispatcher.Dispatch(null, new OnPlayerDead());
		}
	}

	public static void ResetGameForEndless(bool resetDistance)
	{
		GameObject gameObject = GameObject.FindWithTag("LevelRoot");
		if (gameObject != null)
		{
			gameObject.GetComponent<LevelRandomManager>().ResetLevel();
		}
		InputManager.ResetSuperSprintTimer();
		ExplosionManager.ResetExplosions();
		ChunkRelocator.SetChangeVisibility(true);
		CharHelper.GetCharStateMachine().RemoveWings(true, true);
		CheckPointManager.RespawnForEndless();
		if (resetDistance)
		{
			SoundManager.PlaySound(32);
			GameEventDispatcher.Dispatch(null, new OnEndLessReset());
		}
		else
		{
			SoundManager.PlaySound(65);
			GameEventDispatcher.Dispatch(null, new OnEndLessResurrect());
		}
		fredIsDead = false;
	}

	public static void BackToResurrectDialog()
	{
		ItemInfo item = Store.Instance.GetItem(120);
		if (Store.Instance.CheckMoney(item.Id))
		{
			Store.Instance.Purchase(item.Id);
			GUI3DManager.Instance.Activate("Hud", true, true);
			onResurrectPopupClose(GUI3DPopupManager.PopupResult.Yes);
		}
		else
		{
			GameEventDispatcher.Dispatch(null, new OnPlayerDead());
		}
	}
}
