using UnityEngine;

public class GetFreeSkulliesButton : MonoBehaviour
{
	public SkullieButtonType ButtonType;

	private GUI3DButton button;

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
		if (Application.internetReachability != NetworkReachability.NotReachable)
		{
			if (ConfigParams.IsKongregate())
			{
				FullScreenChecker.ChangeToFullScreen(false);
				KongregateAPI.OpenOfferWall();
			}
			else if (BeLordTapJoy.IsReadyToUse)
			{
				StatsManager.LogEvent(StatVar.TAPJOY);
				if (ButtonType == SkullieButtonType.VideoOnDemand)
				{
					TapjoyPlacementsManager.PlacementLoadAndShow("shop_video_on_demand");
				}
				else
				{
					TapjoyPlacementsManager.ShowOffers();
				}
			}
		}
		else
		{
			GUI3DPopupManager.Instance.ShowPopup("Error", "Internet connection unavailable", "Error");
		}
	}
}
