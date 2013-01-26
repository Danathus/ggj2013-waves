using UnityEngine;
using System.Collections;

public class TestScript : MonoBehaviour {
	
	private float unitsPerSecond = 10.0f;
	GameObject[] player;
	
	void Awake() {
		
	}

	// Use this for initialization
	void Start () {
		Camera.main.transform.position += Vector3.back * 10;

		GameObject spherePrefab = (GameObject)Resources.Load("Sphere");

		int numPlayers = 4;
		player = new GameObject[numPlayers];
		for (int i = 0; i < numPlayers; ++i)
		{
			player[i] = (GameObject)GameObject.Instantiate(spherePrefab);
			player[i].transform.position += Vector3.right * i;
		}
	}
	
	// Update is called once per frame
	void Update () {

		Vector3 direction = Vector3.zero;

		if (Input.GetKey(KeyCode.RightArrow)) {
			direction += Vector3.right;
		}
		
		if (Input.GetKey(KeyCode.LeftArrow)) {
			direction -= Vector3.right;
		}
		
		if (Input.GetKey(KeyCode.UpArrow)) {
			direction += Vector3.up;
		}
		
		if (Input.GetKey(KeyCode.DownArrow)) {
			direction -= Vector3.up;
		}
		//Input.GetButtonDown("Fire1")
		

		player[0].transform.position += direction * Time.deltaTime * unitsPerSecond;
	}
}
