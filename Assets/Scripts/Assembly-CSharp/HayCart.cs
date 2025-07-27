using UnityEngine;

public class HayCart : MonoBehaviour
{
	public GameObject hayCartPS;

	private bool collide;

	private float hayCartYPos;

	private Vector2 zRange;

	private Vector2 xRange;

	private Vector3 playerPos;

	private float accumTime;

	private ParticleSystem[] ps;

	private void Start()
	{
		collide = false;
		ps = hayCartPS.transform.GetComponentsInChildren<ParticleSystem>();
		hayCartPS.SetActive(false);
	}

	private void Update()
	{
		if (collide)
		{
			accumTime += Time.deltaTime;
			if (accumTime >= 1f)
			{
				accumTime = 0f;
				collide = false;
			}
		}
	}

	private void OnTriggerEnter(Collider c)
	{
		if (!collide && CharHelper.IsColliderFromPlayer(c))
		{
			collide = true;
			CharHelper.GetCharStateMachine().ResetLastYPos();
			CharHelper.GetCharStateMachine().MoveDirection = Vector3.zero;
			CharHelper.GetCharStateMachine().SwitchTo(ActionCode.DOUBLE_JUMP);
			hayCartPS.SetActive(true);
			for (int i = 0; i < ps.Length; i++)
			{
				ps[i].Emit = true;
			}
		}
	}
}
