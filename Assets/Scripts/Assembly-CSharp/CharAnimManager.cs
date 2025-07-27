using UnityEngine;

public class CharAnimManager
{
	public enum Anim
	{
		none = 0,
		bellyDrag = 1,
		bellyDragStart = 2,
		bellyDragEnd = 3,
		bounce = 4,
		bounceEnd = 5,
		bounceEndFast = 6,
		doubleJump = 7,
		dramaticJump1 = 8,
		dramaticJump2 = 9,
		glideLoop = 10,
		jump = 11,
		duck = 12,
		run = 13,
		sprint1 = 14,
		sprint2 = 15,
		stagger = 16,
		wallRunLeft = 17,
		wallRunRight = 18,
		roll = 19,
		climb = 20,
		climbEnd = 21,
		balance = 22,
		balanceLeft = 23,
		balanceRight = 24,
		dive = 25,
		diveLoop = 26,
		trip = 27,
		tripEnd = 28,
		surf = 29,
		burnt = 30,
		burnEnd = 31,
		stairJump = 32,
		wallGrab = 33,
		wallJumpLeft = 34,
		wallJumpRight = 35,
		wallBounceLeft = 36,
		wallBounceRight = 37,
		wallGrabLeft = 38,
		wallGrabRight = 39,
		freeze = 40,
		hurtFootLeft = 41,
		hurtFootRight = 42,
		spinLeft = 43,
		spinRight = 44,
		megaSprint = 45,
		jetpack = 46,
		jetpackSprint = 47,
		dramaticFalling = 48,
		dramaticFallingStart = 49,
		surfLeft = 50,
		surfRight = 51,
		chickenFlap = 52,
		respawn = 53,
		flyBoost = 54,
		flyBoostLoop = 55
	}

	private static Animation charAnim;

	private static Anim lastTriggeredAnim;

	public static void SetAnimationReference(Animation anim)
	{
		charAnim = anim;
		lastTriggeredAnim = Anim.none;
	}

	public static void StopAll()
	{
		charAnim.Stop();
	}

	public static void Run()
	{
		if (lastTriggeredAnim != Anim.run)
		{
			lastTriggeredAnim = Anim.run;
			charAnim.wrapMode = WrapMode.Loop;
			charAnim.CrossFade("Run");
		}
	}

	public static void ForceRun()
	{
		lastTriggeredAnim = Anim.run;
		charAnim.wrapMode = WrapMode.Loop;
		charAnim.CrossFade("Run");
	}

	public static void RunAndSprint(float t, float animSpeed)
	{
		if (lastTriggeredAnim != Anim.run)
		{
			lastTriggeredAnim = Anim.run;
			charAnim.Stop();
		}
		charAnim.wrapMode = WrapMode.Loop;
		charAnim.Blend("Run", 1f - t);
		charAnim.Blend("Sprint", t);
		charAnim["Run"].speed = animSpeed;
		charAnim["Sprint"].speed = animSpeed;
	}

	public static void SuperSprint()
	{
		if (lastTriggeredAnim != Anim.sprint2)
		{
			lastTriggeredAnim = Anim.sprint2;
			charAnim.wrapMode = WrapMode.Loop;
			charAnim.CrossFade("Sprint2");
		}
	}

	public static void MegaSprint()
	{
		if (lastTriggeredAnim != Anim.megaSprint)
		{
			lastTriggeredAnim = Anim.megaSprint;
			charAnim.wrapMode = WrapMode.Loop;
			charAnim.CrossFade("SuperSprint");
		}
	}

	public static void SuperSprintToRun()
	{
		lastTriggeredAnim = Anim.run;
		charAnim.CrossFade("Sprint");
	}

	public static void Jump()
	{
		if (lastTriggeredAnim != Anim.dramaticJump1)
		{
			lastTriggeredAnim = Anim.dramaticJump1;
			charAnim.wrapMode = WrapMode.Loop;
			charAnim.Play("DramaticJump1");
		}
	}

