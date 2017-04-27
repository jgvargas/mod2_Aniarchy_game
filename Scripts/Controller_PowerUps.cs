using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Controller_PowerUps : NetworkBehaviour {

	public GameObject powerUpPrefab;
	public int numberOfPowerUps;
	private int PowerUpCount;
	public float spawnWait;

	float nextSpawn;

	public Transform[] spawner;

	Vector3 spawnPosition;

	// Use this for initialization
	public override void OnStartServer()
	{
		if (!isServer)
			return;

		nextSpawn = Time.time + spawnWait;

		for (int i = 0; i < numberOfPowerUps; i++)
		{
			Spawn();
			PowerUpCount++;
		}
	}

	// Update is called once per frame
	void Update () {

		if (nextSpawn <= Time.time && PowerUpCount < numberOfPowerUps)
		{
			Spawn();
			PowerUpCount++;
			nextSpawn = Time.time + spawnWait;
		}
	}

	void Spawn()
	{
		spawnPosition = spawner [PowerUpCount].transform.position;

		GameObject pickUp = (GameObject)Instantiate(powerUpPrefab, spawnPosition, new Quaternion(0, 0, 0, 0));
		NetworkServer.Spawn(pickUp);
	}
}
