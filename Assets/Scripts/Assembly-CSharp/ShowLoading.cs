using UnityEngine;

public class ShowLoading : MonoBehaviour
{
	public Texture loading;

	private void OnGUI()
	{
		float num = (float)Screen.width / ((float)loading.width * 5f);
		int num2 = (int)((float)loading.width * num);
		int num3 = (int)((float)loading.height * num);
		GUI.DrawTexture(new Rect((Screen.width >> 1) - (num2 >> 1), (Screen.height >> 1) - (num3 >> 1), num2, num3), loading);
	}
}
