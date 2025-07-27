public class SoundListChallenge : SoundList
{
	private SoundProp[] sounds = new SoundProp[1]
	{
		new SoundProp(1000, "Challenge", 1, true, SndType.SND_MUSIC, 35)
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
		return 1000;
	}
}
