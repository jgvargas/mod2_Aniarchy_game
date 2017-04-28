using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class timeUI : NetworkBehaviour {

    public Text TimerText;
    //public static timeUI instance;
    [SyncVar] private float targetTime = 300.0f;

    // Use this for initialization
    void Start () {

        TimerText.text =  targetTime.ToString("N2");
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (targetTime >= 0)
        {
            string minutes = Mathf.Floor(targetTime / 60).ToString();
            string seconds = Mathf.Floor(targetTime % 60).ToString("00");

            targetTime -= Time.deltaTime;


            TimerText.text = minutes + ":" + seconds;
        }
    }


}
