using UnityEngine;

public class ChangeGUIOnTime : MonoBehaviour
{
	public float Timer = 2f;

	public GUI3DTransition Transition;

	public bool ChangeOnTap = true;

	public string ActivateGUI = string.Empty;

	private float timeAccum;

	private void Awake()
	{
		Transition = GetComponentInChildren<GUI3DTransition>();
	}

	private void Start()
	{
	}

	private void OnEnable()
	{
	}

	private void OnDisable()
	{
	}

	private void Update()
	{
		if (timeAccum < Timer)
		{
			timeAccum += GUI3DManager.Instance.DeltaTime;
			if (timeAccum >= Timer || (ChangeOnTap && ((MogaInput.Instance.IsConnected() && (MogaInput.Instance.GetButtonADown() || MogaInput.Instance.GetButtonBDown())) || Input.GetButtonDown("Fire1") || Input.GetButtonDown("Jump") || Input.GetKeyDown(KeyCode.X))))
			{
				Transition.TransitionEndEvent += OnTransitionEnd;
				Transition.StartTransition();
				base.enabled = false;
			}
		}
	}

	private void OnTransitionEnd(GUI3DOnTransitionEndEvent evt)
	{
		GUI3DManager.Instance.Activate(ActivateGUI, true, true);
		Transition.TransitionEndEvent -= OnTransitionEnd;
	}
}
