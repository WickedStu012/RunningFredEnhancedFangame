using UnityEngine;

public class TurnPageButton : MonoBehaviour
{
	public bool NextPage = true;

	public GUI3DPageSlider PageSlider;

	private GUI3DButton button;

	private void OnEnable()
	{
		if (button == null)
		{
			button = GetComponent<GUI3DButton>();
		}
		button.ReleaseEvent += OnRelease;
	}

	private void OnDisable()
	{
		if (button == null)
		{
			button = GetComponent<GUI3DButton>();
		}
		button.ReleaseEvent -= OnRelease;
	}

	private void OnRelease(GUI3DOnReleaseEvent evt)
	{
		if (PageSlider != null)
		{
			if (NextPage)
			{
				PageSlider.CurrentPage++;
			}
			else
			{
				PageSlider.CurrentPage--;
			}
		}
	}

	private void Update()
	{
		if (!(button != null) || !(PageSlider != null))
		{
			return;
		}
		if (NextPage)
		{
			if (PageSlider.IsLastPage())
			{
				if (button.CheckEvents)
				{
					button.CheckEvents = false;
					button.GetComponent<Renderer>().enabled = false;
				}
			}
			else if (!button.CheckEvents)
			{
				button.CheckEvents = true;
				button.GetComponent<Renderer>().enabled = true;
			}
		}
		else if (PageSlider.IsFirstPage())
		{
			if (button.CheckEvents)
			{
				button.CheckEvents = false;
				button.GetComponent<Renderer>().enabled = false;
			}
		}
		else if (!button.CheckEvents)
		{
			button.CheckEvents = true;
			button.GetComponent<Renderer>().enabled = true;
		}
	}
}
