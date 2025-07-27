using UnityEngine;

public class JetpackFuel : MonoBehaviour
{
	private bool collide;

	private void Start()
	{
		collide = false;
	}

	private void Update()
	{
		base.transform.Rotate(Vector3.forward, Time.deltaTime * 100f);
	}

	private void OnTriggerEnter(Collider c)
	{
		if (!collide && CharHelper.IsColliderFromPlayer(c))
		{
			SoundManager.PlaySound(base.transform.position, 19);
			CharHelper.GetProps().JetPackFuelLeft = CharHelper.GetProps().MaxJetPackFuel;
			Object.Destroy(base.gameObject);
			collide = true;
		}
	}
}
