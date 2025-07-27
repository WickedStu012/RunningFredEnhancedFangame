using System;
using System.Collections;
using Prime31;
using UnityEngine;

public class TwitterManager : MonoBehaviour
{
	public static event Action twitterLogin;

	public static event Action<string> twitterLoginFailed;

	public static event Action twitterPost;

	public static event Action<string> twitterPostFailed;

	public static event Action<ArrayList> twitterHomeTimelineReceived;

	public static event Action<string> twitterHomeTimelineFailed;

	public static event Action<object> twitterRequestDidFinishEvent;

	public static event Action<string> twitterRequestDidFailEvent;

	private void Awake()
	{
		base.gameObject.name = GetType().ToString();
		UnityEngine.Object.DontDestroyOnLoad(this);
	}

	public void twitterLoginSucceeded(string empty)
	{
		if (TwitterManager.twitterLogin != null)
		{
			TwitterManager.twitterLogin();
		}
	}

	public void twitterLoginDidFail(string error)
	{
		if (TwitterManager.twitterLoginFailed != null)
		{
			TwitterManager.twitterLoginFailed(error);
		}
	}

	public void twitterPostSucceeded(string empty)
	{
		if (TwitterManager.twitterPost != null)
		{
			TwitterManager.twitterPost();
		}
	}

	public void twitterPostDidFail(string error)
	{
		if (TwitterManager.twitterPostFailed != null)
		{
			TwitterManager.twitterPostFailed(error);
		}
	}

	public void twitterHomeTimelineDidFail(string error)
	{
		if (TwitterManager.twitterHomeTimelineFailed != null)
		{
			TwitterManager.twitterHomeTimelineFailed(error);
		}
	}

	public void twitterHomeTimelineDidFinish(string results)
	{
		if (TwitterManager.twitterHomeTimelineReceived != null)
		{
			ArrayList obj = (ArrayList)Json.jsonDecode(results);
			TwitterManager.twitterHomeTimelineReceived(obj);
		}
	}

	public void twitterRequestDidFinish(string results)
	{
		if (TwitterManager.twitterRequestDidFinishEvent != null)
		{
			TwitterManager.twitterRequestDidFinishEvent(Json.jsonDecode(results));
		}
	}

	public void twitterRequestDidFail(string error)
	{
		if (TwitterManager.twitterRequestDidFailEvent != null)
		{
			TwitterManager.twitterRequestDidFailEvent(error);
		}
	}
}
