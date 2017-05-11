using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using Prototype.NetworkLobby;


public class TimeUI : NetworkBehaviour {

    public Text TimerText;
    [SyncVar]public float targetTime = 180.0f;

    float endCountdown = 5f;
    private GameObject[] players;
    int[] scores = new int[] {0, 0, 0, 0};
    string[] names = new string[] {"", "", "", ""};

    public GameObject displayPanel;
    public Text winText;
    public Text rankingText;

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
            //LobbyManager.s_Singleton.ServerReturnToLobby();
            //wait
            if (endCountdown >= 0)
            {
                //finds player score and names and sorts based on scores
                if (winText.text == "")
                {
                    players = GameObject.FindGameObjectsWithTag("Player");
                    int i = 0;
                    foreach (GameObject player in players)
                    {
                        scores[i] = player.GetComponent<Score>().score_count;
                        names[i] = player.GetComponent<Controller_Player>().playerName;
                        i++;

                       // print(names[i]);
                    }

                    Array.Sort(scores, names);
                    displayPanel.GetComponent<RawImage>().color = new Color(1f, 1f, 1f, .5f);
                    winText.text = names[3] + " WINS!!!";

                    for(int j = 3; j >= 0; j--)
                    {
                        rankingText.text += 4-j + ": " + names[j] + " - " + scores[j] + "\n";
                    }
                }

                
                
                endCountdown -= Time.deltaTime;
            }

            else
            { //load main menu
                EndRound();
            }
            //StartCoroutine(ReturnToLobby());

        }


    }

    void EndRound()
    {
        LobbyManager.s_Singleton.ServerReturnToLobby();
    }

}

//public class myReverserClass : IComparer
//{

    // Calls CaseInsensitiveComparer.Compare with the parameters reversed.
   /// int IComparer.Compare(object x, object y)
   // {
  //      return ((new CaseInsensitiveComparer()).Compare(y, x));
   // }

//}


