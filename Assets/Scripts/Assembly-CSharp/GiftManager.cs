using UnityEngine;

public class GiftManager : MonoBehaviour
{
	public class GiftType
	{
		public int Id;

		public string Name;

		public int Probability;

		public string Description;

		public string Picture;
	}

	public GiftType[] GiftTypes;

	public GiftType[] GiftTypeById;

	public GiftType CurrentActiveGift;

	public GiftType CollectedGift;

	public int Probability = 10;

	public GameObject GiftPrefab;

	private int maxRandom;

	private static GiftManager instance;

	private bool isPiggyBankActive;

	public static GiftManager Instance
	{
		get
		{
			return instance;
		}
	}

	public bool IsPiggyBankActive
	{
		get
		{
			return isPiggyBankActive;
		}
	}

	private void Awake()
	{
		GameEventDispatcher.AddListener("OnGiftPlacerReady", OnPlaceGift);
		instance = this;
		isPiggyBankActive = PlayerAccount.Instance.IsPiggyBankActive();
		GiftInfo[] array = ItemsLoader.Load<GiftInfo>("Shop/giftslist");
		if (array != null && array.Length != 0)
		{
			GiftTypes = new GiftType[array.Length];
			GiftTypeById = new GiftType[array.Length];
			int num = 0;
			GiftInfo[] array2 = array;
			foreach (GiftInfo giftInfo in array2)
			{
				GiftType giftType = new GiftType();
				giftType.Id = giftInfo.Id;
				giftType.Name = giftInfo.Name;
				giftType.Probability = giftInfo.Probability;
				giftType.Description = giftInfo.Description;
				giftType.Picture = giftInfo.Picture;
				GiftTypeById[giftInfo.Id] = giftType;
				GiftTypes[num++] = giftType;
			}
			OrderByProbability();
			for (int j = 0; j < GiftTypes.Length; j++)
			{
				maxRandom += GiftTypes[j].Probability;
			}
			if (PlayerAccount.Instance.IsGiftActive())
			{
				CurrentActiveGift = GiftTypeById[PlayerAccount.Instance.GetGift()];
				PlayerAccount.Instance.ClearGift();
			}
		}
	}

	private void OnPlaceGift(object sender, GameEvent evt)
	{
		int num = Random.Range(0, 100);
		if (num <= Probability)
		{
			Vector3 randomGiftPosition = GiftPlacer.GetRandomGiftPosition();
			GameObject gameObject = Object.Instantiate(GiftPrefab) as GameObject;
			gameObject.transform.position = randomGiftPosition;
		}
	}

	private void OnPlayerIsDead(object sender, GameEvent evt)
	{
		GameEventDispatcher.RemoveListener("OnLevelComplete", OnLevelCompleted);
	}

