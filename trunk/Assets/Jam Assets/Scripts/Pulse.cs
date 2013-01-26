using UnityEngine;
using System.Collections;

public class Pulse : MonoBehaviour {
	
	public float beatsPerSecond = 1.0f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
		float radius = 2.0f + Mathf.Sin(Time.timeSinceLevelLoad * Mathf.PI * beatsPerSecond);
		renderer.material.SetFloat("_PulseRadiusSquared", radius);
	}
}