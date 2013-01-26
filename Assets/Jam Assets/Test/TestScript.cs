using UnityEngine;
using System.Collections;

public class WaveField // there can be only one!
{
	// constant
	public static int kNumBuffers = 2;
	public static int kPointFieldWidth  = 10; // width of (square) field in points
	public static int kPointFieldHeight = 10;
	public static int kBufferSize = kPointFieldWidth*kPointFieldHeight;
	public class WavePoint
	{
		Vector3 direction;
		public WavePoint()
		{
			direction = Vector3.zero;
		}
	}

	// these update regularly
	WavePoint[] vectorField; // double buffered; 2 * height * width
	int currBufferIdx;
	public int nextBufferIdx
	{
		get { return (currBufferIdx+1)%kNumBuffers; }
	}
	
	public WaveField()
	{
		vectorField = new WavePoint[kNumBuffers*kBufferSize];
		for (int bufferIdx = 0; bufferIdx < kNumBuffers; ++bufferIdx)
		{
			for (int y = 0; y < kPointFieldHeight; ++y)
			{
				for (int x = 0; x < kPointFieldWidth; ++x)
				{
					vectorField[bufferIdx*kBufferSize+y*kPointFieldWidth+x] = new WavePoint();
				}
			}
		}
		currBufferIdx = 0;
	}

	WavePoint GetPoint(int bufferIdx, int x, int y)
	{
		return vectorField[bufferIdx*kBufferSize + y*kPointFieldWidth + x];
	}

	void Update()
	{
		// propagate the wave fields
		for (int y = 0; y < kPointFieldHeight; ++y)
		{
			for (int x = 0; x < kPointFieldWidth; ++x)
			{
				WavePoint curr = GetPoint(currBufferIdx, x, y);
			}
		}
	}
}

public class TestScript : MonoBehaviour {
	
	private float unitsPerSecond = 10.0f;
	private int numPlayers = 4;
	Player[] player;
	
	void Awake() {
		
	}

	/*
	// djmc -- make these public to expose to unity editor
	private Vector3 point = Vector3.up;
	private int numberOfPoints = 10;

	private Mesh mesh;
	private Vector3[] vertices;
	private int[] triangles;
	//*/

	// Use this for initialization
	void Start () {
		Camera.main.transform.position += Vector3.back * 10;

		GameObject spherePrefab = (GameObject)Resources.Load("Sphere");

		//player = new SpherePlayer[numPlayers];
		player = new Player[numPlayers];
		//player = new GameObject[numPlayers];
		for (int i = 0; i < numPlayers; ++i)
		{
			//player[i] = new SpherePlayer();
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

		/*
		// create a star mesh
		mesh = new Mesh();
		mesh.name = "Star Mesh";
		vertices = new Vector3[numberOfPoints + 1];
		triangles = new int[numberOfPoints * 3];
		float angle = -360f / numberOfPoints;
		for(int v = 1, t = 1; v < vertices.Length; v++, t += 3){
			vertices[v] = Quaternion.Euler(0f, 0f, angle * (v - 1)) * point;
			triangles[t] = v;
			triangles[t + 1] = v + 1;
		}
		triangles[triangles.Length - 1] = 1;

		mesh.vertices = vertices;
		mesh.triangles = triangles;
		
		GameObject go = new GameObject(); //GameObject.Instantiate();
		MeshFilter mf = (MeshFilter)go.AddComponent(typeof(MeshFilter));
		mf.mesh = mesh;
		GameObject.Instantiate(go);
		go.AddComponent(typeof(MeshRenderer));
		go.name = "Wave";
		//*/
	}
	
	// Update is called once per frame
	void Update () {
		for (int i = 0; i < numPlayers; ++i)
		{
			player[i].Update();
		}
	}
}
