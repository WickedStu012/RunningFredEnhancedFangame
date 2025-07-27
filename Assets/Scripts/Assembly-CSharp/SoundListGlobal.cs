public class SoundListGlobal : SoundList
{
	private SoundProp[] sounds = new SoundProp[75]
	{
		new SoundProp(0, "Jump", 1, 100),
		new SoundProp(1, "JumpDouble", 1, 100),
		new SoundProp(2, "Click", 1, false, SndType.SND_FX, 100),
		new SoundProp(3, "GoldSkully", 1, 100),
		new SoundProp(4, "SilverSkully", 1, 100),
		new SoundProp(5, "FloorImpact", 1, 100),
		new SoundProp(6, "Gore_Impact_Generic", 1, 100),
		new SoundProp(7, "Material_WoodCrack", 1, 80),
		new SoundProp(8, "Purchase", 1, false, SndType.SND_FX, 100),
		new SoundProp(9, "Avatar_Burnt", 1, 100),
		new SoundProp(10, "Avatar_FinalGasp_Fred", 1, 100),
		new SoundProp(11, "Trigger_MetalTrigger", 1, 100),
		new SoundProp(12, "Material_Crank", 1, 100),
		new SoundProp(13, "Avatar_UseJetpack", 1, true, SndType.SND_FX, 100),
		new SoundProp(14, "Trap_MetalSnap", 1, 100),
		new SoundProp(15, "Avatar_GenericPain_Fred", 1, 100),
		new SoundProp(16, "Avatar_Step_Speed1", 1, 100),
		new SoundProp(17, "Avatar_Step_Speed2", 1, 100),
		new SoundProp(18, "Jetpack", 1, 100),
		new SoundProp(19, "Fuel", 1, 100),
		new SoundProp(20, "Trap_BoulderLoop", 1, true, SndType.SND_FX, 100),
		new SoundProp(21, "Trap_HallSaw", 1, true, SndType.SND_FX, 100),
		new SoundProp(22, "Avatar_Bouncer", 1, false, SndType.SND_FX, 100),
		new SoundProp(23, "Avatar_Ouch", 1, false, SndType.SND_FX, 100),
		new SoundProp(24, "LevelWon", 1, false, SndType.SND_FX, 100),
		new SoundProp(25, "Chandelier", 1, false, SndType.SND_FX, 100),
		new SoundProp(26, "Explosion1", 1, false, SndType.SND_FX, 100),
		new SoundProp(27, "DownAbyss", 1, false, SndType.SND_FX, 100),
		new SoundProp(28, "Irongates", 1, false, SndType.SND_FX, 100),
		new SoundProp(29, "WoodenDoor", 1, false, SndType.SND_FX, 100),
		new SoundProp(30, "Avatar_Bounce", 1, false, SndType.SND_FX, 100),
		new SoundProp(31, "MisteryBox", 1, false, SndType.SND_FX, 100),
		new SoundProp(32, "Avatar_Teleporter", 1, false, SndType.SND_FX, 100),
		new SoundProp(33, "Gore_Spurr", 1, false, SndType.SND_FX, 100),
		new SoundProp(34, "Avatar_Catapult", 1, false, SndType.SND_FX, 100),
		new SoundProp(35, "Avatar_DragLoop", 1, true, SndType.SND_FX, 100),
		new SoundProp(36, "Avatar_JumpLanding", 1, false, SndType.SND_FX, 50),
		new SoundProp(37, "Trap_FireBlowerLoop", 1, true, SndType.SND_FX, 100),
		new SoundProp(38, "Avatar_GlideLoop", 1, true, SndType.SND_FX, 30),
		new SoundProp(39, "Excited2", 1, false, SndType.SND_FX, 50),
		new SoundProp(40, "Excited3", 1, false, SndType.SND_FX, 50),
		new SoundProp(41, "Grab4", 1, false, SndType.SND_FX, 50),
		new SoundProp(42, "Groan2", 1, false, SndType.SND_FX, 50),
		new SoundProp(43, "ReaperSwing", 1, false, SndType.SND_FX, 50),
		new SoundProp(44, "IntroDing", 1, false, SndType.SND_FX, 50),
		new SoundProp(45, "Material_MetalDrag", 1, false, SndType.SND_FX, 50),
		new SoundProp(46, "Derrota", 0, false, SndType.SND_FX, 100),
		new SoundProp(47, "Victory", 0, false, SndType.SND_FX, 100),
		new SoundProp(48, "Checkpoint", 1, false, SndType.SND_FX, 100),
		new SoundProp(49, "GoldMedal", 1, false, SndType.SND_FX, 100),
		new SoundProp(50, "SilverMedal", 1, false, SndType.SND_FX, 100),
		new SoundProp(51, "BronzeMedal", 1, false, SndType.SND_FX, 100),
		new SoundProp(52, "LogImpact", 1, false, SndType.SND_FX, 100),
		new SoundProp(53, "ReaperNearby", 1, true, SndType.SND_FX, 100),
		new SoundProp(54, "Achievement", 1, false, SndType.SND_FX, 100),
		new SoundProp(55, "Gore_Cripsy", 1, true, SndType.SND_FX, 40),
		new SoundProp(56, "Gore_Tear", 1, false, SndType.SND_FX, 100),
		new SoundProp(57, "Avatar_Accelerator", 1, false, SndType.SND_FX, 100),
		new SoundProp(58, "Tutorial_Doors", 1, false, SndType.SND_FX, 50),
		new SoundProp(59, "NoPowerLeft", 1, false, SndType.SND_FX, 100),
		new SoundProp(60, "ResurrectLife1", 1, false, SndType.SND_FX, 100),
		new SoundProp(61, "ResurrectLife2", 1, false, SndType.SND_FX, 100),
		new SoundProp(62, "ResurrectLife3", 1, false, SndType.SND_FX, 100),
		new SoundProp(63, "ResurrectLife4", 1, false, SndType.SND_FX, 100),
		new SoundProp(64, "ResurrectLife5", 1, false, SndType.SND_FX, 100),
		new SoundProp(65, "Resurrect", 1, false, SndType.SND_FX, 100),
		new SoundProp(66, "BounceSpring", 1, false, SndType.SND_FX, 100),
		new SoundProp(67, "Shield", 1, false, SndType.SND_FX, 100),
		new SoundProp(68, "Gore_Freezing", 1, false, SndType.SND_FX, 100),
		new SoundProp(69, "Grinder", 1, true, SndType.SND_FX, 50),
		new SoundProp(70, "GuillotinaVertical", 1, false, SndType.SND_FX, 100),
		new SoundProp(71, "Crusher_Hitground", 1, false, SndType.SND_FX, 100),
		new SoundProp(72, "IdolPickup", 1, false, SndType.SND_FX, 100),
		new SoundProp(73, "WingsPickup", 1, false, SndType.SND_FX, 100),
		new SoundProp(74, "jetpackOverheating", 1, true, SndType.SND_FX, 100)
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
