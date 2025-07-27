public class MyStringReader
{
	private int idx;

	private string[] lines;

	public MyStringReader(string text)
	{
		lines = text.Split('\n');
		idx = 0;
	}

	public string ReadLine()
	{
		if (idx < lines.Length)
		{
			idx++;
			return lines[idx - 1];
		}
		return null;
	}
}
