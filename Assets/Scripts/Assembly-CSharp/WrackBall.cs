using UnityEngine;

public class WrackBall : MonoBehaviour
{
	public bool clockWise = true;

	private float angle;

	[HideInInspector]
	public bool collide;

	private Rigidbody[] rbs;

	private void Start()
	{
		collide = false;
		rbs = GetComponentsInChildren<Rigidbody>();
		for (int i = 0; i < rbs.Length; i++)
		{
			rbs[i].useGravity = false;
			rbs[i].isKinematic = true;
		}
		WrackBallTrigger[] componentsInChildren = GetComponentsInChildren<WrackBallTrigger>();
		for (int j = 0; j < componentsInChildren.Length; j++)
		{
			componentsInChildren[j].SetParent(this);
		}
	}

	private void Update()
	{
		if (!collide)
		{
			angle += Time.deltaTime * 400f * (float)((!clockWise) ? 1 : (-1));
			angle %= 360f;
			base.transform.localRotation = Quaternion.Euler(new Vector3(0f, 0f - angle + 180f, 0f));
		}
	}

	public void OnCollide(GameObject hittedObj)
	{
		if (!collide)
		{
			if (!ProtectiveVestHelper.UseProtectiveVestIfAvailable())
			{
				SoundManager.PlaySound(SndId.SND_GORE_IMPACT_GENERIC);
				CharHelper.GetCharStateMachine().Hit(-1f * new Vector3(Random.Range(-10, 10), 0f, Random.Range(-40, -80)));
			}
			else
			{
				SoundManager.PlaySound(SndId.SND_FRED_OUCH);
				CharHelper.GetCharStateMachine().SwitchTo(ActionCode.BOUNCE);
			}
			for (int i = 0; i < rbs.Length; i++)
			{
				rbs[i].useGravity = true;
				rbs[i].isKinematic = false;
			}
			ScreenShaker.Shake(0.5f, 8f);
		}
		collide = true;
	}
}
