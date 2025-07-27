using System;
using UnityEngine;

public class CinematicManager : MonoBehaviour
{
	private enum State
	{
		SET_OPEN_DOOR = 0,
		OPEN_DOOR = 1,
		SET_RUN1 = 2,
		RUN1 = 3,
		REDUCE_TS = 4,
		SHOW_FRED_TITLE = 5,
		INCREASE_TS = 6,
		RUN_GRIMMY = 7,
		REDUCE_TS2 = 8,
		SHOW_GRIMMY_TITLE = 9,
		INCREASE_TS2 = 10,
		GRIMMY_GO_AWAY = 11,
		PRE_FINISH = 12,
		FINISH = 13
	}

	private const float TITLE_LEN_IN_SECS = 2f;

	public GameObject door;

	public GameObject door2;

	public Cinematic1 cinCamera;

	public GameObject reaper;

	public GUI3D cinGUI;

	public GUI3DText cinText;

	public GUI3DText cinText2;

	public Camera mainCamera;

	private GameObject player;

	private State state;

	private CharacterController cc;

	private float accumTime;

	private float doorY1;

	private float doorY2;

	private DateTime dt1;

	private CharacterController rcc;

	private bool triggerReaperSound;

	private void Awake()
	{
		if (!SceneParamsManager.Instance.GetBool("LaunchCinematic", false, true))
		{
			base.gameObject.SetActive(false);
			return;
		}
		if (mainCamera == null)
		{
			mainCamera = Camera.main;
		}
		mainCamera.gameObject.SetActive(false);
		player = CharHelper.GetPlayer();
		cc = CharHelper.GetCharacterController();
		prepareCharacterForCinematic();
		accumTime = 0f;
		state = State.SET_OPEN_DOOR;
		doorY1 = door.transform.position.y;
		player.transform.position = new Vector3(0f, 0f, -150f);
		doorY2 = door.transform.position.y + 14f;
		triggerReaperSound = false;
	}

	private void Update()
	{
		switch (state)
		{
		case State.SET_OPEN_DOOR:
			SoundManager.PlaySound(cinCamera.transform.position, 28);
			state = State.OPEN_DOOR;
			break;
		case State.OPEN_DOOR:
			accumTime += Time.deltaTime;
			door.transform.position = Vector3.Lerp(new Vector3(door.transform.position.x, doorY1, door.transform.position.z), new Vector3(door.transform.position.x, doorY2, door.transform.position.z), accumTime);
			if (door2 != null)
			{
				door2.transform.position = Vector3.Lerp(new Vector3(door2.transform.position.x, doorY1, door2.transform.position.z), new Vector3(door2.transform.position.x, doorY2, door2.transform.position.z), accumTime);
			}
			if (accumTime >= 1f)
			{
				state = State.SET_RUN1;
			}
			break;
		case State.SET_RUN1:
			SoundManager.PlaySound(cinCamera.transform.position, 40);
			CharAnimManager.SuperSprint();
			accumTime = 0f;
			state = State.RUN1;
			break;
		case State.RUN1:
			cc.Move(Vector3.forward * Time.deltaTime * 40f);
			accumTime += Time.deltaTime;
			if (accumTime > 0.9f)
			{
				dt1 = DateTime.Now;
				accumTime = 0f;
				state = State.REDUCE_TS;
			}
			break;
		case State.REDUCE_TS:
			cc.Move(Vector3.forward * Time.deltaTime * 40f);
			accumTime = (float)((DateTime.Now - dt1).TotalMilliseconds / 1000.0);
			Time.timeScale = Mathf.Lerp(1f, 0f, accumTime * 8f);
			if (accumTime * 8f > 1f)
			{
				dt1 = DateTime.Now;
				accumTime = 0f;
				showFredTitle();
				state = State.SHOW_FRED_TITLE;
			}
			break;
		case State.SHOW_FRED_TITLE:
			accumTime = (float)((DateTime.Now - dt1).TotalMilliseconds / 1000.0);
			if (accumTime > 2f)
			{
				hideFredTitle();
				accumTime = 0f;
				dt1 = DateTime.Now;
				cinCamera.StopLookAt();
				state = State.INCREASE_TS;
			}
			break;
		case State.INCREASE_TS:
			cc.Move(Vector3.forward * Time.deltaTime * 40f);
			accumTime = (float)((DateTime.Now - dt1).TotalMilliseconds / 1000.0);
			Time.timeScale = Mathf.Lerp(0f, 1f, accumTime * 8f);
			if (accumTime * 8f > 1f)
			{
				cinCamera.TurnToTransform(reaper.transform);
				accumTime = 0f;
				reaper.SetActive(true);
				rcc = reaper.GetComponent<CharacterController>();
				if (rcc == null)
				{
					rcc = reaper.AddComponent<CharacterController>();
				}
				state = State.RUN_GRIMMY;
			}
			break;
		case State.RUN_GRIMMY:
			cc.Move(Vector3.forward * Time.deltaTime * 40f);
			rcc.Move(Vector3.forward * Time.deltaTime * 40f);
			accumTime += Time.deltaTime;
			if (!triggerReaperSound && accumTime > 0.4f)
			{
				SoundManager.PlaySound(cinCamera.transform.position, 43);
				triggerReaperSound = true;
			}
			if (accumTime > 1f)
			{
				dt1 = DateTime.Now;
				accumTime = 0f;
				state = State.REDUCE_TS2;
			}
			break;
		case State.REDUCE_TS2:
			rcc.Move(Vector3.forward * Time.deltaTime * 40f);
			accumTime = (float)((DateTime.Now - dt1).TotalMilliseconds / 1000.0);
			Time.timeScale = Mathf.Lerp(1f, 0f, accumTime * 4f);
			if (accumTime * 4f > 1f)
			{
				accumTime = 0f;
				showGrimmyTitle();
				state = State.SHOW_GRIMMY_TITLE;
			}
			break;
		case State.SHOW_GRIMMY_TITLE:
			accumTime = (float)((DateTime.Now - dt1).TotalMilliseconds / 1000.0);
			if (accumTime > 2f)
			{
				hideGrimmyTitle();
				accumTime = 0f;
				dt1 = DateTime.Now;
				state = State.INCREASE_TS2;
			}
			break;
		case State.INCREASE_TS2:
			rcc.Move(Vector3.forward * Time.deltaTime * 40f);
			accumTime = (float)((DateTime.Now - dt1).TotalMilliseconds / 1000.0);
			Time.timeScale = Mathf.Lerp(0f, 1f, accumTime * 8f);
			if (accumTime * 8f > 1f)
			{
				accumTime = 0f;
				cinCamera.StopLookAt();
				CameraFade.FadeOutMain();
				state = State.GRIMMY_GO_AWAY;
			}
			break;
		case State.GRIMMY_GO_AWAY:
			if (rcc == null)
			{
				rcc = reaper.AddComponent<CharacterController>();
			}
			rcc.Move(Vector3.forward * Time.deltaTime * 40f);
			accumTime += Time.deltaTime;
			if (accumTime > 1f)
			{
				mainCamera.gameObject.SetActive(true);
				CameraFade.FadeInMain();
				prepareCharacterForGame();
				CharHelper.PlaceCharacterOnStart();
				state = State.PRE_FINISH;
			}
			break;
		case State.PRE_FINISH:
			CharHelper.GetCharStateMachine().SwitchTo(ActionCode.RUNNING);
			state = State.FINISH;
			GameEventDispatcher.Dispatch(this, new OnEndCinematic());
			base.gameObject.SetActive(false);
			break;
		}
		checkSkip();
	}

