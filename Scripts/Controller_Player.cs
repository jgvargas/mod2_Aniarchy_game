using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Controller_Player : NetworkBehaviour {

	// Reference to the level's individual objects
	// Flat level, mid level
	public GameObject []level_mesh;
	//public GameObject []level_flat;

	// Threshold is used to determine the distance a player falls before respawning
	public float threshold;

	// Assigned an empty game opject for spawn point to be assigned
	public Transform playerSpawnPoint = null;

	public float powerTimer;

	public float speed;
	public float speedMod = 1.0f;
	public float jumpHeight = 100.0f;
	private bool onGround = false;

	// Physics component
	public Rigidbody rigidbody_ref;

	ScoreScript score;

	Camera cam;
	public float distance = 5.0f;

    public override void OnStartLocalPlayer()
	{
		rigidbody_ref = GetComponent<Rigidbody> ();
		
        score = GameObject.FindGameObjectWithTag("ScoreManager").GetComponent<ScoreScript>();
        //Sets camera target when player is spawned on network
        Camera.main.GetComponent<ThirdPersonCamera>().lookAt = transform;
        cam = GameObject.Find ("Player Camera").GetComponent<Camera>();

		powerTimer = Time.time + 10;
        
		//GameObject powerUps = Game
		//level_mid = GameObject.FindGameObjectWithTag("MainLevel").GetComponentsInChildren<GameObject>().FindGameObjectsWithTag("MidLevel");
		//level_flat = GameObject.FindGameObjectWithTag("MainLevel").GetComponentsInChildren<GameObject>().FindGameObjectsWithTag("FlatLevel");

	}

	public void Start()
	{

	}

	//Update: Called before a frame is rendered. 
	void Update()
	{
        if (!isLocalPlayer)
            return;

		if (Input.GetKeyDown("space") && onGround == true)
		{
			jump ();
		}

		// Checks if time for PowerUp has expired
		PowerUpTimer ();
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

			// Also, decrease points of fallen player
			score.SubPoints(1);
			score.SetCountText();
		}
	// Code for player movement
		float moveHorizontal = Input.GetAxis ("Horizontal");
		float moveVertical = Input.GetAxis ("Vertical");

		//Ribidbody: class, .AddForce to move RB
		Vector3 movement = new Vector3 (moveHorizontal, 0.0f, moveVertical);

		Vector3 actMovement = cam.transform.TransformDirection (movement);

		rigidbody_ref.AddForce (actMovement * (speed * speedMod));

	}
		
	void OnTriggerEnter( Collider other)
	{
		// Calculates points from ScoreScript
        if (other.gameObject.CompareTag("PickUp"))
		{
			
            NetworkServer.Destroy(other.gameObject);

			score.AddPoints(1);
			score.SetCountText();
		}

		if (other.gameObject.CompareTag ("PowerUp")) 
		{
			NetworkServer.Destroy (other.gameObject);

			//foreach (GameObject level in level_mid)
			//{
			//	level.enabled = false;
			//}
			// Cause player to glow, indicates Player has PowerUp

			// Check to see which PowerUp was picked up
			//if (other.name == "SpeedUp")
				//speedMod = 4;
		}
	}

	// OnCollisionStay: Called once per frame for every collider/rigidbody
	// 					that is touching rigidbody/collider
	void OnCollisionStay ()
	{
		onGround = true;
	}

    //Calculates force to apply for collision
    void OnCollisionEnter(Collision col)
    {

        if (col.gameObject.CompareTag("Player"))
        {
            Rigidbody otherRigidbody = col.collider.GetComponent<Rigidbody>();
            Vector3 test = GetComponent<Rigidbody>().velocity;


            otherRigidbody.AddRelativeForce(test);
            test = Vector3.Reflect(test, Vector3.right);
        }

    }

    void jump()
	{
		Vector3 jump = new Vector3 (0.0f, jumpHeight, 0.0f);
		rigidbody_ref.AddForce (jump);

		onGround = false;
	}

	void PowerUpTimer()
	{
		//Update time
		float timeLeft = powerTimer - Time.time;

		//Time expired; reset values
		if (timeLeft < 0) 
		{
			timeLeft = 0.0f;
			speedMod = 1;
		}

	}
}