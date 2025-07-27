using UnityEngine;

public class TutorialManager : MonoBehaviour
{
	private const int PickCoins = 3;

	private const int Steer = 6;

	private const int Jump = 9;

	private const int Accelerators = 12;

	private const int DIALOGS_COUNT = 17;

	public LevelTutorialManager tutorialManager;

	public GUI3DTransition ReaperTransition;

	public GUI3DTransition BackgroundTransition;

	public GUI3DTransition[] DialogImgTransitions;

	public GUI3DTransition[] DialogImgTransDesktop;

	public float DialogTimer = 4f;

	public GUI3DPopFrontTransition TapScreenTransition;

	public GUI3DPopFrontTransition PressSpaceTransition;

	public string TryAgainText = string.Empty;

	private int currentDialog;

	private int currentTitle;

	private float time;

	private int lastDialog;

	private bool tryAgain;

	private bool isDialog;

	private TutorialDoor door;

	private bool paused;

	private GUI3DTransition currentImgTransition;

	private void Awake()
	{
	}

	private void OnEnable()
	{
		MonoBehaviorSingleton<GUI3DLocalization>.Instance.Load("Tutorial");
		ReaperTransition.TransitionStartEvent += OnReaperTransitionStart;
		ReaperTransition.TransitionEndEvent += OnReaperTransitionEnd;
		GameEventDispatcher.AddListener("OnTutorialPause", OnTutorialPaused);
		GameEventDispatcher.AddListener("OnTutorialObjectiveComplete", OnObjectiveComplete);
		GameEventDispatcher.AddListener("OnTutorialObjectiveFail", OnObjectiveFail);
		GameEventDispatcher.AddListener("OnLevelComplete", OnLevelCompleted);
		time = Time.time;
	}

	private void OnDisable()
	{
		GameEventDispatcher.RemoveListener("OnLevelComplete", OnLevelCompleted);
		GameEventDispatcher.RemoveListener("OnTutorialObjectiveFail", OnObjectiveFail);
		GameEventDispatcher.RemoveListener("OnTutorialObjectiveComplete", OnObjectiveComplete);
		GameEventDispatcher.RemoveListener("OnTutorialPause", OnTutorialPaused);
		ReaperTransition.TransitionEndEvent -= OnReaperTransitionEnd;
		ReaperTransition.TransitionStartEvent -= OnReaperTransitionStart;
	}

	private void OnLevelCompleted(object sender, GameEvent evt)
	{
		GUI3DManager.Instance.Activate("TutorialComplete", true, true);
	}

	private void OnReaperTransitionStart(GUI3DOnTransitionStartEvent evt)
	{
		PlayerPrefsWrapper.SetTutorial(false);
	}

	private void OnReaperTransitionEnd(GUI3DOnTransitionEndEvent evt)
	{
		if (tutorialManager == null)
		{
			tutorialManager = Object.FindObjectOfType(typeof(LevelTutorialManager)) as LevelTutorialManager;
		}
		if (tutorialManager == null)
		{
			return;
		}
		time = Time.time;
		if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
		{
			if (currentDialog < DialogImgTransitions.Length && DialogImgTransitions[currentDialog] != null)
			{
				currentImgTransition = DialogImgTransitions[currentDialog];
				currentImgTransition.StartTransition();
			}
			string text = MonoBehaviorSingleton<GUI3DLocalization>.Instance.GetText("Tutorial", "TutorialDialog" + currentDialog++, "!BAD_TEXT!");
			GUI3DPopupManager.Instance.ShowPopup("Dialog", text, OnDialogClose, false);
		}
		else
		{
			if (currentDialog < DialogImgTransDesktop.Length && DialogImgTransDesktop[currentDialog] != null)
			{
				currentImgTransition = DialogImgTransDesktop[currentDialog];
				currentImgTransition.StartTransition();
			}
			string text2 = MonoBehaviorSingleton<GUI3DLocalization>.Instance.GetText("Tutorial", "TutorialDialogDesktop" + currentDialog++, "!BAD_TEXT!");
			GUI3DPopupManager.Instance.ShowPopup("Dialog", text2, OnDialogClose, false);
		}
		isDialog = true;
		ReaperTransition.TransitionEndEvent -= OnReaperTransitionEnd;
	}

	private void OnTutorialPaused(object sender, GameEvent e)
	{
		OnTutorialPause onTutorialPause = (OnTutorialPause)e;
		ReaperTransition.StartTransition();
		BackgroundTransition.StartTransition();
		isDialog = false;
		GUI3DPopupManager.Instance.CloseCurrentPopup(GUI3DPopupManager.PopupResult.Yes);
		paused = true;
		time = Time.time;
		door = onTutorialPause.door;
	}

