using UnityEngine;

public class CharProps : MonoBehaviour
{
	[HideInInspector]
	public float RunningAcceleration = 20f;

	public float MinJumpTime = 0.15f;

	public float MaxJumpTime = 0.2f;

	public float DoubleJumpTime = 0.1f;

	public float SuperSprintTime = 30f; // Increased from 2f to allow longer sprint time

	public float JetpackSuperSprintTime = 60f; // Increased from 10f to allow much longer jetpack sprint time

	public float MegaSprintTime = 3f;

	public float RunningAccelK = 1.01f;

	public float SuperSprintAccelK = 1.01f;

	public float SuperMegaSprintAccelK = 1.01f;

	public float JetpackAccelK = 1.02f;

	public int SuccesiveJumpCount = 2;

	public float DragMinTime = 1f;

	public float minHeightToRoll = 12f;

	public float minHeightToTrip = 26f;

	public float minHeightToDie = 30f;

	public float minHeightToExplode = 36f;

	public float DragMaxTime = 3f;

	public float GlideMaxTime = 3f;

	public float MinAngleToSurf = 25f;

	public float CarltropPenaltyTime = 2f;

	public float MinRunSpeed = 15f;

	public float MaxRunSpeed = 20f;

	public float MaxJetpackSpeed = 30f;

	public float MinSprintSpeed = 25f;

	public float MaxSprintSpeed = 30f;

	public float MinMegaSprintSpeed = 30f;

	public float MaxMegaSprintSpeed = 35f;

	public bool HasJetpack = true; // Set to true for testing - normally enabled by picking up jetpack item

	public float JetPackFuelLeft = 100f;

	public float MaxJetPackFuel = 100f;

	public float JetpackConsumption = 0f; // Disabled fuel consumption for infinite jetpack usage

	public float JetpackConsumptionSprint = 0f; // Disabled sprint fuel consumption for infinite jetpack sprint usage

	public bool JetpackOverheating;

	public int SuperSprintsLeft = 3;

	public int MaxSuperSprintsCount = 6;

	public bool HasWings;

	public float TimeOverABlower = 4f;

	public int MagnetLevel;

	public int Lives = 1;

	public int ChickenFlaps;

	public int WallGrip;

	public int WallBounce;

	public bool PanicPower;

	public bool FakirFeet;

	public bool LuckyCharm;

	public bool ProtectiveVest;

	public bool RubberBones;

	public bool Airbags;

	public bool FastRecovery;

	public ResurrectStatus freeResurectByTapjoy;
}
