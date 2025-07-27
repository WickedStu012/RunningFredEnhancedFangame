using UnityEngine;

public class FadeAudioSource
{
	public float targetVolume;

	public float initialVolume;

	public float fadeInSecs;

	public float initialTime;

	public float accumTime;

	public AudioSource audioSrc;

	public SoundManagerCallback fnCb;
}
