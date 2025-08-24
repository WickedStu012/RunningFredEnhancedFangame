using UnityEngine;

public class CharHead : MonoBehaviour
{
	private enum State
	{
		Fear = 0,
		Pain = 1,
		Terror = 2,
		Dying = 3,
		Dead = 4
	}

	public float Timer = 2f;

	public float PainTimer = 1f;

	public string[] FearAnimations;

	public string[] TerrorAnimations;

	public string[] PainAnimations;

	public string DyingAnimation;

	public string DeadAnimation;

	private State state;

	private float timer;

	private bool setHeadMaterial;

	private GameObject player;

	// New variables for animation cycling
	private int currentFearAnimationIndex = 0;
	private int fearAnimationsPlayed = 0;
	private bool isTerrorCycle = false;
	private int terrorAnimationsPlayed = 0;
	private State previousState = State.Fear;
	
	// Flag to prevent interruption of dying/dead animations
	private bool isDyingOrDead = false;
	
	// Timer to prevent immediate state changes after dying animation
	private float dyingAnimationEndTime = 0f;
	private const float DYING_ANIMATION_COOLDOWN = 0.5f; // Half second cooldown

	private void Awake()
	{
		Debug.Log("CharHead: Awake called");
		state = State.Fear;
		player = CharHelper.GetPlayer();
		if (player != null)
		{
			addAttachs(player);
		}
		CharHeadHelper.SetHeadGameObject(base.gameObject);
		
		// Initialize CharHeadAnimManager with the head animation component
		Animation headAnimation = base.GetComponent<Animation>();
		if (headAnimation != null)
		{
			CharHeadAnimManager.SetAnimationReference(headAnimation);
		}
	}

	private void OnEnable()
	{
		Debug.Log("CharHead: OnEnable called - registering event listeners");
		GameEventDispatcher.AddListener("CharChangeState", OnChangeState);
		GameEventDispatcher.AddListener("OnLevelLoaded", OnLevelLoaded);
		GameEventDispatcher.AddListener("OnEndLessReset", OnEndLessReset);
		GameEventDispatcher.AddListener("OnEndLessResurrect", OnEndLessResurrect);
		GameEventDispatcher.AddListener("OnPlayerRespawningNow", OnPlayerRespawningNow);
		Debug.Log("CharHead: Event listeners registered successfully");
	}

	private void OnDisable()
	{
		Debug.Log("CharHead: OnDisable called - removing event listeners");
		GameEventDispatcher.RemoveListener("CharChangeState", OnChangeState);
		GameEventDispatcher.RemoveListener("OnLevelLoaded", OnLevelLoaded);
		GameEventDispatcher.RemoveListener("OnEndLessReset", OnEndLessReset);
		GameEventDispatcher.RemoveListener("OnEndLessResurrect", OnEndLessResurrect);
		GameEventDispatcher.RemoveListener("OnPlayerRespawningNow", OnPlayerRespawningNow);
	}
	
	// Public method to force reset the head state - can be called from other scripts
	public void ForceResetToFear()
	{
		Debug.Log("CharHead: ForceResetToFear called - resetting to Fear state");
		state = State.Fear;
		isDyingOrDead = false;
		dyingAnimationEndTime = 0f;
		ResetAnimationCycle();
		timer = Time.time - Timer; // Trigger immediate animation
	}

	private void addAttachs(GameObject player)
	{
		CharHeadHelper.SyncHeadAttachs(player.name, CharHelper.GetCharBloodSplat().characterMaterial);
	}

