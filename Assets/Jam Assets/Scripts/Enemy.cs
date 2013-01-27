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

	Vector3 desiredPos;
	Vector3 currAngle, desiredAngle;
	public Enemy()
	{
		gameObj = (GameObject)GameObject.Instantiate(cubePrefab);
		gameObj.name = "Enemy #" + ++enemyCount;
	}

	// Use this for initialization
	public void Initialize()
	{
		desiredPos = gameObj.transform.position;
		desiredAngle = currAngle = gameObj.transform.eulerAngles;
	}

	float marchTimeout = 1.0f;
	// Update is called once per frame
	public void Update()
	{
		marchTimeout -= Time.deltaTime;
		if (marchTimeout < 0)
		{
			marchTimeout += 1.0f;
			desiredPos += new Vector3(1.0f, 0.0f, 0.0f);
			desiredAngle += new Vector3(0.0f, -90.0f, 0.0f);
		}
		
		// ease-in
		// k is the fractional change in distance in one second, and dt is the number of seconds since the last frame.
		//float weight = 0.99f;
		float k = 0.999f;
		float dt = Time.deltaTime;
		//float weight = 0.9f;
		float weight = Mathf.Pow(1-k, dt);
		gameObj.transform.position = weight*gameObj.transform.position + (1-weight)*desiredPos;
		Vector3 prevAngle = currAngle;
		currAngle = weight*currAngle + (1-weight)*desiredAngle;
		//
		gameObj.transform.Rotate(Vector3.up * (currAngle.y - prevAngle.y), Space.World);

		//
		//gameObj.transform.position += Vector3.right * Time.deltaTime * 1.0f;
		//gameObj.transform.eulerAngles += new Vector3(0.0f, Time.deltaTime * -45, 0.0f);
	}
}
