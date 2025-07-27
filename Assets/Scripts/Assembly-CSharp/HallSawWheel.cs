using System;
using UnityEngine;

public class HallSawWheel : MonoBehaviour
{
	private enum JumpState
	{
		IDLE = 0,
		MOVE_UP = 1
	}

	public float speed = 1f;

	public GameObject baseW;

	public bool wheelJump;

	public bool moveTranslate;

	public float maxJumpHeight = 10f;

	public float jumpInterval = 1f;

	public float jumpIntervalOffset;

	private float accumTime;

	private bool rightToLeft;

	private bool collide;

	private RaycastHit hit;

	private Vector3 pos1;

	private Vector3 pos2;

	private Vector3 iniPosV;

	private JumpState state;

	private float accumTimeJump;

	private float yPos;

	private int triggerSound = -1;

	private float accumTimeCollide;

	private void Start()
	{
		if (baseW == null)
		{
			moveTranslate = false;
		}
		if (moveTranslate)
		{
			moveTranslate = baseW.transform.localScale.y > 0.2f;
		}
		float num = 0f;
		if (baseW != null)
		{
			num = 5.3f * baseW.transform.localScale.y;
		}
		pos1 = base.transform.parent.TransformPoint(new Vector3(num, base.transform.localPosition.y, base.transform.localPosition.z));
		pos2 = base.transform.parent.TransformPoint(new Vector3(0f - num, base.transform.localPosition.y, base.transform.localPosition.z));
		iniPosV = base.transform.position;
		state = JumpState.IDLE;
		accumTimeJump = jumpIntervalOffset;
		accumTimeCollide = 0f;
	}

	private void OnEnable()
	{
		triggerSound = 2;
	}

	private void Update()
	{
		if (triggerSound > -1)
		{
			triggerSound--;
			if (triggerSound == 0)
			{
				base.GetComponent<AudioSource>().Play();
				triggerSound = -1;
			}
		}
		accumTime += Time.deltaTime;
		if (rightToLeft)
		{
			base.transform.localRotation = Quaternion.Euler(0f, 0f, accumTime * speed * 400f);
			if (moveTranslate)
			{
				base.transform.position = Vector3.Lerp(iniPosV, pos2, accumTime * speed);
				if (accumTime * speed >= 1f)
				{
					iniPosV = pos2;
					rightToLeft = false;
					accumTime = 0f;
				}
			}
		}
		else
		{
			base.transform.localRotation = Quaternion.Euler(0f, 0f, accumTime * speed * -400f);
			if (moveTranslate)
			{
				base.transform.position = Vector3.Lerp(iniPosV, pos1, accumTime * speed);
				if (accumTime * speed >= 1f)
				{
					iniPosV = pos1;
					rightToLeft = true;
					accumTime = 0f;
				}
			}
		}
		if (wheelJump)
		{
			switch (state)
			{
			case JumpState.IDLE:
				accumTimeJump += Time.deltaTime;
				if (accumTimeJump > jumpInterval)
				{
					accumTimeJump = 0f;
					state = JumpState.MOVE_UP;
				}
				break;
			case JumpState.MOVE_UP:
				accumTimeJump += Time.deltaTime;
				yPos = maxJumpHeight * Mathf.Sin(accumTimeJump * 100f * ((float)Math.PI / 180f));
				if (accumTimeJump > 1.8f)
				{
					accumTimeJump = 0f;
					state = JumpState.IDLE;
				}
				break;
			}
			base.transform.position = new Vector3(base.transform.position.x, iniPosV.y + yPos, base.transform.position.z);
		}
		if (collide)
		{
			accumTimeCollide += Time.deltaTime;
			if (accumTimeCollide > 3f)
			{
				collide = false;
				accumTimeCollide = 0f;
			}
		}
	}

	private void OnTriggerEnter(Collider c)
	{
		if (collide || !CharHelper.IsColliderFromPlayer(c))
		{
			return;
		}
		if (!ProtectiveVestHelper.UseProtectiveVestIfAvailable())
		{
			Transform playerTransform = CharHelper.GetPlayerTransform();
			float num = 1f;
			Vector3 origin = new Vector3(playerTransform.position.x, base.transform.position.y + num, base.transform.position.z);
			if (Physics.Raycast(origin, Vector3.back, out hit, 2f, 524288) || Physics.Raycast(origin, Vector3.right, out hit, 2f, 524288) || Physics.Raycast(origin, Vector3.left, out hit, 2f, 524288) || Physics.Raycast(origin, Vector3.forward, out hit, 2f, 524288))
			{
				if (ConfigParams.useGore)
				{
					CharHelper.GetCharSkin().Dismember(hit.collider.name);
				}
				else
				{
					CharHelper.GetCharStateMachine().SwitchTo(ActionCode.DIE_IMPCT);
				}
			}
			else if (ConfigParams.useGore)
			{
				CharHelper.GetCharSkin().DismemberRandom();
			}
			else
			{
				CharHelper.GetCharStateMachine().SwitchTo(ActionCode.DIE_IMPCT);
			}
		}
		else
		{
			SoundManager.PlaySound(SndId.SND_FRED_OUCH);
			CharHelper.GetCharStateMachine().SwitchTo(ActionCode.BOUNCE);
		}
		collide = true;
	}
}
