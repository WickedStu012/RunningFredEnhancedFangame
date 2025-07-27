using System;
using System.Collections;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
	public const int CHANNELS = 16;

	private static SoundManager _instance;

	private static AudioSource[] _fx;

	private static AudioSource _music;

	private static AudioSource _musicIntro;

	private static AudioSource _background;

	private static SoundList[] _soundList;

	private static SoundManagerCallback _fadeOutAllSoundsCallback;

	private static bool _fadingAllSounds;

	private static ArrayList _asFades = new ArrayList();

	private static SoundProp _currentBackgroundSndProp = null;

	private static SoundProp _currentMusicSndProp = null;

	private static bool playingIntro = false;

	public static bool initialized = false;

	public static float MasterVolume
	{
		get
		{
			return ConfigParams.masterVolume;
		}
		set
		{
			ConfigParams.masterVolume = value;
		}
	}

	public static float BackgroundVolume
	{
		get
		{
			return ConfigParams.backgroundVolume;
		}
		set
		{
			ConfigParams.backgroundVolume = value;
			_background.volume = value * MasterVolume * ((_currentBackgroundSndProp == null) ? 1f : ((float)_currentBackgroundSndProp.volume / 100f));
		}
	}

	public static float MusicVolume
	{
		get
		{
			return ConfigParams.musicVolume;
		}
		set
		{
			ConfigParams.musicVolume = value;
			AudioSource musicIntro = _musicIntro;
			float volume = value * MasterVolume * ((_currentMusicSndProp == null) ? 1f : ((float)_currentMusicSndProp.volume / 100f));
			_music.volume = volume;
			musicIntro.volume = volume;
		}
	}

	public static float FxVolume
	{
		get
		{
			return ConfigParams.fxVolume;
		}
		set
		{
			ConfigParams.fxVolume = value;
			for (int i = 0; i < 16; i++)
			{
				_fx[i].volume = value * MasterVolume;
			}
		}
	}

	public static event Action callbackOnInitialized;

	private void Awake()
	{
		Init();
	}

	private void Init()
	{
		_instance = this;
		_soundList = GetSoundList();
		_music = base.gameObject.AddComponent(typeof(AudioSource)) as AudioSource;
		_music.playOnAwake = false;
		_musicIntro = base.gameObject.AddComponent(typeof(AudioSource)) as AudioSource;
		_musicIntro.playOnAwake = false;
		_background = base.gameObject.AddComponent(typeof(AudioSource)) as AudioSource;
		_background.playOnAwake = false;
		_fx = new AudioSource[16];
		for (int i = 0; i < 16; i++)
		{
			_fx[i] = base.gameObject.AddComponent(typeof(AudioSource)) as AudioSource;
			_fx[i].playOnAwake = false;
		}
	}

	private void Start()
	{
		MasterVolume = ConfigParams.masterVolume;
		BackgroundVolume = ConfigParams.backgroundVolume;
		MusicVolume = ConfigParams.musicVolume;
		FxVolume = ConfigParams.fxVolume;
		AudioSource musicIntro = _musicIntro;
		float volume = ConfigParams.musicVolume * MasterVolume;
		_music.volume = volume;
		musicIntro.volume = volume;
		_background.volume = ConfigParams.backgroundVolume * MasterVolume;
		_fadingAllSounds = false;
		if (!initialized)
		{
			initialized = true;
			if (SoundManager.callbackOnInitialized != null)
			{
				SoundManager.callbackOnInitialized();
			}
		}
	}

	private void OnEnable()
	{
		_instance = this;
	}

	private void OnDisable()
	{
		StopAll();
	}

	private void Update()
	{
		if (playingIntro && !_musicIntro.isPlaying)
		{
			playingIntro = false;
			_music.Play();
		}
		if (_soundList == null)
		{
			_soundList = GetSoundList();
		}
		if (Camera.main != null && _music != null && _background != null)
		{
			_music.transform.position = Camera.main.transform.position;
			_background.transform.position = Camera.main.transform.position;
		}
		foreach (FadeAudioSource asFade in _asFades)
		{
			asFade.accumTime = Time.realtimeSinceStartup - asFade.initialTime;
			float num = asFade.targetVolume - asFade.initialVolume;
			float num2 = num * (asFade.accumTime / asFade.fadeInSecs);
			asFade.audioSrc.volume = asFade.initialVolume + num2;
			if (asFade.accumTime >= asFade.fadeInSecs)
			{
				asFade.audioSrc.volume = asFade.targetVolume;
				_asFades.Remove(asFade);
				if (asFade.fnCb != null)
				{
					asFade.fnCb();
				}
				return;
			}
		}
		if (_fadingAllSounds && _asFades.Count == 0 && _fadeOutAllSoundsCallback != null)
		{
			_fadeOutAllSoundsCallback();
			_fadeOutAllSoundsCallback = null;
		}
	}

	public static void PauseMusic(bool pauseThem)
	{
		if (pauseThem)
		{
			_music.Pause();
			_musicIntro.Pause();
		}
		else if (_musicIntro.time > 0f)
		{
			_musicIntro.Play();
		}
		else if (_music.time > 0f)
		{
			_music.Play();
		}
	}

	public static void PauseAll(bool pauseThem)
	{
		if (pauseThem)
		{
			_background.Pause();
			_music.Pause();
			_musicIntro.Pause();
			for (int i = 0; i < 16; i++)
			{
				_fx[i].Pause();
			}
			foreach (FadeAudioSource asFade in _asFades)
			{
				asFade.audioSrc.volume = asFade.targetVolume;
			}
			_asFades.Clear();
			return;
		}
		if (_background.time > 0f)
		{
			_background.Play();
		}
		if (_musicIntro.time > 0f)
		{
			_musicIntro.Play();
		}
		else if (_music.time > 0f)
		{
			_music.Play();
		}
		for (int j = 0; j < 16; j++)
		{
			if (_fx[j].time > 0f)
			{
				_fx[j].Play();
			}
		}
	}

	public static void PlayBackground(int sndId)
	{
		if (!ConfigParams.useGameMusic)
		{
			return;
		}
		SoundProp soundProp = GetSoundProp(sndId);
		if (soundProp != null)
		{
			_currentBackgroundSndProp = soundProp;
			if (Camera.main != null)
			{
				_background.transform.position = Camera.main.transform.position;
			}
			_background.clip = soundProp.audioClip;
			_background.loop = soundProp.loop;
			_background.volume = BackgroundVolume * MasterVolume * ((float)soundProp.volume / 100f);
			_background.Play();
		}
	}

	public static SoundProp GetSoundProp(int sndId)
	{
		if (_soundList == null)
		{
			_soundList = GetSoundList();
			if (_soundList == null)
			{
				return null;
			}
		}
		int num = (int)((float)sndId / 1000f);
		if (num < _soundList.Length)
		{
			return _soundList[num].GetSoundProp(sndId);
		}
		return null;
	}

	public static void StopBackground()
	{
		_background.Stop();
	}

	public static void PlayMusic(int sndId)
	{
		PlayMusic(sndId, false);
	}

	public static void PlayMusic(int introId, int loopId)
	{
		if (!(_music == null))
		{
			PlayMusic(loopId);
			_music.Pause();
			PlayMusic(introId, loopId, false);
		}
	}

	public static void PlayMusic(int introId, int loopId, bool fadeOutCurrent)
	{
		if (!ConfigParams.useGameMusic || _instance == null)
		{
			return;
		}
		SoundProp soundProp = GetSoundProp(introId);
		playingIntro = true;
		if (soundProp != null)
		{
			_currentMusicSndProp = soundProp;
			if (fadeOutCurrent && _music.isPlaying)
			{
				FadeOutMusic(1f, playMusicAfterFadeOut);
			}
			else
			{
				playMusicAfterFadeOut();
			}
		}
	}

	public static void PlayMusic(int sndId, bool fadeOutCurrent)
	{
		if (!ConfigParams.useGameMusic || _instance == null)
		{
			return;
		}
		SoundProp soundProp = GetSoundProp(sndId);
		if (soundProp != null)
		{
			_currentMusicSndProp = soundProp;
			if (fadeOutCurrent && _music.isPlaying)
			{
				FadeOutMusic(1f, playMusicAfterFadeOut);
			}
			else
			{
				playMusicAfterFadeOut();
			}
		}
	}

	public static bool IsPlayingMusic()
	{
		return _music.isPlaying;
	}

	private static void playMusicAfterFadeOut()
	{
		if (playingIntro)
		{
			if (Camera.main != null)
			{
				_musicIntro.transform.position = Camera.main.transform.position;
			}
			_musicIntro.clip = _currentMusicSndProp.audioClip;
			_musicIntro.loop = _currentMusicSndProp.loop;
			_musicIntro.volume = MusicVolume * MasterVolume * ((float)_currentMusicSndProp.volume / 100f);
			_musicIntro.Play();
		}
		else
		{
			if (Camera.main != null)
			{
				_music.transform.position = Camera.main.transform.position;
			}
			_music.clip = _currentMusicSndProp.audioClip;
			_music.loop = _currentMusicSndProp.loop;
			_music.volume = MusicVolume * MasterVolume * ((float)_currentMusicSndProp.volume / 100f);
			_music.Play();
		}
	}

	public static void StopMusic()
	{
		playingIntro = false;
		_music.Stop();
		_musicIntro.Stop();
	}

	public static void FadeOutMusic(float inSecs, SoundManagerCallback cbfn)
	{
		FadeAudioSource fadeAudioSource = new FadeAudioSource();
		fadeAudioSource.initialTime = Time.realtimeSinceStartup;
		fadeAudioSource.accumTime = 0f;
		if (playingIntro)
		{
			fadeAudioSource.initialVolume = _musicIntro.volume;
			fadeAudioSource.audioSrc = _musicIntro;
		}
		else
		{
			fadeAudioSource.initialVolume = _music.volume;
			fadeAudioSource.audioSrc = _music;
		}
		fadeAudioSource.fadeInSecs = inSecs;
		fadeAudioSource.targetVolume = 0f;
		fadeAudioSource.fnCb = (SoundManagerCallback)Delegate.Combine(fadeAudioSource.fnCb, cbfn);
		_asFades.Add(fadeAudioSource);
	}

	public static void FadeInMusic(int sndId, float inSecs, SoundManagerCallback cbfn)
	{
		if (!ConfigParams.useGameMusic)
		{
			return;
		}
		SoundProp soundProp = GetSoundProp(sndId);
		if (soundProp != null)
		{
			_currentMusicSndProp = soundProp;
			if (Camera.main != null)
			{
				_music.transform.position = Camera.main.transform.position;
			}
			_music.clip = soundProp.audioClip;
			_music.loop = soundProp.loop;
			_music.volume = 0f;
			_music.Play();
			FadeAudioSource fadeAudioSource = new FadeAudioSource();
			fadeAudioSource.initialTime = Time.realtimeSinceStartup;
			fadeAudioSource.accumTime = 0f;
			fadeAudioSource.initialVolume = 0f;
			fadeAudioSource.targetVolume = MusicVolume * MasterVolume * ((float)soundProp.volume / 100f);
			fadeAudioSource.audioSrc = _music;
			fadeAudioSource.fadeInSecs = inSecs;
			if (cbfn != null)
			{
				fadeAudioSource.fnCb = (SoundManagerCallback)Delegate.Combine(fadeAudioSource.fnCb, cbfn);
			}
			else
			{
				fadeAudioSource.fnCb = null;
			}
			_asFades.Add(fadeAudioSource);
		}
	}

	public static void FadeOutBackground(float inSecs, SoundManagerCallback cbfn)
	{
		FadeAudioSource fadeAudioSource = new FadeAudioSource();
		fadeAudioSource.initialTime = Time.realtimeSinceStartup;
		fadeAudioSource.accumTime = 0f;
		fadeAudioSource.initialVolume = _background.volume;
		fadeAudioSource.targetVolume = 0f;
		fadeAudioSource.audioSrc = _background;
		fadeAudioSource.fadeInSecs = inSecs;
		fadeAudioSource.fnCb = (SoundManagerCallback)Delegate.Combine(fadeAudioSource.fnCb, cbfn);
		_asFades.Add(fadeAudioSource);
	}

	public static int PlaySound<T>(T sndId)
	{
		return PlaySound(Convert.ToInt32(sndId));
	}

	public static int PlaySound(int sndId)
	{
		if (Camera.main != null)
		{
			return PlaySound(Camera.main.transform.position, sndId);
		}
		return -1;
	}

	public static int PlaySound(Vector3 pos, int sndId)
	{
		if (_fx == null)
		{
			return -1;
		}
		SoundProp soundProp = GetSoundProp(sndId);
		if (soundProp != null)
		{
			if (soundProp.type == SndType.SND_FX)
			{
				int channelIdx = getChannelIdx(soundProp);
				if (channelIdx != -1)
				{
					playThisSoundOnSource(channelIdx, soundProp, pos);
					return channelIdx;
				}
				Debug.Log("All audiosource are busy. Cannot play sound: " + soundProp.name);
			}
			else
			{
				Debug.Log(string.Format("Trying to play a sound that is not a FX ({0})", soundProp.name));
			}
		}
		return -1;
	}

	private static void playThisSoundOnSource(int idx, SoundProp sp, Vector3 pos)
	{
		_fx[idx].Stop();
		_fx[idx].clip = sp.audioClip;
		_fx[idx].loop = sp.loop;
		_fx[idx].volume = FxVolume * MasterVolume * ((float)sp.volume / 100f);
		_fx[idx].transform.position = pos;
		_fx[idx].Play();
	}

	private static int getChannelIdx(SoundProp sp)
	{
		for (int i = 1; i < 16; i++)
		{
			if (_fx[i].clip != null)
			{
				if (!_fx[i].isPlaying)
				{
					return i;
				}
				continue;
			}
			return i;
		}
		for (int j = 0; j < 16; j++)
		{
			if (_fx[j].clip != null)
			{
				SoundProp soundPropByName = getSoundPropByName(_fx[j].clip.name);
				if (sp.priority > soundPropByName.priority)
				{
					Debug.Log("Returning a used channel");
					return j;
				}
			}
		}
		for (int k = 0; k < 16; k++)
		{
			if (_fx[k].clip != null)
			{
				SoundProp soundPropByName2 = getSoundPropByName(_fx[k].clip.name);
				if (sp.priority == soundPropByName2.priority)
				{
					return k;
				}
			}
		}
		return -1;
	}

	private static SoundProp getSoundPropByName(string name)
	{
		if (_soundList == null)
		{
			_soundList = GetSoundList();
			if (_soundList == null)
			{
				return null;
			}
		}
		SoundList[] soundList = _soundList;
		foreach (SoundList soundList2 in soundList)
		{
			SoundProp soundPropByName = soundList2.getSoundPropByName(name);
			if (soundPropByName != null)
			{
				return soundPropByName;
			}
		}
		return null;
	}

	public static void StopSound(int channelIdx)
	{
		if (channelIdx != -1 && channelIdx < 16 && _fx[channelIdx] != null && _fx[channelIdx].clip != null)
		{
			_fx[channelIdx].Stop();
		}
	}

	public static void FadeOutSound(int channelIdx, float inSecs, SoundManagerCallback cbfn)
	{
		FadeAudioSource fadeAudioSource = new FadeAudioSource();
		fadeAudioSource.initialTime = Time.realtimeSinceStartup;
		fadeAudioSource.accumTime = 0f;
		fadeAudioSource.initialVolume = _fx[channelIdx].volume;
		fadeAudioSource.targetVolume = 0f;
		fadeAudioSource.audioSrc = _fx[channelIdx];
		fadeAudioSource.fadeInSecs = inSecs;
		fadeAudioSource.fnCb = (SoundManagerCallback)Delegate.Combine(fadeAudioSource.fnCb, cbfn);
		_asFades.Add(fadeAudioSource);
	}

	public static void FadeInSound(int sndId, float inSecs, SoundManagerCallback cbfn)
	{
		SoundProp soundProp = GetSoundProp(sndId);
		if (soundProp == null)
		{
			return;
		}
		int channelIdx = getChannelIdx(soundProp);
		if (channelIdx != -1)
		{
			if (Camera.main != null)
			{
				_fx[channelIdx].transform.position = Camera.main.transform.position;
			}
			_fx[channelIdx].clip = soundProp.audioClip;
			_fx[channelIdx].loop = soundProp.loop;
			_fx[channelIdx].volume = 0f;
			_fx[channelIdx].Play();
			FadeAudioSource fadeAudioSource = new FadeAudioSource();
			fadeAudioSource.initialTime = Time.realtimeSinceStartup;
			fadeAudioSource.accumTime = 0f;
			fadeAudioSource.initialVolume = 0f;
			fadeAudioSource.targetVolume = FxVolume * MasterVolume * ((float)soundProp.volume / 100f);
			fadeAudioSource.audioSrc = _fx[channelIdx];
			fadeAudioSource.fadeInSecs = inSecs;
			if (cbfn != null)
			{
				fadeAudioSource.fnCb = (SoundManagerCallback)Delegate.Combine(fadeAudioSource.fnCb, cbfn);
			}
			else
			{
				fadeAudioSource.fnCb = null;
			}
			_asFades.Add(fadeAudioSource);
		}
	}

	public static void StopAll()
	{
		if ((bool)_background)
		{
			_background.Stop();
		}
		if ((bool)_music)
		{
			_music.Stop();
		}
		if ((bool)_musicIntro)
		{
			_musicIntro.Stop();
		}
		for (int i = 0; i < 16; i++)
		{
			if (_fx != null && _fx[i] != null)
			{
				_fx[i].Stop();
			}
		}
		_asFades.Clear();
	}

	public static void FadeOutAll(float inSecs, SoundManagerCallback cbfn)
	{
		foreach (FadeAudioSource asFade in _asFades)
		{
			asFade.audioSrc.volume = asFade.targetVolume;
		}
		_asFades.Clear();
		FadeOutMusic(inSecs, null);
		FadeOutBackground(inSecs, null);
		for (int i = 0; i < 16; i++)
		{
			if (_fx[i].isPlaying)
			{
				FadeOutSound(i, inSecs, null);
			}
		}
		_fadingAllSounds = true;
		_fadeOutAllSoundsCallback = (SoundManagerCallback)Delegate.Combine(_fadeOutAllSoundsCallback, cbfn);
	}

	public static AudioSource GetChannelById(int id)
	{
		return _fx[id];
	}

	public static SoundList[] GetSoundList()
	{
		if (_instance != null)
		{
			_soundList = null;
			SoundList[] components = _instance.GetComponents<SoundList>();
			if (components != null)
			{
				_soundList = new SoundList[components.Length];
				SoundList[] array = components;
				foreach (SoundList soundList in array)
				{
					int num = (int)((float)soundList.GetFirstIdNum() / 1000f);
					_soundList[num] = soundList;
				}
			}
			else
			{
				Debug.LogError("No sound lists found!");
			}
		}
		return _soundList;
	}
}
