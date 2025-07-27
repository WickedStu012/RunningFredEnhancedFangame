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
				string[] array = null;
				array = ((state != State.Fear) ? TerrorAnimations : FearAnimations);
				if (base.GetComponent<Animation>() != null && array != null && array.Length > 0)
				{
					int num = Random.Range(0, array.Length);
					base.GetComponent<Animation>().wrapMode = WrapMode.Loop;
					base.GetComponent<Animation>().CrossFade(array[num]);
				}
				timer = Time.time;
			}
		}
		else if (!base.GetComponent<Animation>().isPlaying)
		{
			if (state == State.Pain)
			{
				state = State.Fear;
			}
			else if (state == State.Dying)
			{
				state = State.Dead;
				base.GetComponent<Animation>().Play(DeadAnimation);
				base.GetComponent<Animation>().wrapMode = WrapMode.ClampForever;
			}
		}
	}

	private void OnChangeState(object sender, GameEvent e)
	{
		CharChangeState charChangeState = (CharChangeState)e;
		if (charChangeState.CurrentState.GetState() == ActionCode.MEGA_SPRINT || charChangeState.CurrentState.GetState() == ActionCode.DRAMATIC_JUMP || charChangeState.CurrentState.GetState() == ActionCode.BURNT)
		{
			state = State.Terror;
		}
		else if (charChangeState.CurrentState.GetState() == ActionCode.EXPLODE || charChangeState.CurrentState.GetState() == ActionCode.EXPLODE_ON_WALL || charChangeState.CurrentState.GetState() == ActionCode.FROZEN || charChangeState.CurrentState.GetState() == ActionCode.RAGDOLL || charChangeState.CurrentState.GetState() == ActionCode.DIE_IMPCT)
		{
			state = State.Dying;
			base.GetComponent<Animation>().Play(DyingAnimation);
			base.GetComponent<Animation>().wrapMode = WrapMode.Once;
		}
		else
		{
			if (state == State.Pain)
			{
				return;
			}
			if (charChangeState.CurrentState.GetState() == ActionCode.BOUNCE || charChangeState.CurrentState.GetState() == ActionCode.BURNT || charChangeState.CurrentState.GetState() == ActionCode.CARLTROP || charChangeState.CurrentState.GetState() == ActionCode.DIE_IMPCT || charChangeState.CurrentState.GetState() == ActionCode.EXPLODE || charChangeState.CurrentState.GetState() == ActionCode.EXPLODE_ON_WALL || charChangeState.CurrentState.GetState() == ActionCode.FROZEN || charChangeState.CurrentState.GetState() == ActionCode.RAGDOLL || charChangeState.CurrentState.GetState() == ActionCode.STAGGER)
			{
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
			else
			{
				state = State.Fear;
			}
		}
	}
}
