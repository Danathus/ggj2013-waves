using UnityEngine;
using System.Collections;

public class Pulse : MonoBehaviour {
	
	public float baseRadius = 2.0f;
	public float amplitude = 1.0f;
	
	[HideInInspector]
	public float beatsPerSecond = 1.0f;
	
	[HideInInspector]
	public float currentPulseRadius = 0.0f;
	
	float timeElapsed = 0.0f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		
		timeElapsed += Time.deltaTime * beatsPerSecond;
	
		currentPulseRadius = baseRadius + amplitude * Mathf.Sin(timeElapsed * Mathf.PI);
		renderer.material.SetFloat("_PulseRadiusSquared", currentPulseRadius);
	}
}
