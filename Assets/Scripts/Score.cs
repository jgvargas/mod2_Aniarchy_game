using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Score : NetworkBehaviour
{

	// Private: only accessible from script
	[SyncVar] public int score_count;
	GameObject[] players;

	// Use this for initialization
	void Start()
	{
		players = GameObject.FindGameObjectsWithTag("Player");  
		score_count = 0;
	}

	// Update is called once per frame
	void Update()
	{

	}

	//Updates UI text to server and all clients
	[ClientRpc]
	void RpcUpdateUI(string[] scores, string[] names, int[] portraits)
	{

        int i = 0;
        //updates each UI instance for all player using data given from server
        foreach (string s in scores)
        {
            if (s != "")
            {
                ScoreUI.instance.scoreText[i].text = scores[i];
                ScoreUI.instance.nameText[i].text = names[i];
                ScoreUI.instance.portraits[i].color = new Color(1f, 1f, 1f, 1f);
                ScoreUI.instance.portraits[i].texture = FindTexture(portraits[i]);
                i++;
            }
       }
	}

    //Adds points on Server
    public void AddPoints(int i)
	{
		if (!isServer)
			return;

		score_count += i;

		SetUIText();
	}

    //subtracts points on Server
    public void SubPoints(int i)
	{
		if (!isServer)
			return;

		score_count -= i;

		SetUIText();
	}

	//sets text to be used in UI
	void SetUIText()
	{
		string[] scores = new string[4];
        string[] names = new string[4];
        int[] portaits = new int[4];

        //find players to send information to UI
        players = GameObject.FindGameObjectsWithTag("Player");
        int i = 0;

        //fetches information from player and sets UI elements
		foreach (GameObject p in players)
		{
			scores[i] = "Score: " + p.GetComponent<Score>().score_count.ToString();
            names[i] = p.GetComponent<Controller_Player>().playerName;
            portaits[i] = p.GetComponent<Controller_Player>().playerMaterial;
            i++;
		}

        //Update UI on all clients with information sent
		RpcUpdateUI(scores, names, portaits);
	}

    Texture FindTexture(int indx)
    {
        return ScoreUI.instance.characters[indx];
    }
}
