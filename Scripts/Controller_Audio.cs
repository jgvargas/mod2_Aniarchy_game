using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(AudioSource))]
public class Controller_Audio : NetworkBehaviour {

	private AudioSource source;

	public AudioClip[] clips;
	// Use this for initialization 
	void Start () {
		source = this.GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	public void playSound (int id) {
		// Is there a valid sound id
		if (id >= 0 &&  id < clips.Length)
		CmdServerSoundId (id);
	}

	[Command]
	void CmdServerSoundId(int id){

		// Seems a response to all other clients (other instances of the game)  
		RpcSendSoundIdToClient (id);

	}

	[ClientRpc]
	void RpcSendSoundIdToClient (int id)
	{
		// Actually plays the audio clip 
		source.PlayOneShot (clips [id]); 
	}
}
