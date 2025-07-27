public class SoundListCredits : SoundList
{
	private SoundProp[] sounds = new SoundProp[3]
	{
		new SoundProp(0, "Music-RF-8-Intro-Credits", 1, false, SndType.SND_MUSIC, 35),
		new SoundProp(1, "Music-RF-8-Loop-Credits", 1, true, SndType.SND_MUSIC, 35),
		new SoundProp(2, "Click", 1, false, SndType.SND_FX, 100)
	};

	private new void Start()
	{
		base.Start();
	}

	public override int GetByName(string soundName)
	{
		for (int i = 0; i < sounds.Length; i++)
		{
			if (string.Compare(sounds[i].name, soundName, true) == 0)
			{
				return sounds[i].id;
			}
		}
		return -1;
	}

	protected override SoundProp[] GetSoundProps()
	{
		return sounds;
	}

	public override int GetFirstIdNum()
	{
		return 0;
	}
}
