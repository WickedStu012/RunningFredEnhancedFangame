using UnityEngine;

public class PickeableItem : MonoBehaviour
{
	public enum ItemType
	{
		GOLD = 0,
		SILVER = 1,
		SUPER_SPRINT = 2,
		GIFT = 3
	}

	public ItemType itemType;

	public GameObject Shine;

	public bool rotate = true;

	private bool exploding;

	private float accumTime;

	private bool picked;

	private Transform transf;

	private CharProps props;

	private Transform playerT;

	private bool startAtract;

	private ParticleSystem[] psShine;

	private GameObject ps;

	private void Start()
	{
		picked = false;
		transf = base.transform;
		props = CharHelper.GetProps();
		playerT = CharHelper.GetPlayerTransform();
		psShine = GetComponentsInChildren<ParticleSystem>();
	}

	private void OnDisable()
	{
		if (startAtract && !picked)
		{
			pickCoin();
		}
	}

	private void Update()
	{
		if (exploding)
		{
			accumTime += Time.deltaTime;
			if (accumTime > 0.5f)
			{
				exploding = false;
				base.gameObject.SetActive(false);
			}
		}
		if (rotate)
		{
			transf.localRotation = RotatorManager.angle * new Quaternion(-0.7f, 0f, 0f, 0.7f);
		}
		if ((itemType == ItemType.GOLD || itemType == ItemType.SILVER) && props.MagnetLevel > 0)
		{
			checkMagnetAction();
		}
		if (startAtract && !picked)
		{
			transf.position = Vector3.Lerp(transf.position, playerT.position + Vector3.forward, 0.5f);
		}
	}

	private void OnTriggerEnter(Collider c)
	{
		if (!picked && CharHelper.IsColliderFromPlayer(c))
		{
			pickCoin();
		}
	}

	private void checkMagnetAction()
	{
		float num = Vector3.Distance(playerT.position, transf.position);
		if (num < (float)props.MagnetLevel + 1f)
		{
			startAtract = true;
		}
	}

	private void pickCoin()
	{
		switch (itemType)
		{
		case ItemType.GOLD:
			SoundManager.PlaySound(base.transform.position, 3);
			GameEventDispatcher.Dispatch(this, GoldCoinPickup.Instance);
			break;
		case ItemType.SILVER:
			SoundManager.PlaySound(base.transform.position, 4);
			GameEventDispatcher.Dispatch(this, SilverCoinPickup.Instance);
			break;
		case ItemType.SUPER_SPRINT:
			SoundManager.PlaySound(base.transform.position, 3);
			break;
		case ItemType.GIFT:
			SoundManager.PlaySound(base.transform.position, 31);
			GameEventDispatcher.Dispatch(this, GiftPickup.Instance);
			GUI3DPopupManager.Instance.ShowPopup("MisteryBox", "MYSTERY BOX!", false);
			break;
		}
		if (Shine != null)
		{
			ps = Object.Instantiate(Shine, base.transform.position, Quaternion.Euler(Vector3.zero)) as GameObject;
			changeGameObjectParent();
		}
		base.GetComponent<Renderer>().enabled = false;
		if (psShine != null)
		{
			for (int i = 0; i < psShine.Length; i++)
			{
				psShine[i].enabled = false;
			}
		}
		accumTime = 0f;
		exploding = true;
		picked = true;
		startAtract = false;
	}

	private void changeGameObjectParent()
	{
		ps.transform.parent = base.transform;
	}
}
