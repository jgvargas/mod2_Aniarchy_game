using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {

	public float speed;
	public Text scoreText;
	public Text winText;

	public float jumpSpeed = 100.0f;
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
		if (Input.GetKeyDown("space"))
		{
			rigidbody_ref.AddForce(Vector3.up * jumpSpeed);
		}
	}

	//FixedUpdate: Before any physics is applied
	void FixedUpdate()
	{
		float moveHorizontal = Input.GetAxis ("Horizontal");
		float moveVertical = Input.GetAxis ("Vertical");

		//Ribidbody: class, .AddForce to move RB
		Vector3 movement = new Vector3 (moveHorizontal, 0.0f, moveVertical);

		rigidbody_ref.AddForce (movement * speed);

	}

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
		if (score_count >= 3) {
			winText.text = "You Win!";
		}
	}

	void OnCollisionStay ()
	{
		onGround = true;
	}
}

