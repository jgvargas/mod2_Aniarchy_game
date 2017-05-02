using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Controller_PickUps : NetworkBehaviour {

    public GameObject pickUpPrefab;
    public int numberOfPickUps;
	private int PickUpCount;
	public float spawnWait;

	// Time until the next pick up is spawned
    float nextSpawn;

	// Array used to hold all possible spawn locations
	public Transform[] spawner;
	bool []spawnerActive = new bool [6];
    
    Vector3 spawnPosition;
    
    // Use this for initialization
    public override void OnStartServer()
    {
        if (!isServer)
            return;

        nextSpawn = Time.time + spawnWait;

		// Sets all elements to false
		for (int i = 0; i < numberOfPickUps; i++)
			spawnerActive[i] = false;

		// Spawns PickUps in all available spots
        for (int i = 0; i < numberOfPickUps; i++)
        {
            init_Spawn(i);
			PickUpCount++;
        }
    }

    // Update is called once per frame
    void Update () {
        if (!isServer)
            return;

		if (nextSpawn <= Time.time && PickUpCount < numberOfPickUps)
        {
            Spawn();
			PickUpCount++;
            nextSpawn = Time.time + spawnWait;
        }
	}

	public void Despawn( Transform deactivatedPoint)
	{
		// Checks which point was deactivated
		for (int i = 0; i < numberOfPickUps; i++) {
			// Once deleted object index is known, disable spawn and decrement PickUpCount
			if (deactivatedPoint.position == spawner [i].transform.position) {
				spawnerActive [i] = false;
				PickUpCount--;
				break;
			}
		}
	}

    void Spawn()
    {
		// Loop threw all possible spawns
		for (int i = 0 ; i < numberOfPickUps; i++)
		{
			// If spawn is not in use, false, spawn PickUp
			if (spawnerActive [i] == false)
			{
				spawnPosition = spawner [i].transform.position;

				// Sets the same element of spawner as active
				spawnerActive [i] = true;

				GameObject pickUp = (GameObject)Instantiate(pickUpPrefab, spawnPosition, new Quaternion(0, 0, 0, 0));
				NetworkServer.Spawn(pickUp);
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

		GameObject pickUp = (GameObject)Instantiate(pickUpPrefab, spawnPosition, new Quaternion(0, 0, 0, 0));
		NetworkServer.Spawn(pickUp);
	}
}
