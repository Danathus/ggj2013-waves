using UnityEngine;
using System.Collections;

public class GameManager : MonoSingleton<GameManager> {
	
	public float testValue = 1.0f;

	AudioClip heartBeat;
	
	
	// Use this for initialization
	void Start () {
		
		heartBeat = (AudioClip)Resources.Load ("GGJ13_Theme", typeof(AudioClip));
		audio.clip = heartBeat;
		audio.Play();

	}
	
	// Update is called once per frame
	void Update () {
		

	
	}
}
