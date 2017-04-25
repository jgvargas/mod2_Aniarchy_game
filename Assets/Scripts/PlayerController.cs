using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {

	// Threshold is used to determine the distance a player falls before respawning
	public float threshold;
	// Assigned an empty game opject for spawn point to be assigned
	public Transform playerSpawnPoint = null;

	public float speed;
	public float jumpHeight = 100.0f;
	private bool onGround = false;

	// Private: only accessible from script
	private int score_count;

	// Physics component
	public Rigidbody rigidbody_ref;

	ScoreScript score; 
	Camera cam;


	public float distance = 5.0f;

	void Start()
	{
		rigidbody_ref = GetComponent<Rigidbody> ();
		score = GetComponent<ScoreScript> ();
		cam = GameObject.Find ("Player Camera").GetComponent<Camera>();
	}

	//Update: Called before a frame is rendered. 
	void Update()
	{
		if (Input.GetKeyDown("space") && onGround == true)
		{
			jump ();
		}
	}

	//FixedUpdate: Before any physics is applied
	void FixedUpdate()
	{
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
		if (other.gameObject.CompareTag("PickUp"))
		{
			other.gameObject.SetActive( false);
			score.AddPoints(1);
			score.SetCountText();
		}

		if (other.gameObject. CompareTag ("Power Up")) {
			//

		}
	}

	// OnCollisionStay: Called once per frame for every collider/rigidbody
	// 					that is touching rigidbody/collider
	void OnCollisionStay ()
	{
		onGround = true;
	}

	void jump()
	{
		Vector3 jump = new Vector3 (0.0f, jumpHeight, 0.0f);
		rigidbody_ref.AddForce (jump);

		onGround = false;
	}
}