	private void Update()
	{
		// Debug: Log current state every few seconds to confirm script is running
		if (Time.frameCount % 300 == 0) // Every 300 frames (about 5 seconds at 60fps)
		{
			Debug.Log("CharHead: Update running - Current state: " + state + ", isDyingOrDead: " + isDyingOrDead + ", dyingAnimationEndTime: " + dyingAnimationEndTime);
		}
		
		if (player == null)
		{
			player = CharHelper.GetPlayer();
			if (player != null)
			{
				addAttachs(player);
			}
		}
		if (!setHeadMaterial && CharHelper.GetCharBloodSplat() != null)
		{
			CharHelper.GetCharBloodSplat().SetCharacterMaterialToHead(base.gameObject);
			setHeadMaterial = true;
		}
		if (state != State.Pain && state != State.Dying && state != State.Dead && !isDyingOrDead)
		{
			if (Time.time - timer >= Timer)
			{
				Debug.Log("CharHead: About to play animation - Current state: " + state + ", isDyingOrDead: " + isDyingOrDead);
				if (state == State.Fear)
				{
					PlayFearAnimation();
				}
				else if (state == State.Terror)
				{
					PlayTerrorAnimation();
				}
				timer = Time.time;
			}
		}
		else
		{
			// Debug: Log when we're in Pain/Dying/Dead state or isDyingOrDead is true
			if (Time.frameCount % 60 == 0) // Every 60 frames (about 1 second at 60fps)
			{
				Debug.Log("CharHead: In Pain/Dying/Dead state or isDyingOrDead - Current state: " + state + ", isDyingOrDead: " + isDyingOrDead + ", animation playing: " + base.GetComponent<Animation>().isPlaying);
			}
			
			if (!base.GetComponent<Animation>().isPlaying)
			{
				if (state == State.Pain)
				{
					state = State.Fear;
					// Don't reset cycle, just continue from where we left off
					// Reset timer to trigger immediate animation
					timer = Time.time - Timer;
				}
				else if (state == State.Dying)
				{
					// After death animation finishes, set the cooldown timer only once
					if (dyingAnimationEndTime == 0f) // Only set it once
					{
						dyingAnimationEndTime = Time.time;
						Debug.Log("CharHead: Dying animation finished, starting cooldown period");
					}
					// Don't immediately change state - let the cooldown handle it
				}
			}
		}
		
		// Handle cooldown after dying animation
		if (state == State.Dying && !base.GetComponent<Animation>().isPlaying && 
			dyingAnimationEndTime > 0f && Time.time - dyingAnimationEndTime >= DYING_ANIMATION_COOLDOWN)
		{
			// Check if the character is actually dead (not just temporarily dying)
			CharStateMachine charStateMachine = CharHelper.GetCharStateMachine();
			if (charStateMachine != null)
			{
				ActionCode currentCharState = charStateMachine.GetCurrentState();
				Debug.Log("CharHead: Cooldown finished, character state: " + currentCharState);
				
				// If character is still in a dying/dead state, keep the head in dying state
				if (currentCharState == ActionCode.EXPLODE || currentCharState == ActionCode.EXPLODE_ON_WALL || 
					currentCharState == ActionCode.FROZEN || currentCharState == ActionCode.RAGDOLL || 
					currentCharState == ActionCode.DIE_IMPCT)
				{
					Debug.Log("CharHead: Character is still dead, keeping head in dying state");
					// Keep the head in dying state and don't reset isDyingOrDead
					// This prevents the fear/terror cycle from starting
				}
				else
				{
					// Character is no longer dead, safe to return to previous state
					Debug.Log("CharHead: Character is no longer dead, returning to previous state: " + previousState);
					state = previousState;
					isDyingOrDead = false; // Reset the flag
					dyingAnimationEndTime = 0f; // Reset the cooldown timer
					if (state == State.Terror)
					{
						// If returning to terror, reset terror cycle
						terrorAnimationsPlayed = 0;
						isTerrorCycle = true;
					}
					else
					{
						// If returning to fear, reset fear cycle
						ResetAnimationCycle();
					}
					timer = Time.time - Timer; // Trigger immediate animation
				}
			}
			else
			{
				// Fallback: if we can't get the character state, assume it's safe to return
				Debug.Log("CharHead: Could not get character state, returning to previous state: " + previousState);
				state = previousState;
				isDyingOrDead = false;
				dyingAnimationEndTime = 0f;
				if (state == State.Terror)
				{
					terrorAnimationsPlayed = 0;
					isTerrorCycle = true;
				}
				else
				{
					ResetAnimationCycle();
				}
				timer = Time.time - Timer;
			}
		}
	}

