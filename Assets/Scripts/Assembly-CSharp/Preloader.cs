using UnityEngine;

public class Preloader : MonoBehaviour
{
	public Texture logo;

	public Texture preloaderBar;

	public Texture preloaderBarBkg;

	private int BAR_WIDTH = 140;

	private int BAR_HEIGHT = 8;

	private int BAR_POS_Y = 320;

	private float percentageLoaded;

	private bool isPirated;

	private void Start()
	{
		siteLock();
	}

	private void OnGUI()
	{
		if (!isPirated)
		{
			int num = (Screen.width >> 1) - (BAR_WIDTH >> 1);
			GUI.DrawTexture(new Rect((Screen.width >> 1) - (logo.width >> 1), (Screen.height >> 1) - (logo.width >> 1), logo.width, logo.height), logo);
			GUI.DrawTexture(new Rect(num, BAR_POS_Y, BAR_WIDTH, BAR_HEIGHT), preloaderBarBkg);
			GUI.DrawTexture(new Rect(num, BAR_POS_Y, (float)BAR_WIDTH * percentageLoaded, BAR_HEIGHT), preloaderBar);
		}
		else
		{
			GUI.Label(new Rect(20f, Screen.height - 40, Screen.width, 40f), "Error: Wrong download URL. This version of the game can be played from Kongregate.com");
		}
	}

	private void Update()
	{
		if (!isPirated && percentageLoaded < 1f)
		{
			percentageLoaded = Application.GetStreamProgressForLevel("Credits");
			if (percentageLoaded >= 1f)
			{
				percentageLoaded = 1f;
				Application.LoadLevel("StartupKongregate");
			}
		}
	}

	private void siteLock()
	{
		isPirated = false;
	}
}
