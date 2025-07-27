using UnityEngine;

public class LevelProps : MonoBehaviour
{
	public int LevelId;

	public static LevelProps Instance;

	private void Awake()
	{
		Instance = this;
	}
}
