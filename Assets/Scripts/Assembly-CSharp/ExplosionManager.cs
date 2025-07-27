using UnityEngine;

public class ExplosionManager : MonoBehaviour
{
	private static ExplosionManager instance;

	public Material[] bloodPrefab;

	public GameObject[] bodyPartsPrefab;

	public GameObject bloodExplosion;

	public GameObject bloodExplosionDown;

	public GameObject bloodExplosionPS;

	public GameObject plane;

	private GameObject[] bloodInstances;

	private GameObject[] bodyPartsInstances;

	private Transform[] parts;

	private float accumTime;

	private bool explode;

	private RaycastHit hit;

	private GameObject bloodExplosionPSObj;

	private void Awake()
	{
		instance = this;
		bloodInstances = new GameObject[5];
		for (int i = 0; i < bloodInstances.Length; i++)
		{
			bloodInstances[i] = Object.Instantiate(plane) as GameObject;
			bloodInstances[i].GetComponent<Renderer>().material = bloodPrefab[Random.Range(0, bloodPrefab.Length)];
			bloodInstances[i].transform.parent = base.transform;
			bloodInstances[i].name = string.Format("blood{0}", i);
			bloodInstances[i].SetActive(false);
		}
		bodyPartsInstances = new GameObject[bodyPartsPrefab.Length];
		for (int j = 0; j < bodyPartsInstances.Length; j++)
		{
			bodyPartsInstances[j] = Object.Instantiate(bodyPartsPrefab[j]) as GameObject;
			bodyPartsInstances[j].transform.parent = base.transform;
			bodyPartsInstances[j].name = bodyPartsPrefab[j].name;
			bodyPartsInstances[j].SetActive(false);
		}
		MeshRenderer[] componentsInChildren = bloodExplosion.GetComponentsInChildren<MeshRenderer>(true);
		parts = new Transform[componentsInChildren.Length];
		for (int k = 0; k < componentsInChildren.Length; k++)
		{
			parts[k] = componentsInChildren[k].transform;
		}
		bloodExplosionPSObj = Object.Instantiate(bloodExplosionPS) as GameObject;
		bloodExplosionPSObj.SetActive(false);
	}

	private void Update()
	{
		if (explode)
		{
			accumTime += Time.deltaTime;
			for (int i = 0; i < parts.Length; i++)
			{
				parts[i].localScale = new Vector3(parts[i].localScale.x, parts[i].localScale.y, Mathf.Lerp(2f, 3f, accumTime));
			}
		}
	}

	public static void FireOnAir(Transform transf)
	{
		if (instance == null)
		{
			Debug.LogError("ExplosionManager instance is null. Is ExplosionManager gameObject added to the scene?");
		}
		else
		{
			instance.fireOnAir(transf);
		}
	}

	public static void Fire(Vector3 pos)
	{
		if (instance == null)
		{
			Debug.LogError("ExplosionManager instance is null. Is ExplosionManager gameObject added to the scene?");
		}
		else
		{
			instance.fire(pos);
		}
	}

	public static void FireOnWall(Vector3 pos)
	{
		if (instance == null)
		{
			Debug.LogError("ExplosionManager instance is null. Is ExplosionManager gameObject added to the scene?");
		}
		else
		{
			instance.fireOnWall(pos);
		}
	}

	public static void Fire2(Vector3 pos)
	{
		if (instance == null)
		{
			Debug.LogError("ExplosionManager instance is null. Is ExplosionManager gameObject added to the scene?");
		}
		else
		{
			instance.fire2(pos);
		}
	}

	private void fire(Vector3 pos)
	{
		if (Physics.Raycast(new Vector3(pos.x, pos.y + 10f, pos.z), Vector3.down, out hit, 25f, 8704))
		{
			explode = true;
			bloodExplosion.transform.position = new Vector3(hit.point.x, hit.point.y + 0.05f, hit.point.z);
			bloodExplosion.transform.rotation = Quaternion.Euler(hit.transform.rotation.eulerAngles.x - 270f, hit.transform.rotation.eulerAngles.y, hit.transform.rotation.eulerAngles.z);
			bloodExplosion.SetActive(true);
			for (int i = 0; i < bodyPartsInstances.Length; i++)
			{
				bodyPartsInstances[i].transform.position = new Vector3(hit.point.x, hit.point.y, hit.point.z);
				bodyPartsInstances[i].GetComponent<Rigidbody>().AddForce(Random.Range(-100f, 100f), Random.Range(50f, 100f), Random.Range(-100f, 100f));
				bodyPartsInstances[i].GetComponent<Rigidbody>().AddTorque(Random.Range(-100f, 100f), Random.Range(-100f, 100f), Random.Range(-100f, 100f));
				bodyPartsInstances[i].SetActive(true);
			}
			bloodExplosionPSObj.SetActive(true);
			bloodExplosionPSObj.transform.position = new Vector3(hit.point.x, hit.point.y, hit.point.z);
		}
	}

