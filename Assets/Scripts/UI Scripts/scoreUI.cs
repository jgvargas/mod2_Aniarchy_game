using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class scoreUI : MonoBehaviour {

    public static scoreUI instance;

    // Use this for initialization
    void Start () {
     ;
        instance = this;
	}

    public Text scoreText;
    //public Text winText;
}
