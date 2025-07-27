using UnityEngine;

public class CreditsCreateCharacter : MonoBehaviour
{
	private GameObject player;

	private void Awake()
	{
		ItemInfo[] itemsByType = Store.Instance.GetItemsByType("avatar");
		int num = Random.Range(0, itemsByType.Length);
		AvatarItemInfo avatarItemInfo = (AvatarItemInfo)itemsByType[num];
		while (avatarItemInfo.AvatarPrefab == null)
		{
			num = Random.Range(0, itemsByType.Length);
			avatarItemInfo = (AvatarItemInfo)itemsByType[num];
		}
		GameObject gameObject = Resources.Load(string.Format("Characters/CompletePrefabs/{0}", avatarItemInfo.AvatarPrefab), typeof(GameObject)) as GameObject;
		player = Object.Instantiate(gameObject) as GameObject;
		player.name = gameObject.name;
	}

	private void Start()
	{
		CharStateMachine charStateMachine = CharHelper.GetCharStateMachine();
		Object.DestroyImmediate(charStateMachine);
		CharacterController component = player.GetComponent<CharacterController>();
		Object.DestroyImmediate(component);
		Rigidbody[] componentsInChildren = player.GetComponentsInChildren<Rigidbody>();
		Rigidbody[] array = componentsInChildren;
		foreach (Rigidbody rigidbody in array)
		{
			rigidbody.isKinematic = true;
		}
		player.GetComponent<Animation>().wrapMode = WrapMode.Loop;
		player.GetComponent<Animation>().Play("DramaticFalling");
		player.GetComponent<Animation>()["DramaticFalling"].normalizedSpeed = 0.7f;
		player.transform.rotation *= Quaternion.AngleAxis(-20f, Vector3.up);
		player.transform.rotation *= Quaternion.AngleAxis(-120f, Vector3.right);
	}

	private void OnEnable()
	{
	}

	private void OnDisable()
	{
	}
}
