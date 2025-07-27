using UnityEngine;

public class DayHighlighter : MonoBehaviour
{
	public GUI3DObject Day1;

	public GUI3DObject Day2;

	public GUI3DObject Day3;

	public GUI3DObject Day4;

	public GUI3DText Day1Text;

	public GUI3DText Day2Text;

	public GUI3DText Day3Text;

	public GUI3DText Day4Text;

	private void Awake()
	{
		Day1Text.SetDynamicText(10.ToString());
		Day2Text.SetDynamicText(20.ToString());
		Day3Text.SetDynamicText(40.ToString());
		Day4Text.SetDynamicText(80.ToString());
	}

	private void OnEnable()
	{
		if (PlayerAccount.Instance != null)
		{
			switch (PlayerAccount.Instance.DaysInARow)
			{
			case 0:
				Day1.StartSegmentTexName = "bt-levels-left-hover";
				Day1.TextureName = "bt-levels-stretch-hover";
				Day1.EndSegmentTexName = "bt-levels-right-hover";
				break;
			case 1:
				Day2.StartSegmentTexName = "bt-levels-left-hover";
				Day2.TextureName = "bt-levels-stretch-hover";
				Day2.EndSegmentTexName = "bt-levels-right-hover";
				break;
			case 2:
				Day3.StartSegmentTexName = "bt-levels-left-hover";
				Day3.TextureName = "bt-levels-stretch-hover";
				Day3.EndSegmentTexName = "bt-levels-right-hover";
				break;
			case 3:
				Day4.StartSegmentTexName = "bt-levels-left-hover";
				Day4.TextureName = "bt-levels-stretch-hover";
				Day4.EndSegmentTexName = "bt-levels-right-hover";
				break;
			}
		}
	}
}
