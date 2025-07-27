using UnityEngine;

public class BlackLordGetInfoTester : MonoBehaviour
{
	private void OnGUI()
	{
		if (GUILayout.Button("Get Skiing Fred Info"))
		{
			BlackLordGetInfo.Instance.GetSkiingFredInfo(onGetInfoRes);
		}
	}

	private void onGetInfoRes(bool res, SkiingFredInfo sfi)
	{
		if (res && sfi != null)
		{
			Debug.Log(string.Format("---> onGetInfoRes: res: {0} Available: {1} StoreLink: {2} ImageLink: {3}", res, sfi.Available, sfi.StoreLink, sfi.ImageLink));
		}
		else
		{
			Debug.Log(string.Format("---> onGetInfoRes: res: {0}", res));
		}
	}
}