	public static void DoubleJump()
	{
		lastTriggeredAnim = Anim.doubleJump;
		charAnim.wrapMode = WrapMode.Once;
		charAnim.Play("DoubleJump");
		charAnim.PlayQueued("DramaticJump1");
	}

	public static float GetDoubleJumpLength()
	{
		return charAnim["DoubleJump"].length;
	}

	public static void RunningOnLeftWall()
	{
		if (lastTriggeredAnim != Anim.wallRunLeft)
		{
			lastTriggeredAnim = Anim.wallRunLeft;
			charAnim.wrapMode = WrapMode.Loop;
			charAnim.Play("WallRunLeft");
		}
	}

	public static void RunningOnRightWall()
	{
		if (lastTriggeredAnim != Anim.wallRunRight)
		{
			lastTriggeredAnim = Anim.wallRunRight;
			charAnim.wrapMode = WrapMode.Loop;
			charAnim.Play("WallRunRight");
		}
	}

	public static void ChangeAnimationSpeed(float val)
	{
		foreach (AnimationState item in charAnim)
		{
			item.speed = val;
		}
	}

	public static void HitAndContinue()
	{
		if (lastTriggeredAnim != Anim.stagger)
		{
			lastTriggeredAnim = Anim.stagger;
			charAnim.wrapMode = WrapMode.Loop;
			charAnim.Play("Crawl");
		}
	}

	public static float GetHitAndContinueLength()
	{
		return charAnim["Crawl"].length;
	}

	public static void GlideLoop()
	{
		if (lastTriggeredAnim != Anim.glideLoop)
		{
			lastTriggeredAnim = Anim.glideLoop;
			charAnim.wrapMode = WrapMode.Loop;
			charAnim.CrossFade("GlideLoop");
		}
	}

	public static void Duck()
	{
		if (lastTriggeredAnim != Anim.duck)
		{
			lastTriggeredAnim = Anim.duck;
			charAnim.wrapMode = WrapMode.Loop;
		}
	}

	public static void Bounce()
	{
		if (lastTriggeredAnim != Anim.bounce)
		{
			lastTriggeredAnim = Anim.bounce;
			charAnim.wrapMode = WrapMode.Once;
			charAnim.Play("Bounce");
		}
	}

	public static void BounceEnd()
	{
		if (lastTriggeredAnim != Anim.bounceEnd)
		{
			lastTriggeredAnim = Anim.bounceEnd;
			charAnim.wrapMode = WrapMode.Once;
			charAnim.Play("BounceEnd");
		}
	}

	public static float GetBounceEndLength()
	{
		return charAnim["BounceEnd"].length;
	}

	public static void BounceEndFast()
	{
		if (lastTriggeredAnim != Anim.bounceEndFast)
		{
			lastTriggeredAnim = Anim.bounceEndFast;
			charAnim.wrapMode = WrapMode.Once;
			charAnim.Play("BounceEndFast");
		}
	}

	public static float GetBounceEndFastLength()
	{
		return charAnim["BounceEndFast"].length;
	}

	public static void Roll()
	{
		if (lastTriggeredAnim != Anim.roll)
		{
			lastTriggeredAnim = Anim.roll;
			charAnim.wrapMode = WrapMode.Once;
			charAnim.Play("Roll");
		}
	}

	public static float GetRollLength()
	{
		return charAnim["Roll"].length;
	}

	public static void DramaticJump()
	{
		if (lastTriggeredAnim == Anim.dramaticJump1)
		{
			lastTriggeredAnim = Anim.dramaticJump2;
			charAnim.wrapMode = WrapMode.Loop;
			charAnim.CrossFade("DramaticJump2");
		}
		else if (lastTriggeredAnim != Anim.dramaticJump2)
		{
			lastTriggeredAnim = Anim.dramaticJump2;
			charAnim.wrapMode = WrapMode.Loop;
			charAnim.Play("DramaticJump2");
		}
	}

	public static void BlendToDramaticJump()
	{
		if (lastTriggeredAnim != Anim.dramaticJump2)
		{
			lastTriggeredAnim = Anim.dramaticJump2;
			charAnim.wrapMode = WrapMode.Loop;
			charAnim.CrossFade("DramaticJump2");
		}
	}

