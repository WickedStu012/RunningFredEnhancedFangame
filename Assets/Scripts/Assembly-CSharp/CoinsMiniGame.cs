using System;
using System.Collections.Generic;
using UnityEngine;

public class CoinsMiniGame : MonoBehaviour
{
	private const int MAX_COIN_COUNT = 64;

	private const float MIN_SPAWN_TIME = 0.3f;

	private const float MAX_SPAWN_TIME = 0.8f;

	private const float MIN_SPEED = 0.9f;

	private const float MAX_SPEED = 1.1f;

	public GameObject goldSkullyGO;

	public GameObject silverSkullyGO;

	public AudioClip goldPickSound;

	public AudioClip silverPickSound;

	public GameObject shineGold;

	public GameObject shineSilver;

	private List<Coin> coins = new List<Coin>();

	private List<Coin> coinsPool = new List<Coin>(64);

	private List<Coin> pickedCoins = new List<Coin>();

	private List<ShineObj> shineObjPool = new List<ShineObj>();

	private List<ShineObj> shinesToDestroy = new List<ShineObj>();

	private float currentSpawnTime;

	private float accumTime;

	private void Start()
	{
		currentSpawnTime = 0.3f;
		accumTime = currentSpawnTime;
		for (int i = 0; i < 64; i++)
		{
			coinsPool.Add(new Coin());
		}
	}

	private void Update()
	{
		accumTime += Time.deltaTime;
		if (accumTime >= currentSpawnTime)
		{
			spawnCoin();
			currentSpawnTime = Mathf.Clamp(currentSpawnTime * 0.99f, 0.3f, 0.8f);
			accumTime = 0f;
		}
		updateCoinPos();
		Camera main = Camera.main;
		Vector3 vector = main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, main.nearClipPlane));
		Vector3 vector2 = main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, main.farClipPlane));
		Vector3 direction = vector2 - vector;
		direction.Normalize();
		RaycastHit hitInfo;
		if (Physics.Raycast(vector, direction, out hitInfo, float.PositiveInfinity))
		{
			hitInfo.transform.gameObject.SetActive(false);
			CoinPicked(hitInfo.transform.gameObject.GetComponent<CoinTrigger>().coin);
		}
		checkShines();
	}

	private void spawnCoin()
	{
		if (coinsPool.Count > 0)
		{
			Coin coin = coinsPool[0];
			coinsPool.RemoveAt(0);
			coin.pos = new Vector3(UnityEngine.Random.Range(-900, 900), 700f, UnityEngine.Random.Range(980f, 1020f));
			coin.speed = 0.9f + UnityEngine.Random.Range(0f, 1f) * 0.20000005f;
			if (coin.go == null)
			{
				coin.isGold = ((UnityEngine.Random.Range(0, 10) <= 2) ? true : false);
				coin.go = UnityEngine.Object.Instantiate((!coin.isGold) ? silverSkullyGO : goldSkullyGO) as GameObject;
				coin.go.name = ((!coin.isGold) ? "silverSkully" : "goldSkully");
			}
			coin.go.transform.position = coin.pos;
			coin.go.SetActive(true);
			CoinTrigger component = coin.go.GetComponent<CoinTrigger>();
			component.miniGame = this;
			component.coin = coin;
			for (int i = 0; i < component.ps.Length; i++)
			{
				component.ps[i].Emit = true;
			}
			coins.Add(coin);
		}
	}

	private void updateCoinPos()
	{
		foreach (Coin coin in coins)
		{
			coin.pos = new Vector3(coin.pos.x, coin.pos.y - coin.speed * 5f, 1000f);
			coin.go.transform.position = coin.pos;
			coin.go.transform.RotateAroundLocal(Vector3.up, 0.02f);
			if (coin.pos.y < -700f)
			{
				pickedCoins.Add(coin);
			}
		}
		foreach (Coin pickedCoin in pickedCoins)
		{
			coins.Remove(pickedCoin);
			coinsPool.Add(pickedCoin);
			CoinTrigger component = pickedCoin.go.GetComponent<CoinTrigger>();
			for (int i = 0; i < component.ps.Length; i++)
			{
				component.ps[i].Emit = false;
			}
			pickedCoin.go.SetActive(false);
		}
		pickedCoins.Clear();
	}

	public void CoinPicked(Coin pickedCoin)
	{
		if (pickedCoin.isGold)
		{
			base.gameObject.GetComponent<AudioSource>().clip = goldPickSound;
		}
		else
		{
			base.gameObject.GetComponent<AudioSource>().clip = silverPickSound;
		}
		base.gameObject.GetComponent<AudioSource>().Play();
		coins.Remove(pickedCoin);
		coinsPool.Add(pickedCoin);
		CoinTrigger component = pickedCoin.go.GetComponent<CoinTrigger>();
		for (int i = 0; i < component.ps.Length; i++)
		{
			component.ps[i].Emit = false;
		}
		GameObject go = ((!pickedCoin.isGold) ? (UnityEngine.Object.Instantiate(shineSilver, pickedCoin.pos, Quaternion.identity) as GameObject) : (UnityEngine.Object.Instantiate(shineGold, pickedCoin.pos, Quaternion.identity) as GameObject));
		shineObjPool.Add(new ShineObj(DateTime.Now, go));
	}

	private void checkShines()
	{
		foreach (ShineObj item in shineObjPool)
		{
			if ((DateTime.Now - item.dt).TotalSeconds > 1.0)
			{
				shinesToDestroy.Add(item);
				UnityEngine.Object.Destroy(item.shine);
			}
		}
		foreach (ShineObj item2 in shinesToDestroy)
		{
			shineObjPool.Remove(item2);
		}
		shinesToDestroy.Clear();
	}
}
