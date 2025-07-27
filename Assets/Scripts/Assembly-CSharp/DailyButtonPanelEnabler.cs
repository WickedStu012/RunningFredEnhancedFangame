using UnityEngine;

public class DailyButtonPanelEnabler : MonoBehaviour
{
	private GUI3DTransition transition;

	private void Start()
	{
		SalesManager.Instance.OnSaleRequest += OnGetSale;
		RefreshStatus();
	}

	private void OnEnable()
	{
	}

	private void OnDestroy()
	{
		if (SalesManager.Instance != null)
		{
			SalesManager.Instance.OnSaleRequest -= OnGetSale;
		}
	}

	private void RefreshStatus()
	{
		if (transition == null)
		{
			transition = GetComponent<GUI3DTransition>();
		}
		transition.Enabled = false;
		transition.StartOnEnable = true;
		transition.enabled = false;
		SalesManager.Instance.GetCurrentSale();
	}

	private void OnGetSale(OnSale sale)
	{
		if (sale == null)
		{
			transition.Enabled = false;
		}
		else
		{
			transition.Enabled = true;
			transition.StartOnEnable = true;
			transition.StartTransition();
		}
		transition.enabled = true;
	}
}
