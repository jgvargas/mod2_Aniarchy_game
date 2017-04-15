using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class PlayerController : NetworkBehaviour {

	public float speed;

	// Threshold is used to determine the distance a player falls before respawning
	public float threshold;
	// Assigned an empty game opject for spawn point to be assigned
	public Transform playerSpawnPoint;

	public float jumpSpeed = 100.0f;
	private bool onGround = false;

    //Used to assing an individual player a score
    ScoreScript score;

	// Physics component
	Rigidbody rigidbody_ref;

    public override void OnStartLocalPlayer()
    {
        score = GameObject.Find("GameManager").GetComponent<ScoreScript>();
        playerSpawnPoint = GameObject.Find("SpawnPoint1").transform;
        Camera.main.GetComponent<CameraController>().target = transform;
        rigidbody_ref = GetComponent<Rigidbody>();

    }

    void Start()
	{

	}

	//Update: Called before a frame is rendered. 
	void Update()
	{
        if (!isLocalPlayer)
            return;

        if (Input.GetKeyDown("space"))
		{
			rigidbody_ref.AddForce(Vector3.up * jumpSpeed);
		}
	}

	//FixedUpdate: Before any physics is applied
	void FixedUpdate()
	{
        if (!isLocalPlayer)
            return;

	// Used to spawn player when falling off the map
		if (transform.position.y < threshold) {
			transform.position = playerSpawnPoint.position;
			rigidbody_ref.velocity = new Vector3(0, 0, 0);
		}

	// Code for player movement
		float moveHorizontal = Input.GetAxis ("Horizontal");
		float moveVertical = Input.GetAxis ("Vertical");

		//Ribidbody: class, .AddForce to move RB
		Vector3 movement = new Vector3 (moveHorizontal, 0.0f, moveVertical);

		rigidbody_ref.AddForce (movement * speed);

	}

	//
	void OnTriggerEnter( Collider other)
	{
		if (other.gameObject.CompareTag("PickUp"))
		{
			other.gameObject.SetActive( false);
            score.AddPoints(1);
			score.SetCountText ();
		}

	}

    void OnCollisionEnter( Collision col )
    {

        if (col.gameObject.CompareTag("Player"))
        {
            Rigidbody otherRigidbody = col.collider.GetComponent<Rigidbody>();
            Vector3 test = GetComponent<Rigidbody>().velocity;

            otherRigidbody.AddForce(test * otherRigidbody.mass * Time.deltaTime * 100);
        }

        
    }


	// OnCollisionStay: Called once per frame for every collider/rigidbody
	// 					that is touching rigidbody/collider
	void OnCollisionStay ()
	{
		onGround = true;
	}
}

