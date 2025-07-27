using UnityEngine;

public class IceBlock : MonoBehaviour
{
	private Color colorStart;

	private Color colorEnd;

	private bool startAnim;

	private float accumTime;

	private void Start()
	{
		startAnim = false;
		base.GetComponent<Renderer>().enabled = false;
		Color color = base.gameObject.GetComponent<Renderer>().material.color;
		colorStart = new Color(color.r, color.g, color.b, 0.1f);
		colorEnd = new Color(color.r, color.g, color.b, 0.9f);
	}

	private void Update()
	{
		if (startAnim)
		{
			accumTime += Time.deltaTime;
			base.gameObject.GetComponent<Renderer>().material.color = Color.Lerp(colorStart, colorEnd, accumTime / 2f);
			if (accumTime >= 2f)
			{
				base.gameObject.GetComponent<Renderer>().material.color = colorEnd;
				startAnim = false;
			}
		}
	}

	public void StartMaterialAnim()
	{
		accumTime = 0f;
		startAnim = true;
		base.GetComponent<Renderer>().enabled = true;
		base.gameObject.GetComponent<Renderer>().material.color = colorStart;
	}

	public void ResetBlock()
	{
		accumTime = 0f;
	}
}
