using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoSingleton<GameManager> {
	
	MeshRenderer planeMeshRenderer;
	Heart heart;

	AudioClip heartBeat;
	
	int numPlayers = 4;
	Player[] player;
	WaveField waveField;
	
	List<Enemy> enemies;
	
	// Use this for initialization ---------------------------------------------
	void Start()
	{		
		StartAudio();		
				
		waveField = new WaveField();
		waveField.init();
		
		StartPlayers();
		ConfigurePlayerColors();
		
		planeMeshRenderer = (MeshRenderer)((GameObject)GameObject.Instantiate(Resources.Load("waveMesh"))).renderer;
		planeMeshRenderer.GetComponent<MeshFilter>().mesh = Utility.CreateFullscreenPlane(100.0f);
		planeMeshRenderer.material.mainTexture = waveField.texture;
		
		enemies = new List<Enemy>();
		
		heart = GameObject.FindGameObjectWithTag("HeartTag").GetComponent<Heart>();
	}
	
	// -------------------------------------------------------------------------
	void StartAudio() {
		
		heartBeat = (AudioClip)Resources.Load ("GGJ13_Theme", typeof(AudioClip));
		audio.clip = heartBeat;
		audio.pitch = 1.5f;
		audio.Play();
	}
	
	// -------------------------------------------------------------------------
	void StartPlayers() {
		
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
	}
	
	// -------------------------------------------------------------------------
	void ConfigurePlayerColors() {
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
	}

	float spawnNextEnemyTimeout = 3.0f;
	float spawnNextEnemyTimeoutDecrease = 0.2f;
	float enemySpawnTimer = 0.0f;
	
	// ----- TEMP
	//
	
	// Update is called once per frame -----------------------------------------
	void Update ()
	{
		for (int i = 0; i < numPlayers; ++i)
		{
			player[i].Update();
			// todo: don't let players leave the space
			// todo: don't let players overlap the heart
			if ((player[i].gameObj.transform.position - heart.transform.position).magnitude < 1.5f)
			{
				player[i].gameObj.transform.position =
					(player[i].gameObj.transform.position - heart.transform.position).normalized * 1.5f;
			}
			// todo: don't let players overlap each other
		}
		List<Enemy> killthese = new List<Enemy>();
		foreach (Enemy enemy in enemies)
		{
			enemy.Update();

			// if we get close to the heart...
			if ((enemy.gameObj.transform.position - heart.transform.position).sqrMagnitude < 1.0f)
			{
				// for now, just remove the enemy
				killthese.Add(enemy);
			}

			// if we are on a wave that's particularly strong, die
			Vector3 pos3d = Camera.main.WorldToScreenPoint(enemy.gameObj.transform.position);
			Vector2 pos2d = new Vector2(pos3d.x, pos3d.y);
			int tooMuchPressure = 1 << 11; // 13 is too much
			if (waveField.GetPressure(pos2d) > tooMuchPressure)
			{
				killthese.Add(enemy);
			}
		}
		foreach (Enemy killme in killthese)
		{
			enemies.Remove(killme);
			GameObject.Destroy(killme.gameObj);
		}
		waveField.Update();

		// update enemy spawner
		enemySpawnTimer -= Time.deltaTime;
		if (enemySpawnTimer < 0)
		{
			enemySpawnTimer = spawnNextEnemyTimeout;
			spawnNextEnemyTimeout -= spawnNextEnemyTimeoutDecrease;
			if (spawnNextEnemyTimeout < 0.1f) spawnNextEnemyTimeout = 0.1f;

			Enemy enemy = new Enemy();
			enemy.waveField = waveField;

			// position the enemy
			Vector3 screenPos = new Vector3(Screen.width, Screen.height/2, Mathf.Abs(Camera.main.transform.position.z - heart.transform.position.z));
			Vector3 worldPos = Camera.main.ScreenToWorldPoint(screenPos);
			float screenRadius = worldPos.x;
			float randomAngleDeg = Random.Range(0, 360.0f);
			float randomAngleRad = randomAngleDeg * Mathf.Deg2Rad;
			Vector3 pos = (new Vector3(Mathf.Cos(randomAngleRad), Mathf.Sin(randomAngleRad), 0)) * screenRadius;
			Debug.Log("screenPos " + screenPos + "; worldPos " + worldPos);
			//Vector3 pos = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width/2, Screen.height/2, 0));
			//Vector3 pos = new Vector3(-3.0f, -3.0f, 0.0f); //Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 0));
			//pos = new Vector3(pos.x, pos.y, 0);
			enemy.gameObj.transform.position = pos;

			// orient the enemy
			//   first orient their local coordinate system so they can march correctly
			enemy.gameObj.transform.Rotate(Vector3.up * 90, Space.World);
			enemy.gameObj.transform.Rotate(Vector3.right * 90, Space.World);
			// then give them an appropriate direction
			//enemy.gameObj.transform.Rotate(Vector3.back * Random.Range(0, 360.0f), Space.World);
			enemy.gameObj.transform.Rotate(Vector3.back * (-randomAngleDeg+180), Space.World);
			//enemy.gameObj.transform.Rotate(Vector3.back * Random.Range(0, 360.0f), Space.World);
			//enemy.gameObj.transform.Rotate(Vector3.up * Random.Range(0, 360.0f), Space.World); // left isn't correct
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
