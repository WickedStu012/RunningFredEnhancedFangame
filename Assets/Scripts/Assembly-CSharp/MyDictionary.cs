using System.Collections;

public class MyDictionary<T1, T2>
{
	private Hashtable ht;

	public int Count
	{
		get
		{
			return ht.Count;
		}
	}

	public MyDictionary()
	{
		ht = new Hashtable();
	}

	public void Add(T1 key, T2 p2)
	{
		ht.Add(key, p2);
	}

	public bool ContainsKey(T1 key)
	{
		return ht.ContainsKey(key);
	}

	public void Remove(T1 key)
	{
		ht.Remove(key);
	}

	public IDictionaryEnumerator GetEnumerator()
	{
		return ht.GetEnumerator();
	}
}
