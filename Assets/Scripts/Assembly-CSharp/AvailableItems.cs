using UnityEngine;

public class AvailableItems : MonoBehaviour
{
	public string Type = string.Empty;

	public string Tag = string.Empty;

	public GUI3DText Text;

	private GUI3DObject guiObject;

	private void Awake()
	{
		if (PlayerAccount.Instance != null)
		{
			PlayerAccount.Instance.MoneyChangeEvent += OnMoneyChange;
		}
	}

	private void Start()
	{
	}

	private void OnEnable()
	{
		if (Text == null)
		{
			Text = GetComponentInChildren<GUI3DText>();
		}
		if (guiObject == null)
		{
			guiObject = GetComponent<GUI3DObject>();
		}
		Reset();
	}

	private void OnDestroy()
	{
		if (PlayerAccount.Instance != null)
		{
			PlayerAccount.Instance.MoneyChangeEvent -= OnMoneyChange;
		}
	}

	private void Reset()
	{
		int num = 0;
		num = ((Type == string.Empty) ? Store.Instance.CanPurchaseItemsCount() : ((!(Tag == string.Empty)) ? Store.Instance.CanPurchaseItemsCount(Type, Tag) : Store.Instance.CanPurchaseItemsCount(Type)));
		if (num == 0)
		{
			Text.SetDynamicText(string.Empty);
			if (guiObject.GetComponent<Renderer>() != null)
			{
				guiObject.GetComponent<Renderer>().enabled = false;
			}
			return;
		}
		if (num <= 9)
		{
			Text.SetDynamicText(num.ToString());
		}
		else
		{
			Text.SetDynamicText("+");
		}
		if (guiObject.GetComponent<Renderer>() != null)
		{
			guiObject.GetComponent<Renderer>().enabled = true;
		}
	}

	private void OnMoneyChange(int money)
	{
		Reset();
	}
}
