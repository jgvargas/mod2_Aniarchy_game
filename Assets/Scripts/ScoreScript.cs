﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreScript : MonoBehaviour {

    public Text scoreText;
    public Text winText;
    public int winningScore = 3;

    // Private: only accessible from script
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
        scoreText.text = "Count: " + score_count.ToString ();

        if (score_count >= winningScore) {
        	winText.text = "You Win!";
        }
    }

    public void AddPoints(int i)
    {
        score_count += i;
    }
}