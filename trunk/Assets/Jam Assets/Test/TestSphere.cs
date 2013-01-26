using UnityEngine;
using System.Collections;

public class TestSphere : MonoBehaviour {
	
	public float scaleAmount;
	
	System.Action[] updateList;
	System.Action currentUpdate;

	// Use this for initialization
	void Start () {
	
		//riginalScale = transform.localScale;
		currentUpdate = UpdateLinear;
	}
	
	// Update is called once per frame
	void Update () {
	
		currentUpdate();
	}
	
	void UpdateLinear() {
	}
}
