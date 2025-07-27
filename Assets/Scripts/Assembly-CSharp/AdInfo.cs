using System;
using System.Collections;
using UnityEngine;

public class AdInfo
{
	public int Id;

	public string Message;

	public string ImageURL;

	public int ImageWidth;

	public int ImageHeight;

	public string LinkURL;

	public Texture2D Image;

	public double Probability;

	public double ProbabilityAccum;

	private BlackLordRes cb;

	private static Texture2D cachedImage;

	private static string cachedImageImageURL;

	public AdInfo(Hashtable ht)
	{
		if (ht.ContainsKey("Id"))
		{
			Id = Convert.ToInt32(ht["Id"]);
		}
		Message = (string)ht["Message"];
		ImageURL = (string)ht["ImageURL"];
		LinkURL = (string)ht["LinkURL"];
		Probability = (double)ht["Probability"];
		ProbabilityAccum = 0.0;
		ImageWidth = 0;
		ImageHeight = 0;
	}

	public void LoadImage(OnImageLoadedRes onImageLoaded)
	{
		if (AdImageCacheManager.Instance == null)
		{
			Debug.LogError("AdImageCacheManager.Instance is null");
		}
		else
		{
			AdImageCacheManager.Instance.LoadImage(ImageURL, onImageLoaded);
		}
	}
}
