using UnityEngine;

public class SoundProp
{
	private const string DEF_NAME = "";

	private const int DEF_PRIORITY = 128;

	private const int DEF_VOLUME = 100;

	private const SndType DEF_SND_TYPE = SndType.SND_FX;

	private const bool DEF_LOOP_VALUE = false;

	private const float DEF_PITCH = 1f;

	private const float DEF_PAN = 0f;

	private const float DEF_MIN_DISTANCE = 1f;

	private const float DEF_MAX_DISTANCE = 200f;

	public int id;

	public string name;

	public SndType type;

	public int priority;

	public AudioClip audioClip;

	public bool loop;

	public int volume;

	public float pitch;

	public float pan;

	public float minDistance;

	public float maxDistance;

	public SoundProp()
	{
		priority = 128;
		type = SndType.SND_FX;
		loop = false;
		volume = 100;
		pitch = 1f;
		pan = 0f;
		minDistance = 1f;
		maxDistance = 200f;
	}

	public SoundProp(int sndId, string name, int priority)
	{
		id = sndId;
		this.name = name;
		this.priority = priority;
		type = SndType.SND_FX;
		loop = false;
		volume = 100;
		pitch = 1f;
		pan = 0f;
		minDistance = 1f;
		maxDistance = 200f;
	}

	public SoundProp(int sndId, string name, int priority, int volume)
	{
		id = sndId;
		this.name = name;
		this.priority = priority;
		loop = false;
		type = SndType.SND_FX;
		this.volume = volume;
		pitch = 1f;
		pan = 0f;
		minDistance = 1f;
		maxDistance = 200f;
	}

	public SoundProp(int sndId, string name, int priority, bool loop, SndType type)
	{
		id = sndId;
		this.name = name;
		this.priority = priority;
		this.loop = loop;
		this.type = type;
		volume = 100;
		pitch = 1f;
		pan = 0f;
		minDistance = 1f;
		maxDistance = 200f;
	}

	public SoundProp(int sndId, string name, int priority, bool loop, SndType type, int volume)
	{
		id = sndId;
		this.name = name;
		this.priority = priority;
		this.loop = loop;
		this.type = type;
		this.volume = volume;
		pitch = 1f;
		pan = 0f;
		minDistance = 1f;
		maxDistance = 200f;
	}
}
