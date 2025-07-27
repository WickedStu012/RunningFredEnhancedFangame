using UnityEngine;

[RequireComponent(typeof(Sprite))]
public class SpriteAnimation : MonoBehaviour
{
	public enum LoopType
	{
		None = 0,
		Loop = 1,
		PingPong = 2
	}

	public int FromFrame;

	public int ToFrame;

	public int FPS = 30;

	public string Name = "Animation";

	public LoopType Loop;

	public int[] Frames;

	private Sprite sprite;

	private float deltaT;

	private float lastTime;

	private int currentId;

	private int factor = 1;

	private void Awake()
	{
		sprite = GetComponent<Sprite>();
		if (FromFrame != 0 || ToFrame != 0)
		{
			Frames = new int[Mathf.Abs(ToFrame - FromFrame) + 1];
			int num = 0;
			if (FromFrame < ToFrame)
			{
				for (int i = FromFrame; i <= ToFrame; i++)
				{
					Frames[num++] = i;
				}
			}
			else
			{
				for (int num2 = FromFrame; num2 >= ToFrame; num2--)
				{
					Frames[num++] = num2;
				}
			}
		}
		if (FromFrame > ToFrame)
		{
			factor = -1;
		}
		deltaT = 1f / (float)FPS;
	}

	private void OnEnable()
	{
		currentId = 0;
		if (FromFrame > ToFrame)
		{
			factor = -1;
		}
		else
		{
			factor = 1;
		}
	}

	private void FixedUpdate()
	{
		if (!(Time.time - lastTime >= deltaT))
		{
			return;
		}
		lastTime = Time.time;
		if (currentId < 0 || currentId >= Frames.Length)
		{
			if (Loop == LoopType.Loop)
			{
				currentId %= Frames.Length;
			}
			else
			{
				if (Loop != LoopType.PingPong)
				{
					base.enabled = false;
					return;
				}
				factor *= -1;
				if (factor < 0 && Frames.Length > 1)
				{
					currentId = Frames.Length - 2;
				}
				else if (factor > 0 && Frames.Length - 1 >= 1)
				{
					currentId = 1;
				}
				else
				{
					currentId = 0;
				}
			}
		}
		sprite.SetFrame(Frames[currentId]);
		currentId += factor;
	}

	public bool IsPlaying()
	{
		return base.enabled;
	}
}
