using UnityEngine;

public class CharBloodPoolGenerator : MonoBehaviour
{
	private const int BLOOD_POOL_PRECREATE = 12;

	private const float BLOOD_POOL_CREATE_INTERVAL = 0.25f;

	private const float MIN_INITIAL_SCALE = 0.1f;

	private const float MAX_INITIAL_SCALE = 0.2f;

	private const float MIN_SCALE_SPEED = 4f;

	private const float MAX_SCALE_SPEED = 1f;

	private GameObject[] bloodSplats;

	private GameObject[] splats;

	private float[] initialScale;

	private float[] scaleSpeed;

	private int currentlyVisible;

	private float accumTime;

	private float accumTimeScale;

	private bool createPools;

	private void Start()
	{
		bloodSplats = new GameObject[3];
		bloodSplats[0] = Resources.Load("prefabs/bloodSplatter1") as GameObject;
		bloodSplats[1] = Resources.Load("prefabs/bloodSplatter2") as GameObject;
		bloodSplats[2] = Resources.Load("prefabs/bloodSplatter3") as GameObject;
		splats = new GameObject[12];
		initialScale = new float[12];
		scaleSpeed = new float[12];
		for (int i = 0; i < 12; i++)
		{
			splats[i] = Object.Instantiate(bloodSplats[Random.Range(0, bloodSplats.Length - 1)]) as GameObject;
			splats[i].SetActive(false);
			splats[i].name = string.Format("BloodPool_{0}", base.name);
		}
		currentlyVisible = 0;
		createPools = false;
	}

	private void Update()
	{
		if (currentlyVisible <= 0)
		{
			return;
		}
		if (createPools)
		{
			accumTime += Time.deltaTime;
			if (accumTime >= 0.25f)
			{
				accumTime = 0f;
				currentlyVisible++;
				if (currentlyVisible < splats.Length)
				{
					createPool(currentlyVisible - 1);
				}
				else
				{
					accumTime = 0f;
					createPools = false;
				}
			}
		}
		accumTimeScale += Time.deltaTime;
		for (int i = 0; i < currentlyVisible; i++)
		{
			if (accumTimeScale / scaleSpeed[i] < 1f)
			{
				float num = Mathf.Lerp(initialScale[i], initialScale[i] + 2f, accumTimeScale / scaleSpeed[i]);
				splats[i].transform.localScale = new Vector3(num, num, num);
			}
		}
	}

	public void StartPoolCreation()
	{
		currentlyVisible = 1;
		createPool(currentlyVisible - 1);
		createPools = true;
	}

	private void createPool(int bloodPoolNum)
	{
		RaycastHit hitInfo;
		if (Physics.Raycast(new Vector3(base.transform.position.x, base.transform.position.y + 10f, base.transform.position.z), Vector3.down, out hitInfo, float.PositiveInfinity, 8704))
		{
			splats[bloodPoolNum].transform.position = new Vector3(base.transform.position.x, hitInfo.transform.position.y + 0.01f, base.transform.position.z);
			splats[bloodPoolNum].SetActive(true);
			float num = Random.Range(0.1f, 0.2f);
			splats[bloodPoolNum].transform.localScale = new Vector3(num, num, num);
			initialScale[bloodPoolNum] = num;
			float y = Random.Range(0f, 180f);
			splats[bloodPoolNum].transform.localRotation = Quaternion.Euler(new Vector3(0f, y, 0f));
			scaleSpeed[bloodPoolNum] = Random.Range(4f, 1f);
		}
	}

	private Transform getTransformByName(GameObject goPlayer, string boneName)
	{
		Transform[] componentsInChildren = goPlayer.GetComponentsInChildren<Transform>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			if (string.Compare(boneName, componentsInChildren[i].name, true) == 0)
			{
				componentsInChildren[i].name = boneName;
				return componentsInChildren[i];
			}
		}
		Debug.Log(string.Format("Cannot find the bone {0}", boneName));
		return null;
	}
}
