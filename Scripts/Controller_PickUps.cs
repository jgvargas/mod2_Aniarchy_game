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

    Vector3 spawnPosition;
    
    // Use this for initialization
    public override void OnStartServer()
    {
        if (!isServer)
            return;

        nextSpawn = Time.time + spawnWait;

        for (int i = 0; i < numberOfPickUps; i++)
        {
            Spawn();
			PickUpCount++;
        }
    }

    // Update is called once per frame
    void Update () {

		if (nextSpawn <= Time.time && PickUpCount < numberOfPickUps)
        {
            Spawn();
			PickUpCount++;
            nextSpawn = Time.time + spawnWait;
        }
	}

    void Spawn()
    {
		spawnPosition = spawner [PickUpCount].transform.position;

        GameObject pickUp = (GameObject)Instantiate(pickUpPrefab, spawnPosition, new Quaternion(0, 0, 0, 0));
        NetworkServer.Spawn(pickUp);
    }
}
