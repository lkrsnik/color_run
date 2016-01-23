﻿using UnityEngine;
using System.Collections;

public class AudioVolume : MonoBehaviour {

	AudioSource source;
	// Use this for initialization
	void Start () {
		source = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
		source.volume = GameManager.instance.audioVolume;
	}
}
