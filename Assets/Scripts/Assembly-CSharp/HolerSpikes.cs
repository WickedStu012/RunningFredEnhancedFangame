using UnityEngine;

public class HolerSpikes : MonoBehaviour
{
	private float accumTime;

	private float animLen;

	private int sndId;

	private bool movingUp;

	private void Start()
	{
		movingUp = false;
	}

	private void Update()
	{
		if (movingUp)
		{
			accumTime += Time.deltaTime;
			if (accumTime >= animLen)
			{
				SoundManager.StopSound(sndId);
			}
		}
	}

	public void Trigger(float spd)
	{
		base.GetComponent<Animation>().Play("HolerSpikes");
		SoundManager.PlaySound(SndId.SND_METAL_TRIGGER);
	}

	public void MoveUp()
	{
		animLen = base.GetComponent<Animation>()["HolerSpikesUp"].length;
		base.GetComponent<Animation>().Play("HolerSpikesUp");
		sndId = SoundManager.PlaySound(SndId.SND_MATERIAL_CRANK);
		movingUp = true;
	}
}
