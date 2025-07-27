using UnityEngine;

public class GrimmyIdol : MonoBehaviour
{
	public SndId pickupSound;

	private bool disappear;

	private float accumTime;

	private bool picked;

	public static bool shouldShowUnlock;

	private void Start()
	{
		if (PlayerAccount.Instance != null && !PlayerAccount.Instance.IsGrimmyIdolTakenForCurrentLevel())
		{
			picked = false;
			disappear = false;
		}
		else
		{
			picked = true;
			Object.Destroy(base.gameObject);
		}
		shouldShowUnlock = false;
	}

	private void Update()
	{
		if (disappear)
		{
			accumTime += Time.deltaTime;
			if (accumTime > 0.1f)
			{
				disappear = false;
				picked = false;
				Object.Destroy(base.gameObject);
			}
		}
		else
		{
			base.gameObject.transform.Rotate(Vector3.forward, Time.deltaTime * 50f);
		}
	}

	private void OnTriggerEnter(Collider c)
	{
		if (picked || !CharHelper.IsColliderFromPlayer(c))
		{
			return;
		}
		if (PlayerAccount.Instance != null)
		{
			PlayerAccount.Instance.PickupGrimmyIdolInCurrentLevel();
			if (PlayerAccount.Instance.GetGrimmyIdolPickedCount() == ConfigParams.IronFredGrimmyGoal)
			{
				shouldShowUnlock = true;
			}
		}
		if (!shouldShowUnlock)
		{
			GUI3DPopupManager.Instance.ShowPopup("GrimmyIdol", string.Format("{0} of 30", PlayerAccount.Instance.GetGrimmyIdolPickedCount()), null, null, null, false, null);
		}
		else
		{
			GUI3DPopupManager.Instance.ShowPopup("GrimmyIdolIronFredUnlocked", string.Format("{0} of 30", PlayerAccount.Instance.GetGrimmyIdolPickedCount()), null, null, null, false, null);
		}
		SoundManager.PlaySound(base.transform.position, 72);
		disappear = true;
		accumTime = 0f;
		picked = true;
	}
}
