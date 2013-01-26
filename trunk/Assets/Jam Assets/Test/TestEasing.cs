using UnityEngine;
using System.Collections;

public class TestEasing : MonoBehaviour {
	
	public float duration = 2.0f;
	
	// Update is called once per frame
	void Update () {
		
		float time = (Time.realtimeSinceStartup % duration) / duration;		
		transform.localPosition = Vector3.right * JamUtil.Easing.EaseInOut(time, JamUtil.EasingType.Cubic);
	}
}
