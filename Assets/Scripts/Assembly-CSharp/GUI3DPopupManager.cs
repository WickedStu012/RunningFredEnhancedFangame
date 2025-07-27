using System.Collections.Generic;
using UnityEngine;

public class GUI3DPopupManager : GUI3D
{
	public enum PopupResult
	{
		Yes = 0,
		No = 1,
		Cancel = 2
	}

	private class PopupQueue
	{
		public string PopupType;

		public string Text;

		public string Title;

		public string Icon;

		public OnCloseCallback Callback;

		public PopupQueue(string popupType, string text, string title, string icon, OnCloseCallback callback)
		{
			PopupType = popupType;
			Text = text;
			Title = title;
			Icon = icon;
			Callback = callback;
		}
	}

	public delegate void OnCloseCallback(PopupResult positive);

	private static GUI3DPopupManager instance;

	private Dictionary<string, GUI3DPopup> popups;

	private List<OnCloseCallback> onCloseCallback = new List<OnCloseCallback>();

	private bool showingPopup;

	private Queue<PopupQueue> popupQueue = new Queue<PopupQueue>();

	private GUI3DPopup currentPopup;

	private bool locked;

	public static GUI3DPopupManager Instance
	{
		get
		{
			return instance;
		}
	}

	public GUI3DPopup CurrentPopup
	{
		get
		{
			return currentPopup;
		}
	}

	protected override void Awake()
	{
		instance = this;
		base.Awake();
	}

