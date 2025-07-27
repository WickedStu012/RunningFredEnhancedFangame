using System;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleFallBack : MonoBehaviour
{
	public GameObject bloodSplat;

	public bool duckeable;

	public bool canTrip = true;

	public bool canWalkOver = true;

	public bool canWalkOnWalls = true;

	public bool addForceToObjOnHit;

	public Rigidbody rigidBodyToMove;

	public bool canClimbEdge;

	private List<GameObject> splats;

	private DateTime lastCollision;

	private RaycastHit hit;

	private Transform headTransform;

	private void Start()
	{
		splats = new List<GameObject>();
		headTransform = CharHelper.GetTransformByName("head");
	}

	private void OnTriggerEnter(Collider c)
	{
		if (!base.enabled || c.transform.gameObject.layer == 9 || c.transform.gameObject.layer == 13 || !CharHelper.IsColliderFromPlayer(c))
		{
			return;
		}
		Transform transform = c.transform;
		if ((canWalkOnWalls && (Physics.Raycast(new Vector3(transform.position.x, transform.position.y, transform.position.z), Vector3.left, 1f, 274432) || Physics.Raycast(new Vector3(transform.position.x, transform.position.y, transform.position.z), Vector3.right, 1f, 274432))) || CharHelper.GetCharSkin().IsBlinking() || CharHelper.GetCharStateMachine().GetCurrentState() == ActionCode.SAFETY_SPRING || (duckeable && CharHelper.GetCharStateMachine().GetCurrentState() == ActionCode.DUCK))
		{
			return;
		}
		bool flag = Physics.Raycast(new Vector3(transform.position.x, transform.position.y + 0.2f, transform.position.z), Vector3.forward, out hit, 1f, 274432);
		bool flag2 = flag && hit.collider == base.gameObject.GetComponent<Collider>();
		if ((canWalkOver && !flag2) || (flag && hit.normal.z > -0.6f) || !((DateTime.Now - lastCollision).TotalMilliseconds > 200.0))
		{
			return;
		}
		lastCollision = DateTime.Now;
		Vector3 position = CharHelper.GetPlayerTransform().position;
		if (canTrip && (Physics.Raycast(position + new Vector3(0f, 0.25f, 0f), Vector3.forward, 1f, 274432) || Physics.Raycast(position + new Vector3(0f, 0.5f, 0f), Vector3.forward, 1f, 274432)) && !Physics.Raycast(position + new Vector3(0f, 1f, 0f), Vector3.forward, 1f, 274432))
		{
			CharHelper.GetCharStateMachine().SwitchTo(ActionCode.TRIP);
		}
		else if ((CharHelper.GetCharStateMachine().GetCurrentState() == ActionCode.MEGA_SPRINT || CharHelper.GetCharStateMachine().GetCurrentState() == ActionCode.CATAPULT) && !CharHelper.GetProps().Airbags)
		{
			CharHelper.GetCharStateMachine().SwitchTo(ActionCode.EXPLODE_ON_WALL);
		}
		else
		{
			if (canClimbEdge && headTransform != null && !Physics.Raycast(headTransform.position + Vector3.up * 1f, Vector3.forward, 2f, 274432))
			{
				CharHelper.GetCharStateMachine().SwitchTo(ActionCode.CLIMB_END);
				return;
			}
			if (ConfigParams.useGore)
			{
				createSplat();
			}
			SoundManager.PlaySound(30);
			CharHelper.GetCharStateMachine().SwitchTo(ActionCode.BOUNCE);
		}
		if (addForceToObjOnHit)
		{
			if (rigidBodyToMove == null)
			{
				rigidBodyToMove = base.gameObject.GetComponent<Rigidbody>();
			}
			base.gameObject.GetComponent<Rigidbody>().isKinematic = false;
			base.gameObject.GetComponent<Rigidbody>().AddForce(Vector3.forward * 200f);
		}
	}

	private void createSplat()
	{
		if (bloodSplat != null)
		{
			Transform transformByName = CharHelper.GetTransformByName("head");
			RaycastHit hitInfo;
			if (Physics.Raycast(new Vector3(transformByName.position.x, transformByName.position.y, transformByName.position.z), Vector3.forward, out hitInfo, 1f, 274432) && Mathf.Abs(hitInfo.point.x) < 1f)
			{
				GameObject gameObject = UnityEngine.Object.Instantiate(bloodSplat) as GameObject;
				gameObject.transform.position = new Vector3(hitInfo.point.x + UnityEngine.Random.Range(-0.1f, 0.1f), hitInfo.point.y + UnityEngine.Random.Range(-0.1f, 0.1f), hitInfo.point.z - 0.1f);
				float num = UnityEngine.Random.Range(-0.4f, 0.4f);
				gameObject.transform.localScale = new Vector3(1f + num, 1f + num, 1f + num);
				gameObject.transform.parent = base.gameObject.transform;
				splats.Add(gameObject);
			}
		}
	}
}
