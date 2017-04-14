/* Third person camera rig: followers player in a third person
 * 
 * credit: code.tutsplus.com/tutorials
 */

using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

	public GameObject player;
	public Vector3 offset;


	//Used to adjust damping of camera angle and player orientation
	public float damping = 1;


	void Start ()
	{
		offset = transform.position - player.transform.position;
	}

	void LateUpdate ()
	{	
		// Old implementation
		transform.position = player.transform.position + offset;
		/*
		float currentAngle = transform.eulerAngles.y;
		
		float wantedAngle = player.transform.eulerAngles.y;
		float angle = Mathf.LerpAngle( currentAngle, wantedAngle, Time.deltaTime* damping);

		Quaternion rotation = Quaternion.Euler(0, wantedAngle, 0);
		transform.position = player.transform.position - (rotation * offset);

		transform.LookAt(player.transform);

*/
	}
}