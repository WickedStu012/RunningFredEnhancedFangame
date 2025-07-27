public class CavePattern
{
	private const int LVL_COUNT = 10;

	public static LevelRandomUnit[][] pattern = new LevelRandomUnit[10][]
	{
		new LevelRandomUnit[2]
		{
			new LevelRandomUnit(LevelRandomUnitType.CON0, 0.3f),
			new LevelRandomUnit(LevelRandomUnitType.GRP0, 0.7f)
		},
		new LevelRandomUnit[3]
		{
			new LevelRandomUnit(LevelRandomUnitType.CON0, 0.2f),
			new LevelRandomUnit(LevelRandomUnitType.GRP0, 0.45f),
			new LevelRandomUnit(LevelRandomUnitType.GRP1, 0.35f)
		},
		new LevelRandomUnit[3]
		{
			new LevelRandomUnit(LevelRandomUnitType.CON0, 0.1f),
			new LevelRandomUnit(LevelRandomUnitType.GRP0, 0.4f),
			new LevelRandomUnit(LevelRandomUnitType.GRP1, 0.5f)
		},
		new LevelRandomUnit[3]
		{
			new LevelRandomUnit(LevelRandomUnitType.CON0, 0.1f),
			new LevelRandomUnit(LevelRandomUnitType.GRP0, 0.2f),
			new LevelRandomUnit(LevelRandomUnitType.GRP1, 0.7f)
		},
		new LevelRandomUnit[4]
		{
			new LevelRandomUnit(LevelRandomUnitType.CON0, 0.1f),
			new LevelRandomUnit(LevelRandomUnitType.GRP0, 0.1f),
			new LevelRandomUnit(LevelRandomUnitType.GRP1, 0.55f),
			new LevelRandomUnit(LevelRandomUnitType.GRP2, 0.25f)
		},
		new LevelRandomUnit[4]
		{
			new LevelRandomUnit(LevelRandomUnitType.CON0, 0.1f),
			new LevelRandomUnit(LevelRandomUnitType.GRP1, 0.25f),
			new LevelRandomUnit(LevelRandomUnitType.GRP2, 0.45f),
			new LevelRandomUnit(LevelRandomUnitType.GRP3, 0.2f)
		},
		new LevelRandomUnit[4]
		{
			new LevelRandomUnit(LevelRandomUnitType.CON0, 0.1f),
			new LevelRandomUnit(LevelRandomUnitType.GRP1, 0.2f),
			new LevelRandomUnit(LevelRandomUnitType.GRP2, 0.4f),
			new LevelRandomUnit(LevelRandomUnitType.GRP3, 0.3f)
		},
		new LevelRandomUnit[5]
		{
			new LevelRandomUnit(LevelRandomUnitType.CON0, 0.05f),
			new LevelRandomUnit(LevelRandomUnitType.GRP1, 0.1f),
			new LevelRandomUnit(LevelRandomUnitType.GRP2, 0.3f),
			new LevelRandomUnit(LevelRandomUnitType.GRP3, 0.4f),
			new LevelRandomUnit(LevelRandomUnitType.GRP4, 0.15f)
		},
		new LevelRandomUnit[5]
		{
			new LevelRandomUnit(LevelRandomUnitType.CON0, 0.05f),
			new LevelRandomUnit(LevelRandomUnitType.GRP1, 0.05f),
			new LevelRandomUnit(LevelRandomUnitType.GRP2, 0.1f),
			new LevelRandomUnit(LevelRandomUnitType.GRP3, 0.4f),
			new LevelRandomUnit(LevelRandomUnitType.GRP4, 0.4f)
		},
		new LevelRandomUnit[5]
		{
			new LevelRandomUnit(LevelRandomUnitType.CON0, 0.05f),
			new LevelRandomUnit(LevelRandomUnitType.GRP1, 0.05f),
			new LevelRandomUnit(LevelRandomUnitType.GRP2, 0.1f),
			new LevelRandomUnit(LevelRandomUnitType.GRP3, 0.2f),
			new LevelRandomUnit(LevelRandomUnitType.GRP4, 0.6f)
		}
	};
}