	private void PlayFearAnimation()
	{
		if (base.GetComponent<Animation>() != null && FearAnimations != null && FearAnimations.Length > 0)
		{
			// Play fear animation in sequence
			string animationName = FearAnimations[currentFearAnimationIndex];
			Debug.Log("CharHead: Playing Fear animation: " + animationName + " (index: " + currentFearAnimationIndex + ")");
			base.GetComponent<Animation>().wrapMode = WrapMode.Loop;
			base.GetComponent<Animation>().CrossFade(animationName);
			
			// Move to next fear animation
			currentFearAnimationIndex = (currentFearAnimationIndex + 1) % FearAnimations.Length;
			fearAnimationsPlayed++;
			
			// After completing all fear animations, switch to terror
			if (fearAnimationsPlayed >= FearAnimations.Length)
			{
				Debug.Log("CharHead: Fear cycle complete, switching to Terror");
				state = State.Terror;
				fearAnimationsPlayed = 0;
				terrorAnimationsPlayed = 0;
				isTerrorCycle = true;
				// Reset timer to trigger immediate terror animation
				timer = Time.time - Timer;
			}
		}
	}

	private void PlayTerrorAnimation()
	{
		if (base.GetComponent<Animation>() != null && TerrorAnimations != null && TerrorAnimations.Length > 0)
		{
			// Play terror animation in sequence
			string animationName = TerrorAnimations[terrorAnimationsPlayed];
			Debug.Log("CharHead: Playing Terror animation: " + animationName + " (index: " + terrorAnimationsPlayed + ")");
			base.GetComponent<Animation>().wrapMode = WrapMode.Loop;
			base.GetComponent<Animation>().CrossFade(animationName);
			
			terrorAnimationsPlayed++;
			
			// After completing all terror animations, switch back to fear
			if (terrorAnimationsPlayed >= TerrorAnimations.Length)
			{
				Debug.Log("CharHead: Terror cycle complete, switching back to Fear");
				state = State.Fear;
				fearAnimationsPlayed = 0;
				terrorAnimationsPlayed = 0;
				isTerrorCycle = false;
				currentFearAnimationIndex = 0;
				// Reset timer to trigger immediate fear animation
				timer = Time.time - Timer;
			}
		}
	}

	private void ResetAnimationCycle()
	{
		currentFearAnimationIndex = 0;
		fearAnimationsPlayed = 0;
		terrorAnimationsPlayed = 0;
		isTerrorCycle = false;
	}

	private void OnChangeState(object sender, GameEvent e)
	{
		CharChangeState charChangeState = (CharChangeState)e;
		ActionCode currentActionCode = charChangeState.CurrentState.GetState();
		
		Debug.Log("CharHead: OnChangeState received - ActionCode: " + currentActionCode + ", Current state: " + state + ", isDyingOrDead: " + isDyingOrDead);
		
		// RESPAWN should always be allowed to override dying/dead state
		if (currentActionCode == ActionCode.RESPAWN)
		{
			// Force return to fear state when respawning
			Debug.Log("CharHead: Respawn detected, returning to Fear state");
			state = State.Fear;
			isDyingOrDead = false; // Reset the flag
			dyingAnimationEndTime = 0f; // Reset cooldown timer
			ResetAnimationCycle(); // Reset to start of fear cycle
			timer = Time.time - Timer; // Trigger immediate animation
			return;
		}
		
		// If we're in dying or dead state, don't allow any other interruptions
		if (isDyingOrDead)
		{
			Debug.Log("CharHead: Blocking state change - in dying/dead state");
			return;
		}
		
		// Only allow OnChangeState to interrupt the cycle for high-priority events
		// All other state changes should be ignored to preserve the animation cycle
		
		if (currentActionCode == ActionCode.MEGA_SPRINT || currentActionCode == ActionCode.DRAMATIC_JUMP || currentActionCode == ActionCode.BURNT)
		{
			previousState = state;
			state = State.Terror;
			ResetAnimationCycle();
		}
		else if (currentActionCode == ActionCode.EXPLODE || currentActionCode == ActionCode.EXPLODE_ON_WALL || currentActionCode == ActionCode.FROZEN || currentActionCode == ActionCode.RAGDOLL || currentActionCode == ActionCode.DIE_IMPCT)
		{
			previousState = state;
			state = State.Dying;
			isDyingOrDead = true; // Set flag to prevent interruptions
			base.GetComponent<Animation>().Play(DyingAnimation);
			base.GetComponent<Animation>().wrapMode = WrapMode.Once;
		}
		else if (currentActionCode == ActionCode.BOUNCE || currentActionCode == ActionCode.BURNT || currentActionCode == ActionCode.CARLTROP || currentActionCode == ActionCode.DIE_IMPCT || currentActionCode == ActionCode.EXPLODE || currentActionCode == ActionCode.EXPLODE_ON_WALL || currentActionCode == ActionCode.FROZEN || currentActionCode == ActionCode.RAGDOLL || currentActionCode == ActionCode.STAGGER)
		{
			previousState = state;
			state = State.Pain;
			string[] array = null;
			array = PainAnimations;
			if (base.GetComponent<Animation>() != null && array != null && array.Length > 0)
			{
				int num = Random.Range(0, array.Length);
				base.GetComponent<Animation>().Play(array[num]);
				base.GetComponent<Animation>().wrapMode = WrapMode.Once;
			}
		}
		// All other state changes (RUNNING, JUMPING, etc.) are ignored to preserve the animation cycle
	}
	
