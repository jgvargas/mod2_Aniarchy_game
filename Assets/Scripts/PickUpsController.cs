using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PickUpsController : NetworkBehaviour {

    public GameObject pickUpPrefab;
    public int numberOfPickUps;
    public float spawnWait;

    float nextSpawn;

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
        }

       

    }

    // Update is called once per frame
    void Update () {

        if (nextSpawn <= Time.time)
        {
            Spawn();
            nextSpawn = Time.time + spawnWait;
        }
	}

    void Spawn()
    {
        spawnPosition = new Vector3(Random.Range(-20f, 20f), 2.0f, Random.Range(-20f, 20f));

        GameObject pickUp = (GameObject)Instantiate(pickUpPrefab, spawnPosition, new Quaternion(0, 0, 0, 0));
        NetworkServer.Spawn(pickUp);
    }
}
