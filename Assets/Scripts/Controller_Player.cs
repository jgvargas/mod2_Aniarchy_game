using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Controller_Player : NetworkBehaviour {

	// Reference to the level's individual objects
	// Flat level, mid level
	public Renderer []level_mesh;
	public Collider []level_collide;

	// Threshold is used to determine the distance a player falls before respawning
	public float threshold;

	// Assigned an empty game opject for spawn point to be assigned
	public Transform playerSpawnPoint = null;

	// Timer variables:
	public float waitTimer;
	float timer;
	bool timerRunning;

	// Player physics variables:
	public float speed;
	public float speedMod = 1.0f;
	public float jumpHeight = 100.0f;
	private bool onGround = false;

	// Physics component
	public Rigidbody rigidbody_ref;

    Controller_PickUps pkUps;
	Score score;

    public Material[] mats = new Material[3];

    [SyncVar]
    public string pname = "player";

    [SyncVar]
    public int playerMaterial = 0;

    [SyncVar (hook="OnToggleLevel")]
    public bool levelActive = true;

	public Camera cam;
	public float distance = 5.0f;

    public override void OnStartLocalPlayer()
	{
		rigidbody_ref = GetComponent<Rigidbody> ();
        //pkUps = p.GetComponent<Controller_PickUps>();
        //Sets camera target when player is spawned on network
        Camera.main.GetComponent<ThirdPersonCamera>().lookAt = transform;
        cam = GameObject.Find ("Player Camera").GetComponent<Camera>();

		// used to activate/deactivate stage GameObjects
		level_mesh   = GameObject.FindGameObjectWithTag("MainLevel").GetComponentsInChildren<Renderer> ();
		level_collide = GameObject.FindGameObjectWithTag("MainLevel").GetComponentsInChildren<Collider > ();

  

    }

    public void Start()
    {
       
        score = gameObject.GetComponent<Score>();

        levelActive = true;

        //locally sets appropriate player skin for each player
        Renderer[] playerSkins = GetComponentsInChildren<Renderer>();
        foreach (Renderer player in playerSkins)
            player.material = mats[playerMaterial];
    }
        

	//Update: Called before a frame is rendered. 
	void Update()
	{

        if (!pkUps)
            pkUps = GameObject.Find("Spawner_PickUps").GetComponent<Controller_PickUps>();

        // Checks if time for PowerUp has expired
        if (timerRunning)
            PowerUpTimer();

        if (!isLocalPlayer)
            return;

        if (Input.GetKeyDown("space") && onGround == true)
			jump ();


	}

	//FixedUpdate: Before any physics is applied
	void FixedUpdate()
	{
        if (!isLocalPlayer)
            return;

	// Used to spawn player when falling off the map
		if (transform.position.y < threshold) 
		{
			transform.position = playerSpawnPoint.position;
			rigidbody_ref.velocity = new Vector3(0, 0, 0);

			// Also, decrease points of fallen player
			score.SubPoints(1);
			//score.SetCountText();
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
            // Visual effects for pickup

            // Sound effects for pickup

            // Remove Gameobject from stage
            //NetworkServer.Destroy(other.gameObject);
            other.gameObject.SetActive(false);
            pkUps.Despawn(other.gameObject.transform);
            score.AddPoints(1);
        }

		if (other.gameObject.CompareTag ("PowerUp")) 
		{

			NetworkServer.Destroy (other.gameObject);

            // Turns on/off different parts of stage
            // True = 1, False = 0
            timerRunning = true;
            levelActive = false;
            //toggleLevel(false);
            // Cause player to glow, indicates Player has PowerUp

            // Check to see which PowerUp was picked up
            //if (other.name == "SpeedUp")
            //speedMod = 4;
        }

		/*if (other.gameObject.CompareTag ("LevelMod") )
		{

		}*/
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

	void OnToggleLevel( bool status)
	{
		// Consider: Which is switched on/off MidLevel, FlatLevel?
		//			 Each toggleLevel() call should enable/disable

		// True = 1, False = 0
		bool state = status; 
		int rand = Random.Range (0, 1);

        level_mesh = GameObject.FindGameObjectWithTag("MainLevel").GetComponentsInChildren<Renderer>();
        level_collide = GameObject.FindGameObjectWithTag("MainLevel").GetComponentsInChildren<Collider>();

        foreach (Renderer level in level_mesh)
		{
			if (level.CompareTag ("MidLevel") && rand == 0)
				level.enabled = state;
			else if (level.CompareTag ("FlatLevel") && rand == 1)
				level.enabled = state;
		}

		foreach (Collider level in level_collide)
		{
			if (level.CompareTag("MidLevel") && rand == 0)
				level.enabled = state;
			else if (level.CompareTag ("FlatLevel") && rand == 1)
				level.enabled = state;
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
		timer += Time.deltaTime;
		if (timer > waitTimer)
		{
			// The PowerUp time has ended
			timer = 0.0f;
			timerRunning = false;

            // True = 1, False = 0
            levelActive = true;
			//toggleLevel (true);
		}

	}
}