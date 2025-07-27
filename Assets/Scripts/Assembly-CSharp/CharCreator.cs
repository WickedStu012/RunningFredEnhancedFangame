using UnityEngine;

public class CharCreator : MonoBehaviour
{
	public string defChar = "Fred Classic";

	public string defLevel = "Level5";

	public GUI3D ThisGUI;

	private GUI3DTransition transition;

	private bool tutorial;

	private bool cinematic;

	private void Awake()
	{
		if (GUI3DPopupManager.Instance == null)
		{
			Debug.LogError("GUI3DPopupManager.Instance is null at this moment");
		}
		GUI3DPopupManager.Instance.Lock(true);
		transition = GetComponent<GUI3DTransition>();
		transition.TransitionEndEvent += OnEndTransitionIn;
		if (PlayerAccount.Instance.CurrentAvatarInfo != null)
		{
			defChar = PlayerAccount.Instance.CurrentAvatarInfo.AvatarPrefab;
		}
		if (GameObject.FindWithTag("Player") == null)
		{
			GameObject gameObject = Resources.Load(string.Format("Characters/CompletePrefabs/{0}", defChar), typeof(GameObject)) as GameObject;
			GameObject gameObject2 = Object.Instantiate(gameObject) as GameObject;
			gameObject2.name = gameObject.name;
		}
		if (PlayerAccount.Instance.CurrentGameMode == PlayerAccount.GameMode.Adventure)
		{
			cinematic = SceneParamsManager.Instance.GetBool("LaunchCinematic", false);
		}
		else
		{
			cinematic = false;
		}
	}

	private void Start()
	{
		if (!tutorial)
		{
			CharHelper.GetProps().MagnetLevel = PlayerAccount.Instance.MagnetLevel();
			CharHelper.GetProps().WallGrip = PlayerAccount.Instance.WallGrip();
			CharHelper.GetProps().ChickenFlaps = PlayerAccount.Instance.ChickenFlaps();
			CharHelper.GetProps().WallBounce = PlayerAccount.Instance.WallBounce();
			CharHelper.GetProps().SuccesiveJumpCount = ((!PlayerAccount.Instance.DoubleJump()) ? 1 : 2);
			CharHelper.GetProps().FastRecovery = PlayerAccount.Instance.FastRecovery();
			CharHelper.GetProps().Lives = PlayerAccount.Instance.Lives();
			CharHelper.GetProps().freeResurectByTapjoy = ResurrectStatus.READY;
			if (PlayerAccount.Instance.UnlockEverything)
			{
				CharHelper.GetProps().MagnetLevel = 5;
				CharHelper.GetProps().WallGrip = 5;
				CharHelper.GetProps().ChickenFlaps = 5;
				CharHelper.GetProps().SuccesiveJumpCount = 2;
				CharHelper.GetProps().WallBounce = 5;
				CharHelper.GetProps().FastRecovery = true;
			}
			if (GiftManager.Instance != null && GiftManager.Instance.CurrentActiveGift != null && GiftManager.Instance.CurrentActiveGift.Id == 3)
			{
				CharHelper.GetProps().Lives++;
			}
			if (PlayerAccount.Instance.CurrentGameMode == PlayerAccount.GameMode.Survival)
			{
				CharHelper.GetProps().Lives = 0;
			}
		}
	}

	private void OnEndTransitionIn(GUI3DOnTransitionEndEvent evt)
	{
		transition.TransitionEndEvent -= OnEndTransitionIn;
		transition.TransitionEndEvent += OnEndTransitionOut;
		tutorial = PlayerPrefsWrapper.GetTutorial();
		if (tutorial && PlayerAccount.Instance.CurrentGameMode != PlayerAccount.GameMode.Challenge)
		{
			defLevel = "Tutorial";
		}
		else
		{
			PlayerPrefsWrapper.SetTutorial(false);
			if (PlayerAccount.Instance.CurrentGameMode == PlayerAccount.GameMode.Challenge)
			{
				defLevel = PlayerAccount.Instance.CurrentChallenge;
			}
			else if (PlayerAccount.Instance.CurrentLevel != null)
			{
				defLevel = PlayerAccount.Instance.CurrentLevel;
			}
		}
		Application.LoadLevelAdditive(string.Format("{0}", defLevel));
		transition.StartTransition();
	}

	private void OnEndTransitionOut(GUI3DOnTransitionEndEvent evt)
	{
		transition.TransitionEndEvent -= OnEndTransitionOut;
		CameraFade.Instance.FadeIn(OnFadeIn);
		if (ThisGUI != null)
		{
			ThisGUI.SetVisible(false);
		}
	}

	private void OnFadeIn()
	{
		if (tutorial && PlayerAccount.Instance.CurrentGameMode != PlayerAccount.GameMode.Challenge)
		{
			GUI3DManager.Instance.Activate("Tutorial", false, false);
		}
		else if (!cinematic)
		{
			GUI3DManager.Instance.Activate("Hud", true, true);
		}
		else
		{
			GameEventDispatcher.AddListener("OnEndCinematic", OnCinematicEnds);
		}
		GUI3DPopupManager.Instance.Lock(false);
	}

	private void OnCinematicEnds(object sender, GameEvent evt)
	{
		GUI3DManager.Instance.Activate("Hud", true, true);
		GUI3DPopupManager.Instance.Lock(false);
	}
}
