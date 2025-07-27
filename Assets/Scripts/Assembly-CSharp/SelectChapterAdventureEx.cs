using UnityEngine;

public class SelectChapterAdventureEx : MonoBehaviour
{
	public static SelectChapterAdventureEx Instance;

	public GUI3DObject Picture;

	public GUI3DText Title;

	public GUI3DText Description;

	public GUI3DText CurrentLevel;

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
			CurrentLevel.SetDynamicText(string.Format(MonoBehaviorSingleton<GUI3DLocalization>.Instance.GetText("Unsorted", "Level2", "!BAD_TEXT!"), PlayerAccount.Instance.CurrentLevelNum));
		}
	}
}