	private void prepareCharacterForCinematic()
	{
		CharHelper.GetCharStateMachine().SwitchTo(ActionCode.STOPPED);
		CharHelper.GetCharStateMachine().enabled = false;
	}

	private void prepareCharacterForGame()
	{
		CharHelper.GetCharStateMachine().enabled = true;
	}

	private void showFredTitle()
	{
		cinText.SetDynamicText(MonoBehaviorSingleton<GUI3DLocalization>.Instance.GetText("Unsorted", "CinematicFredTitle", "!BAD_TEXT!"));
		cinText2.SetDynamicText(MonoBehaviorSingleton<GUI3DLocalization>.Instance.GetText("Unsorted", "CinematicFredDescription", "!BAD_TEXT!"));
		cinText.GetComponent<Renderer>().sharedMaterial.color = new Color(1f, 0.39f, 0f);
		GUI3DManager.Instance.Activate(cinGUI, false, false);
		SoundManager.PlaySound(cinCamera.transform.position, 44);
		SoundManager.PauseMusic(true);
	}

	private void checkSkip()
	{
		if (InputManager.GetJump() && state != State.SET_OPEN_DOOR && state != State.GRIMMY_GO_AWAY && state != State.PRE_FINISH && state != State.FINISH)
		{
			Time.timeScale = 1f;
			SoundManager.PauseMusic(false);
			CameraFade.FadeOutMain();
			state = State.GRIMMY_GO_AWAY;
		}
	}

	private void hideFredTitle()
	{
		cinGUI.SetVisible(false);
		SoundManager.PauseMusic(false);
	}

	private void showGrimmyTitle()
	{
		cinText.SetDynamicText(MonoBehaviorSingleton<GUI3DLocalization>.Instance.GetText("Unsorted", "CinematicGrimmyTitle", "!BAD_TEXT!"));
		cinText2.SetDynamicText(MonoBehaviorSingleton<GUI3DLocalization>.Instance.GetText("Unsorted", "CinematicGrimmyDescription", "!BAD_TEXT!"));
		cinText.GetComponent<Renderer>().sharedMaterial.color = new Color(0.75f, 0f, 1f);
		GUI3DManager.Instance.Activate(cinGUI, false, false);
		SoundManager.PlaySound(cinCamera.transform.position, 44);
		SoundManager.PauseMusic(true);
	}

	private void hideGrimmyTitle()
	{
		cinGUI.SetVisible(false);
		SoundManager.PauseMusic(false);
	}
}
