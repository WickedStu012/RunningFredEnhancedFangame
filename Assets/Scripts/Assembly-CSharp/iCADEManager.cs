using UnityEngine;

public class iCADEManager : MonoBehaviour
{
	private static iCADEManager instance;

	private void Start()
	{
	}

	private void OnEnable()
	{
		instance = this;
	}

	private void OnDisable()
	{
		instance = null;
	}

	private void Update()
	{
	}

	public static void EnableiCade()
	{
		if (instance != null)
		{
			iCadeBinding.setActive(true);
		}
	}

	public static void DisableiCade()
	{
		if (instance != null)
		{
			iCadeBinding.setActive(false);
		}
	}
}
