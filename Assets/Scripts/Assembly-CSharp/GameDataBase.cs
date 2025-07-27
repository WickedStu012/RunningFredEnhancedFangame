using System.Collections.Generic;
using System.IO;
using System.Text;
using ICSharpCode.SharpZipLib.GZip;
using UnityEngine;

public abstract class GameDataBase
{
	public enum RevisionResult
	{
		Error = -1,
		Equal = 0,
		Lower = 1,
		Higher = 2
	}

	public enum eVersionCheckResult
	{
		NOT_CHECKED = 0,
		SAME_VERSION = 1,
		SERVER_HAS_GREATER_VERSION = 2,
		CLIENT_HAS_GREATER_VERSION = 3
	}

	public const string Magic = "DDL";

	public const string EncryptionMagic = "Dedalord";

	public byte Version = 1;

	public int Revision;

	public GameDataBase()
	{
	}

	protected MemoryStream Compress(MemoryStream sourceStream)
	{
		MemoryStream memoryStream = new MemoryStream();
		try
		{
			GZipOutputStream gZipOutputStream = new GZipOutputStream(memoryStream);
			gZipOutputStream.Write(sourceStream.GetBuffer(), 0, sourceStream.GetBuffer().Length);
			gZipOutputStream.Close();
		}
		catch
		{
			Debug.Log("[GameData:Compress] Error while packing...");
			return null;
		}
		finally
		{
			memoryStream.Close();
		}
		return memoryStream;
	}

	protected MemoryStream Decompress(MemoryStream sourceStream)
	{
		try
		{
			int num = 2048;
			byte[] array = new byte[num];
			int num2 = 0;
			List<byte> list = new List<byte>();
			int num3 = 0;
			MemoryStream memoryStream = new MemoryStream();
			GZipInputStream gZipInputStream = new GZipInputStream(sourceStream);
			do
			{
				num2 = gZipInputStream.Read(array, num3, num);
				if (num2 > 0)
				{
					for (int i = 0; i < num2; i++)
					{
						list.Add(array[i]);
					}
					num3 += num2;
				}
			}
			while (num2 > 0);
			gZipInputStream.Close();
			byte[] array2 = list.ToArray();
			memoryStream.Write(array2, 0, array2.Length);
			memoryStream.Position = 0L;
			return memoryStream;
		}
		catch
		{
			Debug.Log("[GameData:Decompress] Error while unpacking...");
			return null;
		}
	}

	public string EncodeToBase64()
	{
		MemoryStream memoryStream = new MemoryStream();
		BinaryWriter binaryWriter = new BinaryWriter(memoryStream);
		try
		{
			binaryWriter.Write(Revision);
			OnWrite(binaryWriter);
			binaryWriter.Close();
			MemoryStream memoryStream2 = Compress(memoryStream);
			memoryStream.Close();
			byte[] buffer = memoryStream2.GetBuffer();
			byte[] bytes = Encoding.ASCII.GetBytes("Dedalord");
			byte[] array = new byte[buffer.Length + "Dedalord".Length];
			for (int i = 0; i < bytes.Length; i++)
			{
				array[i] = bytes[i];
			}
			for (int j = 0; j < buffer.Length; j++)
			{
				array[j + bytes.Length] = buffer[j];
			}
			byte[] array2 = Encrypt(array);
			byte[] bytes2 = Encoding.ASCII.GetBytes("DDL");
			byte[] array3 = new byte[array2.Length + bytes2.Length + 1];
			for (int k = 0; k < bytes2.Length; k++)
			{
				array3[k] = bytes2[k];
			}
			array3[bytes2.Length] = Version;
			for (int l = 0; l < array2.Length; l++)
			{
				array3[l + bytes2.Length + 1] = array2[l];
			}
			return StringUtil.EncodeTo64(array3);
		}
		catch
		{
			Debug.Log("[GameData:Encoding] Error while trying to encode...");
			return null;
		}
	}

