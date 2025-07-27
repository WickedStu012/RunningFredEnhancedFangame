using UnityEngine;

public class Wings : MonoBehaviour
{
	private bool waitingToFold;

	private float accumTime;

	private float unfoldTime;

	private float unfoldAndFoldSpeed = 3f;

	private SkinnedMeshRenderer[] blinkObjs;

	private void Start()
	{
		unfoldTime = GetUnfoldLength();
	}

	private void OnEnable()
	{
		if (!CharHelper.GetProps().HasWings)
		{
			base.gameObject.SetActive(false);
		}
	}

	private void Update()
	{
		if (waitingToFold)
		{
			accumTime += Time.deltaTime;
			if (accumTime >= unfoldTime / unfoldAndFoldSpeed)
			{
				Fold();
				waitingToFold = false;
			}
		}
	}

	public void Unfolded()
	{
		base.GetComponent<Animation>().wrapMode = WrapMode.Once;
		base.GetComponent<Animation>().Play("Unfolded");
	}

	public void Unfold()
	{
		base.GetComponent<Animation>()["UnFolding"].speed = unfoldAndFoldSpeed;
		base.GetComponent<Animation>().wrapMode = WrapMode.Once;
		base.GetComponent<Animation>().Play("UnFolding");
	}

	public float GetUnfoldLength()
	{
		return base.GetComponent<Animation>()["UnFolding"].length;
	}

	public void Folded()
	{
		base.GetComponent<Animation>().wrapMode = WrapMode.Once;
		base.GetComponent<Animation>().Play("Folded");
	}

	public void Fold()
	{
		base.GetComponent<Animation>()["Folding"].speed = unfoldAndFoldSpeed;
		base.GetComponent<Animation>().wrapMode = WrapMode.Once;
		base.GetComponent<Animation>().Play("Folding");
	}

	public void Glide()
	{
		base.GetComponent<Animation>().wrapMode = WrapMode.Loop;
		base.GetComponent<Animation>().Play("Gliding");
	}

	public void UnfoldAndFold()
	{
		Unfold();
		waitingToFold = true;
	}

	public void Boost()
	{
		base.GetComponent<Animation>().wrapMode = WrapMode.Once;
		base.GetComponent<Animation>().Play("Boost");
	}

	public void SetVisibility(bool val)
	{
		if (blinkObjs == null)
		{
			blinkObjs = base.gameObject.GetComponentsInChildren<SkinnedMeshRenderer>();
		}
		if (blinkObjs != null)
		{
			for (int i = 0; i < blinkObjs.Length; i++)
			{
				blinkObjs[i].enabled = val;
			}
		}
	}
}
