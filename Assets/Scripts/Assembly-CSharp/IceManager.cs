using UnityEngine;

public class IceManager : MonoBehaviour
{
	private static IceManager instance;

	public GameObject iceBlock;

	private IceBlock ib;

	private void Awake()
	{
		instance = this;
	}

	private void Start()
	{
		GameObject original = Resources.Load("Extras/IceBlock", typeof(GameObject)) as GameObject;
		iceBlock = Object.Instantiate(original) as GameObject;
		ib = iceBlock.GetComponent<IceBlock>();
		iceBlock.SetActive(false);
	}

	public static IceBlock GetIceBlock()
	{
		return instance.ib;
	}

	private void OnDestroy()
	{
		instance = null;
	}
}
