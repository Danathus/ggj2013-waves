using UnityEngine;
using System.Collections;

public class TestXBoxControl : MonoBehaviour {
	
		private float unitsPerSecond = 10.0f;
		GameObject[] player;
	
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
	
		/*if (Input.GetAxis ("Horizontal")) {
			Debug.Log("Fire1");
		}*/
		
				Vector3 direction = Vector3.zero;
		
		/*if (Input.GetButton(KeyCode.Joystick1Button8)) {
			
		//	direction += new Vector3(Input.GetAxis("Horizontal"),0,0);
		}*/
		player[0].transform.Translate (Input.GetAxis ("Horizontal") *Time.deltaTime * unitsPerSecond,0,0);
		player[0].transform.Translate(0,Input.GetAxis("Vertical") * Time.deltaTime * unitsPerSecond, 0);
		
		/*if (Input.GetKey(KeyCode.LeftArrow)) {
			direction -= Vector3.right;
		}
		
		if (Input.GetKey(KeyCode.UpArrow)) {
			direction += Vector3.up;
		}
		
		if (Input.GetKey(KeyCode.DownArrow)) {
			direction -= Vector3.up;
		}

		obj.transform.position += direction * Time.deltaTime * unitsPerSecond;*/
	}
}
