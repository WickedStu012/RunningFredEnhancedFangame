using UnityEngine;

public class CurrentSelectedAvatar : MonoBehaviour
{
	private GUI3DText avatarText;

	private GUI3DObject avatarPicture;

	private ItemInfo avatarInfo;

	private string avatarName = string.Empty;

	private string avatarPictureName = string.Empty;

	private void OnEnable()
	{
		if (PlayerAccount.Instance != null)
		{
			PlayerAccount.Instance.AvatarChangeEvent += OnAvatarChange;
			avatarInfo = PlayerAccount.Instance.CurrentAvatarInfo;
			Refresh();
		}
	}

	private void OnDisable()
	{
		if (PlayerAccount.Instance != null)
		{
			PlayerAccount.Instance.AvatarChangeEvent -= OnAvatarChange;
		}
	}

	private void OnAvatarChange(ItemInfo avatar)
	{
		avatarInfo = avatar;
		Refresh();
	}

	private void Refresh()
	{
		if (avatarInfo == null)
		{
			return;
		}
		avatarName = avatarInfo.Name;
		avatarPictureName = avatarInfo.Picture;
		avatarText = GetComponentInChildren<GUI3DText>();
		if (avatarText == null)
		{
			Debug.LogError("Avatar text doesn't exist.");
			return;
		}
		avatarPicture = GetComponentInChildren<GUI3DObject>();
		if (avatarPicture == null)
		{
			Debug.LogError("Avatar picture doesn't exist.");
			return;
		}
		avatarText.SetDynamicText(avatarName);
		avatarPicture.TextureName = avatarPictureName;
		avatarPicture.ObjectSize = Vector2.zero;
		if (!avatarPicture.CreateOwnMesh)
		{
			avatarPicture.CreateOwnMesh = true;
			avatarPicture.CreateMesh();
		}
		else
		{
			avatarPicture.RefreshUV();
		}
	}
}
