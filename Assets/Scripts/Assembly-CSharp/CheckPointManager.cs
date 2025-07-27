using UnityEngine;

public class CheckPointManager : MonoBehaviour
{
	private static CheckPointManager instance;

	private Vector3 lastPlayerPosition;

	private Transform playerT;

	private void Awake()
	{
		instance = this;
		playerT = CharHelper.GetPlayerTransform();
		if (playerT != null)
		{
			storeLastPlayerPosition();
		}
	}

	private void Update()
	{
		if (playerT == null)
		{
			playerT = CharHelper.GetPlayerTransform();
			if (playerT != null)
			{
				storeLastPlayerPosition();
			}
		}
	}

	private void storeLastPlayerPosition()
	{
		if (playerT.position.z < 0f)
		{
			lastPlayerPosition = new Vector3(playerT.position.x, playerT.position.y, 0f);
		}
		else
		{
			lastPlayerPosition = playerT.position;
		}
	}

	public static void RespawnOnLastCheckPoint()
	{
		Rock.Relocate();
		Barrel.Relocate();
		instance.playerT.position = instance.lastPlayerPosition;
		ChunkRelocator.ModifyChunkVisibility();
		instance.placeCharOnFloor();
		CharHelper.GetCharStateMachine().ResetChar();
		CharHelper.GetCharSkin().Blink(2f);
		CharHelper.GetCharSkin().ClearBloodSpurrs();
		CharHelper.GetCharBloodSplat().ResetDamage();
		CharHelper.GetEffects().EnableRespawnParticles();
		FallTrigger.Reset();
		SoundManager.PlaySound(32);
	}

	public static void RespawnForEndless()
	{
		instance.playerT.position = instance.lastPlayerPosition;
		CharHelper.GetCharStateMachine().ResetChar();
		instance.placeCharOnFloor();
		CharHelper.GetCharSkin().ClearBloodSpurrs();
		CharHelper.GetCharBloodSplat().ResetDamage();
		CharHelper.GetEffects().EnableRespawnParticles();
	}

	private void respawnOnLastCheckPointCont()
	{
	}

	private void placeCharOnFloor()
	{
		RaycastHit hitInfo;
		if (Physics.Raycast(new Vector3(instance.playerT.position.x, (instance.playerT.position.y != 0f) ? (instance.playerT.position.y + 4f) : 1000f, instance.playerT.position.z), Vector3.down, out hitInfo, float.PositiveInfinity, 4207104))
		{
			instance.playerT.position = new Vector3(hitInfo.point.x, hitInfo.point.y + 2f, hitInfo.point.z);
			Camera.main.GetComponent<FredCamera>().ChangeCameraPosNow();
		}
		else
		{
			Debug.Log("Cannot find the floor");
		}
	}

	public static void CheckPoint(Vector3 cpPos)
	{
		instance.lastPlayerPosition = cpPos;
	}

	private void OnDestroy()
	{
		instance = null;
	}
}
