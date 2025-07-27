public class LevelRandomUnit
{
	public LevelRandomUnitType unitType;

	public float prob;

	public float probAccum;

	public LevelRandomUnit(LevelRandomUnitType unitType, float prob)
	{
		this.unitType = unitType;
		this.prob = prob;
	}
}
