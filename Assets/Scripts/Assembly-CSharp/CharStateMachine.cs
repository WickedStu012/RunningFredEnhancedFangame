using UnityEngine;

public class CharStateMachine : MonoBehaviour
{
	private IAction currentAction;

	private ActionCode prevActionState;

	private IAction[] actions;

	private IEvent[] events;

	private CharacterController cc;

	private Transform blobT;

	private CharChangeState charChangeStateEvent = new CharChangeState();

	private GameObject jetpack;

	private GameObject wings;

	public bool characterPlaced;

	private float accumTimeInertia;

	private Vector3 prevPos;

	private GameObject jetpackMeterGO;

	private OnWingsDropped onWingsDropped = new OnWingsDropped();

	private OnWingsTaken onWingsTaken = new OnWingsTaken();

	private OnJetpackTaken onJetpackTaken = new OnJetpackTaken();

	private Transform playerTransform;

	public bool IsGrounded;

	public string groundTag;

	public float Speed;

	public float FloorNormalZ;

	public float FloorXAngle;

	public float FloorZAngle;

	public Vector3 MoveDirection;

	public float FloorYPos;

	public int ConsecutiveJumpCounter;

	public int ConsecutiveWallJumpCounter;

	public float SteerDirection;

	public float SteerDirectionUpDown;

	public float AccumAccel;

	public float Height = 2.1f;

	public float lastYPosition;

	public FloorType floorType;

	public bool IsGoingUp;

	public Vector3 inertia;

	public float GlideTimeLeft;

	public int SafetySpringUseCountInThisMatch;

	public int ProtectiveVestUseCountInThisMatch;

	public int AfterBurnerDisplayCount;

	private int groundUpdateCounter;

	private float duration = 1.299f;

	private float accumTimeSteps = 2f;

	private int channel = -1;

	private int lastChannel = -1;

