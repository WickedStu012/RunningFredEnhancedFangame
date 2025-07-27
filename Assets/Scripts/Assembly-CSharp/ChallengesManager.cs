using System.Collections.Generic;
using UnityEngine;

public class ChallengesManager : MonoBehaviour
{
	public float Probability = 30f;

	public bool OfferChallenge;

	public GUI3DTransition CongratsTransition;

	public ChallengeItemInfo SelectedChallenge;

	private static ChallengesManager instance;

	private bool init;

	public static ChallengesManager Instance
	{
		get
		{
			if (instance == null)
			{
				instance = Object.FindObjectOfType(typeof(ChallengesManager)) as ChallengesManager;
				if (instance != null)
				{
					instance.Init();
				}
			}
			return instance;
		}
	}

	private void Awake()
	{
		instance = this;
	}

	private void Init()
	{
		if (init)
		{
			return;
		}
		init = true;
		if (PlayerAccount.Instance.CurrentGameMode == PlayerAccount.GameMode.Adventure)
		{
			if (!Store.Instance.HasAllMandatories())
			{
				return;
			}
			OfferChallenge = (float)Random.Range(0, 100) <= Probability;
			if (!OfferChallenge)
			{
				return;
			}
			ChallengeItemInfo[] array = ItemsLoader.Load<ChallengeItemInfo>("Shop/challengeslist");
			List<ChallengeItemInfo> list = new List<ChallengeItemInfo>();
			ChallengeItemInfo[] array2 = array;
			foreach (ChallengeItemInfo challengeItemInfo in array2)
			{
				if (!PlayerAccount.Instance.IsChallengeUnlocked(challengeItemInfo.Id))
				{
					list.Add(challengeItemInfo);
				}
			}
			if (list.Count > 0)
			{
				SelectedChallenge = list[Random.Range(0, list.Count)];
				if (CongratsTransition != null)
				{
					CongratsTransition.TransitionEndEvent += OnTransitionEnd;
				}
			}
			else
			{
				OfferChallenge = false;
			}
		}
		else if (PlayerAccount.Instance.CurrentGameMode == PlayerAccount.GameMode.Challenge)
		{
			SelectedChallenge = (ChallengeItemInfo)SceneParamsManager.Instance.GetObject("ChallengeItemInfo", null);
			GameEventDispatcher.AddListener("OnLevelComplete", OnLevelCompleted);
			GameEventDispatcher.AddListener("OnPlayerDead", OnPlayerIsDead);
		}
	}

	private void OnEnable()
	{
		instance = this;
		Init();
	}

	private void OnPlayerIsDead(object sender, GameEvent evt)
	{
		SoundManager.PlaySound(46);
	}

	private void OnLevelCompleted(object sender, GameEvent evt)
	{
		SoundManager.PlaySound(47);
		if (PlayerAccount.Instance.IsChallengeUnlocked(SelectedChallenge.Id))
		{
			Debug.Log("Challenge already unlocked...");
			GUI3DManager.Instance.Activate("Congratulations", false, false);
			return;
		}
		Debug.Log("Challenge unlocked...");
		PlayerAccount.Instance.UnlockeChallenge(SelectedChallenge.Id);
		SceneParamsManager.Instance.SetBool("ChallengeUnlocked", true);
		GUI3DManager.Instance.Activate("Congratulations", false, false);
	}

	private void OnTransitionEnd(GUI3DOnTransitionEndEvent evt)
	{
		CongratsTransition.TransitionEndEvent -= OnTransitionEnd;
		if (!PlayerAccount.Instance.ShowShopSuggestion())
		{
			GUI3DPopupManager.Instance.ShowPopup("ChallengePopup", "Will you take it?", "New Challenge Available!", OnClose);
		}
	}

	private void OnClose(GUI3DPopupManager.PopupResult result)
	{
		if (result == GUI3DPopupManager.PopupResult.Yes)
		{
			SceneParamsManager.Instance.SetBool("ChallengeUnlocked", false);
			SceneParamsManager.Instance.SetObject("ChallengeItemInfo", SelectedChallenge);
			PlayerAccount.Instance.SelectChallenge(SelectedChallenge.SceneName);
			CameraFade.Instance.FadeOut(OnFadeOut);
		}
	}

	private void OnFadeOut()
	{
		DedalordLoadLevel.LoadLevel("CharCreator");
	}

	private void OnDisable()
	{
		instance = null;
		if (!OfferChallenge && PlayerAccount.Instance != null && (PlayerAccount.Instance.CurrentGameMode != PlayerAccount.GameMode.Challenge || !SceneParamsManager.Instance.GetBool("ChallengeUnlocked", false)))
		{
			PlayerAccount.Instance.UnselectChallenge();
		}
	}
}
