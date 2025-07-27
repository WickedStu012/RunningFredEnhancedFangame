using UnityEngine;

public class GUI3DTransition : MonoBehaviour
{
	public enum States
	{
		Intro = 0,
		Outro = 1,
		Collapsing = 2,
		Expanding = 3,
		Collapsed = 4,
		Expanded = 5,
		Hide = 6,
		Show = 7
	}

	public delegate void OnTransitionStartEvent(GUI3DOnTransitionStartEvent evt);

	public delegate void OnTransitionEndEvent(GUI3DOnTransitionEndEvent evt);

	public States CurrentState = States.Hide;

	public bool StartOnEnable = true;

	public float TimerToStart;

	public float TimerToEnd;

	public bool DelayOnce;

	public bool Enabled = true;

	public bool ExecOutro = true;

	public bool ResetOnEnable;

	protected GUI3DOnTransitionStartEvent onTransitionStartEvent = new GUI3DOnTransitionStartEvent();

	protected GUI3DOnTransitionEndEvent onTransitionEndEvent = new GUI3DOnTransitionEndEvent();

	protected GUI3DPanel panel;

	protected float deltaTime;

	private bool start;

	private float currentTimer;

	private float currentEndTimer;

	private bool firstTime = true;

	private bool end;

	private bool checkEvents;

	private GUI3DTransition[] otherTransitions;

	private Renderer[] renderers;

	private States startState;

	public event OnTransitionStartEvent TransitionStartEvent;

	public event OnTransitionEndEvent TransitionEndEvent;

	protected virtual void Awake()
	{
		startState = CurrentState;
		onTransitionStartEvent.Target = this;
		onTransitionEndEvent.Target = this;
		otherTransitions = GetComponents<GUI3DTransition>();
		if (panel == null)
		{
			panel = GetComponent<GUI3DPanel>();
		}
		if (panel != null)
		{
			checkEvents = panel.CheckEvents;
		}
		deltaTime = 0f;
		Reset();
		if (StartOnEnable || CurrentState != States.Hide)
		{
			return;
		}
		GUI3DTransition[] array = otherTransitions;
		foreach (GUI3DTransition gUI3DTransition in array)
		{
			if (gUI3DTransition != this && gUI3DTransition.CurrentState != States.Hide && gUI3DTransition.CurrentState != States.Outro)
			{
				return;
			}
		}
		EnableRenderers(false);
	}

	public void Reset()
	{
		CurrentState = startState;
		OnResetTransition();
	}

	protected virtual void OnEnable()
	{
		if (panel == null)
		{
			panel = GetComponent<GUI3DPanel>();
		}
		if (ResetOnEnable)
		{
			Reset();
		}
		if (StartOnEnable && Enabled && (CurrentState == States.Hide || CurrentState == States.Outro))
		{
			if (!ResetOnEnable)
			{
				OnResetTransition();
			}
			if (TimerToStart == 0f || (DelayOnce && !firstTime))
			{
				StartIntroTransition();
			}
			else
			{
				currentTimer = TimerToStart;
				start = true;
				EnableRenderers(false);
			}
		}
		deltaTime = 0f;
	}

	protected virtual void OnDisable()
	{
		end = false;
		deltaTime = 0f;
		if (panel != null && CurrentState == States.Hide)
		{
			GUI3DTransition[] array = otherTransitions;
			foreach (GUI3DTransition gUI3DTransition in array)
			{
				if (gUI3DTransition != this && gUI3DTransition.CurrentState != States.Hide && gUI3DTransition.CurrentState != States.Outro)
				{
					return;
				}
			}
			panel.CheckEvents = false;
		}
		if (ResetOnEnable)
		{
			Reset();
		}
	}

	private void EnableRenderers(bool enable)
	{
	}