	private IEvent[][][] evnMatrix = new IEvent[38][][]
	{
		new IEvent[47][]
		{
			null,
			new IEvent[1]
			{
				new EvnJump()
			},
			new IEvent[1]
			{
				new EvnSuperSprint()
			},
			null,
			null,
			new IEvent[1]
			{
				new EvnWallRunPlatform()
			},
			null,
			null,
			null,
			null,
			new IEvent[1]
			{
				new EvnBounce()
			},
			new IEvent[1]
			{
				new EvnTouchGroundToRoll()
			},
			new IEvent[1]
			{
				new EvnDramaticJump()
			},
			new IEvent[1]
			{
				new EvnClimb()
			},
			null,
			new IEvent[1]
			{
				new EvnBalance()
			},
			null,
			null,
			new IEvent[1]
			{
				new EvnSurf()
			},
			new IEvent[1]
			{
				new EvnSurfSide()
			},
			null,
			null,
			null,
			null,
			null,
			new IEvent[1]
			{
				new EvnJetpack()
			},
			null,
			null,
			null,
			new IEvent[1]
			{
				new EvnFlyAuto()
			},
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			new IEvent[1]
			{
				new EvnTouchGroundToExplode()
			},
			new IEvent[1]
			{
				new EvnTouchGroundToDie()
			},
			null,
			null,
			null,
			null,
			null,
			null
		},
		new IEvent[47][]
		{
			new IEvent[1]
			{
				new EvnTouchGround()
			},
			null,
			null,
			null,
			new IEvent[1]
			{
				new EvnDJump()
			},
			new IEvent[1]
			{
				new EvnWallRunPlatform()
			},
			null,
			new IEvent[1]
			{
				new EvnWallJump()
			},
			null,
			null,
			new IEvent[1]
			{
				new EvnBounce()
			},
			null,
			new IEvent[1]
			{
				new EvnDramaticJump()
			},
			new IEvent[1]
			{
				new EvnClimb()
			},
			null,
			null,
			new IEvent[1]
			{
				new EvnDive()
			},
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			new IEvent[1]
			{
				new EvnJetpack()
			},
			null,
			null,
			null,
			new IEvent[1]
			{
				new EvnFlyAuto()
			},
			null,
			null,
			null,
			null,
			null,
			null,
			new IEvent[1]
			{
				new EvnSuperSprint()
			},
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null
		},
		new IEvent[47][]
		{
			null,
			new IEvent[1]
			{
				new EvnJump()
			},
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			new IEvent[1]
			{
				new EvnDramaticJump()
			},
			new IEvent[1]
			{
				new EvnClimb()
			},
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			new IEvent[1]
			{
				new EvnJetpack()
			},
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null
		},
		new IEvent[47][]
		{
			new IEvent[2]
			{
				new EvnDragTimeout(),
				new EvnDragOut()
			},
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			new IEvent[1]
			{
				new EvnClimb()
			},
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null
		},
		new IEvent[47][]
		{
			new IEvent[1]
			{
				new EvnTouchGround()
			},
			null,
			null,
			null,
			new IEvent[1]
			{
				new EvnDJump()
			},
			null,
			null,
			new IEvent[1]
			{
				new EvnWallJump()
			},
			null,
			null,
			new IEvent[1]
			{
				new EvnBounce()
			},
			null,
			new IEvent[1]
			{
				new EvnDramaticJump()
			},
			new IEvent[1]
			{
				new EvnClimb()
			},
			null,
			null,
			new IEvent[1]
			{
				new EvnDive()
			},
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			new IEvent[1]
			{
				new EvnJetpack()
			},
			null,
			null,
			null,
			new IEvent[1]
			{
				new EvnFlyAuto()
			},
			null,
			null,
			null,
			null,
			null,
			null,
			new IEvent[1]
			{
				new EvnSuperSprint()
			},
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null
		},
		new IEvent[47][]
		{
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			new IEvent[1]
			{
				new EvnWallRunPlatformOff()
			},
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			new IEvent[1]
			{
				new EvnJump()
			},
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			new IEvent[1]
			{
				new EvnSuperSprint()
			},
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null
		},
		new IEvent[47][]
		{
			null,
			new IEvent[1]
			{
				new EvnJump()
			},
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null
		},
		new IEvent[47][]
		{
			new IEvent[1]
			{
				new EvnTouchGround()
			},
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			new IEvent[1]
			{
				new EvnBounce()
			},
			null,
			new IEvent[1]
			{
				new EvnDramaticJump()
			},
			new IEvent[1]
			{
				new EvnClimb()
			},
			null,
			null,
			new IEvent[1]
			{
				new EvnDive()
			},
			null,
			null,
			null,
			null,
			null,
			null,
			new IEvent[1]
			{
				new EvnSideJump()
			},
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			new IEvent[1]
			{
				new EvnSuperSprint()
			},
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null
		},
		new IEvent[47][]
		{
			new IEvent[1]
			{
				new EvnTouchGround()
			},
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			new IEvent[1]
			{
				new EvnTouchGroundToRoll()
			},
			new IEvent[1]
			{
				new EvnGlideOff()
			},
			new IEvent[1]
			{
				new EvnClimb()
			},
			null,
			null,
			new IEvent[1]
			{
				new EvnDive()
			},
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			new IEvent[1]
			{
				new EvnJetpack()
			},
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			new IEvent[1]
			{
				new EvnTouchGroundToExplode()
			},
			new IEvent[1]
			{
				new EvnTouchGroundToDie()
			},
			null,
			null,
			null,
			null,
			null,
			null
		},
		new IEvent[47][]
		{
			null,
			new IEvent[1]
			{
				new EvnJump()
			},
			null,
			new IEvent[1]
			{
				new EvnDrag()
			},
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			new IEvent[1]
			{
				new EvnNotTouchGround()
			},
			new IEvent[1]
			{
				new EvnClimb()
			},
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			new IEvent[1]
			{
				new EvnJetpack()
			},
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null
		},
		new IEvent[47][]
		{
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			new IEvent[1]
			{
				new EvnTouchGroundToExplode()
			},
			new IEvent[1]
			{
				new EvnTouchGroundToDie()
			},
			null,
			null,
			null,
			null,
			null,
			null
		},
		new IEvent[46][],
		new IEvent[47][]
		{
			new IEvent[1]
			{
				new EvnTouchGround()
			},
			null,
			null,
			null,
			null,
			new IEvent[1]
			{
				new EvnWallRunPlatform()
			},
			null,
			new IEvent[1]
			{
				new EvnWallJump()
			},
			new IEvent[1]
			{
				new EvnGlide()
			},
			null,
			null,
			new IEvent[1]
			{
				new EvnTouchGroundToRoll()
			},
			null,
			new IEvent[1]
			{
				new EvnClimb()
			},
			null,
			null,
			new IEvent[1]
			{
				new EvnDive()
			},
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			new IEvent[1]
			{
				new EvnJetpackImmediateWallJump()
			},
			null,
			null,
			null,
			new IEvent[1]
			{
				new EvnFlyAuto()
			},
			null,
			null,
			null,
			null,
			null,
			null,
			new IEvent[1]
			{
				new EvnSuperSprint()
			},
			null,
			null,
			new IEvent[1]
			{
				new EvnTouchGroundToExplode()
			},
			new IEvent[1]
			{
				new EvnTouchGroundToDie()
			},
			null,
			null,
			null,
			null,
			null,
			null
		},
		new IEvent[47][]
		{
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			new IEvent[1]
			{
				new EvnClimbOff()
			},
			null,
			null,
			null,
			null,
			null,
			new IEvent[1]
			{
				new EvnClimbJump()
			},
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null
		},
		new IEvent[47][]
		{
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			new IEvent[1]
			{
				new EvnClimbJump()
			},
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null
		},
		new IEvent[47][]
		{
			new IEvent[1]
			{
				new EvnBalanceEnd()
			},
			new IEvent[1]
			{
				new EvnJump()
			},
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null
		},
		new IEvent[47][]
		{
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			new IEvent[2]
			{
				new EvnTouchGround(),
				new EvnTouchGroundToRoll()
			},
			null,
			new IEvent[1]
			{
				new EvnClimb()
			},
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			new IEvent[1]
			{
				new EvnFlyAuto()
			},
			null,
			null,
			new IEvent[1]
			{
				new EvnTouchGroundToTrip()
			},
			null,
			null,
			null,
			null,
			null,
			null,
			new IEvent[1]
			{
				new EvnTouchGroundToExplode()
			},
			new IEvent[1]
			{
				new EvnTouchGroundToDie()
			},
			null,
			null,
			null,
			null,
			null,
			null
		},
		new IEvent[46][]
		{
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			new IEvent[1]
			{
				new EvnTouchGroundToExplode()
			},
			new IEvent[1]
			{
				new EvnTouchGroundToDie()
			},
			null,
			null,
			null,
			null,
			null
		},
		new IEvent[47][]
		{
			new IEvent[1]
			{
				new EvnSurfOff()
			},
			new IEvent[1]
			{
				new EvnJump()
			},
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			new IEvent[1]
			{
				new EvnDramaticJump()
			},
			new IEvent[1]
			{
				new EvnClimb()
			},
			null,
			new IEvent[1]
			{
				new EvnBalance()
			},
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			new IEvent[1]
			{
				new EvnTouchGroundToDie()
			},
			null,
			null,
			null,
			null,
			null,
			null
		},
		new IEvent[47][]
		{
			new IEvent[1]
			{
				new EvnSurfSideOff()
			},
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			new IEvent[1]
			{
				new EvnDramaticJump()
			},
			new IEvent[1]
			{
				new EvnClimb()
			},
			null,
			new IEvent[1]
			{
				new EvnBalance()
			},
			null,
			null,
			null,
			null,
			null,
			new IEvent[1]
			{
				new EvnJump()
			},
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			new IEvent[1]
			{
				new EvnTouchGroundToDie()
			},
			null,
			null,
			null,
			null,
			null,
			null
		},
		new IEvent[47][]
		{
			new IEvent[1]
			{
				new EvnTouchGround()
			},
			null,
			null,
			null,
			new IEvent[1]
			{
				new EvnDJump()
			},
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			new IEvent[1]
			{
				new EvnDramaticJump()
			},
			new IEvent[1]
			{
				new EvnClimb()
			},
			null,
			null,
			new IEvent[1]
			{
				new EvnDive()
			},
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null
		},
		new IEvent[47][]
		{
			new IEvent[1]
			{
				new EvnTouchGround()
			},
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			new IEvent[1]
			{
				new EvnDramaticJump()
			},
			new IEvent[1]
			{
				new EvnClimb()
			},
			null,
			null,
			new IEvent[1]
			{
				new EvnDive()
			},
			null,
			null,
			null,
			null,
			null,
			null,
			new IEvent[1]
			{
				new EvnDJump()
			},
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null
		},
		new IEvent[47][]
		{
			new IEvent[1]
			{
				new EvnCarltropTimeout()
			},
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			new IEvent[1]
			{
				new EvnDuck()
			},
			new IEvent[1]
			{
				new EvnBounce()
			},
			new IEvent[1]
			{
				new EvnTouchGroundToRoll()
			},
			new IEvent[1]
			{
				new EvnDramaticJump()
			},
			new IEvent[1]
			{
				new EvnClimb()
			},
			null,
			new IEvent[1]
			{
				new EvnBalance()
			},
			null,
			null,
			new IEvent[1]
			{
				new EvnSurf()
			},
			new IEvent[1]
			{
				new EvnSurfSide()
			},
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			new IEvent[1]
			{
				new EvnTouchGroundToExplode()
			},
			new IEvent[1]
			{
				new EvnTouchGroundToDie()
			},
			null,
			null,
			null,
			null,
			null,
			null
		},
		new IEvent[47][]
		{
			new IEvent[1]
			{
				new EvnTouchGround()
			},
			null,
			null,
			null,
			new IEvent[1]
			{
				new EvnDJump()
			},
			null,
			null,
			new IEvent[1]
			{
				new EvnWallJump()
			},
			null,
			null,
			new IEvent[1]
			{
				new EvnBounce()
			},
			null,
			new IEvent[1]
			{
				new EvnDramaticJump()
			},
			new IEvent[1]
			{
				new EvnClimb()
			},
			null,
			null,
			new IEvent[1]
			{
				new EvnDive()
			},
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			new IEvent[1]
			{
				new EvnJetpack()
			},
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			new IEvent[1]
			{
				new EvnSuperSprint()
			},
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null
		},
		new IEvent[47][]
		{
			null,
			new IEvent[1]
			{
				new EvnJump()
			},
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			new IEvent[1]
			{
				new EvnDramaticJump()
			},
			new IEvent[1]
			{
				new EvnClimb()
			},
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null
		},
		new IEvent[47][]
		{
			null,
			null,
			null,
			null,
			null,
			new IEvent[1]
			{
				new EvnWallRunPlatform()
			},
			null,
			new IEvent[1]
			{
				new EvnWallJump()
			},
			null,
			null,
			null,
			null,
			new IEvent[1]
			{
				new EvnJetpackOff()
			},
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			new IEvent[1]
			{
				new EvnJetpackSprint()
			},
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null
		},
		new IEvent[47][]
		{
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			new IEvent[1]
			{
				new EvnJetpackOff()
			},
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null
		},
		new IEvent[47][]
		{
			new IEvent[1]
			{
				new EvnTouchGround()
			},
			null,
			null,
			null,
			new IEvent[1]
			{
				new EvnDJump()
			},
			new IEvent[1]
			{
				new EvnWallRunPlatform()
			},
			null,
			null,
			null,
			null,
			new IEvent[1]
			{
				new EvnBounce()
			},
			null,
			new IEvent[1]
			{
				new EvnDramaticJump()
			},
			new IEvent[1]
			{
				new EvnClimb()
			},
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			new IEvent[1]
			{
				new EvnJetpack()
			},
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null
		},
		new IEvent[47][],
		new IEvent[47][]
		{
			new IEvent[1]
			{
				new EvnTouchGround()
			},
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			new IEvent[1]
			{
				new EvnTouchGroundToRoll()
			},
			null,
			new IEvent[1]
			{
				new EvnClimb()
			},
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			new IEvent[1]
			{
				new EvnSuperSprint()
			},
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null
		},
		new IEvent[47][]
		{
			new IEvent[1]
			{
				new EvnTouchGround()
			},
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			new IEvent[1]
			{
				new EvnTouchGroundToRoll()
			},
			null,
			new IEvent[1]
			{
				new EvnClimb()
			},
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null
		},
		new IEvent[47][]
		{
			new IEvent[1]
			{
				new EvnTouchGround()
			},
			null,
			null,
			null,
			null,
			null,
			null,
			new IEvent[1]
			{
				new EvnWallJump()
			},
			null,
			null,
			new IEvent[1]
			{
				new EvnBounce()
			},
			null,
			new IEvent[1]
			{
				new EvnDramaticJump()
			},
			new IEvent[1]
			{
				new EvnClimb()
			},
			null,
			null,
			new IEvent[1]
			{
				new EvnDive()
			},
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			new IEvent[1]
			{
				new EvnJetpack()
			},
			null,
			null,
			null,
			new IEvent[1]
			{
				new EvnFlyAuto()
			},
			null,
			new IEvent[1]
			{
				new EvnDJump()
			},
			null,
			null,
			null,
			null,
			new IEvent[1]
			{
				new EvnSuperSprint()
			},
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null
		},
		new IEvent[47][],
		new IEvent[47][],
		new IEvent[47][]
		{
			new IEvent[1]
			{
				new EvnTouchGroundFromSafetySpring()
			},
			null,
			null,
			null,
			null,
			null,
			null,
			new IEvent[1]
			{
				new EvnWallJump()
			},
			null,
			null,
			null,
			null,
			null,
			new IEvent[1]
			{
				new EvnClimb()
			},
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null
		},
		new IEvent[47][]
		{
			new IEvent[1]
			{
				new EvnTouchGroundFromSafetySpring()
			},
			null,
			null,
			null,
			null,
			null,
			null,
			new IEvent[1]
			{
				new EvnWallJump()
			},
			null,
			null,
			null,
			null,
			null,
			new IEvent[1]
			{
				new EvnClimb()
			},
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null
		},
		new IEvent[47][]
		{
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			new IEvent[2]
			{
				new EvnTouchGround(),
				new EvnTouchGroundToRoll()
			},
			new IEvent[1]
			{
				new EvnDramaticJump()
			},
			new IEvent[1]
			{
				new EvnClimb()
			},
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			new IEvent[1]
			{
				new EvnFlyAuto()
			},
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null
		},
		new IEvent[47][]
		{
			new IEvent[1]
			{
				new EvnTouchGround()
			},
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			new IEvent[1]
			{
				new EvnTouchGroundToRoll()
			},
			null,
			new IEvent[1]
			{
				new EvnClimb()
			},
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null
		}
	};

