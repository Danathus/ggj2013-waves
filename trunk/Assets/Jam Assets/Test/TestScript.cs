using UnityEngine;
using System.Collections;

public class SpherePlayer
{
	public GameObject gameObj;
	public KeyCode up, down, left, right;
	public string leftMoveAxis, rightMoveAxis;
	public float unitsPerSecond = 10.0f;

	public SpherePlayer()
	{
	}

	public void Initialize(KeyCode up, KeyCode left, KeyCode down, KeyCode right, string leftMoveAxis, string rightMoveAxis)
	{
		this.up = up;
		this.left = left;
		this.down = down;
		this.right = right;
		this.leftMoveAxis = leftMoveAxis;
		this.rightMoveAxis = rightMoveAxis;
	}

	public void Update()
	{
		Vector3 direction = Vector3.zero;
		if (Input.GetKey(right)) {
			direction += Vector3.right;
		}
		if (Input.GetKey(left)) {
			direction -= Vector3.right;
		}
		if (Input.GetKey(up)) {
			direction += Vector3.up;
		}
		if (Input.GetKey(down)) {
			direction -= Vector3.up;
		}
		
		//
		direction += new Vector3(Input.GetAxis(leftMoveAxis),Input.GetAxis(rightMoveAxis),0);
		
		gameObj.transform.position += direction * Time.deltaTime * unitsPerSecond;
	}
}

public class TestScript : MonoBehaviour {
	
	private float unitsPerSecond = 10.0f;
	private int numPlayers = 4;
	//GameObject[] player;
	SpherePlayer[] player;
	
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

		player = new SpherePlayer[numPlayers];
		//player = new GameObject[numPlayers];
		for (int i = 0; i < numPlayers; ++i)
		{
			player[i] = new SpherePlayer();
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
	
	void ControlPlayer(GameObject obj, KeyCode up, KeyCode down, KeyCode left, KeyCode right)
	{
		Vector3 direction = Vector3.zero;
		if (Input.GetKey(right)) {
			direction += Vector3.right;
		}
		if (Input.GetKey(left)) {
			direction -= Vector3.right;
		}
		if (Input.GetKey(up)) {
			direction += Vector3.up;
		}
		if (Input.GetKey(down)) {
			direction -= Vector3.up;
		}
		obj.transform.position += direction * Time.deltaTime * unitsPerSecond;
	}
	
	// Update is called once per frame
	void Update () {
		for (int i = 0; i < numPlayers; ++i)
		{
			player[i].Update();
		}
		
		/*
		ControlPlayer(player[0].gameObj, KeyCode.UpArrow, KeyCode.DownArrow, KeyCode.LeftArrow, KeyCode.RightArrow);
		ControlPlayer(player[1].gameObj, KeyCode.W, KeyCode.S, KeyCode.A, KeyCode.D);
		ControlPlayer(player[2].gameObj, KeyCode.T, KeyCode.G, KeyCode.F, KeyCode.H);
		ControlPlayer(player[3].gameObj, KeyCode.I, KeyCode.K, KeyCode.J, KeyCode.L);
		//*/
	}
}
