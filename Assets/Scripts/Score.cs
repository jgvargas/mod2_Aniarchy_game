using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Score : NetworkBehaviour
{

    // Private: only accessible from script
    [SyncVar (hook = "OnChangeScore")] public int score_count;
    public GameObject[] players;
        string scores = "";

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

    public void OnChangeScore(int s)
    {

        //if (!isServer)
        //    return;
        //players = GameObject.FindGameObjectsWithTag("Player");


       // scoreUI.instance.scoreText.text = scores;//"Score: " + s.ToString(); ;

    }

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

        scores = "";
        players = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject p in players)
        {
            scores += "Score: " + p.GetComponent<Score>().score_count.ToString() + "      ";
        }


        RpcUpdateUI(scores);
    }

    public void SubPoints(int i)
    {
        if (!isServer)
            return;

        score_count -= i;
    }
}
