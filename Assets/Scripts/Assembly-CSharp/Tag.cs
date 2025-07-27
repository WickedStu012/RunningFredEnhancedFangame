using UnityEngine;

public class Tag
{
	public const int LEVEL_ROOT = 7;

	public const int TRAP = 8;

	public const int DECORATION = 10;

	public const int PLATFORM = 11;

	public const int PICKUP = 12;

	public const int STAIRS = 13;

	public const int WALL_RUN = 18;

	public const string TAG_PLATFORM = "Platform";

	public const string TAG_TRAP = "Trap";

	public const string TAG_DECORATION = "Decoration";

	public const string TAG_STAIRS = "Stairs";

	public const string TAG_MANAGERS = "Managers";

	public const string TAG_WALL_RUN = "WallRun";

	public const string TAG_AEREAL_ACCELERATOR = "AerealAccelerator";

	public static bool IsPlatform(string tag)
	{
		return string.Compare(tag, "Platform") == 0;
	}

	public static bool IsWallRun(string tag)
	{
		return string.Compare(tag, "WallRun") == 0;
	}

	public static bool IsStairs(string tag)
	{
		return string.Compare(tag, "Stairs") == 0;
	}

	public static string GetStairsStr()
	{
		return "Stairs";
	}

	public static bool IsBreakablePlatform(Collider c)
	{
		if (c.gameObject.tag == "Platform")
		{
			LevelPlatform component = c.gameObject.GetComponent<LevelPlatform>();
			if (component != null)
			{
				return component.isBreakable;
			}
		}
		return false;
	}
}