	public static void Climb()
	{
		if (lastTriggeredAnim != Anim.climb)
		{
			lastTriggeredAnim = Anim.climb;
			charAnim.wrapMode = WrapMode.Loop;
			charAnim.Play("Climb");
		}
	}

	public static void ClimbEnd()
	{
		if (lastTriggeredAnim != Anim.climbEnd)
		{
			lastTriggeredAnim = Anim.climbEnd;
			charAnim.wrapMode = WrapMode.Once;
			charAnim.Play("ClimbEnd");
		}
	}

	public static float GetClimbEndLength()
	{
		return charAnim["ClimbEnd"].length;
	}

	public static void Balance()
	{
		if (lastTriggeredAnim != Anim.balance)
		{
			lastTriggeredAnim = Anim.balance;
			charAnim.wrapMode = WrapMode.Loop;
			charAnim.Play("Balance");
		}
	}

	public static void BalanceLeft()
	{
		if (lastTriggeredAnim != Anim.balanceLeft)
		{
			lastTriggeredAnim = Anim.balanceLeft;
			charAnim.wrapMode = WrapMode.Loop;
			charAnim.Play("BalanceLeft");
		}
	}

	public static void BalanceRight()
	{
		if (lastTriggeredAnim != Anim.balanceRight)
		{
			lastTriggeredAnim = Anim.balanceRight;
			charAnim.wrapMode = WrapMode.Loop;
			charAnim.Play("BalanceRight");
		}
	}

	public static void DramaticFallingStart()
	{
		if (lastTriggeredAnim != Anim.dramaticFallingStart)
		{
			lastTriggeredAnim = Anim.dramaticFallingStart;
			charAnim.wrapMode = WrapMode.Once;
			charAnim.Play("DramaticFallingStart");
		}
	}

	public static float GetDramaticFallingStartLength()
	{
		return charAnim["DramaticFallingStart"].length;
	}

	public static void DramaticFalling()
	{
		if (lastTriggeredAnim != Anim.dramaticFalling)
		{
			lastTriggeredAnim = Anim.dramaticFalling;
			charAnim.wrapMode = WrapMode.Loop;
			charAnim.Play("DramaticFalling");
		}
	}

	public static void Dive()
	{
		if (lastTriggeredAnim != Anim.dive)
		{
			lastTriggeredAnim = Anim.dive;
			charAnim.wrapMode = WrapMode.Once;
			charAnim.Play("Dive");
		}
	}

	public static void DiveLoop()
	{
		if (lastTriggeredAnim != Anim.diveLoop)
		{
			lastTriggeredAnim = Anim.diveLoop;
			charAnim.wrapMode = WrapMode.Loop;
			charAnim.Play("DiveLoop");
		}
	}

	public static float GetDiveLength()
	{
		return charAnim["Dive"].length;
	}

	public static void Trip()
	{
		if (lastTriggeredAnim != Anim.trip)
		{
			lastTriggeredAnim = Anim.trip;
			charAnim.wrapMode = WrapMode.Once;
			charAnim.Play("TripStart");
		}
	}

	public static float GetTripLength()
	{
		return charAnim["TripStart"].length;
	}

	public static void TripEnd()
	{
		if (lastTriggeredAnim != Anim.tripEnd)
		{
			lastTriggeredAnim = Anim.tripEnd;
			charAnim.wrapMode = WrapMode.Once;
			charAnim.Play("TripEnd");
		}
	}

	public static float GetTripEndLength()
	{
		return charAnim["TripEnd2"].length;
	}

	public static void Surf()
	{
		if (lastTriggeredAnim != Anim.surf)
		{
			lastTriggeredAnim = Anim.surf;
			charAnim.wrapMode = WrapMode.Loop;
			charAnim.Play("Surf");
		}
	}

	public static void SurfLeft()
	{
		if (lastTriggeredAnim != Anim.surfLeft)
		{
			lastTriggeredAnim = Anim.surfLeft;
			charAnim.wrapMode = WrapMode.Loop;
			charAnim.Play("SurfLeft");
		}
	}

