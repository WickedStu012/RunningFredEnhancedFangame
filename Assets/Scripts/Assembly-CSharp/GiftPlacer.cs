using UnityEngine;

public class GiftPlacer : MonoBehaviour
{
	private static GiftPlacer instance;

	private Vector3[] giftPos;

	private void Awake()
	{
		instance = this;
	}

	private void Start()
	{
		Gift[] componentsInChildren = base.transform.GetComponentsInChildren<Gift>();
		giftPos = new Vector3[componentsInChildren.Length];
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			giftPos[i] = componentsInChildren[i].transform.position;
			Object.DestroyImmediate(componentsInChildren[i].gameObject);
		}
		GameEventDispatcher.Dispatch(this, OnGiftPlacerReady.Instance);
	}

	private void OnDestroy()
	{
		instance = null;
	}

	public static Vector3[] GetAllGiftPosition()
	{
		return instance.giftPos;
	}

	public static Vector3 GetRandomGiftPosition()
	{
		int num = Random.Range(0, instance.giftPos.Length);
		return instance.giftPos[num];
	}
}