	public void StartTransition()
	{
		if (!Enabled)
		{
			return;
		}
		base.enabled = true;
		if (TimerToStart != 0f && currentTimer == 0f && (!DelayOnce || firstTime))
		{
			currentTimer = TimerToStart;
			start = true;
		}
		else
		{
			if (panel != null)
			{
				panel.CheckEvents = false;
			}
			OnStartTransition();
			if (this.TransitionStartEvent != null)
			{
				this.TransitionStartEvent(onTransitionStartEvent);
			}
			firstTime = false;
			if (TimerToEnd != 0f)
			{
				currentEndTimer = TimerToEnd;
				end = !end;
			}
		}
		EnableRenderers(true);
	}

	public void StartIntroTransition()
	{
		StartIntroTransition(false);
	}

	public void StartIntroTransition(bool force)
	{
		if (Enabled)
		{
			base.enabled = true;
		}
		if (force)
		{
			CurrentState = States.Hide;
		}
		if (!Enabled || (CurrentState != States.Hide && CurrentState != States.Outro))
		{
			return;
		}
		if (TimerToStart != 0f && currentTimer == 0f && (!DelayOnce || firstTime))
		{
			currentTimer = TimerToStart;
			start = true;
		}
		else
		{
			if (panel != null)
			{
				panel.CheckEvents = false;
			}
			OnStartTransition();
			if (this.TransitionStartEvent != null)
			{
				this.TransitionStartEvent(onTransitionStartEvent);
			}
			firstTime = false;
			if (TimerToEnd != 0f)
			{
				currentEndTimer = TimerToEnd;
				end = !end;
			}
		}
		EnableRenderers(true);
	}

	public void StartOutroTransition()
	{
		if (Enabled)
		{
			base.enabled = true;
		}
		if (!Enabled || CurrentState == States.Hide || CurrentState == States.Outro)
		{
			if (Enabled && CurrentState == States.Hide && this.TransitionEndEvent != null)
			{
				this.TransitionEndEvent(onTransitionEndEvent);
			}
			return;
		}
		if (panel != null)
		{
			panel.CheckEvents = false;
		}
		OnStartTransition();
		if (this.TransitionStartEvent != null)
		{
			this.TransitionStartEvent(onTransitionStartEvent);
		}
		firstTime = false;
		if (TimerToEnd != 0f)
		{
			currentEndTimer = TimerToEnd;
			end = !end;
		}
	}

	public void StopTransition()
	{
		if (panel != null && CurrentState != States.Hide && CurrentState != States.Outro)
		{
			panel.CheckEvents = checkEvents;
		}
		OnStopTransition();
		deltaTime = 0f;
		if (CurrentState == States.Hide)
		{
			if (otherTransitions != null)
			{
				GUI3DTransition[] array = otherTransitions;
				foreach (GUI3DTransition gUI3DTransition in array)
				{
					if (gUI3DTransition != this && gUI3DTransition.CurrentState != States.Hide && gUI3DTransition.CurrentState != States.Outro)
					{
						return;
					}
				}
			}
			EnableRenderers(false);
		}
		if (this.TransitionEndEvent != null)
		{
			this.TransitionEndEvent(onTransitionEndEvent);
		}
	}

	protected virtual void OnResetTransition()
	{
	}

	protected virtual void OnStartTransition()
	{
	}

	protected virtual void OnStopTransition()
	{
	}

	protected virtual void OnUpdate()
	{
	}

	private void Update()
	{
		deltaTime = GUI3DManager.Instance.DeltaTime;
		if (deltaTime > 0.025f)
		{
			deltaTime = 0.025f;
		}
		if (start)
		{
			currentTimer -= deltaTime;
			if (currentTimer <= 0f)
			{
				currentTimer = TimerToStart;
				start = false;
				StartIntroTransition();
			}
		}
		else if (end)
		{
			currentEndTimer -= deltaTime;
			if (currentEndTimer <= 0f)
			{
				currentEndTimer = TimerToEnd;
				StartOutroTransition();
			}
		}
		if (deltaTime != 0f)
		{
			OnUpdate();
		}
	}
}
