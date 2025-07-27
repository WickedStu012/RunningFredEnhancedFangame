using UnityEngine;

public class PlaySoundOnEnable : MonoBehaviour
{
	public SndId SoundId;

	private void Awake()
	{
	}

	private void Start()
	{
	}

	private void OnEnable()
	{
		SoundManager.PlaySound((int)SoundId);
	}

	private void OnDisable()
	{
	}

	private void Update()
	{
	}
}
