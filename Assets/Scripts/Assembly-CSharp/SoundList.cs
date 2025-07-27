using UnityEngine;

public abstract class SoundList : MonoBehaviour
{
	public AudioClip[] soundClips;

	public bool loadedFromResources = true;

	protected SoundProp[] _sounds;

	protected void Start()
	{
		_sounds = GetSoundProps();
		if (_sounds == null)
		{
			Debug.Log("[ERROR] You should define a SoundProp vector at SoundList... class.");
			return;
		}
		if (loadedFromResources)
		{
			soundClips = new AudioClip[_sounds.Length];
			for (int i = 0; i < _sounds.Length; i++)
			{
				if (_sounds[i].type == SndType.SND_FX)
				{
					soundClips[i] = Resources.Load(string.Format("Sounds/{0}", _sounds[i].name)) as AudioClip;
				}
				else
				{
					soundClips[i] = Resources.Load(string.Format("Music/{0}", _sounds[i].name)) as AudioClip;
				}
			}
		}
		for (int j = 0; j < soundClips.Length; j++)
		{
			if (soundClips[j] != null)
			{
				SoundProp soundPropByName = getSoundPropByName(soundClips[j].name);
				if (soundPropByName != null)
				{
					soundPropByName.audioClip = soundClips[j];
				}
				else
				{
					Debug.LogWarning(string.Format("Cannot find the sound {0} on the array list of sounds.", soundClips[j].name));
				}
			}
		}
	}

	public SoundProp getSoundPropByName(string name)
	{
		for (int i = 0; i < _sounds.Length; i++)
		{
			if (string.Compare(name, _sounds[i].name, true) == 0)
			{
				return _sounds[i];
			}
		}
		return null;
	}

	public SoundProp GetSoundProp(int sndId)
	{
		if (_sounds == null)
		{
			return null;
		}
		sndId %= 1000;
		if (sndId < _sounds.Length)
		{
			return _sounds[sndId];
		}
		return null;
	}

	protected abstract SoundProp[] GetSoundProps();

	public abstract int GetByName(string soundName);

	public abstract int GetFirstIdNum();
}
