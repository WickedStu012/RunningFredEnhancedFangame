using System;

public class BeLordLeaderboardItem
{
	public int rank;

	public string alias;

	public string playerId;

	public bool isFriend;

	public long score;

	public string category;

	public DateTime date;

	public BeLordLeaderboardItem(int rank, string alias, string playerId, bool isFriend, long score, string catId, DateTime date)
	{
		this.rank = rank;
		this.alias = alias;
		this.playerId = playerId;
		this.isFriend = isFriend;
		this.score = score;
		category = catId;
		this.date = date;
	}
}
