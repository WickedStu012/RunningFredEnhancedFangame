using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class AdImageCacheManager : MonoBehaviour
{
	public static AdImageCacheManager Instance;

	private List<string> storedImages;

	private UnityWebRequest www;
	private bool waitingResponse;
	private float accumTime;
	private OnImageLoadedRes cb;
	private string loadedTextureName;

	private void Awake()
	{
		Instance = this;
	}

	private void Start()
	{
		Object.DontDestroyOnLoad(this);
	}

	private void Update()
	{
		if (!waitingResponse || www == null)
		{
			return;
		}
		
		if (www.isDone)
		{
			waitingResponse = false;
			if (www.result == UnityWebRequest.Result.Success)
			{
				Texture2D texture = DownloadHandlerTexture.GetContent(www);
				SaveImageOnCache(loadedTextureName, texture);
				if (cb != null)
				{
					cb(true, null, texture);
				}
			}
			else if (cb != null)
			{
				cb(false, www.error, null);
			}
			
			// Properly dispose the UnityWebRequest to prevent memory leaks
			www.Dispose();
			www = null;
			return;
		}
		
		accumTime += Time.deltaTime;
		if (accumTime > 10f)
		{
			waitingResponse = false;
			if (cb != null)
			{
				cb(false, "timeout", null);
			}
			
			// Properly dispose the UnityWebRequest to prevent memory leaks
			www.Dispose();
			www = null;
		}
	}

	public bool LoadImage(string imageName, OnImageLoadedRes onImageLoadedRes)
	{
		Debug.Log(string.Format("--- AdLoadImage: {0}", imageName));
		if (onImageLoadedRes == null)
		{
			Debug.LogError("--- AdImageCacheManager.LoadImage. onImageLoadedRes cannot be null");
			return false;
		}
		cb = onImageLoadedRes;
		Texture2D texture2D = LoadImageFromCache(imageName);
		if (texture2D == null)
		{
			Debug.Log(string.Format("--- Load Image ({0}) from web", imageName));
			if (!LoadImageFromWeb(imageName, onImageLoadedRes))
			{
				onImageLoadedRes(false, "The object is already downloading an image.", null);
				return false;
			}
			return true;
		}
		onImageLoadedRes(true, null, texture2D);
		return true;
	}

	public bool IsImageInCache(string imageName)
	{
		string path = string.Format("{0}/{1}", Application.temporaryCachePath, imageName);
		return File.Exists(path);
	}

	public bool LoadImageFromWeb(string imageName, OnImageLoadedRes onImageLoadedRes)
	{
		if (waitingResponse)
		{
			return false;
		}
		string url = string.Format("{0}/ads/{1}", "https://black-lord.appspot.com", imageName);
		loadedTextureName = imageName;
		www = UnityWebRequestTexture.GetTexture(url);
		www.SendWebRequest();
		waitingResponse = true;
		accumTime = 0f;
		cb = onImageLoadedRes;
		return true;
	}

	public Texture2D LoadImageFromCache(string imageName)
	{
		string path = string.Format("{0}/{1}", Application.temporaryCachePath, imageName);
		if (File.Exists(path))
		{
			byte[] array = File.ReadAllBytes(path);
			if (array != null)
			{
				int width = PlayerPrefs.GetInt(string.Format("{0}-width", imageName), 0);
				int height = PlayerPrefs.GetInt(string.Format("{0}-height", imageName), 0);
				Texture2D texture2D = new Texture2D(width, height, TextureFormat.ARGB32, false, true);
				texture2D.LoadImage(array);
				return texture2D;
			}
		}
		return null;
	}

	private void SaveImageOnCache(string imageName, Texture2D tex)
	{
		PlayerPrefs.SetInt(string.Format("{0}-width", imageName), tex.width);
		PlayerPrefs.SetInt(string.Format("{0}-height", imageName), tex.height);
		string path = string.Format("{0}/{1}", Application.temporaryCachePath, imageName);
		File.WriteAllBytes(path, tex.EncodeToPNG());
	}
}
