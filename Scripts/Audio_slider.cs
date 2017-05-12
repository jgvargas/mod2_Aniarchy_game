using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Audio_slider : MonoBehaviour {

	// Use this for initialization
	void setVolume (Slider s)
	{
		AudioListener.volume = s.value;
	}
}
