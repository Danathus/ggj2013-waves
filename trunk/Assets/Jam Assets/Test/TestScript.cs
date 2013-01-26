using UnityEngine;
using System.Collections;

public class TestScript : MonoBehaviour {
	
	private float unitsPerSecond = 10.0f;
	
	void Awake() {
		
	}

	// Use this for initialization
	void Start () {
		Camera.main.transform.position += Vector3.back * 10;
	}
	
	// Update is called once per frame
	void Update () {
		
		if (Input.GetKey(KeyCode.RightArrow)) {
			transform.position += Vector3.right * Time.deltaTime * unitsPerSecond;
		}
		
		if (Input.GetKey(KeyCode.LeftArrow)) {
			transform.position += -Vector3.right * Time.deltaTime * unitsPerSecond;
		}
		
		if (Input.GetKey(KeyCode.UpArrow)) {
			transform.position += Vector3.up * Time.deltaTime * unitsPerSecond;
		}
		
		if (Input.GetKey(KeyCode.DownArrow)) {
			transform.position += -Vector3.up * Time.deltaTime * unitsPerSecond;
		}

	}
}
