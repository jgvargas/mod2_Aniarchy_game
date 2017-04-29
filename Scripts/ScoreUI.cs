using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreUI : MonoBehaviour {

	public static ScoreUI instance;

	// Use this for initialization
	void Start () {
		;
		instance = this;
	}

	public Text scoreText;
	//public Text winText;
}
