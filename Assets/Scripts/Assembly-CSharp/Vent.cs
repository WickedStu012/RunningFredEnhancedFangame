using UnityEngine;

public class Vent : MonoBehaviour
{
	public enum SpinDir
	{
		CW = 0,
		CCW = 1,
		CW_AND_CCW = 2
	}

	public enum SpinAxis
	{
		FORWARD = 0,
		UP = 1,
		RIGHT = 2
	}

	public enum DamageType
	{
		HIT = 0,
		PIERCE = 1,
		DISMEMBER = 2,
		EXPLOTE = 3,
		FALLBACK = 4
	}

	public DamageType dmgType;

	public SpinDir spinDir;

	public SpinAxis spinAxis;

	public float spinSpeed = 200f;

	public float spinSwitch;

	public GameObject fixedJointGO;

	public Collider[] colliderToDeactiveOnPierce;

	private Vector3 spinAxisV;

	private float accumAngle;

	private bool rotCW = true;

	private VentBlade[] blades;

	private bool enableRot;

	private bool impaled;

	private float accumTimeReset;

	private int triggerSound = -1;

	private void Start()
	{
		if (spinAxis == SpinAxis.FORWARD)
		{
			spinAxisV = base.transform.forward;
		}
		else if (spinAxis == SpinAxis.UP)
		{
			spinAxisV = base.transform.up;
		}
		else if (spinAxis == SpinAxis.RIGHT)
		{
			spinAxisV = base.transform.right;
		}
		if (spinDir == SpinDir.CW || spinDir == SpinDir.CW_AND_CCW)
		{
			rotCW = true;
		}
		else
		{
			rotCW = false;
		}
		blades = GetComponentsInChildren<VentBlade>();
		if (fixedJointGO == null)
		{
			FixedJoint componentInChildren = GetComponentInChildren<FixedJoint>();
			if (componentInChildren != null)
			{
				fixedJointGO = componentInChildren.gameObject;
			}
		}
		enableRot = true;
	}

	private void OnEnable()
	{
		triggerSound = 2;
	}

	private void OnDisable()
	{
		if (fixedJointGO != null)
		{
			FixedJoint component = fixedJointGO.GetComponent<FixedJoint>();
			if (component != null)
			{
				Object.Destroy(component);
			}
		}
	}

	private void Update()
	{
		if (triggerSound > -1)
		{
			triggerSound--;
			if (triggerSound == 0)
			{
				if (base.GetComponent<AudioSource>() != null)
				{
					base.GetComponent<AudioSource>().Play();
				}
				triggerSound = -1;
			}
		}
		if (enableRot)
		{
			float num = Time.deltaTime * spinSpeed * (float)(rotCW ? 1 : (-1));
			accumAngle += num;
			base.transform.Rotate(spinAxisV, num);
			if (spinDir == SpinDir.CW_AND_CCW && Mathf.Abs(accumAngle) > spinSwitch)
			{
				accumAngle = 0f;
				rotCW = !rotCW;
			}
		}
		if (!impaled)
		{
			return;
		}
		accumTimeReset += Time.deltaTime;
		if (accumTimeReset >= 3f)
		{
			FixedJoint component = fixedJointGO.GetComponent<FixedJoint>();
			if (component != null)
			{
				Object.Destroy(component);
			}
			impaled = false;
			accumTimeReset = 0f;
		}
	}

	public void TouchPlayer(Vector3 spikePos, float distanceToPlayer)
	{
		Vector3 vector = ((!rotCW) ? Vector3.right : Vector3.left);
		switch (dmgType)
		{
		case DamageType.HIT:
			if (!ProtectiveVestHelper.UseProtectiveVestIfAvailable())
			{
				SoundManager.PlaySound(SndId.SND_GORE_IMPACT_GENERIC);
				CharHelper.GetCharStateMachine().Hit(vector * (spinSpeed / 4f));
			}
			else
			{
				SoundManager.PlaySound(SndId.SND_FRED_OUCH);
				CharHelper.GetCharStateMachine().SwitchTo(ActionCode.BOUNCE);
			}
			break;
		case DamageType.DISMEMBER:
			if (!ProtectiveVestHelper.UseProtectiveVestIfAvailable())
			{
				if (ConfigParams.useGore)
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
			break;
		case DamageType.PIERCE:
			if (!ProtectiveVestHelper.UseProtectiveVestIfAvailable())
			{
				if (ConfigParams.useGore && distanceToPlayer < 2f)
				{
					Transform transformByName = CharHelper.GetTransformByName("torso1");
					CharHelper.GetCharSkin().Pierced(transformByName.position);
					CharHelper.GetPlayerTransform().position = spikePos - (transformByName.position - CharHelper.GetPlayerTransform().position);
					FixedJoint fixedJoint = fixedJointGO.GetComponent<FixedJoint>();
					if (fixedJoint == null)
					{
						fixedJoint = fixedJointGO.AddComponent<FixedJoint>();
					}
					if (fixedJoint != null)
					{
						setCollidersEnableValue(false);
						fixedJoint.gameObject.transform.position = spikePos;
						fixedJoint.connectedBody = transformByName.GetComponent<Rigidbody>();
						impaled = true;
					}
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
			break;
		case DamageType.EXPLOTE:
			if (!ProtectiveVestHelper.UseProtectiveVestIfAvailable())
			{
				if (ConfigParams.useGore)
				{
					CharHelper.GetCharStateMachine().SwitchTo(ActionCode.EXPLODE);
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
			break;
		case DamageType.FALLBACK:
			SoundManager.PlaySound(30);
			CharHelper.GetCharStateMachine().SwitchTo(ActionCode.BOUNCE);
			break;
		}
	}

	private void switchToIdle()
	{
		if (blades != null)
		{
			for (int i = 0; i < blades.Length; i++)
			{
				blades[i].SetActiveFB(true);
			}
		}
		enableRot = false;
	}

	private void setCollidersEnableValue(bool val)
	{
		if (colliderToDeactiveOnPierce != null)
		{
			for (int i = 0; i < colliderToDeactiveOnPierce.Length; i++)
			{
				colliderToDeactiveOnPierce[i].enabled = val;
			}
		}
	}
}
