using UnityEngine;

public class CharEffects : MonoBehaviour
{
	private GameObject groundImpact;

	private GameObject hardLanding;

	private GameObject playerImpact;

	private GameObject burnParticles;

	private GameObject respawnParticles;

	private GameObject protectiveVestParticles;

	private GameObject bounceSpringParticles;

	private GameObject afterBurnerParticles;

	private GameObject dragParticles;

	private GameObject wingsExplosionParticles;

	private bool disableRespawnParticles;

	private bool burnParticlesEnabled;

	private Transform torso1;

	private float accumTime;

	private void Start()
	{
		GameObject original = Resources.Load("Prefabs/GroundImpact", typeof(GameObject)) as GameObject;
		groundImpact = Object.Instantiate(original) as GameObject;
		groundImpact.name = "GroundImpact";
		groundImpact.transform.parent = base.transform;
		groundImpact.transform.localPosition = Vector3.zero;
		groundImpact.SetActive(false);
		GameObject original2 = Resources.Load("Prefabs/HardLandingParticles", typeof(GameObject)) as GameObject;
		hardLanding = Object.Instantiate(original2) as GameObject;
		hardLanding.name = "HardLanding";
		hardLanding.transform.parent = base.transform;
		hardLanding.transform.localPosition = Vector3.zero;
		hardLanding.SetActive(false);
		GameObject original3 = Resources.Load("Prefabs/PlayerImpact", typeof(GameObject)) as GameObject;
		playerImpact = Object.Instantiate(original3) as GameObject;
		playerImpact.name = "PlayerImpact";
		playerImpact.transform.parent = base.transform;
		playerImpact.transform.localPosition = Vector3.zero;
		playerImpact.SetActive(false);
		GameObject original4 = Resources.Load("Prefabs/BurningPlayerParticles", typeof(GameObject)) as GameObject;
		burnParticles = Object.Instantiate(original4) as GameObject;
		burnParticles.name = "BurningPlayerParticles";
		burnParticles.transform.parent = base.transform;
		burnParticles.transform.localPosition = Vector3.zero;
		burnParticles.SetActive(false);
		burnParticlesEnabled = false;
		GameObject original5 = Resources.Load("Prefabs/PlayerRespawn", typeof(GameObject)) as GameObject;
		respawnParticles = Object.Instantiate(original5) as GameObject;
		respawnParticles.name = "RespawnPlayerParticles";
		respawnParticles.transform.parent = base.transform;
		respawnParticles.transform.localPosition = Vector3.zero;
		respawnParticles.SetActive(false);
		GameObject original6 = Resources.Load("Prefabs/PlayerUseProtectiveVest", typeof(GameObject)) as GameObject;
		protectiveVestParticles = Object.Instantiate(original6) as GameObject;
		protectiveVestParticles.name = "ProtectiveVestParticles";
		protectiveVestParticles.transform.parent = base.transform;
		protectiveVestParticles.transform.localPosition = Vector3.zero;
		protectiveVestParticles.SetActive(false);
		GameObject original7 = Resources.Load("Prefabs/PlayerBounceSpring", typeof(GameObject)) as GameObject;
		bounceSpringParticles = Object.Instantiate(original7) as GameObject;
		bounceSpringParticles.name = "PlayerBounceSpring";
		bounceSpringParticles.transform.parent = base.transform;
		bounceSpringParticles.transform.localPosition = Vector3.zero;
		bounceSpringParticles.SetActive(false);
		GameObject original8 = Resources.Load("Prefabs/PlayerAccelerate", typeof(GameObject)) as GameObject;
		afterBurnerParticles = Object.Instantiate(original8) as GameObject;
		afterBurnerParticles.name = "PlayerAccelerate";
		afterBurnerParticles.transform.parent = null;
		afterBurnerParticles.transform.localPosition = Vector3.zero;
		afterBurnerParticles.SetActive(false);
		GameObject original9 = Resources.Load("Prefabs/PlayerDragParticles", typeof(GameObject)) as GameObject;
		dragParticles = Object.Instantiate(original9) as GameObject;
		dragParticles.name = "PlayerDrag";
		dragParticles.transform.parent = base.transform;
		dragParticles.transform.localPosition = Vector3.zero;
		dragParticles.SetActive(false);
		GameObject original10 = Resources.Load("Prefabs/WingsExplosion", typeof(GameObject)) as GameObject;
		wingsExplosionParticles = Object.Instantiate(original10) as GameObject;
		wingsExplosionParticles.name = "WingsExplosion";
		wingsExplosionParticles.transform.parent = base.transform;
		wingsExplosionParticles.transform.localPosition = Vector3.zero;
		wingsExplosionParticles.SetActive(false);
	}

