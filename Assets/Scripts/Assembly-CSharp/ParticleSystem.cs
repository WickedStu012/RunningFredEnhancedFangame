using UnityEngine;

public class ParticleSystem : MonoBehaviour
{
	public Material ImageMaterial;

	public Color[] Colors = new Color[2]
	{
		new Color(1f, 1f, 0f, 1f),
		new Color(1f, 0f, 0f, 0f)
	};

	public Vector3 Speed;

	public Vector3 RndSpeed;

	public float RotationSpeed;

	public float RndRotationSpeed;

	public Vector3 ScaleSpeed;

	public Vector3 RndScaleSpeed;

	public Vector3 Scale = new Vector3(1f, 1f, 1f);

	public Vector3 RndScale;

	public Vector3 Rotation;

	public Vector3 RndRotation;

	public Vector3 Force;

	public Vector3 ScaleSpeedForce;

	public Vector3 StartArea;

	public float FloorDistance;

	public float FloorFrictionFactor = 0.5f;

	public float BounceFactor = 0.7f;

	public Vector3 BounceNoise;

	public float LifeTime = 1f;

	public float RndLifeTime;

	public int MaxParticlesCount = 10;

	public bool SimulateFloor;

	public bool CastRayPerParticle;

	public bool SimulateInWorldSpace = true;

	public bool SpeedSimulatedInWorldSpace = true;

	public bool Billboard = true;

	public bool JustRotateY;

	public bool Emit = true;

	public bool OneShot;

	public string PoolName = "BillboardPool";

	private Pool particlesPool;

	private Particle[] deadParticles;

	private Particle[] aliveParticles;

	private PoolGameObject[] particleGameObjects;

	private Renderer[] particleRenderers;

	private int deadId;

	private int aliveId;

	private float creationDeltaTime;

	private float lastCreationTime;

	private Vector3 tempVector;

	private int shotCount = 1;

	private Vector3 force;

	private void Awake()
	{
		deadParticles = new Particle[MaxParticlesCount];
		aliveParticles = new Particle[MaxParticlesCount];
		particleGameObjects = new PoolGameObject[MaxParticlesCount];
		particleRenderers = new Renderer[MaxParticlesCount];
		for (int i = 0; i < MaxParticlesCount; i++)
		{
			deadParticles[i] = new Particle();
		}
		creationDeltaTime = (LifeTime + RndLifeTime / 2f) / (float)MaxParticlesCount;
		if (OneShot)
		{
			shotCount = MaxParticlesCount;
		}
		if (particlesPool == null && PoolManager.Instance != null)
		{
			particlesPool = PoolManager.Instance.GetPool(PoolName);
		}
	}

	private void OnDisable()
	{
		recycleParticles(false);
	}