	protected void Start()
	{
		GUI3DPopup[] componentsInChildren = GetComponentsInChildren<GUI3DPopup>();
		if (componentsInChildren != null)
		{
			popups = new Dictionary<string, GUI3DPopup>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				popups[componentsInChildren[i].name] = componentsInChildren[i];
				componentsInChildren[i].gameObject.SetActive(false);
			}
		}
	}

	public void Lock(bool locked)
	{
		if (this.locked != locked)
		{
			if (this.locked && !locked)
			{
				this.locked = locked;
				CheckQueue();
			}
			else
			{
				this.locked = locked;
			}
		}
	}

	private void OnEnable()
	{
		instance = this;
	}

	private void OnDestroy()
	{
		instance = null;
	}

	public void ShowPopup(string popupType)
	{
		ShowPopup(popupType, null, null, null, null, true, null);
	}

	public void ShowPopup(string popupType, OnCloseCallback callback)
	{
		ShowPopup(popupType, null, null, null, callback, true, null);
	}

	public void ShowPopup(string popupType, OnCloseCallback callback, bool disableGUI)
	{
		ShowPopup(popupType, null, null, null, callback, disableGUI, null);
	}

	public void ShowPopup(string popupType, string text)
	{
		ShowPopup(popupType, text, null, null, null, true, null);
	}

	public void ShowPopup(string popupType, string text, bool disableGUI)
	{
		ShowPopup(popupType, text, null, null, null, disableGUI, null);
	}

	public void ShowPopup(string popupType, string text, string title)
	{
		ShowPopup(popupType, text, title, null, null, true, null);
	}

	public void ShowPopup(string popupType, string text, string title, bool disableGUI)
	{
		ShowPopup(popupType, text, title, null, null, disableGUI, null);
	}

	public void ShowPopup(string popupType, string text, string title, string icon)
	{
		ShowPopup(popupType, text, title, icon, null, true, null);
	}

	public void ShowPopup(string popupType, string text, string title, string icon, object customData)
	{
		ShowPopup(popupType, text, title, icon, null, true, customData);
	}

	public void ShowPopup(string popupType, string text, string title, string icon, bool disableGUI)
	{
		ShowPopup(popupType, text, title, icon, null, disableGUI, null);
	}

	public void ShowPopup(string popupType, string text, OnCloseCallback onClose)
	{
		ShowPopup(popupType, text, null, null, onClose, true, null);
	}

	public void ShowPopup(string popupType, string text, OnCloseCallback onClose, bool disableGUI)
	{
		ShowPopup(popupType, text, null, null, onClose, disableGUI, null);
	}

	public void ShowPopup(string popupType, string text, string title, OnCloseCallback onClose)
	{
		ShowPopup(popupType, text, title, null, onClose, true, null);
	}

	public void ShowPopup(string popupType, string text, string title, OnCloseCallback onClose, bool disableGUI)
	{
		ShowPopup(popupType, text, title, null, onClose, disableGUI, null);
	}

	public void ShowPopup(string popupType, string text, string title, string icon, OnCloseCallback onClose)
	{
		ShowPopup(popupType, text, title, icon, onClose, true, null);
	}

	public void ShowPopup(string popupType, string text, string title, string icon, OnCloseCallback onClose, bool disableGUI)
	{
		ShowPopup(popupType, text, title, icon, onClose, disableGUI, null);
	}

	public void ShowPopup(string popupType, string text, string title, string icon, OnCloseCallback onClose, bool disableGUI, object customData)
	{
		if (!showingPopup && !locked)
		{
			GUI3DManager.Instance.SaveCurrentState();
			GUI3DManager.Instance.Activate(this, disableGUI, false);
			foreach (GUI3DPopup value in popups.Values)
			{
				value.gameObject.SetActive(false);
			}
			if (popups.ContainsKey(popupType))
			{
				showingPopup = true;
				GUI3DPopup gUI3DPopup = popups[popupType];
				if (text != null && gUI3DPopup.TextField != null)
				{
					gUI3DPopup.TextField.SetDynamicText(text);
				}
				if (title != null && gUI3DPopup.Title != null)
				{
					gUI3DPopup.Title.SetDynamicText(title);
				}
				if (icon != null && gUI3DPopup.Icon != null)
				{
					gUI3DPopup.Icon.ObjectSize = Vector2.zero;
					gUI3DPopup.Icon.RefreshMaterial(icon);
				}
				gUI3DPopup.Visible = true;
				onCloseCallback.Add(onClose);
				gUI3DPopup.AddOnCloseCallback(OnClosePopup);
				gUI3DPopup.SetCustomData(customData);
				currentPopup = gUI3DPopup;
			}
			else
			{
				Debug.LogWarning("Warning: " + popupType + " is not a valid Popup name!");
			}
		}
		else
		{
			PopupQueue item = new PopupQueue(popupType, text, title, icon, onClose);
			popupQueue.Enqueue(item);
		}
	}

	public void ShowPopup(string popupType, string text, string title, string icon, string price, OnCloseCallback onClose)
	{
		if (!showingPopup && !locked)
		{
			GUI3DManager.Instance.SaveCurrentState();
			GUI3DManager.Instance.Activate(this, true, false);
			foreach (GUI3DPopup value in popups.Values)
			{
				value.gameObject.SetActive(false);
			}
			if (popups.ContainsKey(popupType))
			{
				showingPopup = true;
				GUI3DPopup gUI3DPopup = popups[popupType];
				if (text != null && gUI3DPopup.TextField != null)
				{
					gUI3DPopup.TextField.SetDynamicText(text);
				}
				if (title != null && gUI3DPopup.Title != null)
				{
					gUI3DPopup.Title.SetDynamicText(title);
				}
				if (icon != null && gUI3DPopup.Icon != null)
				{
					gUI3DPopup.Icon.ObjectSize = Vector2.zero;
					gUI3DPopup.Icon.RefreshMaterial(icon);
				}
				BuyItemPopup buyItemPopup = gUI3DPopup as BuyItemPopup;
				if (buyItemPopup != null)
				{
					buyItemPopup.Price.SetDynamicText(price);
				}
				gUI3DPopup.Visible = true;
				onCloseCallback.Add(onClose);
				gUI3DPopup.AddOnCloseCallback(OnClosePopup);
				gUI3DPopup.SetCustomData(null);
				currentPopup = gUI3DPopup;
			}
			else
			{
				Debug.LogWarning("Warning: " + popupType + " is not a valid Popup name!");
			}
		}
		else
		{
			PopupQueue item = new PopupQueue(popupType, text, title, icon, onClose);
			popupQueue.Enqueue(item);
		}
	}

	public void CloseCurrentPopup(PopupResult result)
	{
		CloseCurrentPopup(result, false);
	}

	public void CloseCurrentPopup(PopupResult result, bool purgeQueue)
	{
		if (purgeQueue)
		{
			popupQueue.Clear();
		}
		if (currentPopup != null)
		{
			currentPopup.Close(result);
		}
	}

	public string CurrentPopupName()
	{
		if (currentPopup != null)
		{
			return currentPopup.name;
		}
		return string.Empty;
	}

	private void OnClosePopup(PopupResult result)
	{
		showingPopup = false;
		currentPopup.gameObject.SetActive(false);
		currentPopup = null;
		GUI3DManager.Instance.RestoreLastState();
		if (this.onCloseCallback.Count > 0)
		{
			OnCloseCallback onCloseCallback = this.onCloseCallback[0];
			this.onCloseCallback.RemoveAt(0);
			if (onCloseCallback != null)
			{
				onCloseCallback(result);
			}
		}
		CheckQueue();
	}

	private void CheckQueue()
	{
		if (this.popupQueue.Count > 0)
		{
			PopupQueue popupQueue = this.popupQueue.Dequeue();
			ShowPopup(popupQueue.PopupType, popupQueue.Text, popupQueue.Title, popupQueue.Icon, popupQueue.Callback);
		}
	}

	public GUI3DPopup GetPopupByName(string popupType)
	{
		GUI3DPopup result = null;
		if (popups.ContainsKey(popupType))
		{
			showingPopup = true;
			result = popups[popupType];
		}
		return result;
	}
}