	public bool DecodeFromBase64(string base64)
	{
		if (base64 == null || base64 == string.Empty)
		{
			Debug.Log("[GameData:Decoding] Base64 is empty...");
			return false;
		}
		byte[] bytes = Encoding.ASCII.GetBytes("DDL");
		byte[] array = null;
		try
		{
			array = StringUtil.DecodeFrom64ToByteArray(base64);
		}
		catch
		{
			Debug.Log("[GameData:CheckRevision] Base64 invalid length...");
			return false;
		}
		try
		{
			if (array == null || array.Length == 0)
			{
				Debug.Log("[GameData:CheckMagic] Base64 bytearray is empty...");
				return false;
			}
			if (array.Length <= bytes.Length)
			{
				Debug.Log("[GameData:CheckMagic] Bytes length is less than magic length...");
				return false;
			}
			for (int i = 0; i < bytes.Length && i < array.Length; i++)
			{
				if (bytes[i] != array[i])
				{
					Debug.Log("[GameData:CheckMagic] Magic types mismatch...");
					return false;
				}
			}
			if (array.Length <= bytes.Length)
			{
				Debug.Log("[GameData:CheckVersion] Base64 bytearray length is too small...");
				return false;
			}
			byte b = array[bytes.Length];
			if (b != Version)
			{
				Debug.Log("[GameData:CheckVersion] Versions mismatch. Will convert the data.");
				return Convert(base64);
			}
			if (array.Length <= bytes.Length + 1)
			{
				Debug.Log("[GameData:Decrypting] Base64 bytearray length is too small...");
				return false;
			}
			byte[] array2 = new byte[array.Length - 1 - bytes.Length];
			for (int j = 0; j < array2.Length; j++)
			{
				array2[j] = array[j + 1 + bytes.Length];
			}
			byte[] array3 = Decrypt(array2);
			byte[] bytes2 = Encoding.ASCII.GetBytes("Dedalord");
			if (array3.Length <= bytes2.Length)
			{
				Debug.Log("[GameData:CheckEncryptMagic] Base64 bytearray length is too small...");
				return false;
			}
			for (int k = 0; k < bytes2.Length; k++)
			{
				if (array3[k] != bytes2[k])
				{
					Debug.Log("[GameData:CheckEncryptMagic] Magic mismatch...");
					return false;
				}
			}
			byte[] array4 = new byte[array3.Length - bytes2.Length];
			for (int l = 0; l < array4.Length; l++)
			{
				array4[l] = array3[l + bytes2.Length];
			}
			MemoryStream memoryStream = new MemoryStream(array4);
			MemoryStream memoryStream2 = Decompress(memoryStream);
			if (memoryStream2 == null)
			{
				memoryStream.Close();
				Debug.Log("[GameData:Decompress] Error while unpacking...");
				return false;
			}
			BinaryReader binaryReader = new BinaryReader(memoryStream2);
			int num = binaryReader.ReadInt32();
			if (Revision > num)
			{
				memoryStream.Close();
				memoryStream2.Close();
				binaryReader.Close();
				Debug.Log("[GameData:CheckRevision] Current revision is newer... Ignoring...");
				return true;
			}
			Revision = num;
			try
			{
				OnRead(binaryReader);
			}
			catch (EndOfStreamException ex)
			{
				Debug.Log(string.Format("[GameData:Reading]{0} caught and ignored. Using default values.", ex.GetType().Name));
			}
			memoryStream.Close();
			memoryStream2.Close();
			binaryReader.Close();
			return true;
		}
		catch
		{
			Debug.Log("[GameData:Decoding] Unknown error while decoding...");
			return false;
		}
	}

	protected abstract void OnWrite(BinaryWriter bw);

	protected abstract void OnRead(BinaryReader br);