	public Transform playerT
	{
		get
		{
			if (playerTransform == null)
			{
				playerTransform = base.gameObject.transform;
			}
			return playerTransform;
		}
	}

	private void Awake()
	{
		playerTransform = base.gameObject.transform;
		cc = GetComponent<CharacterController>();
		CharAnimManager.SetAnimationReference(base.gameObject.GetComponent<Animation>());
		AccumAccel = 0.7f;
	}

	private void Start()
	{
		actions = new IAction[47]
		{
			new ActRunning(base.gameObject),
			new ActJumping(base.gameObject),
			new ActSuperSprint(base.gameObject),
			new ActDrag(base.gameObject),
			new ActDoubleJumping(base.gameObject),
			new ActRunningOnWallPlatform(base.gameObject),
			new ActStagger(base.gameObject),
			new ActWallJump(base.gameObject),
			new ActGlide(base.gameObject),
			new ActDuck(base.gameObject),
			new ActBounce(base.gameObject),
			new ActRoll(base.gameObject),
			new ActDramaticJump(base.gameObject),
			new ActClimb(base.gameObject),
			new ActClimbEnd(base.gameObject),
			new ActBalance(base.gameObject),
			new ActDive(base.gameObject),
			new ActTrip(base.gameObject),
			new ActSurf(base.gameObject),
			new ActSurfSide(base.gameObject),
			new ActClimbJump(base.gameObject),
			new ActSurfSideJump(base.gameObject),
			new ActCarltrop(base.gameObject),
			new ActDoubleJumpSide(base.gameObject),
			new ActMegaSprint(base.gameObject),
			new ActJetpack(base.gameObject),
			new ActJetpackSprint(base.gameObject),
			new ActRunWallPlatformJump(base.gameObject),
			new ActBouncerJump(base.gameObject),
			new ActFly(base.gameObject),
			new ActCatapult(base.gameObject),
			new ActChickenFlap(base.gameObject),
			new ActFallToTrip(base.gameObject),
			new ActRespawn(base.gameObject),
			new ActSafetySpring(base.gameObject),
			new ActBounceSpring(base.gameObject),
			new ActAfterBurnerJump(base.gameObject),
			new ActAfterBurnerFly(base.gameObject),
			new ActRagdoll(base.gameObject),
			new ActExplode(base.gameObject),
			new ActDieOnImpact(base.gameObject),
			new ActBurnt(base.gameObject),
			new ActFrozen(base.gameObject),
			new ActExplodeOnWall(base.gameObject),
			new ActRunningToGoal(base.gameObject),
			new ActStopped(base.gameObject),
			new ActExplodeOnAir(base.gameObject)
		};
		initEvents();
		currentAction = actions[0];
		currentAction.GetIn();
		addShadow();
		CharBuilderHelper.RelocateSkeleton();
		if (PlayerAccount.Instance.CurrentAvatarInfo.Id == 32 && string.Compare(DedalordLoadLevel.GetLevel(), "TutorialLoader") != 0)
		{
			loadAndAttachJetpack();
		}
		if (string.Compare(DedalordLoadLevel.GetLevel(), "TutorialLoader") == 0)
		{
			CharHelper.GetProps().HasJetpack = false;
		}
	}

