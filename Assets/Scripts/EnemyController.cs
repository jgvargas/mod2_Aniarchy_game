using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour {

	NavMeshAgent agent;
	GameObject[] collectables;
	Vector3 pos;
	Vector3 test;
	Rigidbody r;

	// Use this for initialization
	void Start () {

		agent = GetComponent<NavMeshAgent>();
		r = GetComponent<Rigidbody>();

		SetNewTarget();
	}

	// Update is called once per frame
	void Update () {

		int layerMask = 1 << 8;

		Debug.DrawLine(transform.position, pos, Color.red);

		if (Physics.Linecast(transform.position, pos, layerMask) && agent.isActiveAndEnabled)
		{//checks if obstacle is between current position and target position

			Vector3 direction = pos - transform.position;

			//clamps jump direction and distance
			if (direction.x > 0 && direction.x > 3)
				direction.x = 3;
			if (direction.z < 0 && direction.z < -3)
				direction.z = -3;
			direction.y += 6;

			agent.enabled = false;
			r.isKinematic = false;
			r.useGravity = true;

			r.AddForce(direction, ForceMode.Impulse);

		}
		else if (transform.position.y <= 1  && !agent.isActiveAndEnabled )
		{
			agent.enabled = true;
			r.isKinematic = true;
			r.useGravity = false;
			SetNewTarget();
		}

		if (Mathf.Abs(pos.x - transform.position.x) < 1 && Mathf.Abs(pos.z - transform.position.z) < 1)
		{
			SetNewTarget();
		}

	}

	void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("PickUp"))
		{
			other.gameObject.SetActive(false);
			SetNewTarget();
		}

	}

	void SetNewTarget()
	{
		collectables = GameObject.FindGameObjectsWithTag("PickUp");

		if (collectables != null && agent.isActiveAndEnabled)
		{
			float min = 0f;

			//find shortest travel position
			foreach (GameObject c in collectables)
			{
				float dist = Vector3.Distance(c.transform.position, transform.position);

				//print(dist);
				if (min <= 0)
				{
					min = dist;
					pos = c.transform.position;
				}
				else if (dist < min)
				{
					min = dist;
					pos = c.transform.position;
				}

			}

			agent.SetDestination(pos);
		}

	}
}
