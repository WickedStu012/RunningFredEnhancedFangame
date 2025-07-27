using UnityEngine;

public class WingsLeftTime : MonoBehaviour
{
	public static WingsLeftTime Instance;

	public GUI3DSlideTransition transition;

	private OnTimerFinish onFinish;

	private float angle;

	private bool rotateBar;

	private void Awake()
	{
		Instance = this;
	}

	private void Update()
	{
		if (!rotateBar)
		{
			return;
		}
		angle += Time.deltaTime * 100f;
		base.transform.localRotation = Quaternion.Euler(0f, 0f, 0f - angle);
		if (angle >= 180f)
		{
			rotateBar = false;
			transition.StartOutroTransition();
			if (onFinish != null)
			{
				onFinish();
			}
		}
	}

	public void StartWingsOutTimer(OnTimerFinish onFinish)
	{
		this.onFinish = onFinish;
		angle = 0f;
		rotateBar = true;
		transition.StartIntroTransition();
	}

	public void ForceCloseWingsOutTimer()
	{
		rotateBar = false;
		transition.StartOutroTransition();
	}
}
