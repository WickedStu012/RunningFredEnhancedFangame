using UnityEngine;

public class InstantActionButton : PlayButton
{
	public float Timer = 1f;

	private float time;

	private bool activated;

	private float timer;

	private void Awake()
	{
		timer = Timer;
	}

	protected override void OnEnable()
	{
		base.OnEnable();
		button.CheckEvents = false;
		activated = false;
		time = Time.time;
	}

	private void Update()
	{
		if (MogaInput.Instance.GetButtonADown() || MogaInput.Instance.GetButtonStartDown())
		{
			OnRelease(null);
		}
		if (!activated && Time.time - time >= timer)
		{
			timer = 1f;
			button.CheckEvents = true;
			activated = true;
		}
	}

	protected override void OnRelease(GUI3DOnReleaseEvent evt)
	{
		base.OnRelease(evt);
		StatsManager.LogEvent(StatVar.MAIN_MENU_BUTTON, "QUICK_START");
	}
}
