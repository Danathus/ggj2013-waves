using UnityEngine;
using System.Collections;

public class TestScript : MonoBehaviour {
	
	private float unitsPerSecond = 10.0f;
	GameObject[] player;
	
	void Awake() {
		
	}

	// djmc -- make these public to expose to unity editor
	private Vector3 point = Vector3.up;
	private int numberOfPoints = 10;

	private Mesh mesh;
	private Vector3[] vertices;
	private int[] triangles;

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
