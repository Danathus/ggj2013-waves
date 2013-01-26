using UnityEngine;
using System.Collections;

public class TestScript : MonoBehaviour {
	
	public float unitsPerSecond = 1.0f;
	
	void Awake() {
		
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		
		if (Input.GetKey(KeyCode.RightArrow)) {
			transform.position += Vector3.right * Time.deltaTime * unitsPerSecond;
		}
		
		if (Input.GetKey(KeyCode.LeftArrow)) {
			transform.position += -Vector3.right * Time.deltaTime * unitsPerSecond;
		}
	
	}
}
