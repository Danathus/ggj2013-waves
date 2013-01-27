using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoSingleton<GameManager> {
	
	MeshRenderer planeMeshRenderer;
	Heart heart;

	AudioClip heartBeat;
	
	int numPlayers = 0;
	Player[] player;
	WaveField waveField;
	string formatXAxis;
	string formatYAxis;
	Dictionary<int, string> playerToColor;
	List<Color> l_pColor;
	
	List<Enemy> enemies;
	Vector3 cameraStartPosition;
	float shakeMagnitude = 0.0f;
	
	float restartTimeout;
	public float gameTime;
	
	// Use this for initialization ---------------------------------------------
	void Start()
	{
		gameTime = 0.0f;
		restartTimeout = 5.0f;
		StartAudio();		
				
		playerToColor = new Dictionary<int, string>();
		
		waveField = new WaveField();
		waveField.init();
		
		StartPlayers();
		ConfigurePlayerColors();
		
		planeMeshRenderer = (MeshRenderer)((GameObject)GameObject.Instantiate(Resources.Load("waveMesh"))).renderer;
		planeMeshRenderer.GetComponent<MeshFilter>().mesh = Utility.CreateFullscreenPlane(100.0f, 1.0f, 1.0f);
		planeMeshRenderer.material.mainTexture = waveField.texture;
		
		enemies = new List<Enemy>();
		
		heart = GameObject.FindGameObjectWithTag("HeartTag").GetComponent<Heart>();
		heart.waveField = waveField;
		cameraStartPosition = Camera.main.transform.position;
		
		EventMessenger.Instance.AddListener<EventTriggerEnter>(OnEventTriggerEnter);
	}
	
	// -------------------------------------------------------------------------
	void StartAudio() {
		
		heartBeat = (AudioClip)Resources.Load ("GGJ13_Theme", typeof(AudioClip));
		audio.clip = heartBeat;
		audio.pitch = 1.5f;
		audio.ignoreListenerVolume = true;
		audio.minDistance = 10000.0f;
		audio.Play();
	}
	
	void UpdateAudio()
	{
		if (heart.dead)
		{
			audio.Stop();
			return;
		}
		audio.volume = Mathf.Clamp((float)enemies.Count / 5, 0.5f, 1.0f);
		audio.pitch = Mathf.Clamp((float)enemies.Count/5 + 1.0f, 1.0f, 2.5f);
		//Debug.Log (audio.volume);
	}
	
	// -------------------------------------------------------------------------
	void StartPlayers()
	{
		l_pColor = new List<Color>();
		if(PlayerPrefs.GetString("Player0") == "Blue"){
			playerToColor[numPlayers] = "Blue";
			++numPlayers;
		}
		if(PlayerPrefs.GetString("Player1") == "Green"){
			playerToColor[numPlayers] = "Green";
			++numPlayers;
		}
		if(PlayerPrefs.GetString("Player2") == "Red"){
			playerToColor[numPlayers] = "Red";
			++numPlayers;
		}
		if(PlayerPrefs.GetString("Player3") == "Yellow"){
			playerToColor[numPlayers] = "Yellow";
			++numPlayers;
		}
		player = new Player[numPlayers];
		for (int id = 0; id < numPlayers; ++id)
		{
			player[id] = new Player(id, Player.colorCodeFromName(playerToColor[id]));
			l_pColor.Add (Player.toColor (player[id].playerColor));
			player[id].gameObj.transform.position += Vector3.right * id;
			player[id].waveField = waveField;
		}
		
		for(int i = 0; i < numPlayers; ++i){
			for(int j = i + 1; j < numPlayers; ++j){
				if(i == j)
					continue;
				float red = Mathf.Clamp (Player.toColor(player[i].playerColor).r + Player.toColor(player[j].playerColor).r, 0f, 1f);
				float green = Mathf.Clamp (Player.toColor(player[i].playerColor).g + Player.toColor(player[j].playerColor).g, 0f, 1f);
				float blue = Mathf.Clamp (Player.toColor(player[i].playerColor).b + Player.toColor(player[j].playerColor).b, 0f, 1f);
				float alpha = Mathf.Clamp (Player.toColor (player[i].playerColor).a + Player.toColor(player[j].playerColor).a, 0f,1f);
				Color nColor = new Color(red, green, blue, alpha);
				l_pColor.Add (nColor);
				for(int k = j + 1; k < numPlayers; ++k){
					if(i == k && j == k)
						continue;
					red = Mathf.Clamp (Player.toColor(player[i].playerColor).r + Player.toColor(player[j].playerColor).r + Player.toColor(player[k].playerColor).r, 0f, 1f);
					green = Mathf.Clamp (Player.toColor(player[i].playerColor).g + Player.toColor(player[j].playerColor).g + Player.toColor(player[k].playerColor).g, 0f, 1f);
					blue = Mathf.Clamp (Player.toColor(player[i].playerColor).b + Player.toColor(player[j].playerColor).b + Player.toColor(player[k].playerColor).b, 0f, 1f);
					alpha = Mathf.Clamp (Player.toColor (player[i].playerColor).a + Player.toColor(player[j].playerColor).a + Player.toColor(player[k].playerColor).a, 0f,1f);
					nColor = new Color(red, green, blue, alpha);
					l_pColor.Add (nColor);
				}
			}
		}
		
		PositionPlayersAroundHeart();
		
		for(int i = 0; i < numPlayers; ++i){
			if(i == 0){
				formatXAxis = "L_XAxis_"+(i+1);
				formatXAxis = formatXAxis.Replace("0","");
				formatYAxis = "L_YAxis_"+(i+1);
				formatYAxis = formatYAxis.Replace("0","");
				player[i].Initialize(KeyCode.UpArrow, KeyCode.LeftArrow, KeyCode.DownArrow, KeyCode.RightArrow, formatXAxis, formatYAxis, "A_1");
			}
			
			if(i == 1){
				formatXAxis = "L_XAxis_"+(i+1);
				formatXAxis = formatXAxis.Replace("0","");
				formatYAxis = "L_YAxis_"+(i+1);
				formatYAxis = formatYAxis.Replace("0","");
				player[i].Initialize(KeyCode.W, KeyCode.A, KeyCode.S, KeyCode.D, formatXAxis, formatYAxis, "A_2");
			}
				
			if(i == 2){
				formatXAxis = "L_XAxis_"+(i+1);
				formatXAxis = formatXAxis.Replace("0","");
				formatYAxis = "L_YAxis_"+(i+1);
				formatYAxis = formatYAxis.Replace("0","");
				player[i].Initialize(KeyCode.T, KeyCode.F, KeyCode.G, KeyCode.H, formatXAxis, formatYAxis, "A_3");
			}
				
			if(i == 3){
				formatXAxis = "L_XAxis_"+(i+1);
				formatXAxis = formatXAxis.Replace("0","");
				formatYAxis = "L_YAxis_"+(i+1);
				formatYAxis = formatYAxis.Replace("0","");
				player[i].Initialize(KeyCode.I, KeyCode.J, KeyCode.K, KeyCode.L, formatXAxis, formatYAxis, "A_4");
			}
		}
	}
	
	// -------------------------------------------------------------------------
	void ConfigurePlayerColors() {
		// procedurally alter a pulse
		Pulse mypulse;
		for(int i = 0; i < numPlayers; ++i){
			mypulse = player[i].gameObj.GetComponentInChildren<Pulse>();
			mypulse.renderer.material.SetColor ("_PulseColor", Player.toColor(player[i].playerColor));
			/*
			if(PlayerPrefs.GetString("Player" + i) == "Blue"){
				mypulse = player[i].gameObj.GetComponentInChildren<Pulse>();
				mypulse.renderer.material.SetColor ("_PulseColor", Color.blue);
			}
		
			if(PlayerPrefs.GetString("Player" + i) == "Green"){
				mypulse = player[i].gameObj.GetComponentInChildren<Pulse>();
				mypulse.renderer.material.SetColor ("_PulseColor", Color.green);
			}
			
			if(PlayerPrefs.GetString("Player" + i) == "Red"){
				mypulse = player[i].gameObj.GetComponentInChildren<Pulse>();
				mypulse.renderer.material.SetColor ("_PulseColor", Color.red);
			}
			
			if(PlayerPrefs.GetString("Player" + i) == "Yellow"){
				mypulse = player[i].gameObj.GetComponentInChildren<Pulse>();
				mypulse.renderer.material.SetColor ("_PulseColor", Color.yellow);
			}
			//*/
		}
	}

	float spawnNextEnemyTimeout = 3.0f;
	float spawnNextEnemyTimeoutChange = -0.4f;
	float enemySpawnTimer = 0.0f;
	
	// ----- TEMP
	//
	
	// -------------------------------------------------------------------------
	void UpdateCameraShake()
	{
		float camShakeRandomAngle = Random.Range(0.0f, 360.0f);
		Camera.main.transform.position = cameraStartPosition + shakeMagnitude *
			new Vector3(Mathf.Cos(camShakeRandomAngle), Mathf.Sin(camShakeRandomAngle), 0.0f);
		//
		float k = 0.999f;
		float dt = Time.deltaTime;
		//float weight = 0.9f;
		float weight = Mathf.Pow(1-k, dt);
		shakeMagnitude = weight*shakeMagnitude + (1-weight)*0.0f;
	}
	
	// Update is called once per frame -----------------------------------------
	void Update ()
	{
		if (!heart.dead)
		{
			gameTime += Time.deltaTime;
		}
		for (int i = 0; i < numPlayers; ++i)
		{
			if (!heart.dead)
			{
				player[i].Update();
			}
			// don't let players leave the space
			Vector3 screenPos3d = Camera.main.WorldToScreenPoint(player[i].gameObj.transform.position);
			screenPos3d.x = Mathf.Clamp(screenPos3d.x, 10.0f, Camera.main.pixelWidth-10.0f);
			screenPos3d.y = Mathf.Clamp(screenPos3d.y, 10.0f, Camera.main.pixelHeight-10.0f);
			player[i].gameObj.transform.position = Camera.main.ScreenToWorldPoint(screenPos3d);

			// don't let players overlap the heart
			if ((player[i].gameObj.transform.position - heart.transform.position).magnitude < 1.5f)
			{
				player[i].gameObj.transform.position =
					(player[i].gameObj.transform.position - heart.transform.position).normalized * 1.5f;
			}
			// don't let players overlap each other
			for (int j = 0; j < numPlayers; ++j)
			{
				if (i == j) continue;
				Vector3 displacement = player[i].gameObj.transform.position - player[j].gameObj.transform.position;
				if (displacement.magnitude < 1.0f)
				{
					player[i].gameObj.transform.position += displacement * Time.deltaTime * 10;
					player[j].gameObj.transform.position -= displacement * Time.deltaTime * 10;
				}
			}
		}
		
		foreach (Enemy enemy in enemies)
		{
			if (!heart.dead)
			{
				enemy.Update();
			}

			// if we get close to the heart...
			if (!heart.dead && !enemy.dead)
			{
				if ((enemy.gameObj.transform.position - heart.transform.position).sqrMagnitude < 2.0f)
				{
					// shake the camera
					shakeMagnitude = 1.0f;
					// hurt the heart
					heart.GetHurt();
					// kill the enemy
					StartCoroutine(KillEnemy(enemy, true));
				}
			}

			// if we are on a wave that's particularly strong, die
			if (!enemy.dead)
			{
				Vector3 pos3d = Camera.main.WorldToScreenPoint(enemy.gameObj.transform.position);
				Vector2 pos2d = new Vector2(pos3d.x, pos3d.y);
				
				if (WaveField.WavePixelAmplitude(enemy.gameObj.renderer.material.color, waveField.GetPressure(pos2d)) > Enemy.tooMuchPressure)
				{
					StartCoroutine(KillEnemy(enemy));		
				}
			}
		}
		
		waveField.Update();
		UpdateAudio();

		// update camera shake
		UpdateCameraShake();

		UpdateEnemySpawner();
		
		//Debug.Log(player[0].pulseStrength + ", " + player[1].pulseStrength);

		// hack controls to speed up time
		Time.timeScale = 1.0f;
		if (Input.GetKey(KeyCode.LeftShift))
		{
			Time.timeScale = 10.0f;
		}
		
		// restart management
		if (heart.dead)
		{
			restartTimeout -= Time.deltaTime;
			if (restartTimeout < 0.0f)
			{
				Application.LoadLevel("MainMenuScene");
			}
		}
	}
	
	// -------------------------------------------------------------------------
	void UpdateEnemySpawner()
	{
		if (heart.dead) { return; }
		// update enemy spawner
		enemySpawnTimer -= Time.deltaTime;
		if (enemySpawnTimer < 0)
		{
			enemySpawnTimer = spawnNextEnemyTimeout;
			spawnNextEnemyTimeout += spawnNextEnemyTimeoutChange;
			if (spawnNextEnemyTimeout < 0.5f) spawnNextEnemyTimeoutChange = Mathf.Abs(spawnNextEnemyTimeoutChange);
			if (spawnNextEnemyTimeout > 3.0f) spawnNextEnemyTimeoutChange = -Mathf.Abs(spawnNextEnemyTimeoutChange);
			
			int r_Color = Random.Range (0, l_pColor.Count);
			Enemy enemy = new Enemy(l_pColor[r_Color]);
			enemy.waveField = waveField;

			// position the enemy
			Vector3 screenPos = new Vector3(Screen.width, Screen.height/2, Mathf.Abs(Camera.main.transform.position.z - heart.transform.position.z));
			Vector3 worldPos = Camera.main.ScreenToWorldPoint(screenPos);
			float screenRadius = worldPos.x;
			float randomAngleDeg = Random.Range(0, 360.0f);
			float randomAngleRad = randomAngleDeg * Mathf.Deg2Rad;
			Vector3 pos = (new Vector3(Mathf.Cos(randomAngleRad), Mathf.Sin(randomAngleRad), 0)) * screenRadius;
			//Debug.Log("screenPos " + screenPos + "; worldPos " + worldPos);
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
	IEnumerator KillEnemy(Enemy enemy, bool killImmediately = false) {
		enemy.dead = true;
					
		yield return null;	
		
		Transform enemyTransform = enemy.gameObj.transform;
		Transform heartTransform = heart.transform;
		
		enemies.Remove(enemy);

		if (!killImmediately)
		{
			Vector3 repelDirection = enemyTransform.position - heartTransform.position;
			float distanceSquared = repelDirection.sqrMagnitude;
			repelDirection.Normalize();
			
			while (distanceSquared < 1000.0f) {
				
				enemyTransform.position += repelDirection * Time.deltaTime * 10.0f;
				enemyTransform.Rotate(-Vector3.left * Time.realtimeSinceStartup * 0.5f, Space.Self);
				
				distanceSquared = (enemyTransform.position - heartTransform.position).sqrMagnitude;
				yield return null;
			}
		}
		
		
		Destroy(enemy.gameObj);		
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
	
	// -------------------------------------------------------------------------
	void OnEventTriggerEnter(EventTriggerEnter triggerEvent) {
	
		
	}
	
}
