using UnityEngine;

public class StalactiteTrigger : MonoBehaviour
{
	public GameObject[] stalactites;

	public float triggerTimeDif;

	private bool collide;

	private Stalactite[] stalactitesScr;

	private float accumTime;

	private bool triggerStalactites;

	private int lastStalactiteIdxTriggered;

	private void Start()
	{
		stalactitesScr = new Stalactite[stalactites.Length];
		for (int i = 0; i < stalactitesScr.Length; i++)
		{
			stalactitesScr[i] = stalactites[i].GetComponentInChildren<Stalactite>();
		}
	}

	private void Update()
	{
		if (!triggerStalactites)
		{
			return;
		}
		accumTime += Time.deltaTime;
		if (!(accumTime > triggerTimeDif))
		{
			return;
		}
		lastStalactiteIdxTriggered++;
		if (lastStalactiteIdxTriggered < stalactitesScr.Length)
		{
			if (stalactitesScr[lastStalactiteIdxTriggered] != null)
			{
				stalactitesScr[lastStalactiteIdxTriggered].Trigger();
			}
			accumTime = 0f;
		}
		else
		{
			triggerStalactites = false;
		}
	}

	private void OnTriggerEnter(Collider c)
	{
		if (collide || !CharHelper.IsColliderFromPlayer(c))
		{
			return;
		}
		if (stalactitesScr.Length > 0)
		{
			if (stalactitesScr[0] != null)
			{
				stalactitesScr[0].Trigger();
			}
			lastStalactiteIdxTriggered = 0;
			if (stalactitesScr.Length > 1)
			{
				triggerStalactites = true;
			}
		}
		collide = true;
	}
}
