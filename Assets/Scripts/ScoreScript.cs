using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class ScoreScript : MonoBehaviour {

	public Text scoreText;
	public Text winText;
	public int winningScore;

	// Private: only accessible from script
    //[SyncVar]
	private int score_count;

	// Use this for initialization
	void Start () {

		score_count = 0;
		SetCountText();
		winText.text = "";
	}

	// Update is called once per frame
	void Update () {

	}

	public void SetCountText()
	{
        scoreText.text = "Score: " + score_count.ToString();

		if (score_count >= winningScore) {
			winText.text = "You Win!";
		}
	}

	public void AddPoints(int i)
	{


		score_count += i;
	}

	public void SubPoints( int i)
	{
        score_count -= i;
	}
}
