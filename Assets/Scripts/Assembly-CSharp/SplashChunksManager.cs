using UnityEngine;

public class SplashChunksManager : MonoBehaviour
{
	public Vector3 InitialPos;

	public float DistanceBetweenChunks = 22.5f;

	public float Speed = 10f;

	public GameObject[] prefabs;

	public Transform[] decorations;

	public int Count = 2;

	private GameObject[] chunks;

	private void Awake()
	{
		chunks = new GameObject[Count];
		for (int i = 0; i < Count; i++)
		{
			GameObject gameObject = Object.Instantiate(prefabs[i % prefabs.Length]) as GameObject;
			gameObject.transform.parent = base.transform;
			gameObject.transform.localPosition = Vector3.zero;
			gameObject.transform.localRotation = Quaternion.Euler(new Vector3(22.5f * (float)i, 0f, 0f));
			chunks[i] = gameObject;
		}
	}

	private void Update()
	{
		Quaternion quaternion = Quaternion.Euler(Vector3.right * Speed * Time.deltaTime);
		for (int i = 0; i < Count; i++)
		{
			Quaternion quaternion2 = chunks[i].transform.localRotation;
			float num = chunks[i].transform.localEulerAngles.x - DistanceBetweenChunks * (float)Count;
			if (num > 0f)
			{
				quaternion2 = Quaternion.Euler(Vector3.right * num);
			}
			chunks[i].transform.localRotation = quaternion2 * quaternion;
		}
		for (int j = 0; j < decorations.Length; j++)
		{
			decorations[j].localRotation = decorations[j].localRotation * quaternion;
		}
	}
}