	public eVersionCheckResult IsSameVersion(string base64)
	{
		if (base64 == null || base64 == string.Empty)
		{
			Debug.Log("[GameData:CheckVersions] Base64 is empty...");
			return eVersionCheckResult.CLIENT_HAS_GREATER_VERSION;
		}
		byte[] bytes = Encoding.ASCII.GetBytes("DDL");
		byte[] array = null;
		try
		{
			array = StringUtil.DecodeFrom64ToByteArray(base64);
		}
		catch
		{
			Debug.Log("[GameData:CheckRevision] Base64 invalid length...");
			return eVersionCheckResult.CLIENT_HAS_GREATER_VERSION;
		}
		try
		{
			if (array == null || array.Length == 0)
			{
				Debug.Log("[GameData:CheckMagic] Base64 bytearray is empty...");
				return eVersionCheckResult.CLIENT_HAS_GREATER_VERSION;
			}
			if (array.Length <= bytes.Length)
			{
				Debug.Log("[GameData:CheckMagic] Bytes length is less than magic length...");
				return eVersionCheckResult.CLIENT_HAS_GREATER_VERSION;
			}
			for (int i = 0; i < bytes.Length && i < array.Length; i++)
			{
				if (bytes[i] != array[i])
				{
					Debug.Log("[GameData:CheckMagic] Magic types mismatch...");
					return eVersionCheckResult.CLIENT_HAS_GREATER_VERSION;
				}
			}
			if (array.Length <= bytes.Length)
			{
				Debug.Log("[GameData:CheckVersion] Base64 bytearray length is too small...");
				return eVersionCheckResult.CLIENT_HAS_GREATER_VERSION;
			}
			byte b = array[bytes.Length];
			if (b != Version)
			{
				if (b > Version)
				{
					return eVersionCheckResult.SERVER_HAS_GREATER_VERSION;
				}
				return eVersionCheckResult.CLIENT_HAS_GREATER_VERSION;
			}
		}
		catch
		{
			Debug.Log("[GameData:CheckVersion] Unknown error while checking version...");
			return eVersionCheckResult.CLIENT_HAS_GREATER_VERSION;
		}
		return eVersionCheckResult.SAME_VERSION;
	}

	public RevisionResult CheckRevision(string base64)
	{
		string text = string.Format("Checking GameData revision...");
		Application.ExternalEval("console.log(\"" + text + "\");");
		Debug.Log(text);
		if (base64 == null || base64 == string.Empty)
		{
			text = string.Format("[GameData:CheckRevision] Base64 is empty...");
			Application.ExternalEval("console.log(\"" + text + "\");");
			Debug.Log(text);
			return RevisionResult.Error;
		}
		byte[] bytes = Encoding.ASCII.GetBytes("DDL");
		byte[] array = null;
		try
		{
			array = StringUtil.DecodeFrom64ToByteArray(base64);
		}
		catch
		{
			text = string.Format("[GameData:CheckRevision] Base64 invalid length...");
			Application.ExternalEval("console.log(\"" + text + "\");");
			Debug.Log(text);
			return RevisionResult.Error;
		}
		try
		{
			if (array == null || array.Length == 0)
			{
				text = string.Format("[GameData:CheckMagic] Base64 bytearray is empty...");
				Application.ExternalEval("console.log(\"" + text + "\");");
				Debug.Log(text);
				return RevisionResult.Error;
			}
			if (array.Length <= bytes.Length)
			{
				text = string.Format("[GameData:CheckMagic] Bytes length is less than magic length...");
				Application.ExternalEval("console.log(\"" + text + "\");");
				Debug.Log(text);
				return RevisionResult.Error;
			}
			for (int i = 0; i < bytes.Length && i < array.Length; i++)
			{
				if (bytes[i] != array[i])
				{
					text = string.Format("[GameData:CheckMagic] Magic types mismatch...");
					Application.ExternalEval("console.log(\"" + text + "\");");
					Debug.Log(text);
					return RevisionResult.Error;
				}
			}
			if (array.Length <= bytes.Length)
			{
				text = string.Format("[GameData:CheckVersion] Base64 bytearray length is too small...");
				Application.ExternalEval("console.log(\"" + text + "\");");
				Debug.Log(text);
				return RevisionResult.Error;
			}
			byte b = array[bytes.Length];
			if (b != Version)
			{
				text = string.Format("[GameData:CheckVersion] Versions mismatch...");
				Application.ExternalEval("console.log(\"" + text + "\");");
				Debug.Log(text);
				Convert(base64);
				return RevisionResult.Error;
			}
			if (array.Length <= bytes.Length + 1)
			{
				text = string.Format("[GameData:Decrypting] Base64 bytearray length is too small...");
				Application.ExternalEval("console.log(\"" + text + "\");");
				Debug.Log(text);
				return RevisionResult.Error;
			}
			byte[] array2 = new byte[array.Length - 1 - bytes.Length];
			for (int j = 0; j < array2.Length; j++)
			{
				array2[j] = array[j + 1 + bytes.Length];
			}
			byte[] array3 = Decrypt(array2);
			byte[] bytes2 = Encoding.ASCII.GetBytes("Dedalord");
			if (array3.Length <= bytes2.Length)
			{
				text = string.Format("[GameData:CheckEncryptMagic] Base64 bytearray length is too small...");
				Application.ExternalEval("console.log(\"" + text + "\");");
				Debug.Log(text);
				return RevisionResult.Error;
			}
			for (int k = 0; k < bytes2.Length; k++)
			{
				if (array3[k] != bytes2[k])
				{
					text = string.Format("[GameData:CheckEncryptMagic] Magic mismatch...");
					Application.ExternalEval("console.log(\"" + text + "\");");
					Debug.Log(text);
					return RevisionResult.Error;
				}
			}
			byte[] array4 = new byte[array3.Length - bytes2.Length];
			for (int l = 0; l < array4.Length; l++)
			{
				array4[l] = array3[l + bytes2.Length];
			}
			MemoryStream memoryStream = new MemoryStream(array4);
			MemoryStream memoryStream2 = Decompress(memoryStream);
			if (memoryStream2 == null)
			{
				memoryStream.Close();
				text = string.Format("[GameData:Decompress] Error while unpacking...");
				Application.ExternalEval("console.log(\"" + text + "\");");
				Debug.Log(text);
				return RevisionResult.Error;
			}
			BinaryReader binaryReader = new BinaryReader(memoryStream2);
			int num = binaryReader.ReadInt32();
			if (Revision > num)
			{
				text = string.Format("[GameData:CheckRevision] Current revision is newer...(rev1: {0}, rev2: {1})", Revision, num);
				Application.ExternalEval("console.log(\"" + text + "\");");
				Debug.Log(text);
				return RevisionResult.Higher;
			}
			if (Revision < num)
			{
				text = string.Format("[GameData:CheckRevision] Current revision is old...(rev1: {0}, rev2: {1})", Revision, num);
				Application.ExternalEval("console.log(\"" + text + "\");");
				Debug.Log(text);
				return RevisionResult.Lower;
			}
			return OnCheckRevision(base64);
		}
		catch
		{
			text = string.Format("[GameData:CheckRevision] Unknown error...");
			Application.ExternalEval("console.log(\"" + text + "\");");
			Debug.Log(text);
			return RevisionResult.Error;
		}
	}

