using UnityEngine;
using System.Collections;

public class TestScript : MonoBehaviour {
	
	//private float unitsPerSecond = 10.0f;
	
	PressureField pressureField;
	
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

		pressureField = new PressureField();
		pressureField.init();
	}
	
	// Update is called once per frame
	void Update () {
	}
}