	public static void SurfRight()
	{
		if (lastTriggeredAnim != Anim.surfRight)
		{
			lastTriggeredAnim = Anim.surfRight;
			charAnim.wrapMode = WrapMode.Loop;
			charAnim.Play("SurfRight");
		}
	}

	public static void Burnt()
	{
		if (lastTriggeredAnim != Anim.burnt)
		{
			lastTriggeredAnim = Anim.burnt;
			charAnim.wrapMode = WrapMode.Loop;
			charAnim.Play("Burnt");
		}
	}

	public static void BurnEnd()
	{
		if (lastTriggeredAnim != Anim.burnEnd)
		{
			lastTriggeredAnim = Anim.burnEnd;
			charAnim.wrapMode = WrapMode.Once;
			charAnim.Blend("BurnEnd");
		}
	}

	public static float GetBurnEndLength()
	{
		return charAnim["BurnEnd"].length;
	}

	public static void StairJump()
	{
		if (lastTriggeredAnim != Anim.stairJump)
		{
			lastTriggeredAnim = Anim.stairJump;
			charAnim.wrapMode = WrapMode.Once;
			charAnim.CrossFade("Jump");
		}
	}

	public static void StairsGrab()
	{
		if (lastTriggeredAnim != Anim.wallGrab)
		{
			lastTriggeredAnim = Anim.wallGrab;
			charAnim.wrapMode = WrapMode.Once;
			charAnim.CrossFade("WallGrab");
		}
	}

	public static float GetStairsGrabLength()
	{
		return charAnim["WallGrab"].length;
	}

	public static void WallJumpLeft()
	{
		if (lastTriggeredAnim != Anim.wallJumpLeft)
		{
			lastTriggeredAnim = Anim.wallJumpLeft;
			charAnim.wrapMode = WrapMode.Loop;
			charAnim.CrossFade("WallRunLeft");
		}
	}

	public static float GetWallJumpLeftLength()
	{
		return charAnim["WallRunLeft"].length;
	}

	public static void WallJumpRight()
	{
		if (lastTriggeredAnim != Anim.wallJumpRight)
		{
			lastTriggeredAnim = Anim.wallJumpRight;
			charAnim.wrapMode = WrapMode.Loop;
			charAnim.CrossFade("WallRunRight");
		}
	}

	public static void WallBounceLeft()
	{
		if (lastTriggeredAnim != Anim.wallBounceLeft)
		{
			lastTriggeredAnim = Anim.wallBounceLeft;
			charAnim.wrapMode = WrapMode.Once;
			charAnim.Play("WallBounceLeft");
		}
	}

	public static float GetWallBounceLeftLength()
	{
		return charAnim["WallBounceLeft"].length;
	}

	public static void WallBounceRight()
	{
		if (lastTriggeredAnim != Anim.wallBounceRight)
		{
			lastTriggeredAnim = Anim.wallBounceRight;
			charAnim.wrapMode = WrapMode.Once;
			charAnim.Play("WallBounceRight");
		}
	}

	public static void Freeze()
	{
		if (lastTriggeredAnim != Anim.freeze)
		{
			lastTriggeredAnim = Anim.freeze;
			charAnim.wrapMode = WrapMode.Once;
			charAnim.Play("Freeze");
		}
	}

	public static float GetFreezeLength()
	{
		return charAnim["Freeze"].length;
	}

	public static void StairsGrabLeft()
	{
		if (lastTriggeredAnim != Anim.wallBounceLeft)
		{
			lastTriggeredAnim = Anim.wallBounceLeft;
			charAnim.wrapMode = WrapMode.Once;
			charAnim.Play("WallBounceLeft");
		}
	}

	public static void WallGrabLeft()
	{
		if (lastTriggeredAnim != Anim.wallGrabLeft)
		{
			lastTriggeredAnim = Anim.wallGrabLeft;
			charAnim.wrapMode = WrapMode.Once;
			charAnim.Play("WallGrabLeft");
		}
	}

