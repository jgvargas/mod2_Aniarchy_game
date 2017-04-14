using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour
{
	public int rotateX;
	public int rotateY;
	public int rotateZ;
	// Called once per frame
	void Update()  //(15, 30, 45)
	{
		transform.Rotate (new Vector3  (rotateX, rotateY, rotateZ)* Time.deltaTime);
	}

}
