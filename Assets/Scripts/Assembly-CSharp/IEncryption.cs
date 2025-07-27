internal interface IEncryption
{
	string GetMD5(string str, string variableValue);

	string EncryptData(string Message, string key);

	string DecryptData(string Message, string key);
}
