using UnityEngine;
using System.Collections;

public class GUITimer : MonoBehaviour {
	
	GUIText guiText;

	// Use this for initialization
	void Start () {
		guiText = GetComponent<GUIText>();
	
	}
	
	// Update is called once per frame
	void Update () {
		int time = (int)GameManager.instance.gameTime; // (int)Time.timeSinceLevelLoad
		guiText.text = time.ToString();
	}
}