	private void FixedUpdate()
	{
		if (Camera.main == null)
		{
			return;
		}
		if (particlesPool == null)
		{
			if (PoolManager.Instance == null)
			{
				return;
			}
			particlesPool = PoolManager.Instance.GetPool(PoolName);
			if (particlesPool == null)
			{
				return;
			}
		}
		if (Emit && ((!OneShot && Time.time - lastCreationTime >= creationDeltaTime) || (OneShot && deadId == 0)) && !particlesPool.IsEmpty())
		{
			for (int i = 0; i < shotCount; i++)
			{
				lastCreationTime = Time.time;
				if (deadId >= deadParticles.Length || aliveId >= aliveParticles.Length)
				{
					continue;
				}
				Particle particle = deadParticles[deadId];
				tempVector.x = Random.Range((0f - RndSpeed.x) / 2f, RndSpeed.x / 2f);
				tempVector.y = Random.Range((0f - RndSpeed.y) / 2f, RndSpeed.y / 2f);
				tempVector.z = Random.Range((0f - RndSpeed.z) / 2f, RndSpeed.z / 2f);
				particle.Speed = Speed + tempVector;
				particle.Speed = base.transform.TransformDirection(particle.Speed);
				force = Force;
				tempVector.x = Random.Range((0f - RndScaleSpeed.x) / 2f, RndScaleSpeed.x / 2f);
				tempVector.y = Random.Range((0f - RndScaleSpeed.y) / 2f, RndScaleSpeed.y / 2f);
				tempVector.z = Random.Range((0f - RndScaleSpeed.z) / 2f, RndScaleSpeed.z / 2f);
				particle.ScaleSpeed = ScaleSpeed + tempVector;
				particle.RotationSpeed = RotationSpeed + Random.Range((0f - RndRotationSpeed) / 2f, RndRotationSpeed / 2f);
				particle.CurrentLifeTime = LifeTime + Random.Range((0f - RndLifeTime) / 2f, RndLifeTime / 2f);
				tempVector = Random.insideUnitSphere;
				tempVector.x *= StartArea.x;
				tempVector.y *= StartArea.y;
				tempVector.z *= StartArea.z;
				particleGameObjects[aliveId] = particlesPool.Create(base.transform.position + tempVector, Scale);
				if (!(particleGameObjects[aliveId] != null))
				{
					continue;
				}
				if (!SimulateInWorldSpace)
				{
					particleGameObjects[aliveId].transform.parent = base.transform;
				}
				particleGameObjects[aliveId].transform.localScale = Scale;
				tempVector = Random.insideUnitCircle;
				tempVector.x *= RndRotation.x;
				tempVector.y *= RndRotation.y;
				tempVector.z *= RndRotation.z;
				particle.BounceNoise.x = Random.value * BounceNoise.x;
				particle.BounceNoise.y = Random.value * BounceNoise.y;
				particle.BounceNoise.z = Random.value * BounceNoise.z;
				particle.InitialRotation = Quaternion.Euler(Rotation + tempVector);
				if (Billboard)
				{
					if (JustRotateY)
					{
						Vector3 vector = Camera.main.transform.position - particleGameObjects[aliveId].transform.position;
						vector.x = (vector.z = 0f);
						particleGameObjects[aliveId].transform.LookAt(Camera.main.transform.position - vector);
					}
					else
					{
						particleGameObjects[aliveId].transform.LookAt(Camera.main.transform);
					}
					particleGameObjects[aliveId].transform.localRotation *= particle.InitialRotation;
				}
				else
				{
					particleGameObjects[aliveId].transform.localRotation = particle.InitialRotation;
				}
				tempVector = base.transform.rotation.eulerAngles;
				tempVector.x = (tempVector.y = 0f);
				particleGameObjects[aliveId].transform.localRotation *= Quaternion.Euler(-tempVector);
				particleRenderers[aliveId] = particleGameObjects[aliveId].GetComponent<Renderer>();
				particleGameObjects[aliveId].Material.shader = ImageMaterial.shader;
				particleGameObjects[aliveId].Material.mainTexture = ImageMaterial.mainTexture;
				if (Colors != null && Colors.Length > 1)
				{
					particle.CurrentColor = Colors[0];
					particle.FromColor = Colors[0];
					particle.ToColor = Colors[1];
					particle.ColorFactor = particle.CurrentLifeTime / (float)(Colors.Length - 1);
					particle.ColorTime = 0f;
					particleGameObjects[aliveId].Material.SetColor("_TintColor", Colors[0]);
					particle.CurrentColorId = 0;
				}
				else
				{
					particle.CurrentColorId = -1;
				}
				aliveParticles[aliveId] = particle;
				aliveId++;
				deadId++;
			}
			if (OneShot)
			{
				Emit = false;
			}
		}
		for (int j = 0; j < aliveId; j++)
		{
			Particle particle2 = aliveParticles[j];
			PoolGameObject poolGameObject = particleGameObjects[j];
			poolGameObject.transform.localRotation *= Quaternion.AngleAxis(particle2.RotationSpeed * Time.deltaTime, Vector3.forward);
			if (SimulateInWorldSpace)
			{
				poolGameObject.transform.position += particle2.Speed * Time.fixedDeltaTime;
			}
			else if (!SpeedSimulatedInWorldSpace)
			{
				poolGameObject.transform.localPosition += base.transform.localRotation * particle2.Speed * Time.fixedDeltaTime;
			}
			else
			{
				poolGameObject.transform.localPosition += particle2.Speed * Time.fixedDeltaTime;
			}
			if (SimulateFloor)
			{
				if (CastRayPerParticle)
				{
					if (Physics.Raycast(poolGameObject.transform.position, Vector3.down, 1f))
					{
						particle2.Speed.x *= FloorFrictionFactor;
						particle2.Speed.z *= FloorFrictionFactor;
						particle2.Speed.y *= 0f - BounceFactor;
						particle2.Speed += particle2.BounceNoise;
					}
				}
				else if (particle2.Speed.y < 0f && poolGameObject.transform.position.y <= base.transform.position.y + FloorDistance)
				{
					particle2.Speed.x *= FloorFrictionFactor;
					particle2.Speed.z *= FloorFrictionFactor;
					particle2.Speed.y *= 0f - BounceFactor;
					particle2.Speed += particle2.BounceNoise;
				}
			}
			poolGameObject.transform.localScale += particle2.ScaleSpeed * Time.fixedDeltaTime;
			particle2.Speed += force * Time.fixedDeltaTime;
			particle2.ScaleSpeed += ScaleSpeedForce * Time.fixedDeltaTime;
			if (particle2.CurrentColorId != -1 && particle2.CurrentColorId < Colors.Length)
			{
				particle2.CurrentColor = particleGameObjects[j].Material.GetColor("_TintColor");
				particle2.ColorTime += Time.fixedDeltaTime;
				particle2.CurrentColor = Color.Lerp(particle2.FromColor, particle2.ToColor, particle2.ColorTime / particle2.ColorFactor);
				if (particle2.ColorTime >= particle2.ColorFactor)
				{
					particle2.CurrentColorId++;
					if (particle2.CurrentColorId < Colors.Length)
					{
						particle2.CurrentColor = Colors[particle2.CurrentColorId];
						particle2.FromColor = Colors[particle2.CurrentColorId];
						if (particle2.CurrentColorId < Colors.Length - 1)
						{
							particle2.ToColor = Colors[particle2.CurrentColorId + 1];
						}
						else
						{
							particle2.ToColor = particle2.FromColor;
						}
					}
					else
					{
						particle2.FromColor = particle2.ToColor;
					}
					particle2.ColorTime = 0f;
				}
				particleGameObjects[j].Material.SetColor("_TintColor", particle2.CurrentColor);
			}
			particle2.CurrentLifeTime -= Time.fixedDeltaTime;
			if (particle2.CurrentLifeTime <= 0f)
			{
				poolGameObject.Recycle(true);
				deadId--;
				deadParticles[deadId] = particle2;
				if (aliveId > 0)
				{
					aliveId--;
					aliveParticles[j] = aliveParticles[aliveId];
					particleGameObjects[j] = particleGameObjects[aliveId];
					particleRenderers[j] = particleRenderers[aliveId];
					j--;
				}
			}
		}
	}

	public void recycleParticles(bool changeParentImmediately)
	{
		for (int i = 0; i < aliveId; i++)
		{
			PoolGameObject poolGameObject = particleGameObjects[i];
			poolGameObject.Recycle(changeParentImmediately);
			particleGameObjects[i] = null;
			particleRenderers[i] = null;
		}
		while (deadId > 0 && aliveId > 0)
		{
			deadId--;
			aliveId--;
			deadParticles[deadId] = aliveParticles[aliveId];
		}
		aliveId = 0;
		deadId = 0;
	}

	public void recycleAllParticles(bool changeParentImmediately)
	{
		if (particleGameObjects == null)
		{
			return;
		}
		for (int i = 0; i < particleGameObjects.Length; i++)
		{
			if (particleGameObjects[i] != null)
			{
				particleGameObjects[i].Recycle(changeParentImmediately);
			}
			particleGameObjects[i] = null;
			particleRenderers[i] = null;
		}
		aliveId = 0;
		deadId = 0;
	}
}