	protected virtual RevisionResult OnCheckRevision(string base64)
	{
		string text = string.Format("[Base:OnCheckRevision] RevisionResult.Equal");
		Application.ExternalEval("console.log(\"" + text + "\");");
		Debug.Log(text);
		return RevisionResult.Equal;
	}

	protected virtual byte[] Encrypt(byte[] data)
	{
		return data;
	}

	protected virtual byte[] Decrypt(byte[] data)
	{
		return data;
	}

	protected bool Convert(string base64)
	{
		if (base64 == null || base64 == string.Empty)
		{
			Debug.Log("[Convert:CheckVersions] Base64 is empty...");
			return false;
		}
		byte[] bytes = Encoding.ASCII.GetBytes("DDL");
		byte[] array = StringUtil.DecodeFrom64ToByteArray(base64);
		try
		{
			if (array == null || array.Length == 0)
			{
				Debug.Log("[Convert:CheckMagic] Base64 bytearray is empty...");
				return false;
			}
			if (array.Length <= bytes.Length)
			{
				Debug.Log("[Convert:CheckMagic] Bytes length is less than magic length...");
				return false;
			}
			for (int i = 0; i < bytes.Length && i < array.Length; i++)
			{
				if (bytes[i] != array[i])
				{
					Debug.Log("[Convert:CheckMagic] Magic types mismatch...");
					return false;
				}
			}
			if (array.Length <= bytes.Length)
			{
				Debug.Log("[Convert:CheckVersion] Base64 bytearray length is too small...");
				return false;
			}
			byte version = array[bytes.Length];
			return OnConvert(base64, version);
		}
		catch
		{
			Debug.Log("[GameData:Convert] Unknown error...");
			return false;
		}
	}

	protected virtual bool OnConvert(string base64, int version)
	{
		return true;
	}
}
