using UnityEngine;
using System.Collections;

public class Player
{
	public enum ColorCode{
		RED = 0,
		BLUE = 1,
		GREEN = 2,
		YELLOW = 3,
		WTF
	};
	
	static GameObject spherePrefab;// = (GameObject)Resources.Load("Sphere");

	public int id;
	public GameObject gameObj;
	public KeyCode up, down, left, right;
	public string leftMoveAxis, rightMoveAxis;
	public float unitsPerSecond = 10.0f;
	public WaveField waveField;
	float heartbeatTimer = 0.0f;
	public ColorCode playerColor;
	//Color     playerColor;

	static Player()
	{
		spherePrefab = (GameObject)Resources.Load("Sphere");
	}

	public Player(int id, ColorCode colorCode)
	{
		this.id = id;
		playerColor = colorCode;
		gameObj = (GameObject)GameObject.Instantiate(spherePrefab);
		gameObj.name = "Player #" + id;

		//gameObj.renderer.material = (Material)Resources.Load("mat" + id, typeof(Material));
		/*
		if(PlayerPrefs.GetString("Player0") == "Blue")
			playerColor = Color.blue * 0.5f;
		if(PlayerPrefs.GetString("Player1") == "Green")
			playerColor = Color.green * 0.5f;
		if(PlayerPrefs.GetString("Player2") == "Red")
			playerColor = Color.red * 0.5f;
		if(PlayerPrefs.GetString("Player3") == "Yellow")
			playerColor = Color.yellow * 0.5f;
		//*/
		gameObj.renderer.material.color = toColor(playerColor) * 0.5f;
		/*switch (id)
		{
		case 0:
			gameObj.renderer.material.color = Color.blue * 0.5f;
			break;
		case 1:
			gameObj.renderer.material.color = Color.green * 0.5f;
			break;
		case 2:
			gameObj.renderer.material.color = Color.red * 0.5f;
			break;
		case 3:
			gameObj.renderer.material.color = Color.yellow * 0.5f;
			break;
		}*/
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

	public static Color toColor(ColorCode cC){
		switch(cC){
		case ColorCode.RED: return Color.red; break;
		case ColorCode.BLUE: return Color.blue; break;
		case ColorCode.GREEN: return Color.green; break;
		case ColorCode.YELLOW: return Color.yellow; break;
		default: return new Color(255, 0, 255, 255);
		}
	}
	
	public static ColorCode fromColor(Color c){
		if(c == Color.red) return ColorCode.RED;
		else if(c== Color.blue) return ColorCode.BLUE;
		else if(c == Color.green) return ColorCode.GREEN;
		else if(c == Color.yellow) return ColorCode.YELLOW;
		return ColorCode.WTF;
	}
	
	public static ColorCode colorCodeFromName(string name)
	{
		if (name == "Red") return ColorCode.RED;
		if (name == "Blue") return ColorCode.BLUE;
		if (name == "Green") return ColorCode.GREEN;
		if (name == "Yellow") return ColorCode.YELLOW;
		return ColorCode.WTF;
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
			waveField.SetPressure(pos2d, 1 << 16, this.playerColor); //15);
			heartbeatTimer += 0.5f;
		}

		// hacked special controls for now
		if (Input.GetButton("A_1"))
		{
		}
		// hacked special controls for now
	}
}
