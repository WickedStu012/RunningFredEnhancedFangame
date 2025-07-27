using UnityEngine;

public class FpsMeterGUI : MonoBehaviour
{
	private FpsMeter fpsMeter = new FpsMeter();

	private float[] fpsArray;

	private int fixedUpdateCounter;

	private float fps;

	private float minFps;

	private float avgFps;

	private float maxFps;

	private bool isVisible = true;

	private void Start()
	{
		fpsArray = new float[3600];
		fixedUpdateCounter = 0;
		minFps = float.MaxValue;
	}

	private void OnGUI()
	{
		if (isVisible)
		{
			GUI.BeginGroup(new Rect(Screen.width - 180, 0f, 180f, 80f));
			GUI.Box(new Rect(0f, 0f, 180f, 80f), string.Empty);
			GUI.Label(new Rect(10f, 0f, 180f, 22f), "Game Stats");
			if (fpsMeter.HasFPS())
			{
				GUI.Label(new Rect(10f, 20f, 180f, 22f), string.Format("fps: {0}", fps.ToString("f2")));
				GUI.Label(new Rect(10f, 32f, 180f, 22f), string.Format("min fps: {0}", minFps.ToString("f2")));
				GUI.Label(new Rect(10f, 44f, 180f, 22f), string.Format("max fps: {0}", maxFps.ToString("f2")));
				GUI.Label(new Rect(10f, 56f, 180f, 22f), string.Format("avg fps: {0}", avgFps.ToString("f2")));
			}
			else
			{
				GUI.Label(new Rect(10f, 20f, 180f, 22f), string.Format("Calculating..."));
			}
			GUI.EndGroup();
		}
	}

	private void Update()
	{
		fpsMeter.Update();
		if (fpsMeter.HasFPS())
		{
			fps = fpsMeter.GetFPS();
			if (fixedUpdateCounter > 10 && fps < minFps)
			{
				minFps = fps;
			}
			if (fixedUpdateCounter > 10 && fps > maxFps)
			{
				maxFps = fps;
			}
			if (fixedUpdateCounter < fpsArray.Length)
			{
				fpsArray[fixedUpdateCounter] = fps;
				avgFps = MathUtil.GetAverage(fpsArray, fixedUpdateCounter);
				fixedUpdateCounter++;
			}
		}
	}
}
