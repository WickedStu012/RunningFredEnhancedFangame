public class FileNameUtil
{
	public static string GetFilePath(string strFilename)
	{
		int num = strFilename.LastIndexOf("/");
		if (num == -1)
		{
			num = strFilename.LastIndexOf("\\");
		}
		if (num == -1)
		{
			return string.Empty;
		}
		return strFilename.Substring(0, num);
	}

	public static string GetFileName(string strFilename)
	{
		int num = strFilename.LastIndexOf("/");
		if (num == -1)
		{
			num = strFilename.LastIndexOf("\\");
		}
		if (num == -1)
		{
			return strFilename;
		}
		return strFilename.Substring(num + 1, strFilename.Length - num - 1);
	}

	public static string ExtractFileExt(string strFilename)
	{
		int num = strFilename.LastIndexOf(".");
		if (num != -1)
		{
			return strFilename.Substring(0, num);
		}
		return strFilename;
	}

	public static string ChangeFileExt(string strFilename, string strExt)
	{
		int num = strFilename.LastIndexOf('.');
		if (num != 1)
		{
			return strFilename.Substring(0, num) + strExt;
		}
		return strFilename;
	}

	public static string GetFileExt(string strFilename)
	{
		int num = strFilename.LastIndexOf('.');
		if (num != 1)
		{
			return strFilename.Substring(num, strFilename.Length - num);
		}
		return strFilename;
	}
}
