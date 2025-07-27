using UnityEngine;

public class SelectAvatarEx : MonoBehaviour
{
	public GUI3DObject Picture;

	public GUI3DText Title;

	public GUI3DText Description;

	public GUI3DText MenuTitle;

	private void OnEnable()
	{
		AvatarItemInfo currentAvatarInfo = PlayerAccount.Instance.CurrentAvatarInfo;
		if (currentAvatarInfo != null)
		{
			GUI3DObject picture = Picture;
			picture.ObjectSize = Vector2.zero;
			picture.TextureName = currentAvatarInfo.Picture;
			picture.CreateOwnMesh = true;
			picture.CreateMesh();
			Title.SetDynamicText(currentAvatarInfo.Name);
			Description.SetDynamicText(currentAvatarInfo.Description);
			if (PlayerAccount.Instance.CurrentGameMode == PlayerAccount.GameMode.Adventure)
			{
				MenuTitle.SetDynamicText(MonoBehaviorSingleton<GUI3DLocalization>.Instance.GetText("Unsorted", "Adventure", "!BAD_TEXT!"));
			}
			else
			{
				MenuTitle.SetDynamicText(MonoBehaviorSingleton<GUI3DLocalization>.Instance.GetText("Unsorted", "Survival", "!BAD_TEXT!"));
			}
		}
	}
}
