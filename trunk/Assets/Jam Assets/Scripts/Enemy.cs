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
	float currAngle, desiredAngle;
	Vector3 forward, right;
	public Enemy()
	{
		gameObj = (GameObject)GameObject.Instantiate(cubePrefab);
		gameObj.name = "Enemy #" + ++enemyCount;
	}

	// Use this for initialization
	public void Initialize()
	{
		desiredPos = gameObj.transform.position;
		desiredAngle = currAngle = 0.0f;

		// figure out which way is forward
		forward = gameObj.transform.forward;
		right = gameObj.transform.right;
	}

	float marchTimeout = 1.0f;
	// Update is called once per frame
	public void Update()
	{
		marchTimeout -= Time.deltaTime;
		if (marchTimeout < 0)
		{
			marchTimeout += 1.0f;
			desiredPos += forward;
			desiredAngle += 90.0f;
		}
		
		// ease-in
		// k is the fractional change in distance in one second, and dt is the number of seconds since the last frame.
		//float weight = 0.99f;
		float k = 0.999f;
		float dt = Time.deltaTime;
		//float weight = 0.9f;
		float weight = Mathf.Pow(1-k, dt);
		gameObj.transform.position = weight*gameObj.transform.position + (1-weight)*desiredPos;
		float prevAngle = currAngle;
		currAngle = weight*currAngle + (1-weight)*desiredAngle;
		//
		gameObj.transform.Rotate(Vector3.left * (currAngle - prevAngle),Space.Self);
		//gameObj.transform.Rotate(Vector3.up * (currAngle - prevAngle),Space.Self);

		//
		//gameObj.transform.position += Vector3.right * Time.deltaTime * 1.0f;
		//gameObj.transform.eulerAngles += new Vector3(0.0f, Time.deltaTime * -45, 0.0f);
	}
}
