using UnityEngine;

public class GUI3DPopup : GUI3DPanel
{
	public delegate void OnClose(GUI3DPopupManager.PopupResult result);

	public GUI3DText Title;

	public GUI3DText TextField;

	public GUI3DObject Icon;

	public GUI3DButton OkButton;

	public GUI3DButton CancelButton;

	public GUI3DButton CloseButton;

	public float Timer;

	public bool CancelOnEscapeButton = true;

	public bool AllowCloseWithButtons = true;

	private OnClose callback;

	private GUI3DPopupManager.PopupResult popupResult = GUI3DPopupManager.PopupResult.Cancel;

	private GUI3DTransition activateTransition;

	private bool closing;

	protected float time;

	protected GUI3DText okText;

	private GUI3DText cancelText;

	private GUI3DText closeText;

	private object customData;

	protected override void Awake()
	{
		base.Awake();
		if (OkButton != null)
		{
			OkButton.ReleaseEvent += OnClick;
			okText = OkButton.GetComponentInChildren<GUI3DText>();
		}
		if (CancelButton != null)
		{
			CancelButton.ReleaseEvent += OnClick;
			cancelText = CancelButton.GetComponentInChildren<GUI3DText>();
		}
		if (CloseButton != null)
		{
			CloseButton.ReleaseEvent += OnClick;
			closeText = CloseButton.GetComponentInChildren<GUI3DText>();
		}
		activateTransition = GetComponent<GUI3DTransition>();
	}

	private void OnDestroy()
	{
		if (OkButton != null)
		{
			OkButton.ReleaseEvent -= OnClick;
		}
		if (CancelButton != null)
		{
			CancelButton.ReleaseEvent -= OnClick;
		}
		if (CloseButton != null)
		{
			CloseButton.ReleaseEvent -= OnClick;
		}
	}

	public void SetOkText(string text)
	{
		if (okText != null)
		{
			okText.SetDynamicText(text);
		}
	}

	public void SetCancelText(string text)
	{
		if (cancelText != null)
		{
			cancelText.SetDynamicText(text);
		}
	}

	public void SetCloseText(string text)
	{
		if (closeText != null)
		{
			closeText.SetDynamicText(text);
		}
	}

	private void OnClick(GUI3DEvent evt)
	{
		GUI3DPopupManager.PopupResult result = GUI3DPopupManager.PopupResult.No;
		if (evt.Target == OkButton)
		{
			result = GUI3DPopupManager.PopupResult.Yes;
		}
		else if (evt.Target == CancelButton)
		{
			result = GUI3DPopupManager.PopupResult.Cancel;
		}
		Close(result);
	}

	protected void OnEnable()
	{
		time = Time.time;
	}

	private void OnDisable()
	{
		closing = false;
		if (activateTransition != null)
		{
			activateTransition.TransitionEndEvent -= OnEndTransition;
		}
	}

	private void OnEndTransition(GUI3DEvent evt)
	{
		InvokeCallback();
		closing = false;
	}

	private void InvokeCallback()
	{
		if (callback != null)
		{
			callback(popupResult);
		}
	}

	public void Close(GUI3DPopupManager.PopupResult result)
	{
		if (!closing)
		{
			popupResult = result;
			if (activateTransition != null)
			{
				closing = true;
				activateTransition.TransitionEndEvent += OnEndTransition;
				activateTransition.StartTransition();
			}
			else
			{
				InvokeCallback();
			}
		}
	}

	public void AddOnCloseCallback(OnClose callback)
	{
		this.callback = callback;
	}

	public void RemoveOnCloseCallback()
	{
		callback = null;
	}

	public object GetCustomData()
	{
		return customData;
	}

	public void SetCustomData(object data)
	{
		customData = data;
	}

	protected void Update()
	{
		if (MogaInput.Instance.IsConnected() && AllowCloseWithButtons)
		{
			if (MogaInput.Instance.GetButtonADown())
			{
				GUI3DPopupManager.Instance.CloseCurrentPopup(GUI3DPopupManager.PopupResult.Yes);
			}
			else if (MogaInput.Instance.GetButtonBDown())
			{
				GUI3DPopupManager.Instance.CloseCurrentPopup(GUI3DPopupManager.PopupResult.No);
			}
		}
		if (Timer != 0f && !closing && Time.time - time >= Timer)
		{
			Close(GUI3DPopupManager.PopupResult.Cancel);
			time = Time.time;
		}
		if (CancelOnEscapeButton && Input.GetKeyUp(KeyCode.Escape))
		{
			Close(GUI3DPopupManager.PopupResult.Cancel);
		}
	}
}
