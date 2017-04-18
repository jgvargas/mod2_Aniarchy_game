using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class PlayerController : NetworkBehaviour {

	// Threshold is used to determine the distance a player falls before respawning
	public float threshold;
	// Assigned an empty game opject for spawn point to be assigned
	public Transform playerSpawnPoint = null;

	public float speed;
	public float jumpHeight = 100.0f;
	private bool onGround = false;

	// Private: only accessible from script
	//private int score_count;

	// Physics component
	public Rigidbody rigidbody_ref;

	ScoreScript score; 
	Camera cam;


	public float distance = 5.0f;

   // public override void OnSta()
    //{
        //score = GetComponent<ScoreScript>();
      //  score = GameObject.Find("RoundManager").GetComponent<ScoreScript>();
    //}

    public override void OnStartLocalPlayer()
	{
		rigidbody_ref = GetComponent<Rigidbody> ();
		
        score = GameObject.FindGameObjectWithTag("ScoreManager").GetComponent<ScoreScript>();
        //Sets camera target when player is spawned on network
        Camera.main.GetComponent<ThirdPersonCamera>().lookAt = transform;
        cam = GameObject.Find ("Player Camera").GetComponent<Camera>();
        
    }

    void Start()
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

		// Ignores player input for movement and auto moves
		//transform.position = transform.position + Camera.main.transform.forward * distance * Time.deltaTime;

		//Ribidbody: class, .AddForce to move RB
		Vector3 movement = new Vector3 (moveHorizontal, 0.0f, moveVertical);

		Vector3 actMovement = cam.transform.TransformDirection (movement);
		//Vector3 movement = new Vector3 (moveHorizontal, 0.0f, moveVertical * cam.y);

		//Transform actualDirection = cam. (movement);
		rigidbody_ref.AddForce (actMovement * speed);

	}

	// Calculates points from ScoreScript
	void OnTriggerEnter( Collider other)
	{
        // (!NetworkServer.active)
        //    return;

        if (other.gameObject.CompareTag("PickUp"))
		{
            NetworkServer.Destroy(other.gameObject);

			score.AddPoints(1);
			score.SetCountText();
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
}