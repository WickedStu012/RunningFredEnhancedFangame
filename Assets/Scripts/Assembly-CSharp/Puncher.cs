using System.Collections.Generic;
using UnityEngine;

public class Puncher : MonoBehaviour
{
	private enum Action
	{
		IDLE = 0,
		TRIGGER = 1,
		TRIGGER_END = 2,
		RESET = 3
	}

	public enum DamageType
	{
		HIT = 0,
		PIERCE = 1,
		DISMEMBER = 2,
		EXPLOTE = 3
	}

	public DamageType dmgType;

	public bool selfReset = true;

	public float selfResetTimerSecs = 3f;

	public float speedOnActivation = 1f;

	public float speedOnDeactivation = 1f;

	public Vector3 targetOffsetPosition;

	public Vector3 targetOffsetRotationEuler;

	public bool transformPos = true;

	public bool transformRot;

	public SndId activateSoundId;

	public SndId deactivateSoundId;

	public SndId hitSoundId;

	public bool autoMove;

	public float activateTimer;

	public float activateTimerInitialValue;

	public Collider[] colliderToDeactiveOnPierce;

	public GameObject fixedJointGO;

	public bool shakeScreenOnHit;

	public float shakeScreenOnDistanceToPlayerMinorThan = 100f;

	private Action action;

	private Vector3 originalPos;

	private Vector3 targetPos;

	private Quaternion originalRot;

	private Quaternion targetRot;

	private float accumTime;

	private float accumTimeAutoMove;

	private ObstacleFallBack objFB;

	private bool collide;

	private bool originalPosAndRotGet;

	private Vector3 forceDir;

	private List<Transform> points;

	private ObstacleFallBack obsFallback;

	private bool disableFJ;

	private float accumTimeDisableFJ;

	private void Start()
	{
		action = Action.IDLE;
		collide = false;
		objFB = GetComponent<ObstacleFallBack>();
		originalPosAndRotGet = false;
		getSpikePoints();
		accumTimeAutoMove = activateTimerInitialValue;
		obsFallback = base.gameObject.GetComponent<ObstacleFallBack>();
		if (obsFallback != null)
		{
			obsFallback.enabled = false;
		}
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
		getOriginalPosAndRot();
		switch (action)
		{
		case Action.IDLE:
			if (autoMove)
			{
				accumTimeAutoMove += Time.deltaTime;
				if (accumTimeAutoMove > activateTimer)
				{
					accumTimeAutoMove = 0f;
					action = Action.TRIGGER;
				}
			}
			break;
		case Action.TRIGGER:
			accumTime += Time.deltaTime;
			if (transformPos)
			{
				base.transform.position = Vector3.Lerp(originalPos, targetPos, accumTime * speedOnActivation);
			}
			if (transformRot)
			{
				base.transform.localRotation = Quaternion.Slerp(originalRot, targetRot, accumTime * speedOnActivation);
			}
			if (!(accumTime * speedOnActivation >= 1f))
			{
				break;
			}
			if (selfReset)
			{
				if (obsFallback != null)
				{
					obsFallback.enabled = true;
				}
				accumTime = 0f;
				action = Action.TRIGGER_END;
			}
			else
			{
				switchToIdle();
			}
			if (hitSoundId != SndId.SND_NONE)
			{
				if (base.GetComponent<AudioSource>() == null)
				{
					Debug.Log(string.Format("There isn't an audio source on this gameobject {0}", base.name));
				}
				else
				{
					SoundProp soundProp = SoundManager.GetSoundProp((int)hitSoundId);
					if (soundProp != null)
					{
						base.GetComponent<AudioSource>().clip = soundProp.audioClip;
						base.GetComponent<AudioSource>().Play();
					}
				}
			}
			if (shakeScreenOnHit && Vector3.Distance(CharHelper.GetPlayerTransform().position, base.transform.position) <= shakeScreenOnDistanceToPlayerMinorThan)
			{
				ScreenShaker.Shake(0.5f, 8f);
			}
			break;
		case Action.TRIGGER_END:
			accumTime += Time.deltaTime;
			if (!(accumTime > selfResetTimerSecs))
			{
				break;
			}
			accumTime = 0f;
			if (deactivateSoundId != SndId.SND_NONE)
			{
				if (base.GetComponent<AudioSource>() == null)
				{
					Debug.Log(string.Format("There isn't an audio source on this gameobject {0}", base.name));
				}
				else
				{
					SoundProp soundProp2 = SoundManager.GetSoundProp((int)deactivateSoundId);
					if (soundProp2 != null)
					{
						base.GetComponent<AudioSource>().clip = soundProp2.audioClip;
						base.GetComponent<AudioSource>().Play();
					}
				}
			}
			if (obsFallback != null)
			{
				obsFallback.enabled = false;
			}
			action = Action.RESET;
			break;
		case Action.RESET:
			accumTime += Time.deltaTime;
			if (transformPos)
			{
				base.transform.position = Vector3.Lerp(targetPos, originalPos, accumTime * speedOnDeactivation);
			}
			if (transformRot)
			{
				base.transform.localRotation = Quaternion.Slerp(targetRot, originalRot, accumTime * speedOnDeactivation);
			}
			if (accumTime * speedOnDeactivation >= 1f)
			{
				switchToIdle();
			}
			break;
		}
		if (!disableFJ)
		{
			return;
		}
		accumTimeDisableFJ += Time.deltaTime;
		if (!(accumTimeDisableFJ >= 3f))
		{
			return;
		}
		if (fixedJointGO != null)
		{
			FixedJoint component = fixedJointGO.GetComponent<FixedJoint>();
			if (component != null)
			{
				Object.Destroy(component);
			}
		}
		disableFJ = false;
	}

