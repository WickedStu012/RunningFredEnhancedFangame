using System;
using System.Collections.Generic;
using UnityEngine;

public class ColumnFallBack : MonoBehaviour
{
	public GameObject bloodSplat;

	private List<GameObject> splats;

	private DateTime lastCollision;

	private RaycastHit hit;

	private void Start()
	{
		splats = new List<GameObject>();
	}

	private void OnTriggerEnter(Collider c)
	{
		Transform transform = c.transform;
		if (transform.gameObject.layer != 9 && transform.gameObject.layer != 13 && CharHelper.IsColliderFromPlayer(c) && !Physics.Raycast(new Vector3(transform.position.x, transform.position.y, transform.position.z), Vector3.left, 1f, 8192) && !Physics.Raycast(new Vector3(transform.position.x, transform.position.y, transform.position.z), Vector3.right, 1f, 8192) && !CharHelper.GetCharSkin().IsBlinking() && Physics.Raycast(new Vector3(transform.position.x, transform.position.y, transform.position.z), Vector3.forward, 1f, 8192) && (DateTime.Now - lastCollision).TotalMilliseconds > 200.0)
		{
			lastCollision = DateTime.Now;
			if (CharHelper.GetCharStateMachine().GetCurrentState() == ActionCode.MEGA_SPRINT)
			{
				CharHelper.GetCharStateMachine().SwitchTo(ActionCode.EXPLODE_ON_WALL);
				return;
			}
			createSplat();
			CharHelper.GetCharStateMachine().SwitchTo(ActionCode.BOUNCE);
		}
	}

	private void createSplat()
	{
		if (bloodSplat != null)
		{
			Transform transformByName = CharHelper.GetTransformByName("head");
			if (Physics.Raycast(new Vector3(transformByName.position.x, transformByName.position.y, transformByName.position.z), Vector3.forward, out hit, 1f, 4096) && Mathf.Abs(hit.point.x) < 1f)
			{
				GameObject gameObject = UnityEngine.Object.Instantiate(bloodSplat) as GameObject;
				gameObject.transform.position = new Vector3(hit.point.x + UnityEngine.Random.Range(-0.1f, 0.1f), hit.point.y + UnityEngine.Random.Range(-0.1f, 0.1f), hit.point.z - 0.1f);
				float num = UnityEngine.Random.Range(-0.4f, 0.4f);
				gameObject.transform.localScale = new Vector3(1f + num, 1f + num, 1f + num);
				gameObject.transform.parent = base.gameObject.transform;
				splats.Add(gameObject);
			}
		}
	}
}
