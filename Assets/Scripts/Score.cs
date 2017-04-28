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
    void RpcUpdateUI(string scores)
    {
        scoreUI.instance.scoreText.text = scores;       
    }

    public void AddPoints(int i)
    {
        if (!isServer)
            return;

        score_count += i;

        SetUIText();
    }

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
        string scores = "";

        players = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject p in players)
        {
            scores += "Score: " + p.GetComponent<Score>().score_count.ToString() + "      ";
        }

        RpcUpdateUI(scores);
    }
}
