using UnityEngine;
using System.Collections;

public class Enemy
{
	static GameObject cubePrefab;
	public GameObject gameObj;
	public WaveField waveField;
	static int enemyCount = 0;

	static Enemy()
	{
		// todo: make this a cube
		cubePrefab = (GameObject)Resources.Load("Cube");
	}
	
	public Enemy()
	{
		gameObj = (GameObject)GameObject.Instantiate(cubePrefab);
		gameObj.name = "Enemy #" + ++enemyCount;
	}

	// Use this for initialization
	public void Initialize()
	{
	}
	
	// Update is called once per frame
	public void Update()
	{
		gameObj.transform.position += Vector3.right * Time.deltaTime * 1.0f;
	}
}
