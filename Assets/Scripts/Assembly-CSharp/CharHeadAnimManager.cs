using UnityEngine;

public class CharHeadAnimManager
{
	public enum Anim
	{
		none = 0,
		dead = 1,
		dying = 2,
		fear = 3,
		impact = 4,
		pain = 5,
		terror = 6,
		terrorLeft = 7,
		terrorRight = 8
	}

	private static Animation charAnim;

	private static Anim lastTriggeredAnim;

	public static void SetAnimationReference(Animation anim)
	{
		charAnim = anim;
		lastTriggeredAnim = Anim.none;
	}

	public static void Fear()
	{
		if (!(charAnim == null) && lastTriggeredAnim != Anim.fear)
		{
			lastTriggeredAnim = Anim.fear;
			charAnim.wrapMode = WrapMode.Loop;
			charAnim.CrossFade("Fear");
		}
	}

	public static void Terror()
	{
		if (!(charAnim == null) && lastTriggeredAnim != Anim.terror)
		{
			lastTriggeredAnim = Anim.terror;
			charAnim.wrapMode = WrapMode.Loop;
			charAnim.CrossFade("Terror");
		}
	}
}
