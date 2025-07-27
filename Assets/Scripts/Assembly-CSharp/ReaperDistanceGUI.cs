using UnityEngine;

public class ReaperDistanceGUI : MonoBehaviour
{
	public GUI3DProgressBar DistanceBar;

	public GUI3DObject Reaper;

	public Color StartColor;

	public Color MidColor;

	public Color EndColor;

	public Vector3 ReaperEndPos;

	private Vector3 direction;

	private Vector3 originalPos;

	private void Awake()
	{
		if (DistanceBar == null)
		{
			DistanceBar = GetComponent<GUI3DProgressBar>();
		}
		direction = ReaperEndPos - Reaper.transform.localPosition;
		direction /= 100f;
		originalPos = Reaper.transform.localPosition;
	}

	private void OnEnable()
	{
		if (base.GetComponent<Renderer>() != null)
		{
			base.GetComponent<Renderer>().material.color = StartColor;
		}
	}

	private void Update()
	{
		if (!(ReaperManager.Instance != null))
		{
			return;
		}
		float distance = ReaperManager.Instance.Distance;
		if (!(distance > 0f))
		{
			return;
		}
		if ((int)distance != DistanceBar.Progress)
		{
			DistanceBar.Progress = (int)distance;
		}
		float num = 100f - distance;
		Reaper.transform.localPosition = originalPos + direction * num;
		if (base.GetComponent<Renderer>() != null)
		{
			if (distance > 50f)
			{
				base.GetComponent<Renderer>().material.color = Color.Lerp(StartColor, MidColor, (50f - (distance - 50f)) / 50f);
			}
			else
			{
				base.GetComponent<Renderer>().material.color = Color.Lerp(MidColor, EndColor, (50f - distance) / 50f);
			}
		}
	}
}
