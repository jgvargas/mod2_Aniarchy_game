using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {
	
	public Text scoreText;
	public Text winText;
	public int winningScore;

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

	void Start()
	{
		score_count = 0;
		rigidbody_ref = GetComponent<Rigidbody> ();
		SetCountText ();
		winText.text = "";
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
			score_count = score_count + 1;
			SetCountText ();
		}
	}

	void SetCountText()
	{
		scoreText.text = "Count: " + score_count.ToString ();
		if (score_count >= winningScore) {
			winText.text = "You Win!";
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

