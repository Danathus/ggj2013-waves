using UnityEngine;
using System.Collections;

public class PlayerSel : MonoBehaviour {
	
	private bool toggleRed = true;
	private bool toggleBlue = true;
	private bool toggleGreen = true;
	private bool toggleYellow = true;
	
	void OnGUI(){
		
		toggleRed = GUI.Toggle (new Rect (Screen.width/2-40, Screen.height/2 - 20, 100, 30), toggleRed, "Red Player");
		toggleBlue = GUI.Toggle (new Rect (Screen.width/2-40, Screen.height/2 + 20, 100, 30), toggleBlue, "Blue Player");
		toggleGreen = GUI.Toggle (new Rect (Screen.width/2-40, Screen.height/2 + 60, 100, 30), toggleGreen, "Green Player");
		toggleYellow = GUI.Toggle (new Rect (Screen.width/2-40, Screen.height/2 + 100, 100, 30), toggleYellow, "Yellow Player");
		
		if (GUI.Button (new Rect (Screen.width / 2 - 80, Screen.height /2 - 70, 140, 50), "Play")) {
			PlayerPrefs.DeleteAll();
			if(toggleRed)
				PlayerPrefs.SetString("Player2", "Red");
			if(toggleBlue)
				PlayerPrefs.SetString ("Player0", "Blue");
			if(toggleGreen)
				PlayerPrefs.SetString("Player1", "Green");
			if(toggleYellow)
				PlayerPrefs.SetString("Player3", "Yellow");
			Application.LoadLevel("TestScene");
		}
	}
	
}
