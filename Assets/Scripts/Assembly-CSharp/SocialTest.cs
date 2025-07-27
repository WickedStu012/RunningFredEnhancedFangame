using UnityEngine;

public class SocialTest : MonoBehaviour
{
	private void OnGUI()
	{
		Rect position = new Rect(10f, 10f, 200f, 35f);
		if (GUI.Button(position, "Init Twitter"))
		{
			BeLordSocial.Instance.InitTwitter();
		}
		position.y += 40f;
		if (GUI.Button(position, "Show Twitter Login"))
		{
			BeLordSocial.Instance.ShowTwitterOauthLoginDialog();
		}
		position.y += 40f;
		if (GUI.Button(position, "Post Twitter Msg"))
		{
			BeLordSocial.Instance.PostTwitterStatusUpdate("Testing 1, 2, 3...");
		}
	}
}
