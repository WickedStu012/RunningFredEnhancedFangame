using UnityEngine;

public class CurrentLevelInfo : MonoBehaviour
{
	public GUI3DObject LevelPicture;

	public GUI3DObject SurvivalIcon;

	private GUI3DText text;

	private string chapterName = string.Empty;

	private string level = string.Empty;

	private void OnEnable()
	{
		if (text == null)
		{
			text = GetComponent<GUI3DText>();
		}
		if (!(PlayerAccount.Instance != null))
		{
			return;
		}
		PlayerAccount.Instance.LevelChangeEvent += OnLevelChange;
		if (PlayerAccount.Instance.CurrentChapterInfo != null)
		{
			chapterName = PlayerAccount.Instance.CurrentChapterInfo.Name;
			level = PlayerAccount.Instance.CurrentLevelNum.ToString();
			LevelPicture.ObjectSize = Vector2.zero;
			if (PlayerAccount.Instance.CurrentGameMode == PlayerAccount.GameMode.Adventure)
			{
				if (SurvivalIcon != null && SurvivalIcon.GetComponent<Renderer>() != null)
				{
					SurvivalIcon.GetComponent<Renderer>().enabled = false;
				}
			}
			else if (SurvivalIcon != null)
			{
				switch (level)
				{
				case "1":
					SurvivalIcon.TextureName = "SurvivalEasy";
					break;
				case "2":
					SurvivalIcon.TextureName = "SurvivalNormal";
					break;
				case "3":
					SurvivalIcon.TextureName = "SurvivalHard";
					break;
				case "4":
					SurvivalIcon.TextureName = "SurvivalHardcore";
					break;
				}
				if (!SurvivalIcon.CreateOwnMesh)
				{
					SurvivalIcon.CreateOwnMesh = true;
					SurvivalIcon.CreateMesh();
				}
				else
				{
					SurvivalIcon.RefreshUV();
				}
				if (SurvivalIcon.GetComponent<Renderer>() != null)
				{
					SurvivalIcon.GetComponent<Renderer>().enabled = true;
				}
			}
			LevelPicture.TextureName = PlayerAccount.Instance.CurrentChapterInfo.Picture;
		}
		Refresh();
	}

	private void OnDisable()
	{
		if (PlayerAccount.Instance != null)
		{
			PlayerAccount.Instance.LevelChangeEvent -= OnLevelChange;
		}
	}

	private void OnLevelChange(LocationItemInfo chapter, int level)
	{
		chapterName = chapter.Name;
		this.level = level.ToString();
		LevelPicture.ObjectSize = Vector2.zero;
		if (PlayerAccount.Instance.CurrentGameMode == PlayerAccount.GameMode.Adventure)
		{
			if (SurvivalIcon != null && SurvivalIcon.GetComponent<Renderer>() != null)
			{
				SurvivalIcon.GetComponent<Renderer>().enabled = false;
			}
		}
		else if (SurvivalIcon != null)
		{
			switch (this.level)
			{
			case "1":
				SurvivalIcon.TextureName = "SurvivalEasy";
				break;
			case "2":
				SurvivalIcon.TextureName = "SurvivalNormal";
				break;
			case "3":
				SurvivalIcon.TextureName = "SurvivalHard";
				break;
			case "4":
				SurvivalIcon.TextureName = "SurvivalHardcore";
				break;
			}
			if (!SurvivalIcon.CreateOwnMesh)
			{
				SurvivalIcon.CreateOwnMesh = true;
				SurvivalIcon.CreateMesh();
			}
			else
			{
				SurvivalIcon.RefreshUV();
			}
			if (SurvivalIcon.GetComponent<Renderer>() != null)
			{
				SurvivalIcon.GetComponent<Renderer>().enabled = true;
			}
		}
		LevelPicture.TextureName = PlayerAccount.Instance.CurrentChapterInfo.Picture;
		Refresh();
	}

	private void Refresh()
	{
		string text = chapterName + "\n";
		text = ((PlayerAccount.Instance.CurrentGameMode != PlayerAccount.GameMode.Adventure) ? (text + string.Format(MonoBehaviorSingleton<GUI3DLocalization>.Instance.GetText("Unsorted", "Best", "!BAD_TEXT!"), PlayerAccount.Instance.GetMetersFromCurrentLevel())) : (text + string.Format(MonoBehaviorSingleton<GUI3DLocalization>.Instance.GetText("Unsorted", "Level", "!BAD_TEXT!"), level)));
		this.text.SetDynamicText(text);
		if (!LevelPicture.CreateOwnMesh)
		{
			LevelPicture.CreateOwnMesh = true;
			LevelPicture.CreateMesh();
		}
		else
		{
			LevelPicture.RefreshUV();
		}
	}
}