	private void OnLevelLoaded(object sender, GameEvent e)
	{
		// Reset head state when level is loaded/reloaded
		Debug.Log("CharHead: OnLevelLoaded received - resetting to Fear state");
		state = State.Fear;
		isDyingOrDead = false;
		dyingAnimationEndTime = 0f;
		ResetAnimationCycle();
		timer = Time.time - Timer; // Trigger immediate animation
	}
	
	private void OnEndLessReset(object sender, GameEvent e)
	{
		// Reset head state when endless mode is reset
		Debug.Log("CharHead: OnEndLessReset received - resetting to Fear state");
		state = State.Fear;
		isDyingOrDead = false;
		dyingAnimationEndTime = 0f;
		ResetAnimationCycle();
		timer = Time.time - Timer; // Trigger immediate animation
	}
	
	private void OnEndLessResurrect(object sender, GameEvent e)
	{
		// Reset head state when endless mode is resurrected
		Debug.Log("CharHead: OnEndLessResurrect received - resetting to Fear state");
		state = State.Fear;
		isDyingOrDead = false;
		dyingAnimationEndTime = 0f;
		ResetAnimationCycle();
		timer = Time.time - Timer; // Trigger immediate animation
	}
	
	private void OnPlayerRespawningNow(object sender, GameEvent e)
	{
		// Reset head state when player is respawning
		Debug.Log("CharHead: OnPlayerRespawningNow received - resetting to Fear state");
		state = State.Fear;
		isDyingOrDead = false;
		dyingAnimationEndTime = 0f;
		ResetAnimationCycle();
		timer = Time.time - Timer; // Trigger immediate animation
	}
	
	// Public method to handle dive actions from ActDive.cs
	public void HandleDiveAction(bool isDiving)
	{
		// If we're in dying or dead state, don't allow any interruptions
		if (isDyingOrDead)
		{
			return;
		}
		
		if (isDiving)
		{
			// When diving starts, set to terror state
			previousState = state;
			state = State.Terror;
			ResetAnimationCycle();
		}
		else
		{
			// When diving ends, return to previous state
			state = previousState;
			if (state == State.Terror)
			{
				// If returning to terror, reset terror cycle
				terrorAnimationsPlayed = 0;
				isTerrorCycle = true;
			}
			else
			{
				// If returning to fear, reset fear cycle
				ResetAnimationCycle();
			}
			timer = Time.time - Timer; // Trigger immediate animation
		}
	}
	
	// Public method to check if the head is in dying/dead state
	public bool IsDyingOrDead()
	{
		// Return true if we're in dying/dead state OR if we're in the cooldown period after dying animation
		// Note: dyingAnimationEndTime = 0f means no cooldown is active
		return isDyingOrDead || (state == State.Dying && !base.GetComponent<Animation>().isPlaying && 
			dyingAnimationEndTime > 0f && Time.time - dyingAnimationEndTime < DYING_ANIMATION_COOLDOWN);
	}
}
