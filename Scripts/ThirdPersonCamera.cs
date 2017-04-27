using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour {

	public Transform lookAt;
	public Transform camTransform;

	private const float Y_MIN = 5.0f;
	private const float Y_MAX = 50.0f;

	private Camera cam;

	public float distance;
	private float currentX;
	private float currentY;
	public float sensivityX;
	public float sensivityY;

	// Use this for initialization
	void Start () {
		camTransform = transform;
		cam = Camera.main;
	}
	
	// Update is called once per frame
	private void Update()
	{
		currentX += Input.GetAxis ("Mouse X");
		currentY -= Input.GetAxis ("Mouse Y");

		currentY = Mathf.Clamp (currentY, Y_MIN, Y_MAX);
	}

	private void OnCollisionEnter(Collision other)
	{
		//if the camera collides with something
	}

	private void LateUpdate () {
        Vector3 direction = new Vector3 (0, 0, -distance);
        Quaternion rotation = Quaternion.Euler (currentY, currentX, 0);
        camTransform.position = lookAt.position + rotation * direction;

		camTransform.LookAt (lookAt.position);
	}
}
