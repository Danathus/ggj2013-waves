using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour {
	
	private bool toggleRed = true;
	private bool toggleBlue = true;
	private bool toggleGreen = true;
	private bool toggleYellow = true;
	
	void OnGUI () {
		
		GUI.Label (new Rect (Screen.width/2 - 30, Screen.height/4 - 50, 100, 60), "Waves");
		
		/*toggleRed = GUI.Toggle (new Rect (Screen.width/2-40, Screen.height/2 - 20, 100, 30), toggleRed, "Red Player");
		toggleBlue = GUI.Toggle (new Rect (Screen.width/2-40, Screen.height/2 + 20, 100, 30), toggleBlue, "Blue Player");
		toggleGreen = GUI.Toggle (new Rect (Screen.width/2-40, Screen.height/2 + 60, 100, 30), toggleGreen, "Green Player");
		toggleYellow = GUI.Toggle (new Rect (Screen.width/2-40, Screen.height/2 + 100, 100, 30), toggleYellow, "Yellow Player");*/
		
		GUI.Label (new Rect(Screen.width/2 - 100, Screen.height/2 + 40, 300,40), "Note: Play this game with Xbox 360 controllers.");
		
		GUI.Label (new Rect(Screen.width/2 - 120, Screen.height - 70, 440, 40), "Made by: Alexandr Syskin, Danny McCue, and Michael Guerrero");
		
		if (GUI.Button (new Rect (Screen.width / 2 - 60, Screen.height /2 - 70, 140, 50), "Character Selection")) {
			Application.LoadLevel("CharSelectionScene");
			//print ("You clicked the button!");
			//Popup.List (new Rect(10,10,150,100),ref showList, ref menuOpt, content, popUpList, GUIStyle.none, callBack);
		}
		
	}
	
	public void callBack(){
	}
}
