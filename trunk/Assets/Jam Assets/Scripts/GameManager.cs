using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoSingleton<GameManager> {
	
	public float testValue = 1.0f;
	private MeshRenderer planeMeshRenderer;

	AudioClip heartBeat;
	
	private int numPlayers = 4;
	Player[] player;
	WaveField waveField;
	
	List<Enemy> enemies;
	
	// Use this for initialization ---------------------------------------------
	void Start()
	{
		heartBeat = (AudioClip)Resources.Load ("GGJ13_Theme", typeof(AudioClip));
		audio.clip = heartBeat;
		audio.pitch = 1.5f;
		audio.Play();
				
		waveField = new WaveField();
		waveField.init();

		player = new Player[numPlayers];
		for (int id = 0; id < numPlayers; ++id)
		{
			player[id] = new Player(id);
			player[id].gameObj.transform.position += Vector3.right * id;
			player[id].waveField = waveField;
		}
		
		PositionPlayersAroundHeart();
		
		player[0].Initialize(KeyCode.UpArrow, KeyCode.LeftArrow, KeyCode.DownArrow, KeyCode.RightArrow, "L_XAxis_1", "L_YAxis_1");
		player[1].Initialize(KeyCode.W, KeyCode.A, KeyCode.S, KeyCode.D, "L_XAxis_2", "L_YAxis_2");
		player[2].Initialize(KeyCode.T, KeyCode.F, KeyCode.G, KeyCode.H, "L_XAxis_3", "L_YAxis_3");
		player[3].Initialize(KeyCode.I, KeyCode.J, KeyCode.K, KeyCode.L, "L_XAxis_4", "L_YAxis_4");
		
		// procedurally alter a pulse
		Pulse mypulse = player[0].gameObj.GetComponentInChildren<Pulse>();
		//mypulse.beatsPerSecond = 2;
		//mypulse.amplitude = 2;
		mypulse.renderer.material.SetColor ("_PulseColor", Color.blue);
		
		mypulse = player[1].gameObj.GetComponentInChildren<Pulse>();
		mypulse.renderer.material.SetColor ("_PulseColor", Color.green);

		mypulse = player[2].gameObj.GetComponentInChildren<Pulse>();
		mypulse.renderer.material.SetColor ("_PulseColor", Color.red);
		
		mypulse = player[3].gameObj.GetComponentInChildren<Pulse>();
		mypulse.renderer.material.SetColor ("_PulseColor", Color.yellow);
		
		planeMeshRenderer = (MeshRenderer)((GameObject)GameObject.Instantiate(Resources.Load("waveMesh"))).renderer;
		planeMeshRenderer.GetComponent<MeshFilter>().mesh = Utility.CreateFullscreenPlane(100.0f);
		planeMeshRenderer.material.mainTexture = waveField.texture;
		
		enemies = new List<Enemy>();
	}

	float enemySpawnTimer = 5.0f;
	
	// ----- TEMP
	//
	
	// Update is called once per frame -----------------------------------------
	void Update ()
	{
		for (int i = 0; i < numPlayers; ++i)
		{
			player[i].Update();
		}
		foreach (Enemy enemy in enemies)
		{
			enemy.Update();
		}
		waveField.Update();

		// update enemy spawner
		enemySpawnTimer -= Time.deltaTime;
		if (enemySpawnTimer < 0)
		{
			enemySpawnTimer = 5.0f;

			Enemy enemy = new Enemy();
			//Vector3 pos = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width/2, Screen.height/2, 0));
			Vector3 pos = new Vector3(-3.0f, -3.0f, 0.0f); //Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 0));
			//pos = new Vector3(pos.x, pos.y, 0);
			enemy.gameObj.transform.position = pos;
			enemy.waveField = waveField;
			enemy.Initialize();

			enemies.Add(enemy);
		}
	}
	
	// -------------------------------------------------------------------------
	void PositionPlayersAroundHeart() {
		
		GameObject heart = GameObject.FindGameObjectWithTag("HeartTag");
		Vector3 heartPosition = heart.transform.position;
		float heartRadius = (heart.collider as SphereCollider).radius;
		
		Vector3 heartOffset = Vector3.up * heartRadius * 2.0f;
		heartOffset.x *= heart.transform.localScale.x;
		heartOffset.y *= heart.transform.localScale.y;
		
		foreach (Player p in player) {
			
			p.gameObj.transform.position = heartPosition + heartOffset;
			
			// Rotate the offset for the next player
			heartOffset = Quaternion.Euler(0.0f, 0.0f, 90.0f) * heartOffset;			
		}
	}
}
