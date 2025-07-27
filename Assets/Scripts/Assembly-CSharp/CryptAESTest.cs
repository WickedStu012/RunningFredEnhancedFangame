using System.Text;
using UnityEngine;

public class CryptAESTest : MonoBehaviour
{
	private string enc;

	private string dec;

	private byte[] arr;

	private void OnGUI()
	{
		if (GUILayout.Button("Test"))
		{
			enc = CryptAES.Encrypt("sides of onion. Cook it with a good white sauce is well beaten. Fill your liver, lard (it is nearly cooked, taking the roe from a flat in water. Put into dice, and place them slightly browned. Decorate the soup is half-cooked, take off the fire with a dinner must have done its small bunch of two onions, and when ready, strain and add the cold remove before sending it than has been soaked roll in the meat and the most delicate fish, take away before serving. If the grenadine. HOCHE POT OF MEAT Your scraps of nearly half pound");
			Debug.Log(enc);
		}
		if (GUILayout.Button("Test 2"))
		{
			dec = CryptAES.Decrypt(enc);
			Debug.Log(dec);
		}
		if (GUILayout.Button("Test 3"))
		{
			byte[] bytes = Encoding.ASCII.GetBytes("Hola a todos");
			arr = CryptAES.Encrypt(bytes);
		}
		if (GUILayout.Button("Test 4"))
		{
			byte[] bytes2 = CryptAES.Decrypt(arr);
			Debug.Log(Encoding.ASCII.GetString(bytes2));
		}
	}
}
