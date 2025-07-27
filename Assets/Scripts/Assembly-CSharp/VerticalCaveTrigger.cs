using System;
using UnityEngine;

public class VerticalCaveTrigger : MonoBehaviour
{
	private static Vector3 toPos;

	private bool activateGoCenter;

	private float accumTime;

	private Transform playerT;

	private static DateTime lastSoundTrigger;

	private void Start()
	{
		activateGoCenter = false;
		lastSoundTrigger = DateTime.MinValue;
	}

	private void Update()
	{
		if (activateGoCenter)
		{
			accumTime += Time.deltaTime;
			if (playerT == null)
			{
				playerT = CharHelper.GetPlayerTransform();
			}
			playerT.position = Vector3.Lerp(playerT.position, new Vector3(toPos.x, playerT.position.y, toPos.z), accumTime * 2f);
			if (accumTime >= 0.5f)
			{
				activateGoCenter = false;
			}
		}
	}

	private void OnTriggerEnter(Collider c)
	{
		if ((DateTime.Now - lastSoundTrigger).TotalMilliseconds > 1000.0)
		{
			SoundManager.PlaySound(23);
			lastSoundTrigger = DateTime.Now;
		}
		activateGoCenter = true;
	}
}
