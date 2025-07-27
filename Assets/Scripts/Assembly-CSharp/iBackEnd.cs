public interface iBackEnd
{
	void OnEnable();

	void OnDisable();

	void Update();

	void ReadData(BackendRes ber);

	void WriteData(string data, BackendRes ber);

	void RemoveData(BackendRes ber);

	bool IsAvailable();

	bool IsUserAuthenticated();

	void SetTestUserName(string un);
}
