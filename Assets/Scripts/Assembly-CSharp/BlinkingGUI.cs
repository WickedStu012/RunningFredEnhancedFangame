using UnityEngine;

public class BlinkingGUI : MonoBehaviour
{
	public float Timer = 1f;

	private float time;

	private void Awake()
	{
		time = Time.time;
	}

	private void Update()
	{
		if (Time.time - time >= Timer)
		{
			time = Time.time;
			if (base.GetComponent<Renderer>() != null)
			{
				base.GetComponent<Renderer>().enabled = !base.GetComponent<Renderer>().enabled;
			}
		}
	}
}
