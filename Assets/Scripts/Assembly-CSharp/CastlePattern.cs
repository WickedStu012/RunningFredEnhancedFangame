public class CastlePattern
{
	private const int LVL_COUNT = 10;

	public static LevelRandomUnit[][] pattern = new LevelRandomUnit[10][]
	{
		new LevelRandomUnit[2]
		{
			new LevelRandomUnit(LevelRandomUnitType.CON0, 0.4f),
			new LevelRandomUnit(LevelRandomUnitType.GRP0, 0.6f)
		},
		new LevelRandomUnit[2]
		{
			new LevelRandomUnit(LevelRandomUnitType.CON0, 0.3f),
			new LevelRandomUnit(LevelRandomUnitType.GRP0, 0.7f)
		},
		new LevelRandomUnit[3]
		{
			new LevelRandomUnit(LevelRandomUnitType.CON0, 0.1f),
			new LevelRandomUnit(LevelRandomUnitType.GRP0, 0.8f),
			new LevelRandomUnit(LevelRandomUnitType.GRP1, 0.1f)
		},
		new LevelRandomUnit[3]
		{
			new LevelRandomUnit(LevelRandomUnitType.CON0, 0.1f),
			new LevelRandomUnit(LevelRandomUnitType.GRP0, 0.5f),
			new LevelRandomUnit(LevelRandomUnitType.GRP1, 0.4f)
		},
		new LevelRandomUnit[3]
		{
			new LevelRandomUnit(LevelRandomUnitType.CON0, 0.1f),
			new LevelRandomUnit(LevelRandomUnitType.GRP1, 0.8f),
			new LevelRandomUnit(LevelRandomUnitType.GRP2, 0.1f)
		},
		new LevelRandomUnit[3]
		{
			new LevelRandomUnit(LevelRandomUnitType.CON0, 0.1f),
			new LevelRandomUnit(LevelRandomUnitType.GRP1, 0.5f),
			new LevelRandomUnit(LevelRandomUnitType.GRP2, 0.4f)
		},
		new LevelRandomUnit[3]
		{
			new LevelRandomUnit(LevelRandomUnitType.CON0, 0.05f),
			new LevelRandomUnit(LevelRandomUnitType.GRP2, 0.15f),
			new LevelRandomUnit(LevelRandomUnitType.GRP3, 0.8f)
		},
		new LevelRandomUnit[3]
		{
			new LevelRandomUnit(LevelRandomUnitType.CON0, 0.05f),
			new LevelRandomUnit(LevelRandomUnitType.GRP4, 0.75f),
			new LevelRandomUnit(LevelRandomUnitType.GRP3, 0.2f)
		},
		new LevelRandomUnit[2]
		{
			new LevelRandomUnit(LevelRandomUnitType.GRP3, 0.1f),
			new LevelRandomUnit(LevelRandomUnitType.GRP4, 0.9f)
		},
		new LevelRandomUnit[1]
		{
			new LevelRandomUnit(LevelRandomUnitType.GRP4, 1f)
		}
	};
}
