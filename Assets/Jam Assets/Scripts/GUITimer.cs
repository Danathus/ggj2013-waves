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
		guiText.text = ((int)Time.timeSinceLevelLoad).ToString();;
	}
}
