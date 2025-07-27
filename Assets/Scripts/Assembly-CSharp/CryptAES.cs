using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

public class CryptAES
{
	private static byte[] _salt = Encoding.ASCII.GetBytes("p3306641kbM3c4");

	private static string sharedSecret = "someday, somehow";

	public static string Encrypt(string plainText)
	{
		return EncryptB64(StringUtil.EncodeTo64(plainText));
	}

	public static string Decrypt(string textB64)
	{
		return StringUtil.DecodeFrom64(DecryptB64(textB64));
	}

	public static string EncryptB64(string textB64)
	{
		byte[] msg = Convert.FromBase64String(textB64);
		byte[] inArray = Encrypt(msg);
		return Convert.ToBase64String(inArray);
	}

	public static string DecryptB64(string textB64)
	{
		byte[] cipherMsg = Convert.FromBase64String(textB64);
		byte[] inArray = Decrypt(cipherMsg);
		return Convert.ToBase64String(inArray);
	}

	public static byte[] Encrypt(byte[] msg)
	{
		if (msg == null)
		{
			return null;
		}
		byte[] array = null;
		RijndaelManaged rijndaelManaged = null;
		try
		{
			Rfc2898DeriveBytes rfc2898DeriveBytes = new Rfc2898DeriveBytes(sharedSecret, _salt);
			rijndaelManaged = new RijndaelManaged();
			rijndaelManaged.Key = rfc2898DeriveBytes.GetBytes(rijndaelManaged.KeySize / 8);
			rijndaelManaged.IV = rfc2898DeriveBytes.GetBytes(rijndaelManaged.BlockSize / 8);
			ICryptoTransform transform = rijndaelManaged.CreateEncryptor(rijndaelManaged.Key, rijndaelManaged.IV);
			using (MemoryStream memoryStream = new MemoryStream())
			{
				using (CryptoStream stream = new CryptoStream(memoryStream, transform, CryptoStreamMode.Write))
				{
					using (StreamWriter streamWriter = new StreamWriter(stream))
					{
						streamWriter.Write(Convert.ToBase64String(msg));
					}
				}
				return memoryStream.ToArray();
			}
		}
		finally
		{
			if (rijndaelManaged != null)
			{
				rijndaelManaged.Clear();
			}
		}
	}

	public static byte[] Decrypt(byte[] cipherMsg)
	{
		if (cipherMsg == null)
		{
			return null;
		}
		RijndaelManaged rijndaelManaged = null;
		byte[] array = null;
		try
		{
			Rfc2898DeriveBytes rfc2898DeriveBytes = new Rfc2898DeriveBytes(sharedSecret, _salt);
			rijndaelManaged = new RijndaelManaged();
			rijndaelManaged.Key = rfc2898DeriveBytes.GetBytes(rijndaelManaged.KeySize / 8);
			rijndaelManaged.IV = rfc2898DeriveBytes.GetBytes(rijndaelManaged.BlockSize / 8);
			ICryptoTransform transform = rijndaelManaged.CreateDecryptor(rijndaelManaged.Key, rijndaelManaged.IV);
			using (MemoryStream stream = new MemoryStream(cipherMsg))
			{
				using (CryptoStream stream2 = new CryptoStream(stream, transform, CryptoStreamMode.Read))
				{
					using (StreamReader streamReader = new StreamReader(stream2))
					{
						string s = streamReader.ReadToEnd();
						return Convert.FromBase64String(s);
					}
				}
			}
		}
		finally
		{
			if (rijndaelManaged != null)
			{
				rijndaelManaged.Clear();
			}
		}
	}
}
