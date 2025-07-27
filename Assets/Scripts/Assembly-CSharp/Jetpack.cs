using UnityEngine;

public class Jetpack : MonoBehaviour
{
	public enum Mode
	{
		NORMAL = 0,
		TURBO = 1,
		FUEL_OUT = 2
	}

	public ParticleSystem[] psNormal;

	public ParticleSystem[] psTurbo;

	public ParticleSystem[] psFuelOut;

	public GameObject explosion;

	private float accumtime;

	private bool disableTimer;

	private void Start()
	{
		DisableAll();
		explosion.SetActive(false);
	}

	private void Update()
	{
		if (disableTimer)
		{
			accumtime += Time.deltaTime;
			if (accumtime > 1f)
			{
				DisableAll();
				disableTimer = false;
				explosion.SetActive(true);
				base.gameObject.GetComponent<Renderer>().enabled = false;
			}
		}
	}

	public void DisableAll()
	{
		for (int i = 0; i < psNormal.Length; i++)
		{
			psNormal[i].gameObject.SetActive(false);
		}
		for (int j = 0; j < psTurbo.Length; j++)
		{
			psTurbo[j].gameObject.SetActive(false);
		}
		for (int k = 0; k < psFuelOut.Length; k++)
		{
			psFuelOut[k].gameObject.SetActive(false);
		}
	}

	public void DisableNormal()
	{
		for (int i = 0; i < psNormal.Length; i++)
		{
			psNormal[i].gameObject.SetActive(false);
		}
	}

	public void DisableTurbo()
	{
		for (int i = 0; i < psTurbo.Length; i++)
		{
			psTurbo[i].gameObject.SetActive(false);
		}
	}

	public void EnableNormal()
	{
		for (int i = 0; i < psNormal.Length; i++)
		{
			psNormal[i].gameObject.SetActive(true);
			psNormal[i].Emit = true;
		}
	}

	public void EnableTurbo()
	{
		for (int i = 0; i < psTurbo.Length; i++)
		{
			psTurbo[i].gameObject.SetActive(true);
			psTurbo[i].Emit = true;
		}
	}

	public void EnableFuelOut()
	{
		for (int i = 0; i < psFuelOut.Length; i++)
		{
			psFuelOut[i].gameObject.SetActive(true);
			psFuelOut[i].Emit = true;
		}
		disableTimer = true;
		accumtime = 0f;
	}

	public void SetVisibility(bool val)
	{
		if (base.GetComponent<Renderer>() != null)
		{
			base.GetComponent<Renderer>().enabled = val;
		}
	}
}
