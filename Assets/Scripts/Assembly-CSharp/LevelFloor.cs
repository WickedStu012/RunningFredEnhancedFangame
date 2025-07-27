using UnityEngine;

public class LevelFloor : MonoBehaviour
{
	public float rotation;

	public bool visible = true;

	public float startYPos;

	public float endYPos;

	public bool generateFrontWall;

	public bool generateFrontWallInverse;

	public bool canChangeSlopeAtRT;

	private Mesh mesh;

	private float width;

	private float length;

	private void Start()
	{
		if (Application.isPlaying && !visible)
		{
			if (base.gameObject.GetComponent<Collider>() != null)
			{
				base.gameObject.GetComponent<Collider>().enabled = false;
			}
			if (base.gameObject.GetComponent<Renderer>() != null)
			{
				base.gameObject.GetComponent<Renderer>().enabled = false;
			}
		}
	}

	private void combineWalls()
	{
		MeshFilter[] componentsInChildren = GetComponentsInChildren<MeshFilter>();
		CombineInstance[] array = new CombineInstance[2];
		GameObject gameObject = null;
		int num = 0;
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			if (componentsInChildren[i].name == "leftWall")
			{
				array[num].mesh = componentsInChildren[i].sharedMesh;
				array[num].transform = componentsInChildren[i].transform.localToWorldMatrix;
				gameObject = componentsInChildren[i].gameObject;
				componentsInChildren[i].gameObject.SetActive(false);
				num++;
			}
			else if (componentsInChildren[i].name == "rightWall")
			{
				array[num].mesh = componentsInChildren[i].sharedMesh;
				array[num].transform = componentsInChildren[i].transform.localToWorldMatrix;
				componentsInChildren[i].gameObject.SetActive(false);
				num++;
			}
		}
		Mesh mesh = new Mesh();
		gameObject.transform.GetComponent<MeshFilter>().mesh = mesh;
		gameObject.transform.GetComponent<MeshFilter>().mesh.CombineMeshes(array);
	}
}
