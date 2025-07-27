using UnityEngine;

public class Cheat : MonoBehaviour
{
	public enum Quadrant
	{
		First = 0,
		Second = 1,
		Third = 2,
		Fourth = 3
	}

	private enum CheatMode
	{
		None = 0,
		DeleteAll = 1,
		Money = 2,
		UnlockLevels = 3
	}

	public int MoneyCount = 1000;

	private Quadrant[] deleteAllCheat = new Quadrant[4]
	{
		Quadrant.Third,
		Quadrant.First,
		Quadrant.Third,
		Quadrant.Fourth
	};

	private CheatMode mode;

	private int id;

	private void Awake()
	{
	}

	private void Update()
	{
		if (!Input.GetButtonDown("Fire1"))
		{
			return;
		}
		Quadrant quadrant = GetQuadrant();
		switch (mode)
		{
		case CheatMode.None:
			id = 0;
			if (quadrant == deleteAllCheat[id])
			{
				mode = CheatMode.DeleteAll;
				id++;
			}
			break;
		case CheatMode.DeleteAll:
			if (quadrant == deleteAllCheat[id])
			{
				id++;
				if (id >= deleteAllCheat.Length)
				{
					PlayerAccount.Instance.DeleteAll();
					id = 0;
					mode = CheatMode.None;
					if (BeLordSocial.Instance != null)
					{
						BeLordSocial.Instance.LogoutTwitter();
					}
				}
			}
			else
			{
				mode = CheatMode.None;
			}
			break;
		}
	}

	private Quadrant GetQuadrant()
	{
		if (Input.mousePosition.x >= (float)(Screen.width / 2))
		{
			if (Input.mousePosition.y < (float)(Screen.height / 2))
			{
				return Quadrant.Fourth;
			}
			return Quadrant.First;
		}
		if (Input.mousePosition.y >= (float)(Screen.height / 2))
		{
			return Quadrant.Second;
		}
		return Quadrant.Third;
	}
}
