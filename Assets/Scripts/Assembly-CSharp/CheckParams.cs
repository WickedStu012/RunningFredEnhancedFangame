using UnityEngine;

public class CheckParams : MonoBehaviour
{
	public string DefaultGUI = "Preferences";

	private bool firstUpdate = true;

	private void Awake()
	{
	}

	private void Start()
	{
	}

	private void OnEnable()
	{
	}

	private void OnDisable()
	{
	}

	private void Update()
	{
		if (firstUpdate)
		{
			if (SceneParamsManager.Instance.IsEmpty)
			{
				GUI3DManager.Instance.Activate(DefaultGUI, true, true);
			}
			else
			{
				string guiName = (string)SceneParamsManager.Instance.Pop();
				GUI3DManager.Instance.Activate(guiName, true, true);
			}
			firstUpdate = false;
			base.enabled = false;
		}
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			Application.Quit();
		}
	}
}
