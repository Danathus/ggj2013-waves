using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

// =============================================================================
public class JamWindow : EditorWindow {
	
	public Transform jamTransform;
	float jamValue = 1.0f;
	
	bool showProperties = true;
	
	
	[MenuItem("Window/Tone Utility")]
	// -------------------------------------------------------------------------
	static void Init() {

		//JamWindow window = (JamWindow)
		EditorWindow.GetWindow(typeof(JamWindow));
	}

	// -------------------------------------------------------------------------
	void OnGUI() {	
		
		//GUI.color = Color.white;
		
		//EditorGUILayout.BeginVertical(text:"Tone Answers", style:"box", options:GUILayout.ExpandWidth(true));
		//EditorGUILayout.BeginVertical();
		//EditorGUILayout.Space(20.0f);
		
		showProperties = EditorGUILayout.Foldout(showProperties, "Jam Stuff");
		
		if (showProperties) {
			
			//EditorGUIUtility.LookLikeInspector();
			//toneAnswerOrigin = EditorGUILayout.Vector3Field("Origin", toneAnswerOrigin);
			jamTransform = (Transform)EditorGUILayout.ObjectField("some transform", jamTransform, typeof(Transform), false);			
			jamValue = EditorGUILayout.FloatField("some value", jamValue);
			
			GUI.enabled = (jamTransform != null);
			if (GUILayout.Button("some action", GUILayout.MaxWidth(150.0f))) {
				//OrientAnswerTagBlocks();
			}
			GUI.enabled = false;
		}
		
	
	    // We need to match all BeginGroup calls with an EndGroup
	    //EditorGUILayout.EndVertical();
	}
	
	// -------------------------------------------------------------------------
	void DestroyChildren(Transform parent) {
		
		var children = new List<GameObject>();

		foreach (Transform child in parent) {
			children.Add(child.gameObject);
		}
		
		children.ForEach(child => DestroyImmediate(child));
	}
}
