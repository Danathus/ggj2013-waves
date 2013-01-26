using UnityEngine;
using System.Collections;

public class TestXBoxControl : MonoBehaviour {
	
	private float unitsPerSecond = 10.0f;
	GameObject[] player;
	
	AudioClip heartBeat;
	
	// Use this for initialization
	void Start () {
			Camera.main.transform.position += Vector3.back * 10;

		GameObject spherePrefab = (GameObject)Resources.Load("Sphere");

		heartBeat = (AudioClip)Resources.Load ("GGJ13_Theme", typeof(AudioClip));
		
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
		
		
		/*if (Input.GetButton(KeyCode.Joystick1Button8)) {
			
		//	direction += new Vector3(Input.GetAxis("Horizontal"),0,0);
		}*/
		player[0].transform.Translate (Input.GetAxis ("L_XAxis_1") *Time.deltaTime * unitsPerSecond,0,0);
		//if(Input.GetAxis("L_XAxis_1") > 0.5)
		//	Debug.Log("L_XAxis_1");
		player[0].transform.Translate(0,Input.GetAxis("L_YAxis_1") * Time.deltaTime * unitsPerSecond, 0);
		//if(Input.GetAxis("L_YAxis_1") > 0.5)
		//	Debug.Log("L_YAxis_1");
		
		player[1].transform.Translate (Input.GetAxis ("L_XAxis_2") *Time.deltaTime * unitsPerSecond,0,0);
		//if(Input.GetAxis("L_XAxis_2") > 0.5)
		//	Debug.Log("L_XAxis_2");
		player[1].transform.Translate(0,Input.GetAxis("L_YAxis_2") * Time.deltaTime * unitsPerSecond, 0);
		//if(Input.GetAxis("L_YAxis_2") > 0.5)
		//	Debug.Log("L_YAxis_2");
		
		player[2].transform.Translate (Input.GetAxis ("L_XAxis_3") *Time.deltaTime * unitsPerSecond,0,0);
		//if(Input.GetAxis("L_XAxis_3") > 0.5)
		//	Debug.Log("L_XAxis_3");
		player[2].transform.Translate(0,Input.GetAxis("L_YAxis_3") * Time.deltaTime * unitsPerSecond, 0);
		//if(Input.GetAxis("L_YAxis_3") > 0.5)
		//	Debug.Log("L_YAxis_3");
		
		player[3].transform.Translate (Input.GetAxis ("L_XAxis_4") *Time.deltaTime * unitsPerSecond,0,0);
		//if(Input.GetAxis("L_XAxis_4") > 0.5)
		//	Debug.Log("L_XAxis_4");
		player[3].transform.Translate(0,Input.GetAxis("L_YAxis_4") * Time.deltaTime * unitsPerSecond, 0);
		
		AudioSource.PlayClipAtPoint(heartBeat, Camera.main.transform.position);
		//if(Input.GetAxis("L_YAxis_4") > 0.5)
		//	Debug.Log("L_YAxis_4");
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
