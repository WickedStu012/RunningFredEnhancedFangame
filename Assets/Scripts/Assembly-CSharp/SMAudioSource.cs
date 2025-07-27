using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public abstract class SMAudioSource : MonoBehaviour
{
	private int sndIdInt;

	private SoundList[] soundList;

	private bool refreshSoundProps;

	private void Start()
	{
	}

	private void OnEnable()
	{
		refreshSoundProps = true;
	}

	private void Update()
	{
		if (refreshSoundProps)
		{
			soundList = SoundManager.GetSoundList();
			RefreshSoundPropProperties();
			refreshSoundProps = false;
		}
	}

	public void RefreshSoundPropProperties()
	{
		if (soundList == null)
		{
			soundList = SoundManager.GetSoundList();
		}
		if (soundList == null)
		{
			Debug.LogError("The soundList returned is null.");
			return;
		}
		sndIdInt = getSndIdInt();
		SoundProp soundProp = GetSoundProp(sndIdInt);
		if (soundProp == null)
		{
			Debug.LogError(string.Format("Cannot find sndId: {0}", sndIdInt));
			return;
		}
		base.gameObject.GetComponent<AudioSource>().clip = soundProp.audioClip;
		base.gameObject.GetComponent<AudioSource>().loop = soundProp.loop;
		base.gameObject.GetComponent<AudioSource>().pitch = soundProp.pitch;
		base.gameObject.GetComponent<AudioSource>().panStereo = soundProp.pan;
		base.gameObject.GetComponent<AudioSource>().minDistance = soundProp.minDistance;
		base.gameObject.GetComponent<AudioSource>().maxDistance = soundProp.maxDistance;
		base.gameObject.GetComponent<AudioSource>().volume = SoundManager.FxVolume * SoundManager.MasterVolume * ((float)soundProp.volume / 100f);
	}

	private SoundProp GetSoundProp(int sndId)
	{
		if (soundList == null)
		{
			soundList = SoundManager.GetSoundList();
			if (soundList == null)
			{
				return null;
			}
		}
		int num = (int)((float)sndId / 1000f);
		if (num < soundList.Length)
		{
			return soundList[num].GetSoundProp(sndId);
		}
		return null;
	}

	protected abstract int getSndIdInt();
}
