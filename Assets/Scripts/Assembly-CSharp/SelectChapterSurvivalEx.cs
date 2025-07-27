using UnityEngine;

public class SelectChapterSurvivalEx : MonoBehaviour
{
	public static SelectChapterSurvivalEx Instance;

	public GUI3DObject Picture;

	public GUI3DText Title;

	public GUI3DText Description;

	public GUI3DText DifficultyButtonText;

	public GUI3DObject DifficultyButtonIcon;

	private void Awake()
	{
		Instance = this;
	}

	private void OnEnable()
	{
		RefreshSelection();
	}

	public void RefreshSelection()
	{
		LocationItemInfo currentChapterInfo = PlayerAccount.Instance.CurrentChapterInfo;
		if (currentChapterInfo != null)
		{
			GUI3DObject picture = Picture;
			picture.ObjectSize = Vector2.zero;
			picture.TextureName = currentChapterInfo.Picture;
			picture.CreateOwnMesh = true;
			picture.CreateMesh();
			Title.SetDynamicText(currentChapterInfo.Name);
			Description.SetDynamicText(currentChapterInfo.Description);
			switch (PlayerAccount.Instance.CurrentLevelNum)
			{
			case 1:
				DifficultyButtonText.SetDynamicText(MonoBehaviorSingleton<GUI3DLocalization>.Instance.GetText("Unsorted", "Easy", "!BAD_TEXT!"));
				DifficultyButtonIcon.TextureName = "SurvivalEasy";
				break;
			case 2:
				DifficultyButtonText.SetDynamicText(MonoBehaviorSingleton<GUI3DLocalization>.Instance.GetText("Unsorted", "Normal", "!BAD_TEXT!"));
				DifficultyButtonIcon.TextureName = "SurvivalNormal";
				break;
			case 3:
				DifficultyButtonText.SetDynamicText(MonoBehaviorSingleton<GUI3DLocalization>.Instance.GetText("Unsorted", "Hard", "!BAD_TEXT!"));
				DifficultyButtonIcon.TextureName = "SurvivalHard";
				break;
			case 4:
				DifficultyButtonText.SetDynamicText(MonoBehaviorSingleton<GUI3DLocalization>.Instance.GetText("Unsorted", "Nightmare", "!BAD_TEXT!"));
				DifficultyButtonIcon.TextureName = "SurvivalHardcore";
				break;
			}
			DifficultyButtonIcon.CreateOwnMesh = true;
			DifficultyButtonIcon.CreateMesh();
		}
	}
}
