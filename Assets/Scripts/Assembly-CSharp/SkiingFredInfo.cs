using System.Collections;

public class SkiingFredInfo
{
	public bool Available;

	public string StoreLink;

	public string ImageLink;

	public SkiingFredInfo()
	{
		Available = false;
		StoreLink = null;
		ImageLink = null;
	}

	public SkiingFredInfo(Hashtable ht)
	{
		Available = (bool)ht["Available"];
		StoreLink = (string)ht["StoreLink"];
		ImageLink = (string)ht["ImageLink"];
	}
}
