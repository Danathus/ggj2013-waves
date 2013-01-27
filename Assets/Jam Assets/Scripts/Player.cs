using UnityEngine;
using System.Collections;

public class Player
{
	static GameObject spherePrefab;// = (GameObject)Resources.Load("Sphere");

	public int id;
	public GameObject gameObj;
	public KeyCode up, down, left, right;
	public string leftMoveAxis, rightMoveAxis;
	public float unitsPerSecond = 10.0f;
	public WaveField waveField;
	float heartbeatTimer = 0.0f;

	static Player()
	{
		spherePrefab = (GameObject)Resources.Load("Sphere");
	}

	public Player(int id)
	{
		this.id = id;
		gameObj = (GameObject)GameObject.Instantiate(spherePrefab);
		gameObj.name = "Player #" + id;

		gameObj.renderer.material = (Material)Resources.Load("mat" + id, typeof(Material));
		switch (id)
		{
		case 0:
			gameObj.renderer.material.color = Color.blue;
			break;
		case 1:
			gameObj.renderer.material.color = Color.green;
			break;
		case 2:
			gameObj.renderer.material.color = Color.red;
			break;
		case 3:
			gameObj.renderer.material.color = Color.yellow;
			break;
		}
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
		
		// normalize player direction
		if (direction.sqrMagnitude > 1)
		{
			direction.Normalize();
		}
		
		// move player by direction
		gameObj.transform.position += direction * Time.deltaTime * unitsPerSecond;

		// update heartbeat
		heartbeatTimer -= Time.deltaTime;
		if (heartbeatTimer < 0.0f)
		{
			Vector3 pos3d = Camera.main.WorldToScreenPoint(gameObj.transform.position);
			Vector2 pos2d = new Vector2(pos3d.x, pos3d.y);
			waveField.SetPressure(pos2d, 1 << 16); //15);
			heartbeatTimer += 0.5f;
		}
	}
}
