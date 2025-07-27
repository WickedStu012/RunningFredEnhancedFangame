using UnityEngine;

public class BeLordStatusGUI : MonoBehaviour
{
	private enum State
	{
		HIDDEN = 0,
		SHOWED = 1,
		SHOWING = 2,
		HIDING = 3,
		WAITING = 4,
		SHOWING_AUTO_HIDE = 5,
		WAITING_TO_HIDE = 6
	}

	public Texture texNotificationPanel;

	public Texture[] achievementsIcons;

	public GUIStyle style;

	public GUIStyle styleBkg;

	private float defTime = 2f;

	private State state;

	private float accumTime;

	private float movingTime;

	private float showTime;

	private StringConsts msg;

	private BeLordStatusNotifier cbFn;

	private BeLordLeaderboardItem lbData;

	private int texIcon;

	private void Awake()
	{
		state = State.HIDDEN;
		texIcon = -1;
	}

	private void OnGUI()
	{
		GUI.depth = -5;
		switch (state)
		{
		case State.SHOWED:
		{
			Rect rect = new Rect((Screen.width >> 1) - (texNotificationPanel.width >> 1), Screen.height - texNotificationPanel.height, texNotificationPanel.width, texNotificationPanel.height);
			GUI.DrawTexture(rect, texNotificationPanel);
			StringUtil.DrawLabel(rect, msg, style, styleBkg);
			if (texIcon != -1)
			{
				GUI.DrawTexture(new Rect(rect.x + 16f, rect.y + 8f, achievementsIcons[texIcon].width, achievementsIcons[texIcon].height), achievementsIcons[texIcon]);
			}
			break;
		}
		case State.SHOWING:
		case State.HIDING:
		case State.SHOWING_AUTO_HIDE:
		{
			accumTime += Time.deltaTime;
			float top;
			if (state == State.SHOWING || state == State.SHOWING_AUTO_HIDE)
			{
				top = Mathf.Lerp(Screen.height, Screen.height - texNotificationPanel.height, accumTime / movingTime);
				if (accumTime >= movingTime)
				{
					if (state == State.SHOWING)
					{
						state = State.SHOWED;
						if (cbFn != null)
						{
							cbFn();
							cbFn = null;
						}
					}
					else
					{
						state = State.WAITING_TO_HIDE;
					}
				}
			}
			else
			{
				top = Mathf.Lerp(Screen.height - texNotificationPanel.height, Screen.height, accumTime / movingTime);
				if (accumTime >= movingTime)
				{
					state = State.HIDDEN;
					if (cbFn != null)
					{
						cbFn();
						cbFn = null;
					}
				}
			}
			Rect rect = new Rect((Screen.width >> 1) - (texNotificationPanel.width >> 1), top, texNotificationPanel.width, texNotificationPanel.height);
			GUI.DrawTexture(rect, texNotificationPanel);
			StringUtil.DrawLabel(rect, msg, style, styleBkg);
			if (texIcon != -1)
			{
				GUI.DrawTexture(new Rect(rect.x + 16f, rect.y + 8f, achievementsIcons[texIcon].width, achievementsIcons[texIcon].height), achievementsIcons[texIcon]);
			}
			break;
		}
		case State.WAITING:
		{
			accumTime += Time.deltaTime;
			if (accumTime >= movingTime)
			{
				accumTime = 0f;
				state = State.HIDING;
				movingTime = defTime;
			}
			Rect rect = new Rect((Screen.width >> 1) - (texNotificationPanel.width >> 1), Screen.height - texNotificationPanel.height, texNotificationPanel.width, texNotificationPanel.height);
			GUI.DrawTexture(rect, texNotificationPanel);
			StringUtil.DrawLabel(rect, msg, style, styleBkg);
			if (texIcon != -1)
			{
				GUI.DrawTexture(new Rect(rect.x + 16f, rect.y + 8f, achievementsIcons[texIcon].width, achievementsIcons[texIcon].height), achievementsIcons[texIcon]);
			}
			break;
		}
		case State.WAITING_TO_HIDE:
		{
			accumTime += Time.deltaTime;
			if (accumTime >= showTime)
			{
				accumTime = 0f;
				state = State.HIDING;
				movingTime = defTime;
			}
			Rect rect = new Rect((Screen.width >> 1) - (texNotificationPanel.width >> 1), Screen.height - texNotificationPanel.height, texNotificationPanel.width, texNotificationPanel.height);
			GUI.DrawTexture(rect, texNotificationPanel);
			StringUtil.DrawLabel(rect, msg, style, styleBkg);
			if (texIcon != -1)
			{
				GUI.DrawTexture(new Rect(rect.x + 16f, rect.y + 8f, achievementsIcons[texIcon].width, achievementsIcons[texIcon].height), achievementsIcons[texIcon]);
			}
			break;
		}
		}
	}

	public void ShowImmediate(StringConsts msg)
	{
		texIcon = -1;
		Show(msg, 0f, null);
	}

	public void Show(StringConsts msg)
	{
		texIcon = -1;
		Show(msg, null);
	}

	public void Show(StringConsts msg, BeLordStatusNotifier cb)
	{
		texIcon = -1;
		Show(msg, defTime, cb);
	}

	public void Show(StringConsts msg, float time, BeLordStatusNotifier cb)
	{
		texIcon = -1;
		accumTime = 0f;
		cbFn = cb;
		this.msg = msg;
		movingTime = time;
		state = State.SHOWING;
	}

	public void ShowFor(StringConsts msg, float showTime)
	{
		ShowForWithIcon(msg, showTime, -1);
	}

	public void ShowForWithIcon(StringConsts msg, float showTime, int icon)
	{
		texIcon = icon;
		accumTime = 0f;
		cbFn = null;
		this.msg = msg;
		movingTime = defTime;
		this.showTime = showTime;
		state = State.SHOWING_AUTO_HIDE;
	}

	public void HideImmediate()
	{
		Hide(0f, null);
	}

	public void Hide()
	{
		Hide(null);
	}

	public void Hide(BeLordStatusNotifier cb)
	{
		Hide(defTime, cb);
	}

	public void Hide(float time, BeLordStatusNotifier cb)
	{
		accumTime = 0f;
		cbFn = cb;
		movingTime = time;
		state = State.HIDING;
	}

	public void HideIn(StringConsts msg)
	{
		HideIn(msg, defTime);
	}

	public void HideIn(StringConsts msg, float time)
	{
		accumTime = 0f;
		cbFn = null;
		movingTime = time;
		state = State.WAITING;
	}
}
