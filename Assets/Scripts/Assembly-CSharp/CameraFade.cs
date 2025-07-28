using System;
using UnityEngine;

public class CameraFade : MonoBehaviour
{
	public Texture2D fadeTexture;

	public float fadeDuration = 1f;

	public int guiDepth = -1000;

	public bool fadeIntoScene = true;

	private float currentAlpha = 1f;

	private float currentDuration;

	private int fadeDirection = -1;

	private float targetAlpha;

	private float alphaDifference;

	private GUIStyle backgroundStyle = new GUIStyle();

	private Texture2D dummyTex;

	public Color alphaColor = default(Color);

	private FadeDone fadeDone;

	private float lastTime;

	private bool fadeIsDone = true;

	private static CameraFade instance;

	public static CameraFade Instance
	{
		get
		{
			if (instance == null)
			{
				GameObject gameObject = new GameObject();
				instance = gameObject.AddComponent<CameraFade>();
			}
			if (instance == null)
			{
				Debug.LogError("No FadeInOut attached to the main camera.");
			}
			return instance;
		}
	}

	// Method to improve camera depth buffer precision
	public static void ImproveCameraDepthPrecision()
	{
		Camera[] cameras = UnityEngine.Object.FindObjectsOfType<Camera>();
		foreach (Camera cam in cameras)
		{
			// Adjust near and far clip planes for better depth precision
			if (cam.nearClipPlane < 0.5f)
			{
				cam.nearClipPlane = 0.5f;
			}
			if (cam.farClipPlane > 500f)
			{
				cam.farClipPlane = 500f;
			}
			
			// Enable depth texture for better rendering
			cam.depthTextureMode = DepthTextureMode.Depth;
		}
	}

	private void Awake()
	{
		instance = this;
		UnityEngine.Object.DontDestroyOnLoad(this);
		dummyTex = new Texture2D(1, 1);
		dummyTex.SetPixel(0, 0, Color.white);
		dummyTex.Apply();
		backgroundStyle.normal.background = fadeTexture;
	}

	public void OnEnable()
	{
		lastTime = 0f;
		if (fadeIntoScene)
		{
			FadeIn();
		}
	}

	private void OnGUI()
	{
		if (currentAlpha > 0f)
		{
			alphaColor.a = currentAlpha;
			GUI.color = alphaColor;
			GUI.depth = guiDepth;
			GUI.DrawTexture(new Rect(0f, 0f, Screen.width, Screen.height), dummyTex);
		}
	}

	private void Update()
	{
		if (fadeIsDone)
		{
			return;
		}
		float num = (float)DateTime.Now.Millisecond / 1000f;
		float num2 = 0f;
		if (lastTime != 0f)
		{
			num2 = num - lastTime;
		}
		lastTime = num;
		if (!(num2 > 0f) || ((fadeDirection != -1 || !(currentAlpha > targetAlpha)) && (fadeDirection != 1 || !(currentAlpha < targetAlpha))))
		{
			return;
		}
		currentAlpha += (float)fadeDirection * alphaDifference * (num2 / currentDuration);
		currentAlpha = Mathf.Clamp01(currentAlpha);
		if (currentAlpha == 0f || currentAlpha == 1f)
		{
			fadeIsDone = true;
			if (fadeDone != null)
			{
				fadeDone();
				fadeDone = null;
			}
		}
	}

	private void FadeIn(float duration, float to)
	{
		currentAlpha = 1f - to;
		currentDuration = duration;
		targetAlpha = to;
		alphaDifference = Mathf.Clamp01(currentAlpha - targetAlpha);
		fadeDirection = -1;
		lastTime = 0f;
		fadeIsDone = false;
	}

	public void FadeIn()
	{
		fadeDone = null;
		FadeIn(fadeDuration, 0f);
	}

	public void FadeIn(FadeDone cbFn)
	{
		fadeDone = cbFn;
		FadeIn(fadeDuration, 0f);
	}

	public void FadeIn(float duration)
	{
		fadeDone = null;
		FadeIn(duration, 0f);
	}

	public void FadeIn(float duration, FadeDone cbFn)
	{
		fadeDone = cbFn;
		FadeIn(duration, 0f);
	}

	private void FadeOut(float duration, float to)
	{
		currentAlpha = 1f - to;
		currentDuration = duration;
		targetAlpha = to;
		alphaDifference = Mathf.Clamp01(targetAlpha - currentAlpha);
		fadeDirection = 1;
		lastTime = 0f;
		fadeIsDone = false;
	}

	public void FadeOut()
	{
		FadeOut(fadeDuration, 1f);
	}

	public void FadeOut(FadeDone cbFn)
	{
		fadeDone = cbFn;
		FadeOut(fadeDuration, 1f);
	}

	public void FadeOut(float duration)
	{
		fadeDone = null;
		FadeOut(duration, 1f);
	}

	public void FadeOut(float duration, FadeDone cbFn)
	{
		fadeDone = cbFn;
		FadeOut(duration, 1f);
	}

	public static void FadeInMain(float duration, float to)
	{
		Instance.FadeIn(duration, to);
	}

	public static void FadeInMain()
	{
		Instance.FadeIn();
	}

	public static void FadeInMain(float duration)
	{
		Instance.FadeIn(duration);
	}

	public static void FadeOutMain(float duration, float to)
	{
		Instance.FadeOut(duration, to);
	}

	public static void FadeOutMain()
	{
		Instance.FadeOut();
	}

	public static void FadeOutMain(float duration)
	{
		Instance.FadeOut(duration);
	}
}
