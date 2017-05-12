using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class TimeUI : NetworkBehaviour {

	public Text TimerText;
	[SyncVar]public float targetTime = 300.0f;
    float endCountdown = 5f;

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
        else
        {
            //display score


            //wait
            if (endCountdown >= 0)
            {
                endCountdown -= Time.deltaTime;
            }
            else
            { //load main menu
                SceneManager.LoadScene(0);
            }
          
        }
	}


}

