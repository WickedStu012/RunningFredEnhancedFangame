using UnityEngine;

public class AchievementItem : MonoBehaviour
{
	public AchievementItemInfo ItemInfo;

	public GUI3DObject Icon;

	private GUI3DText text;

	private GUI3DButton button;

	private void Awake()
	{
		if (button == null)
		{
			button = GetComponent<GUI3DButton>();
		}
		button.ReleaseEvent += OnClick;
	}

	private void Start()
	{
		text = GetComponentInChildren<GUI3DText>();
		text.SetDynamicText(ItemInfo.Name);
	}

	public void Refresh()
	{
		if (PlayerPrefsWrapper.HasAchievement(ItemInfo.AchievementId))
		{
			Debug.Log("Achievement " + ItemInfo.AchievementId + " unlocked!");
			button.StartSegRollOverTexture = "green-hover-left";
			button.EndSegRollOverTexture = "green-hover-right";
			button.RollOverTexture = "green-hover-stretch";
			button.StartSegmentTexName = "green-normal-left";
			button.TextureName = "green-normal-stretch";
			button.EndSegmentTexName = "green-normal-right";
			button.StartSegPressedTexture = "green-down-left";
			button.EndSegPressedTexture = "green-down-right";
			button.PressedTexture = "green-down-stretch";
		}
		else
		{
			button.StartSegmentTexName = "disabled-left";
			button.TextureName = "disabled-stretch";
			button.EndSegmentTexName = "disabled-right";
		}
		button.CreateOwnMesh = true;
		button.CreateMesh();
		button.RefreshUVs();
		if (Icon != null)
		{
			Icon.TextureName = ItemInfo.Picture;
			Icon.CreateOwnMesh = true;
			Icon.CreateMesh();
		}
	}

	private void OnClick(GUI3DOnReleaseEvent evt)
	{
		Debug.Log(string.Format("ItemInfo.Name: {0}", ItemInfo.Name));
		GUI3DPopupManager.Instance.ShowPopup("ShowAchievement", ItemInfo.Description, ItemInfo.Name, ItemInfo.Picture);
	}
}
