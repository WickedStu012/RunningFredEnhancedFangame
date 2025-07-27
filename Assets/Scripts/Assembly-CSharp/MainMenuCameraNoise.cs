using UnityEngine;

public class MainMenuCameraNoise : MonoBehaviour
{
	private LowFrequencyRandom rand = new LowFrequencyRandom(0.15f);

	private Transform trans;

	private Vector3 originalRot;

	private Vector3 rotVel;

	public void Start()
	{
		trans = base.transform;
		originalRot = trans.localEulerAngles;
	}

	private Vector3 SmoothDampAngle(Vector3 current, Vector3 target, ref Vector3 vel, float deltaTime)
	{
		Vector3 result = default(Vector3);
		result.x = Mathf.SmoothDampAngle(current.x, target.x, ref vel.x, deltaTime);
		result.y = Mathf.SmoothDampAngle(current.y, target.y, ref vel.y, deltaTime);
		result.z = Mathf.SmoothDampAngle(current.z, target.z, ref vel.z, deltaTime);
		return result;
	}

	public void Update()
	{
		rand.Update();
		Vector3 euler = SmoothDampAngle(trans.localEulerAngles, originalRot + rand.GetEulerRot() * 5f, ref rotVel, 0.5f);
		trans.localRotation = Quaternion.Euler(euler);
	}
}
