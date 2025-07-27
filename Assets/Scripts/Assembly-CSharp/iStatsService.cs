public interface iStatsService
{
	void SetApiKey(string apiKey);

	void SessionStart();

	void SessionEnd();

	void LogEvent(string sv);

	void LogEvent(string sv, string param1Type, string param1);

	void LogEvent(string sv, string param1Type, string param1, string param2Type, string param2);

	void LogEvent(string sv, string param1Type, string param1, string param2Type, string param2, string param3Type, string param3);
}