	private void OnDialogClose(GUI3DPopupManager.PopupResult result)
	{
		if (currentImgTransition != null)
		{
			currentImgTransition.StartTransition();
		}
		currentImgTransition = null;
		time = Time.time;
		if (isDialog)
		{
			switch (currentDialog)
			{
			case 3:
				Debug.Log("Phase1");
				lastDialog = 2;
				tutorialManager.SetState(LevelTutorialState.PHASE_1);
				door.OpenDoor();
				door = null;
				paused = false;
				currentTitle = 0;
				break;
			case 6:
				Debug.Log("Phase2");
				lastDialog = 5;
				tutorialManager.SetState(LevelTutorialState.PHASE_2);
				door.OpenDoor();
				door = null;
				paused = false;
				currentTitle = 1;
				break;
			case 9:
				Debug.Log("Phase3");
				lastDialog = 8;
				tutorialManager.SetState(LevelTutorialState.PHASE_3);
				door.OpenDoor();
				door = null;
				paused = false;
				currentTitle = 2;
				break;
			case 12:
				Debug.Log("Phase4");
				lastDialog = 11;
				tutorialManager.SetState(LevelTutorialState.PHASE_4);
				door.OpenDoor();
				door = null;
				paused = false;
				currentTitle = 3;
				break;
			}
		}
		if (!tryAgain && paused && currentDialog < 17)
		{
			if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
			{
				if (currentDialog < DialogImgTransitions.Length && DialogImgTransitions[currentDialog] != null)
				{
					currentImgTransition = DialogImgTransitions[currentDialog];
					currentImgTransition.StartTransition();
				}
				string text = MonoBehaviorSingleton<GUI3DLocalization>.Instance.GetText("Tutorial", "TutorialDialog" + currentDialog++, "!BAD_TEXT!");
				GUI3DPopupManager.Instance.ShowPopup("Dialog", text, OnDialogClose, false);
			}
			else
			{
				if (currentDialog < DialogImgTransDesktop.Length && DialogImgTransDesktop[currentDialog] != null)
				{
					currentImgTransition = DialogImgTransDesktop[currentDialog];
					currentImgTransition.StartTransition();
				}
				string text2 = MonoBehaviorSingleton<GUI3DLocalization>.Instance.GetText("Tutorial", "TutorialDialogDesktop" + currentDialog++, "!BAD_TEXT!");
				GUI3DPopupManager.Instance.ShowPopup("Dialog", text2, OnDialogClose, false);
			}
			isDialog = true;
		}
		else if (tryAgain)
		{
			tryAgain = false;
			GUI3DPopupManager.Instance.ShowPopup("Dialog", TryAgainText, OnDialogClose, false);
			isDialog = true;
		}
		else if (currentDialog >= 17)
		{
			door.OpenDoor();
			door = null;
			paused = false;
			tutorialManager.SetState(LevelTutorialState.ENDER);
			ReaperTransition.StartTransition();
			BackgroundTransition.StartTransition();
			isDialog = false;
		}
		else
		{
			string text3 = MonoBehaviorSingleton<GUI3DLocalization>.Instance.GetText("Tutorial", "TutorialTitle" + currentTitle, "!BAD_TEXT!");
			GUI3DPopupManager.Instance.ShowPopup("Title", text3, OnDialogClose, false);
			ReaperTransition.StartTransition();
			BackgroundTransition.StartTransition();
			isDialog = false;
		}
		if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
		{
			if (TapScreenTransition.CurrentState == GUI3DTransition.States.Show || TapScreenTransition.CurrentState == GUI3DTransition.States.Intro)
			{
				TapScreenTransition.StartTransition();
			}
		}
		else if (PressSpaceTransition.CurrentState == GUI3DTransition.States.Show || PressSpaceTransition.CurrentState == GUI3DTransition.States.Intro)
		{
			PressSpaceTransition.StartTransition();
		}
	}

	private void OnObjectiveComplete(object sender, GameEvent evt)
	{
		time = Time.time;
		paused = true;
	}

	private void OnObjectiveFail(object sender, GameEvent evt)
	{
		currentDialog = lastDialog;
		tryAgain = true;
		time = Time.time;
		paused = true;
	}

	private void FixedUpdate()
	{
		if (Time.timeScale == 0f || !paused)
		{
			return;
		}
		if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
		{
			if (TapScreenTransition.CurrentState == GUI3DTransition.States.Hide && Time.time - time >= DialogTimer)
			{
				time = Time.time;
				TapScreenTransition.StartTransition();
			}
		}
		else if (PressSpaceTransition.CurrentState == GUI3DTransition.States.Hide && Time.time - time >= DialogTimer)
		{
			time = Time.time;
			PressSpaceTransition.StartTransition();
		}
		if (isDialog && (InputManager.GetJumpDown() || Input.GetKeyDown(KeyCode.Space) || (MogaInput.Instance.IsConnected() && (MogaInput.Instance.GetButtonADown() || MogaInput.Instance.GetButtonBDown()))))
		{
			GUI3DPopupManager.Instance.CloseCurrentPopup(GUI3DPopupManager.PopupResult.Yes);
		}
	}
}