	private void OnLevelCompleted(object sender, GameEvent evt)
	{
		if (CollectedGift != null)
		{
			if (CollectedGift.Id == 0)
			{
				PlayerAccount.Instance.ActivatePiggyBank();
			}
			else if (CollectedGift.Id == 1)
			{
				PlayerAccount.Instance.ActivateDoubleSkully();
			}
			else if (CollectedGift.Id == 3)
			{
				PlayerAccount.Instance.ActivateExtraLife();
			}
			else if (CollectedGift.Id == 2)
			{
				PlayerAccount.Instance.ActivateDiscount();
				Store.Instance.RefreshItems();
			}
			else if (CollectedGift.Id == 4)
			{
				ItemInfo item = Store.Instance.GetItem(107);
				PlayerPrefsWrapper.AddItem(item);
			}
			else if (CollectedGift.Id == 5)
			{
				ItemInfo item2 = Store.Instance.GetItem(121);
				PlayerPrefsWrapper.AddItem(item2);
			}
			else if (CollectedGift.Id == 6)
			{
				ItemInfo item3 = Store.Instance.GetItem(120);
				PlayerPrefsWrapper.AddItem(item3);
			}
			else if (CollectedGift.Id == 7)
			{
				ItemInfo item4 = Store.Instance.GetItem(122);
				PlayerPrefsWrapper.AddItem(item4);
			}
			else if (CollectedGift.Id == 8)
			{
				ItemInfo item5 = Store.Instance.GetItem(107);
				PlayerPrefsWrapper.AddItem(item5, 3);
			}
			else if (CollectedGift.Id == 9)
			{
				ItemInfo item6 = Store.Instance.GetItem(121);
				PlayerPrefsWrapper.AddItem(item6, 3);
			}
			else if (CollectedGift.Id == 10)
			{
				ItemInfo item7 = Store.Instance.GetItem(120);
				PlayerPrefsWrapper.AddItem(item7, 3);
			}
			else if (CollectedGift.Id == 11)
			{
				ItemInfo item8 = Store.Instance.GetItem(122);
				PlayerPrefsWrapper.AddItem(item8, 3);
			}
			else if (CollectedGift.Id == 12)
			{
				SoundManager.PlaySound(8);
				ItemInfo item9 = Store.Instance.GetItem(2006);
				PlayerAccount.Instance.AddMoney(item9.PackCount);
			}
			GUI3DManager.Instance.Activate("Gift", false, false);
		}
		else if (GrimmyIdol.shouldShowUnlock)
		{
			GrimmyIdol.shouldShowUnlock = false;
			GUI3DManager.Instance.Activate("IronFredUnlocked", true, true);
		}
		else if (PlayerAccount.Instance.CurrentGameMode != PlayerAccount.GameMode.Challenge)
		{
			GUI3DManager.Instance.Activate("Results", false, false);
		}
	}

	private void OnEnable()
	{
		instance = this;
		GameEventDispatcher.AddListener("OnLevelComplete", OnLevelCompleted);
		GameEventDispatcher.AddListener("GiftPickup", OnGiftPickup);
		GameEventDispatcher.AddListener("OnPlayerDead", OnPlayerIsDead);
	}

	private void OnDisable()
	{
		instance = null;
	}

	private void OnDestroy()
	{
		instance = null;
	}

	private void OnGiftPickup(object sender, GameEvent evt)
	{
		GiftType giftType = null;
		int num = Mathf.RoundToInt(Random.Range(0, maxRandom));
		int num2 = 0;
		for (int i = 0; i < GiftTypes.Length; i++)
		{
			num2 += GiftTypes[i].Probability;
			if (num2 >= num && (GiftTypes[i].Id != 0 || !PlayerAccount.Instance.IsPiggyBankActive()))
			{
				giftType = GiftTypes[i];
				break;
			}
		}
		if (giftType != null)
		{
			PlayerAccount.Instance.PickUpGift(giftType.Id);
		}
		CollectedGift = giftType;
	}

	private void OrderByProbability()
	{
		if (GiftTypes != null && GiftTypes.Length != 0)
		{
			OrderByProbability(GiftTypes, 0, GiftTypes.Length - 1);
		}
	}

	private void OrderByProbability(GiftType[] objects, int lo, int hi)
	{
		int num = (lo + hi) / 2;
		float num2 = GiftTypes[num].Probability;
		int num3 = lo;
		int num4 = hi;
		while (true)
		{
			if ((float)GiftTypes[num3].Probability > num2)
			{
				num3++;
				continue;
			}
			while ((float)GiftTypes[num4].Probability < num2)
			{
				num4--;
			}
			if (num3 <= num4)
			{
				GiftType giftType = GiftTypes[num3];
				GiftTypes[num3] = GiftTypes[num4];
				GiftTypes[num4] = giftType;
				num3++;
				num4--;
			}
			if (num3 > num4)
			{
				break;
			}
		}
		if (lo < num4)
		{
			OrderByProbability(objects, lo, num4);
		}
		if (hi > num3)
		{
			OrderByProbability(objects, num3, hi);
		}
	}
}
