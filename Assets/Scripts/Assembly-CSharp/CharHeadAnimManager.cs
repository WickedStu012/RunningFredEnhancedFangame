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

	private static bool IsDyingOrDead()
	{
		// Check if the head is currently playing dying or dead animation
		if (charAnim != null && charAnim.isPlaying)
		{
			string currentClip = charAnim.clip != null ? charAnim.clip.name : "";
			if (currentClip == "Dying" || currentClip == "Dead")
			{
				return true;
			}
		}
		
		// Also check with the CharHead script to see if it's in dying/dead state
		GameObject headGO = CharHeadHelper.GetHeadGameObject();
		if (headGO != null)
		{
			CharHead charHead = headGO.GetComponent<CharHead>();
			if (charHead != null)
			{
				return charHead.IsDyingOrDead();
			}
		}
		
		return false;
	}

	public static void Fear()
	{
		// Check if the head is in dying or dead state
		if (IsDyingOrDead())
		{
			// Don't override dying/dead animations
			Debug.Log("CharHeadAnimManager: Fear() blocked - head is in dying/dead state");
			return;
		}
		
		if (!(charAnim == null) && lastTriggeredAnim != Anim.fear)
		{
			Debug.Log("CharHeadAnimManager: Playing Fear animation");
			lastTriggeredAnim = Anim.fear;
			charAnim.wrapMode = WrapMode.Loop;
			charAnim.CrossFade("Fear");
		}
	}

	public static void Terror()
	{
		// Check if the head is in dying or dead state
		if (IsDyingOrDead())
		{
			// Don't override dying/dead animations
			Debug.Log("CharHeadAnimManager: Terror() blocked - head is in dying/dead state");
			return;
		}
		
		if (!(charAnim == null) && lastTriggeredAnim != Anim.terror)
		{
			Debug.Log("CharHeadAnimManager: Playing Terror animation");
			lastTriggeredAnim = Anim.terror;
			charAnim.wrapMode = WrapMode.Loop;
			charAnim.CrossFade("Terror");
		}
	}
}
