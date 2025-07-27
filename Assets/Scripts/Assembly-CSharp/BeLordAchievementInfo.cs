using System;

public class BeLordAchievementInfo
{
	public string identifier;

	public bool isHidden;

	public bool completed;

	public DateTime lastReportedDate;

	public float percentComplete;

	public BeLordAchievementInfo(string id, bool isHidden, bool completed, DateTime lastDate, float perc)
	{
		identifier = id;
		this.isHidden = isHidden;
		this.completed = completed;
		lastReportedDate = lastDate;
		percentComplete = perc;
	}

	public BeLordAchievementInfo(string id)
	{
		identifier = id;
		isHidden = false;
		completed = false;
		lastReportedDate = DateTime.MinValue;
		percentComplete = 0f;
	}
}
