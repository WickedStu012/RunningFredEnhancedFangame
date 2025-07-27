using UnityEngine;

public class ResurrectByTapjoyButton : MonoBehaviour
{
	private GUI3DButton button;

	public BuyItemPopup ResurectByTapjoyPopup;

	private void Awake()
	{
	}

	private void OnEnable()
	{
		if (button == null)
		{
			button = GetComponent<GUI3DButton>();
		}
		button.ClickEvent += OnClick;
	}

	private void OnDisable()
	{
		if (button == null)
		{
			button = GetComponent<GUI3DButton>();
		}
		button.ClickEvent -= OnClick;
	}

	private void OnClick(GUI3DOnClickEvent evt)
	{
		if (Application.internetReachability != NetworkReachability.NotReachable && CharHelper.GetProps().freeResurectByTapjoy == ResurrectStatus.READY)
		{
			if (BeLordTapJoy.IsReadyToUse)
			{
				StatsManager.LogEvent(StatVar.TAPJOY);
				CharHelper.GetProps().freeResurectByTapjoy = ResurrectStatus.ACTIVATED;
				ResurectByTapjoyPopup.Close(GUI3DPopupManager.PopupResult.Cancel);
				TapjoyPlacementsManager.callbackOnDeactiveProcessing += OnDeactiveProcessing;
				TapjoyPlacementsManager.PlacementLoadAndShow("resurrect");
			}
		}
		else
		{
			GUI3DPopupManager.Instance.ShowPopup("Error", "Internet connection unavailable", "Error");
		}
	}

	private void OnDeactiveProcessing()
	{
		TapjoyPlacementsManager.callbackOnDeactiveProcessing -= OnDeactiveProcessing;
		GameManager.openResurrectPostVideoPopup();
	}
}