	private void getOriginalPosAndRot()
	{
		if (!originalPosAndRotGet)
		{
			if (transformPos)
			{
				originalPos = base.transform.position;
				targetPos = base.transform.position + base.transform.TransformDirection(targetOffsetPosition);
				forceDir = targetPos - originalPos;
				forceDir.Normalize();
			}
			if (transformRot)
			{
				originalRot = base.transform.localRotation;
				targetRot = Quaternion.Euler(originalRot.eulerAngles + targetOffsetRotationEuler);
			}
			originalPosAndRotGet = true;
		}
	}

	public void Trigger()
	{
		if (action != Action.IDLE)
		{
			return;
		}
		action = Action.TRIGGER;
		if (activateSoundId != SndId.SND_NONE)
		{
			if (base.GetComponent<AudioSource>() == null)
			{
				Debug.Log(string.Format("There isn't an audio source on this gameobject {0}", base.name));
			}
			else
			{
				SoundProp soundProp = SoundManager.GetSoundProp((int)activateSoundId);
				if (soundProp != null)
				{
					base.GetComponent<AudioSource>().clip = soundProp.audioClip;
					base.GetComponent<AudioSource>().Play();
				}
			}
		}
		if (objFB != null)
		{
			objFB.enabled = false;
		}
	}

	private void switchToIdle()
	{
		accumTime = 0f;
		if (objFB != null)
		{
			objFB.enabled = true;
		}
		collide = false;
		action = Action.IDLE;
	}

	private void OnTriggerEnter(Collider c)
	{
		if (collide || !CharHelper.IsColliderFromPlayer(c) || action != Action.TRIGGER)
		{
			return;
		}
		switch (dmgType)
		{
		case DamageType.HIT:
			if (!ProtectiveVestHelper.UseProtectiveVestIfAvailable())
			{
				SoundManager.PlaySound(SndId.SND_GORE_IMPACT_GENERIC);
				CharHelper.GetCharStateMachine().Hit(forceDir * speedOnActivation * 50f);
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
				Vector3 position = CharHelper.GetPlayerTransform().position;
				if (ConfigParams.useGore)
				{
					Transform transformByName = CharHelper.GetTransformByName("torso1");
					CharHelper.GetCharSkin().Pierced(transformByName.position);
					Vector3 nearPosition = getNearPosition(position);
					CharHelper.GetPlayerTransform().position = nearPosition - (transformByName.position - CharHelper.GetPlayerTransform().position);
					FixedJoint fixedJoint = null;
					if (fixedJointGO != null)
					{
						fixedJoint = fixedJointGO.GetComponent<FixedJoint>();
						if (fixedJoint == null)
						{
							fixedJoint = fixedJointGO.AddComponent<FixedJoint>();
						}
					}
					if (fixedJoint != null)
					{
						fixedJoint.transform.position = CharHelper.GetPlayerTransform().position;
						fixedJoint.connectedBody = transformByName.GetComponent<Rigidbody>();
						disableFJ = true;
						accumTimeDisableFJ = 0f;
						setCollidersEnableValue(false);
						switchToIdle();
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
		}
		collide = true;
	}

	private void getSpikePoints()
	{
		Transform[] componentsInChildren = GetComponentsInChildren<Transform>(true);
		points = new List<Transform>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			if (string.Compare(componentsInChildren[i].name, "point", true) == 0)
			{
				points.Add(componentsInChildren[i]);
			}
		}
	}

	private Vector3 getNearPosition(Vector3 playerPos)
	{
		int index = 0;
		float num = Vector3.Distance(points[0].position, playerPos);
		for (int i = 1; i < points.Count; i++)
		{
			float num2 = Vector3.Distance(points[i].position, playerPos);
			if (num2 < num)
			{
				num = num2;
				index = i;
			}
		}
		return points[index].position;
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
