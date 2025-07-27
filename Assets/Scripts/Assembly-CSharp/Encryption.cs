using System;
using System.Security.Cryptography;
using System.Text;

public class Encryption : IEncryption
{
	public string GetMD5(string str, string variableValue)
	{
		string empty = string.Empty;
		byte[] array = null;
		byte[] array2 = null;
		MD5 mD = new MD5CryptoServiceProvider();
		array = Encoding.UTF8.GetBytes(str + variableValue);
		array2 = mD.ComputeHash(array);
		return BitConverter.ToString(array2);
	}

	public string EncryptData(string Message, string key)
	{
		if (Message == string.Empty || Message == null)
		{
			return null;
		}
		return XOREncryption.Encrypt(Message, key);
	}

	public string DecryptData(string Message, string key)
	{
		if (Message == string.Empty || Message == null)
		{
			return null;
		}
		return XOREncryption.Decrypt(Message, key);
	}
}
