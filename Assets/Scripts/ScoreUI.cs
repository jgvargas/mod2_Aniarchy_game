using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreUI : MonoBehaviour {

	public static ScoreUI instance;

	// Use this for initialization
	void Start () {
		instance = this;
	}
    
	public Text[] scoreText = new Text[4];
    public Text[] nameText = new Text[4];
    public RawImage[] portraits = new RawImage[4];

    //array to hold character textures
    public Texture[] characters = new Texture[6];
}
