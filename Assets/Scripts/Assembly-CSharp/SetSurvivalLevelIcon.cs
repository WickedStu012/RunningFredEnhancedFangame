using UnityEngine;

public class SetSurvivalLevelIcon : MonoBehaviour
{
	private GUI3DObject icon;

	private void Awake()
	{
		if (icon == null)
		{
			icon = GetComponent<GUI3DObject>();
			switch (PlayerAccount.Instance.CurrentLevelNum)
			{
			case 1:
				icon.TextureName = "SurvivalEasy";
				break;
			case 2:
				icon.TextureName = "SurvivalNormal";
				break;
			case 3:
				icon.TextureName = "SurvivalHard";
				break;
			case 4:
				icon.TextureName = "SurvivalHardcore";
				break;
			}
			icon.ObjectSize = Vector2.zero;
			icon.CreateOwnMesh = true;
			icon.CreateMesh();
		}
	}
}
