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

	private void Awake()
	{
		state = State.Fear;
		player = CharHelper.GetPlayer();
		if (player != null)
		{
			addAttachs(player);
		}
		CharHeadHelper.SetHeadGameObject(base.gameObject);
	}

	private void OnEnable()
	{
		GameEventDispatcher.AddListener("CharChangeState", OnChangeState);
	}

	private void OnDisable()
	{
		GameEventDispatcher.RemoveListener("CharChangeState", OnChangeState);
	}

	private void addAttachs(GameObject player)
	{
		CharHeadHelper.SyncHeadAttachs(player.name, CharHelper.GetCharBloodSplat().characterMaterial);
	}

	private void Update()
	{
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
		if (state != State.Pain && state != State.Dying && state != State.Dead)
		{
			if (Time.time - timer >= Timer)
			{
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
		else if (!base.GetComponent<Animation>().isPlaying)
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
				// After death animation finishes, return to previous state before death
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
	}

	private void PlayFearAnimation()
	{
		if (base.GetComponent<Animation>() != null && FearAnimations != null && FearAnimations.Length > 0)
		{
			// Play fear animation in sequence
			string animationName = FearAnimations[currentFearAnimationIndex];
			base.GetComponent<Animation>().wrapMode = WrapMode.Loop;
			base.GetComponent<Animation>().CrossFade(animationName);
			
			// Move to next fear animation
			currentFearAnimationIndex = (currentFearAnimationIndex + 1) % FearAnimations.Length;
			fearAnimationsPlayed++;
			
			// After completing all fear animations, switch to terror
			if (fearAnimationsPlayed >= FearAnimations.Length)
			{
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
			base.GetComponent<Animation>().wrapMode = WrapMode.Loop;
			base.GetComponent<Animation>().CrossFade(animationName);
			
			terrorAnimationsPlayed++;
			
			// After completing all terror animations, switch back to fear
			if (terrorAnimationsPlayed >= TerrorAnimations.Length)
			{
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
		else if (currentActionCode == ActionCode.RESPAWN)
		{
			// Return to previous state when respawning
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
		// All other state changes (RUNNING, JUMPING, etc.) are ignored to preserve the animation cycle
	}
}
