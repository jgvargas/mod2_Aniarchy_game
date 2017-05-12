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
	bool[] spawnerActive = new bool [3];

	Vector3 spawnPosition;

	// Use this for initialization
	public override void OnStartServer()
	{
		if (!isServer)
			return;

		nextSpawn = Time.time + spawnWait;

		// Sets all elements to false
		for (int i = 0; i < numberOfPowerUps; i++)
			spawnerActive [i] = false;

		for (int i = 0; i < numberOfPowerUps; i++)
		{
			init_Spawn(i);
			PowerUpCount++;
		}
	}

	// Update is called once per frame
	void Update () {
		if (!isServer)
			return;

		if (nextSpawn <= Time.time && PowerUpCount < numberOfPowerUps)
		{
			Spawn();
			PowerUpCount++;
			nextSpawn = Time.time + spawnWait;
		}
	}

	public void Despawn( Transform deactivatedPoint )
	{
		// Checks which point was deactivated
		for (int i = 0; i < numberOfPowerUps; i++) {
			// Once deleted object index is known, disable spawn and decrement PickUpCount
			if (deactivatedPoint.position == spawner [i].transform.position) {
				spawnerActive [i] = false;
				PowerUpCount--;
				break;
			}
		}
	}

	void Spawn()
	{
		// Loop threw all possible spawns
		for (int i = 0 ; i < numberOfPowerUps; i++)
		{
			// If spawn is not in use, false, spawn PickUp
			if (spawnerActive [i] == false)
			{
				spawnPosition = spawner [i].transform.position;

				// Sets the same element of spawner as active
				spawnerActive [i] = true;

				GameObject powerUp = (GameObject)Instantiate(powerUpPrefab, spawnPosition, new Quaternion(270, 90, 0, 0));
				NetworkServer.Spawn(powerUp);
				break;
			}
		}
	}

	// Spawns one element at a time
	void init_Spawn(int index)
	{
		spawnPosition = spawner [index].transform.position;

		// Sets the same element of spawner as active
		spawnerActive [index] = true;

		GameObject pickUp = (GameObject)Instantiate(powerUpPrefab, spawnPosition, new Quaternion(270, 90, 0, 0));
		NetworkServer.Spawn(pickUp);
	}
}
