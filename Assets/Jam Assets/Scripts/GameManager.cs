using UnityEngine;
using System.Collections;

public class GameManager : MonoSingleton<GameManager> {
	
	public float testValue = 1.0f;
	private MeshRenderer planeMeshRenderer;

	AudioClip heartBeat;
	
	Material[] matColor;
	
	private int numPlayers = 4;
	Player[] player;
	PressureField pressureField;
	
	// Use this for initialization
	void Start () {
		
		heartBeat = (AudioClip)Resources.Load ("GGJ13_Theme", typeof(AudioClip));
		audio.clip = heartBeat;
		audio.pitch = 1.5f;
		audio.Play();
		
		GameObject spherePrefab = (GameObject)Resources.Load("Sphere");
		//Component p = spherePrefab.GetComponentInChildren(typeof(Pulse));
		
		matColor = new Material[numPlayers];
		for(int i = 0; i < numPlayers; ++i){
			matColor[i] = (Material)Resources.Load ("mat" + i, typeof(Material));
		}
		
		matColor[0].color = Color.blue;
		matColor[1].color = Color.green;
		//p.renderer.material = matColor[1];
		matColor[2].color = Color.red;
		matColor[3].color = Color.yellow;
		//matColor = (Material)Resources.Load ("mat", typeof(Material));
		//matColor.color = Color.blue;
		player = new Player[numPlayers];
		for (int i = 0; i < numPlayers; ++i)
		{
			player[i] = new Player();
			player[i].gameObj = (GameObject)GameObject.Instantiate(spherePrefab);
			player[i].gameObj.renderer.material = matColor[i];
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
		mypulse.renderer.material.SetColor ("_PulseColor", Color.blue);
		
		mypulse = player[1].gameObj.GetComponentInChildren<Pulse>();
		mypulse.renderer.material.SetColor ("_PulseColor", Color.green);

		mypulse = player[2].gameObj.GetComponentInChildren<Pulse>();
		mypulse.renderer.material.SetColor ("_PulseColor", Color.red);
		
		mypulse = player[3].gameObj.GetComponentInChildren<Pulse>();
		mypulse.renderer.material.SetColor ("_PulseColor", Color.yellow);
		
		pressureField = new PressureField();
		pressureField.init();
		
		planeMeshRenderer = (MeshRenderer)((GameObject)GameObject.Instantiate(Resources.Load("waveMesh"))).renderer;
		planeMeshRenderer.GetComponent<MeshFilter>().mesh = Utility.CreateFullscreenPlane(100.0f);
		planeMeshRenderer.material.mainTexture = pressureField.texture;
	}

	float hackWaveTimer = 1.0f;
	// Update is called once per frame
	void Update ()
	{
		bool makewaves = false;
		hackWaveTimer -= Time.deltaTime;
		if (hackWaveTimer < 0.0f)
		{
			makewaves = true;
			hackWaveTimer += 0.5f;
		}

		for (int i = 0; i < numPlayers; ++i)
		{
			player[i].Update();
			if (makewaves)
			{
				Vector3 pos3d = Camera.main.WorldToScreenPoint(player[i].gameObj.transform.position);
				Vector2 pos2d = new Vector2(pos3d.x, pos3d.y);
				pressureField.SetPressure(pos2d, 1 << 16); //15);
			}
		}
		pressureField.Update();
	}
}
