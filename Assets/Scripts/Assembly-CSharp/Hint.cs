internal class Hint
{
	public string HintId;

	public string Icon;

	public float IconScale;

	public Hint(string hintId, string icon)
	{
		HintId = hintId;
		Icon = icon;
		IconScale = 1f;
	}

	public Hint(string hintId, string icon, float iconScale)
	{
		HintId = hintId;
		Icon = icon;
		IconScale = iconScale;
	}
}