	private void fireOnAir(Transform transf)
	{
		if (transf == null)
		{
			Debug.LogError("Transform is null");
		}
		Vector3 vector = transf.position + Vector3.up;
		explode = true;
		for (int i = 0; i < bodyPartsInstances.Length; i++)
		{
			bodyPartsInstances[i].transform.position = new Vector3(vector.x, vector.y, vector.z);
			bodyPartsInstances[i].GetComponent<Rigidbody>().AddForce(Random.Range(-100f, 100f), Random.Range(50f, 100f), Random.Range(-100f, 100f));
			bodyPartsInstances[i].GetComponent<Rigidbody>().AddTorque(Random.Range(-100f, 100f), Random.Range(-100f, 100f), Random.Range(-100f, 100f));
			bodyPartsInstances[i].SetActive(true);
		}
		bloodExplosionPSObj.transform.position = new Vector3(vector.x, vector.y, vector.z);
		bloodExplosionPSObj.SetActive(true);
		bloodExplosionPSObj.transform.position = new Vector3(vector.x, vector.y, vector.z);
	}

	private void fire2(Vector3 pos)
	{
		RaycastHit hitInfo;
		if (Physics.Raycast(new Vector3(pos.x, pos.y + 10f, pos.z), Vector3.down, out hitInfo, 25f, 8704))
		{
			explode = true;
			bloodExplosion.transform.position = new Vector3(hitInfo.point.x, hitInfo.point.y + 0.05f, hitInfo.point.z);
			bloodExplosion.transform.rotation = Quaternion.Euler(hitInfo.transform.rotation.eulerAngles.x - 270f, hitInfo.transform.rotation.eulerAngles.y, hitInfo.transform.rotation.eulerAngles.z);
			bloodExplosion.SetActive(true);
			for (int i = 0; i < 1; i++)
			{
				bodyPartsInstances[i].transform.position = new Vector3(hitInfo.point.x, hitInfo.point.y, hitInfo.point.z);
				bodyPartsInstances[i].GetComponent<Rigidbody>().AddForce(Random.Range(-100f, 100f), Random.Range(50f, 100f), Random.Range(-100f, 100f));
				bodyPartsInstances[i].GetComponent<Rigidbody>().AddTorque(Random.Range(-100f, 100f), Random.Range(-100f, 100f), Random.Range(-100f, 100f));
				bodyPartsInstances[i].SetActive(true);
			}
			for (int j = 7; j < bodyPartsInstances.Length; j++)
			{
				bodyPartsInstances[j].transform.position = new Vector3(hitInfo.point.x, hitInfo.point.y, hitInfo.point.z);
				bodyPartsInstances[j].GetComponent<Rigidbody>().AddForce(Random.Range(-100f, 100f), Random.Range(50f, 100f), Random.Range(-100f, 100f));
				bodyPartsInstances[j].GetComponent<Rigidbody>().AddTorque(Random.Range(-100f, 100f), Random.Range(-100f, 100f), Random.Range(-100f, 100f));
				bodyPartsInstances[j].SetActive(true);
			}
		}
	}

	private void fireOnWall(Vector3 pos)
	{
		if (Physics.Raycast(new Vector3(pos.x, pos.y + 1f, pos.z), Vector3.forward, out hit, 2f, 12288))
		{
			explode = true;
			bloodExplosionDown.transform.position = new Vector3(hit.point.x, hit.point.y + 0.05f, hit.point.z);
			bloodExplosionDown.transform.rotation = Quaternion.Euler(270f, 0f, 0f);
			bloodExplosionDown.SetActive(true);
			for (int i = 0; i < bodyPartsInstances.Length; i++)
			{
				bodyPartsInstances[i].transform.position = new Vector3(hit.point.x, hit.point.y, hit.point.z);
				bodyPartsInstances[i].GetComponent<Rigidbody>().AddForce(Random.Range(-100f, 100f), Random.Range(50f, 100f), Random.Range(-100f, 100f));
				bodyPartsInstances[i].GetComponent<Rigidbody>().AddTorque(Random.Range(-100f, 100f), Random.Range(-100f, 100f), Random.Range(-100f, 100f));
				bodyPartsInstances[i].SetActive(true);
			}
		}
	}

	private void OnDestroy()
	{
		instance = null;
	}

	private void resetExplosions()
	{
		for (int i = 0; i < bloodInstances.Length; i++)
		{
			bloodInstances[i].SetActive(false);
		}
		for (int j = 0; j < bodyPartsInstances.Length; j++)
		{
			bodyPartsInstances[j].SetActive(false);
		}
		bloodExplosion.SetActive(false);
		bloodExplosionDown.SetActive(false);
		Object.DestroyImmediate(bloodExplosionPSObj);
		bloodExplosionPSObj = Object.Instantiate(bloodExplosionPS) as GameObject;
		bloodExplosionPSObj.SetActive(false);
	}

	public static void ResetExplosions()
	{
		if (instance != null)
		{
			instance.resetExplosions();
		}
	}
}
