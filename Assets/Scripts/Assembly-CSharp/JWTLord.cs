using System.Security.Cryptography;
using System.Text;

public class JWTLord
{
	public static string Encode(string payload, string key)
	{
		byte[] bytes = Encoding.ASCII.GetBytes(key);
		byte[] bytes2 = Encoding.ASCII.GetBytes(payload);
		HMACSHA256 hMACSHA = new HMACSHA256(bytes);
		byte[] bytes3 = hMACSHA.ComputeHash(bytes2);
		return Encoding.ASCII.GetString(bytes3);
	}
}