	private void OnEnable()
	{
		DisableParticles();
	}

	private void Update()
	{
		if (disableRespawnParticles)
		{
			accumTime += Time.deltaTime;
			if (accumTime > 2f)
			{
				if (respawnParticles != null)
				{
					respawnParticles.SetActive(false);
				}
				disableRespawnParticles = false;
			}
		}
		if (burnParticlesEnabled)
		{
			if (torso1 == null)
			{
				torso1 = CharHelper.GetTransformByName("torso1");
			}
			if (torso1 != null)
			{
				burnParticles.transform.position = torso1.position;
			}
		}
	}

	public void DisableParticles()
	{
		if (groundImpact != null)
		{
			groundImpact.SetActive(false);
		}
		if (hardLanding != null)
		{
			hardLanding.SetActive(false);
		}
		if (burnParticles != null)
		{
			burnParticles.SetActive(false);
		}
		if (dragParticles != null)
		{
			dragParticles.SetActive(false);
		}
		if (protectiveVestParticles != null)
		{
			protectiveVestParticles.SetActive(false);
		}
		if (bounceSpringParticles != null)
		{
			bounceSpringParticles.SetActive(false);
		}
		if (afterBurnerParticles != null)
		{
			afterBurnerParticles.SetActive(false);
		}
		burnParticlesEnabled = false;
	}

	public void EnableImpactGround()
	{
		groundImpact.SetActive(true);
		ParticleSystem[] componentsInChildren = groundImpact.GetComponentsInChildren<ParticleSystem>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].Emit = true;
		}
	}

	public void EnableHardLanding()
	{
		hardLanding.SetActive(true);
		ParticleSystem[] componentsInChildren = hardLanding.GetComponentsInChildren<ParticleSystem>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].Emit = true;
		}
	}

	public void EnablePlayerImpact()
	{
		playerImpact.SetActive(true);
		ParticleSystem[] componentsInChildren = playerImpact.GetComponentsInChildren<ParticleSystem>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].Emit = true;
		}
	}

	public void EnableBurnParticles()
	{
		burnParticles.SetActive(true);
		ParticleSystem[] componentsInChildren = burnParticles.GetComponentsInChildren<ParticleSystem>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].Emit = true;
		}
		burnParticlesEnabled = true;
	}

	public void EnableProtectiveVestParticles()
	{
		protectiveVestParticles.SetActive(true);
		ParticleSystem[] componentsInChildren = protectiveVestParticles.GetComponentsInChildren<ParticleSystem>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].Emit = true;
		}
		Invoke("disableProtectiveVestParticles", 3f);
	}

	private void disableProtectiveVestParticles()
	{
		protectiveVestParticles.SetActive(false);
	}

	public void EnableBounceSpringParticles()
	{
		bounceSpringParticles.SetActive(true);
		ParticleSystem[] componentsInChildren = bounceSpringParticles.GetComponentsInChildren<ParticleSystem>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].Emit = true;
		}
		Invoke("disableBounceSpringParticles", 3f);
	}

	private void disableBounceSpringParticles()
	{
		bounceSpringParticles.SetActive(false);
	}

	public void EnableAfterBurnerParticles()
	{
		afterBurnerParticles.SetActive(true);
		afterBurnerParticles.transform.position = base.transform.position;
		ParticleSystem[] componentsInChildren = afterBurnerParticles.GetComponentsInChildren<ParticleSystem>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].Emit = true;
		}
		Invoke("disableAfterBurnerParticles", 3f);
	}

	private void disableAfterBurnerParticles()
	{
		afterBurnerParticles.SetActive(false);
	}

	public void EnableRespawnParticles()
	{
		respawnParticles.SetActive(true);
		ParticleSystem[] componentsInChildren = respawnParticles.GetComponentsInChildren<ParticleSystem>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].Emit = true;
		}
		disableRespawnParticles = true;
		accumTime = 0f;
	}

	public void EnableDragParticles()
	{
		dragParticles.SetActive(true);
		dragParticles.transform.position = base.transform.position;
		ParticleSystem[] componentsInChildren = dragParticles.GetComponentsInChildren<ParticleSystem>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].Emit = true;
		}
	}

	public void DisableDragParticles()
	{
		dragParticles.SetActive(false);
	}

	public void EnableWingsExplosionParticles()
	{
		wingsExplosionParticles.SetActive(true);
		wingsExplosionParticles.transform.position = new Vector3(base.transform.position.x, base.transform.position.y + 1f, base.transform.position.z);
		ParticleSystem[] componentsInChildren = wingsExplosionParticles.GetComponentsInChildren<ParticleSystem>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].Emit = true;
		}
	}

	public void DisableWingsExplosionParticles()
	{
		wingsExplosionParticles.SetActive(false);
	}
}
