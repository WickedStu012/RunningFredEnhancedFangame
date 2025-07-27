using UnityEngine;

public class HeadRandomAnim : MonoBehaviour
{
	public float Timer = 2f;

	public string[] Animations;

	private float timer;

	private void Update()
	{
		if (Time.time - timer >= Timer)
		{
			if (base.GetComponent<Animation>() != null && Animations != null && Animations.Length > 0)
			{
				int num = Random.Range(0, Animations.Length);
				base.GetComponent<Animation>().wrapMode = WrapMode.Loop;
				base.GetComponent<Animation>().CrossFade(Animations[num]);
			}
			timer = Time.time;
		}
	}
}