	public static void WallGrabRight()
	{
		if (lastTriggeredAnim != Anim.wallGrabRight)
		{
			lastTriggeredAnim = Anim.wallGrabRight;
			charAnim.wrapMode = WrapMode.Once;
			charAnim.Play("WallGrabRight");
		}
	}

	public static void HurtFootLeft()
	{
		if (lastTriggeredAnim == Anim.hurtFootRight)
		{
			lastTriggeredAnim = Anim.hurtFootLeft;
			charAnim.wrapMode = WrapMode.Loop;
			charAnim.CrossFade("HurtFootLeft");
		}
		else if (lastTriggeredAnim != Anim.hurtFootLeft)
		{
			lastTriggeredAnim = Anim.hurtFootLeft;
			charAnim.wrapMode = WrapMode.Loop;
			charAnim.Play("HurtFootLeft");
		}
	}

	public static void HurtFootRight()
	{
		if (lastTriggeredAnim == Anim.hurtFootLeft)
		{
			lastTriggeredAnim = Anim.hurtFootRight;
			charAnim.wrapMode = WrapMode.Loop;
			charAnim.CrossFade("HurtFootRight");
		}
		else if (lastTriggeredAnim != Anim.hurtFootRight)
		{
			lastTriggeredAnim = Anim.hurtFootRight;
			charAnim.wrapMode = WrapMode.Loop;
			charAnim.Play("HurtFootRight");
		}
	}

	public static void SpinLeft()
	{
		if (lastTriggeredAnim != Anim.spinLeft)
		{
			lastTriggeredAnim = Anim.spinLeft;
			charAnim.wrapMode = WrapMode.Once;
			charAnim.Play("SpinLeft");
		}
	}

	public static float GetSpinLeftLength()
	{
		return charAnim["SpinLeft"].length;
	}

	public static void SpinRight()
	{
		if (lastTriggeredAnim != Anim.spinRight)
		{
			lastTriggeredAnim = Anim.spinRight;
			charAnim.wrapMode = WrapMode.Once;
			charAnim.Play("SpinRight");
		}
	}

	public static void Jetpack()
	{
		if (lastTriggeredAnim != Anim.jetpack)
		{
			lastTriggeredAnim = Anim.jetpack;
			charAnim.wrapMode = WrapMode.Loop;
			charAnim.CrossFade("JetPack");
		}
	}

	public static void JetpackSprint()
	{
		if (lastTriggeredAnim != Anim.jetpackSprint)
		{
			lastTriggeredAnim = Anim.jetpackSprint;
			charAnim.wrapMode = WrapMode.Loop;
			charAnim.CrossFade("JetPackSprint");
		}
	}

	public static void ChickenFlap()
	{
		lastTriggeredAnim = Anim.chickenFlap;
		charAnim.wrapMode = WrapMode.Once;
		charAnim.Play("Flap");
		charAnim.PlayQueued("DramaticJump1");
	}

	public static void Respawn()
	{
		if (lastTriggeredAnim != Anim.respawn)
		{
			lastTriggeredAnim = Anim.respawn;
			charAnim.wrapMode = WrapMode.Once;
			charAnim.Play("Respawn");
		}
	}

	public static float GetRespawnLength()
	{
		return charAnim["Respawn"].length;
	}

	public static void FlyBoost()
	{
		if (lastTriggeredAnim != Anim.flyBoost)
		{
			lastTriggeredAnim = Anim.flyBoost;
			charAnim.wrapMode = WrapMode.Once;
			charAnim.Play("FlyBoost");
		}
	}

	public static void FlyBoostLoop()
	{
		if (lastTriggeredAnim != Anim.flyBoostLoop)
		{
			lastTriggeredAnim = Anim.flyBoostLoop;
			charAnim.wrapMode = WrapMode.Loop;
			charAnim.Play("FlyBoostLoop");
		}
	}

	public static void Drag()
	{
		if (lastTriggeredAnim != Anim.bellyDrag)
		{
			lastTriggeredAnim = Anim.bellyDrag;
			charAnim.wrapMode = WrapMode.Loop;
			charAnim.Play("BellyDrag");
		}
	}
}
