using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class BlackLordGetAdTester : MonoBehaviour
{
	private AdInfo adInfo;

	private string msg = string.Empty;

	private void OnGUI()
	{
		if (GUILayout.Button("Get Skiing Fred Info"))
		{
			AdManager.Instance.GetAdScheduleFromWeb(onGetAdRes);
		}
		if (GUILayout.Button("Get Ad"))
		{
			adInfo = AdManager.Instance.GetAdFromSchedule();
			Debug.Log(string.Format("adInfo. ", adInfo.Message));
		}
		if (GUILayout.Button("Check File"))
		{
			string path = string.Format("{0}/test.png", Application.temporaryCachePath);
			msg = string.Format("File Exist: {0}", File.Exists(path));
		}
		if (GUILayout.Button("Create File"))
		{
			string path2 = string.Format("{0}/test.png", Application.temporaryCachePath);
			Texture2D texture2D = new Texture2D(128, 128);
			File.WriteAllBytes(path2, texture2D.EncodeToPNG());
			msg = string.Format("File Created");
		}
		GUILayout.TextArea(msg);
		if (adInfo != null && adInfo.Image != null)
		{
			GUI.DrawTexture(new Rect(0f, 100f, adInfo.Image.width, adInfo.Image.height), adInfo.Image);
		}
	}

	private void onGetAdRes(bool res, string str, List<AdInfo> adSchedule)
	{
		Debug.Log(string.Format("---> onGetAdRes: res: {0}", res));
		if (!res)
		{
		}
	}

	private void onImageLoaded(bool res, string str, Texture2D tex)
	{
		Debug.Log(string.Format("---> onImageLoaded: res: {0} str: {1}", res, str));
		if (res)
		{
			adInfo = AdManager.Instance.GetAdFromSchedule();
		}
	}
}
