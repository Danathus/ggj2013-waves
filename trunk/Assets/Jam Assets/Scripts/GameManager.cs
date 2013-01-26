using UnityEngine;
using System.Collections;

public class GameManager : MonoSingleton<GameManager> {
	
	public float testValue = 1.0f;

	AudioClip heartBeat;

	private int numPlayers = 4;
	Player[] player;
	
	// Use this for initialization
	void Start () {
		
		heartBeat = (AudioClip)Resources.Load ("GGJ13_Theme", typeof(AudioClip));
		audio.clip = heartBeat;
		audio.pitch = 1.5f;
		audio.Play();
		
		Camera.main.transform.position += Vector3.back * 10;
		GameObject spherePrefab = (GameObject)Resources.Load("Sphere");
		player = new Player[numPlayers];
		for (int i = 0; i < numPlayers; ++i)
		{
			player[i] = new Player();
			player[i].gameObj = (GameObject)GameObject.Instantiate(spherePrefab);
			player[i].gameObj.transform.position += Vector3.right * i;
		}
		player[0].Initialize(KeyCode.UpArrow, KeyCode.LeftArrow, KeyCode.DownArrow, KeyCode.RightArrow, "L_XAxis_1", "L_YAxis_1");
		player[1].Initialize(KeyCode.W, KeyCode.A, KeyCode.S, KeyCode.D, "L_XAxis_2", "L_YAxis_2");
		player[2].Initialize(KeyCode.T, KeyCode.F, KeyCode.G, KeyCode.H, "L_XAxis_3", "L_YAxis_3");
		player[3].Initialize(KeyCode.I, KeyCode.J, KeyCode.K, KeyCode.L, "L_XAxis_4", "L_YAxis_4");
		
		// procedurally alter a pulse
		Pulse mypulse = player[0].gameObj.GetComponentInChildren<Pulse>();
		mypulse.beatsPerSecond = 2;
		mypulse.amplitude = 2;
	}
	
	// Update is called once per frame
	void Update () {
		for (int i = 0; i < numPlayers; ++i)
		{
			player[i].Update();
		}
	}
}
