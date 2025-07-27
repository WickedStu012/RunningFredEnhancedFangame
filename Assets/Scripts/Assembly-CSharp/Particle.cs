using UnityEngine;

public class Particle
{
	public Quaternion InitialRotation;

	public Vector3 ScaleSpeed;

	public Vector3 Speed;

	public float RotationSpeed;

	public float CurrentLifeTime;

	public int CurrentColorId;

	public Color CurrentColor;

	public Color FromColor;

	public Color ToColor;

	public float ColorTime;

	public float ColorFactor;

	public Vector3 BounceNoise;
}
