using UnityEngine;

public class LivesScaleAnim : MonoBehaviour
{
	private float scaleDelta = 0.2f;

	private float accumTime;

	private bool rescaleEffect;

	private Vector3 normalScale;

	private int livesToRecover;

	private void Start()
	{
		normalScale = base.transform.localScale;
		GameEventDispatcher.AddListener("OnAdventureResurrect", onAdventureResurrect);
	}

	private void Update()
	{
		if (!rescaleEffect)
		{
			return;
		}
		accumTime += Time.deltaTime;
		float num = normalScale.x + scaleDelta * Mathf.Sin(accumTime * 20f);
		base.transform.localScale = new Vector3(num, num, num);
		if (accumTime >= 0.3f)
		{
			CharHelper.GetProps().Lives++;
			switch (CharHelper.GetProps().Lives)
			{
			case 1:
				SoundManager.PlaySound(60);
				break;
			case 2:
				SoundManager.PlaySound(61);
				break;
			case 3:
				SoundManager.PlaySound(62);
				break;
			case 4:
				SoundManager.PlaySound(63);
				break;
			case 5:
				SoundManager.PlaySound(64);
				break;
			}
			if (livesToRecover == CharHelper.GetProps().Lives)
			{
				base.transform.localScale = normalScale;
				rescaleEffect = false;
			}
			else
			{
				accumTime = 0f;
			}
		}
	}

	private void onAdventureResurrect(object sender, GameEvent evt)
	{
		accumTime = 0f;
		rescaleEffect = true;
		SoundManager.PlaySound(44);
		ItemInfo item = Store.Instance.GetItem(111);
		livesToRecover = item.Upgrades;
	}
}
