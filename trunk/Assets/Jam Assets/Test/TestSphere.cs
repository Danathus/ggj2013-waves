using UnityEngine;
using System.Collections;

public class TestSphere : MonoBehaviour {
	
	public float scaleAmount;
	
	System.Action[] updateList;
	System.Action currentUpdate;
	
	Vector3 originalScale;

	// Use this for initialization
	void Start () {
	
		originalScale = transform.localScale;
		currentUpdate = UpdateLinear;
	}
	
	// Update is called once per frame
	void Update () {
	
		currentUpdate();
	}
	
	void UpdateLinear() {
	}
}