	private void OnGUI()
	{
		currentAction.OnGUI();
	}

	private void FixedUpdate()
	{
		if (characterPlaced)
		{
			updateInertia();
			currentAction.Update(Time.fixedDeltaTime);
			checkEvents();
		}
	}

	private void LateUpdate()
	{
		updateFlags();
	}

	public void ResetChar()
	{
		base.gameObject.SetActive(true);
		CharBuilderHelper.RebuildChar();
		CharHelper.GetEffects().DisableParticles();
		IceBlock iceBlock = IceManager.GetIceBlock();
		if (iceBlock != null)
		{
			iceBlock.gameObject.SetActive(false);
		}
		ToothPickSpike[] componentsInChildren = base.gameObject.GetComponentsInChildren<ToothPickSpike>();
		if (componentsInChildren != null)
		{
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].transform.parent = null;
				if (componentsInChildren[i].GetComponent<Renderer>() != null)
				{
					componentsInChildren[i].GetComponent<Renderer>().enabled = false;
				}
			}
		}
		ResetLastYPos();
		SafetySpringUseCountInThisMatch = 0;
		ProtectiveVestUseCountInThisMatch = 0;
		AfterBurnerDisplayCount = 0;
		if (jetpack != null)
		{
			Jetpack component = jetpack.GetComponent<Jetpack>();
			if (component != null)
			{
				component.DisableAll();
				if (PlayerAccount.Instance.CurrentGameMode == PlayerAccount.GameMode.Survival && !CharHelper.IsJetpackVisible())
				{
					ShowJetpack();
				}
			}
			if (JetpackMeter.Instance != null)
			{
				JetpackMeter.Instance.Reset();
			}
		}
		ExplosionManager.ResetExplosions();
		SwitchTo(ActionCode.RESPAWN);
	}

	public void SwitchTo(ActionCode toState, params object[] list)
	{
		if (actions == null)
		{
			Debug.Log("actions is null");
			return;
		}
		IAction action = actions[(int)toState];
		currentAction.GetOut();
		prevActionState = currentAction.GetState();
		currentAction = action;
		for (int i = 0; i < events.Length; i++)
		{
			events[i].StateChange();
		}
		currentAction.GetIn(list);
		charChangeStateEvent.CurrentState = currentAction;
		GameEventDispatcher.Dispatch(this, charChangeStateEvent);
	}

	public void SwitchTo(ActionCode toState)
	{
		SwitchTo(toState, null);
	}

	public bool CanSwitchTo(ActionCode toState)
	{
		return actions[(int)toState].CanGetIn();
	}

	public ActionCode GetCurrentState()
	{
		if (currentAction == null)
		{
			return ActionCode.STOPPED;
		}
		return currentAction.GetState();
	}

	public IAction GetCurrentAction()
	{
		return currentAction;
	}

	public ActionCode GetPrevActionState()
	{
		return prevActionState;
	}

	private void updateFlags()
	{
		Speed = cc.velocity.magnitude;
		updateFloorNormal();
	}

	private void initEvents()
	{
		int num = 0;
		for (int i = 0; i < evnMatrix.Length; i++)
		{
			for (int j = 0; j < evnMatrix[i].Length; j++)
			{
				if (evnMatrix[i][j] == null)
				{
					continue;
				}
				for (int k = 0; k < evnMatrix[i][j].Length; k++)
				{
					if (evnMatrix[i][j][k] != null)
					{
						num++;
						evnMatrix[i][j][k].SetStateMachineRef(this);
					}
				}
			}
		}
		events = new IEvent[num];
		num = 0;
		for (int l = 0; l < evnMatrix.Length; l++)
		{
			for (int m = 0; m < evnMatrix[l].Length; m++)
			{
				if (evnMatrix[l][m] == null)
				{
					continue;
				}
				for (int n = 0; n < evnMatrix[l][m].Length; n++)
				{
					if (evnMatrix[l][m][n] != null)
					{
						events[num] = evnMatrix[l][m][n];
						num++;
					}
				}
			}
		}
	}

	private void checkEvents()
	{
		int state = (int)currentAction.GetState();
		if (state >= evnMatrix.Length)
		{
			return;
		}
		for (int i = 0; i < evnMatrix[state].Length; i++)
		{
			if (evnMatrix[state][i] == null)
			{
				continue;
			}
			for (int j = 0; j < evnMatrix[state][i].Length; j++)
			{
				if (evnMatrix[state][i][j].Check() && CanSwitchTo((ActionCode)i))
				{
					SwitchTo((ActionCode)i);
					break;
				}
			}
		}
	}

	public void EnableBlob()
	{
		if (blobT != null)
		{
			blobT.gameObject.SetActive(true);
		}
	}

	public void DisableBlob()
	{
		if (blobT != null)
		{
			blobT.gameObject.SetActive(false);
		}
	}

	public Transform GetTransformByName(string boneName)
	{
		Transform[] componentsInChildren = base.gameObject.GetComponentsInChildren<Transform>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			if (string.Compare(boneName, componentsInChildren[i].name, true) == 0)
			{
				componentsInChildren[i].name = boneName;
				return componentsInChildren[i];
			}
		}
		Debug.Log(string.Format("Cannot find the bone {0}", boneName));
		return null;
	}

	public void ResetLastYPos()
	{
		lastYPosition = playerT.position.y;
	}

	private void updateInertia()
	{
		accumTimeInertia += Time.deltaTime;
		if (accumTimeInertia >= 0.25f)
		{
			inertia = base.transform.position - prevPos;
			prevPos = base.transform.position;
			accumTimeInertia = 0f;
		}
	}

	private void loadAndAttachJetpack()
	{
		base.transform.position = Vector3.zero;
		jetpack = CharHelper.AttachJetpack();
		CharHelper.GetProps().HasJetpack = true;
		// Ensure the static jetpackGO is set for CharHelper methods
		CharHelper.SetJetpackGO(jetpack);
		if (string.Compare(DedalordLoadLevel.GetLevel(), "TutorialLoader") != 0)
		{
			if (jetpackMeterGO == null)
			{
				jetpackMeterGO = GameObject.Find("JetpackMeter");
			}
			if (jetpackMeterGO != null)
			{
				GUI3DSlideTransition component = jetpackMeterGO.GetComponent<GUI3DSlideTransition>();
				component.StartIntroTransition();
			}
		}
	}

	private void RemoveJetpack()
	{
		CharHelper.HideJetpack();
		CharHelper.GetProps().HasJetpack = false;
		GameObject gameObject = GameObject.Find("JetpackMeter");
		if (gameObject != null)
		{
			GUI3DSlideTransition component = gameObject.GetComponent<GUI3DSlideTransition>();
			component.StartOutroTransition();
		}
	}

	public void ShowJetpack()
	{
		GameEventDispatcher.Dispatch(this, onJetpackTaken);
		if (jetpack != null)
		{
			jetpack.SetActive(true);
			jetpack.GetComponent<Jetpack>().enabled = true;
			CharHelper.ShowJetpack();
			CharHelper.GetProps().HasJetpack = true;
			if (jetpackMeterGO == null)
			{
				jetpackMeterGO = GameObject.Find("JetpackMeter");
			}
			if (jetpackMeterGO != null)
			{
				GUI3DSlideTransition component = jetpackMeterGO.GetComponent<GUI3DSlideTransition>();
				component.StartIntroTransition();
			}
			else
			{
				Debug.Log("Cannot find the jetpack meter");
			}
		}
		else
		{
			Debug.Log("Jetpack is not instanced. Is there at least one JetpackItem in the level?");
		}
		RemoveWings();
	}

	public void onJetpackExplode()
	{
		SwitchTo(ActionCode.EXPLODE_ON_AIR);
	}

	private void loadAndAttachJetpackIfNecessary()
	{
		Vector3 position = base.transform.position;
		base.transform.position = Vector3.zero;
		jetpack = CharHelper.AttachJetpackIfNecessary();
		if (jetpack != null)
		{
			jetpack.SetActive(false);
			// Ensure the static jetpackGO is set for CharHelper methods
			CharHelper.SetJetpackGO(jetpack);
		}
		base.transform.position = position;
	}

	private void loadAndAttachWingsIfNecessary()
	{
		Vector3 position = base.transform.position;
		base.transform.position = Vector3.zero;
		wings = CharHelper.AttachWingsIfNecessary();
		if (wings != null)
		{
			wings.SetActive(false);
		}
		base.transform.position = position;
	}

	public void ShowJetpack(bool val)
	{
		if (jetpack != null)
		{
			jetpack.SetActive(val);
			jetpack.GetComponent<Jetpack>().enabled = val;
			if (val)
			{
				jetpack.GetComponent<Jetpack>().DisableAll();
			}
		}
		else
		{
			Debug.Log("Jetpack is not instanced. Is there at least one JetpackItem in the level?");
		}
	}

	public GameObject GetJetpack()
	{
		// Check if jetpack reference is still valid
		if (jetpack == null || jetpack.gameObject == null)
		{
			// Try to find the jetpack in the scene if it exists
			jetpack = GameObject.Find("Jetpack_torso1");
			if (jetpack == null)
			{
				// If still not found, try to reattach it
				loadAndAttachJetpackIfNecessary();
			}
		}
		return jetpack;
	}

	public void ShowWings()
	{
		GameEventDispatcher.Dispatch(this, onWingsTaken);
		if (wings != null)
		{
			wings.SetActive(true);
			wings.GetComponent<Wings>().UnfoldAndFold();
		}
		else
		{
			Debug.Log("Wings is not instanced. Is there at least one WingsItem in the level?");
		}
		RemoveJetpack();
	}

	public void RemoveWings()
	{
		ResetLastYPos();
		GameEventDispatcher.Dispatch(this, onWingsDropped);
		if (wings != null)
		{
			wings.SetActive(false);
		}
		CharHelper.GetProps().HasWings = false;
		if (GetCurrentState() == ActionCode.FLY)
		{
			SwitchTo(ActionCode.DRAMATIC_JUMP);
		}
	}

	public void RemoveWings(bool showParticles, bool removeFromProps)
	{
		ResetLastYPos();
		GameEventDispatcher.Dispatch(this, onWingsDropped);
		if (wings != null)
		{
			wings.SetActive(false);
		}
		if (removeFromProps)
		{
			CharHelper.GetProps().HasWings = false;
		}
		if (showParticles)
		{
			CharHelper.GetEffects().EnableWingsExplosionParticles();
		}
		if (GetCurrentState() == ActionCode.FLY)
		{
			SwitchTo(ActionCode.DRAMATIC_JUMP);
		}
	}

	public GameObject GetWings()
	{
		return wings;
	}

	public void LoadAndAttachExtras()
	{
		loadAndAttachWingsIfNecessary();
		loadAndAttachJetpackIfNecessary();
		if (base.enabled)
		{
			CharHelper.PlaceCharacterOnStart();
			ResetLastYPos();
		}
	}

	public bool IsCharacterPlaced()
	{
		return characterPlaced;
	}

	private void addShadow()
	{
		GameObject gameObject = Resources.Load("Prefabs/Blob", typeof(GameObject)) as GameObject;
		GameObject gameObject2 = Object.Instantiate(gameObject) as GameObject;
		gameObject2.name = "Blob";
		gameObject2.transform.parent = base.transform;
		gameObject2.transform.localPosition = gameObject.transform.localPosition;
		gameObject2.transform.localRotation = gameObject.transform.localRotation;
		gameObject2.transform.localScale = gameObject.transform.localScale;
		blobT = gameObject2.transform;
	}

	public void Hit(Vector3 hit)
	{
		SwitchTo(ActionCode.RAGDOLL);
		Transform transformByName = CharHelper.GetTransformByName("torso2");
		if (transformByName != null)
		{
			transformByName.GetComponent<Rigidbody>().AddForce(hit * 25f);
		}
		GameEventDispatcher.Dispatch(null, new PlayerWasCrushed());
		DisableBlob();
	}

	private void updateFloorNormal()
	{
		RaycastHit hitInfo;
		if (GetCurrentState() == ActionCode.RUNNING_ON_WALL_PLATFORM)
		{
			IsGrounded = true;
			playSoundSteps();
			lastYPosition = playerT.position.y;
		}
		else if (Physics.Raycast(new Vector3(playerT.position.x - 0.5f, playerT.position.y + 2f, playerT.position.z), Vector3.down, out hitInfo, float.PositiveInfinity, 5255680) || Physics.Raycast(new Vector3(playerT.position.x + 0.5f, playerT.position.y + 2f, playerT.position.z), Vector3.down, out hitInfo, float.PositiveInfinity, 5255680))
		{
			if (GetCurrentState() == ActionCode.DIVE && Tag.IsBreakablePlatform(hitInfo.collider))
			{
				return;
			}
			FloorNormalZ = hitInfo.normal.z;
			FloorXAngle = Mathf.Atan(FloorNormalZ) * 57.29578f;
			FloorYPos = hitInfo.point.y;
			FloorZAngle = Mathf.Atan(hitInfo.normal.x) * 57.29578f;
			float num = Mathf.Abs(playerT.position.y - hitInfo.point.y);
			bool flag = num < 0.7f;
			if (num >= 0.7f)
			{
				flag = cc.isGrounded;
			}
			if (flag)
			{
				playSoundSteps();
				groundUpdateCounter++;
				groundTag = hitInfo.transform.tag;
				if (groundUpdateCounter > 1)
				{
					lastYPosition = playerT.position.y;
				}
			}
			else
			{
				stopSoundSteps();
				groundUpdateCounter = 0;
			}
			IsGrounded = flag;
			if (IsGrounded)
			{
				GlideTimeLeft = CharHelper.GetProps().GlideMaxTime;
			}
			else
			{
				stopSoundSteps();
			}
			setFloorType(hitInfo.transform);
		}
		else
		{
			stopSoundSteps();
			IsGrounded = false;
			floorType = FloorType.NONE;
		}
	}

	private void setFloorType(Transform hitT)
	{
		if (IsGrounded)
		{
			if (Tag.IsPlatform(hitT.tag))
			{
				LevelPlatform component = hitT.gameObject.GetComponent<LevelPlatform>();
				if (component != null)
				{
					floorType = component.platformType;
				}
				else
				{
					floorType = FloorType.PLATFORM;
				}
			}
			else
			{
				floorType = FloorType.FLOOR;
			}
		}
		else
		{
			floorType = FloorType.NONE;
		}
	}

	public void ResetGlideTimeLeft()
	{
		GlideTimeLeft = CharHelper.GetProps().GlideMaxTime;
	}

	private void playSoundSteps()
	{
		if (currentAction.GetState() == ActionCode.RUNNING)
		{
			accumTimeSteps += Time.deltaTime;
			float num = (duration = 1.299f);
			if (accumTimeSteps >= num)
			{
				channel = SoundManager.PlaySound(base.transform.position, 16);
				if (channel != lastChannel)
				{
					SoundManager.StopSound(lastChannel);
				}
				lastChannel = channel;
				accumTimeSteps %= num;
			}
		}
		else if (currentAction.GetState() == ActionCode.SUPER_SPRINT || currentAction.GetState() == ActionCode.MEGA_SPRINT)
		{
			accumTimeSteps += Time.deltaTime;
			float num2 = (duration = 1.228f);
			if (accumTimeSteps >= num2)
			{
				channel = SoundManager.PlaySound(base.transform.position, 17);
				if (channel != lastChannel)
				{
					SoundManager.StopSound(lastChannel);
				}
				lastChannel = channel;
				accumTimeSteps %= num2;
			}
		}
		else if (channel != -1)
		{
			stopSoundSteps();
		}
	}

	private void stopSoundSteps()
	{
		if (channel != -1)
		{
			accumTimeSteps = duration;
			SoundManager.StopSound(channel);
			channel = -1;
		}
	}

	public Vector3 GetVelocity()
	{
		return cc.velocity;
	}

	public float GetDeltaY()
	{
		return lastYPosition - playerT.position.y;
	}

	public bool HittedAgainstHardSurface()
	{
		return string.Compare(groundTag, "Bouncer", true) != 0;
	}
}
