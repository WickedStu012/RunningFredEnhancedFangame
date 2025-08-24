using UnityEngine;

public class AndroidSetUserName : MonoBehaviour
{
	private void Start()
	{
		GUI3DText component = GetComponent<GUI3DText>();
		if (component != null)
		{
			string text = string.Empty;
			if (Application.isEditor)
			{
				GameObject gameObject = GameObject.Find("PlayerAccount");
				text = ((!(gameObject != null)) ? string.Empty : gameObject.GetComponent<WebDataStore>().editorTestUserName);
			}
			else if (Application.platform == RuntimePlatform.Android)
			{
				text = AndroidPluginBypass.GetAndroidUserName();
			}
			else if (Application.platform == RuntimePlatform.NaCl)
			{
				text = ChromeUserName.GetUserName();
			}
			else if (Application.platform == RuntimePlatform.WebGLPlayer)
			{
				text = KongregateAPI.GetUserName();
			}
			if (text != null && text != string.Empty)
			{
				component.SetDynamicText(string.Format(MonoBehaviorSingleton<GUI3DLocalization>.Instance.GetText("Unsorted", "SavesProgress", "!BAD_TEXT!"), text));
			}
			else
			{
				component.SetDynamicText(string.Format(MonoBehaviorSingleton<GUI3DLocalization>.Instance.GetText("Unsorted", "SavesProgress", "!BAD_TEXT!"), "none"));
			}
		}
	}
}
