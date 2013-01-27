using UnityEngine;
using System.Collections;

public class Heart : MonoBehaviour {
	
	// for scaling the heart in the xz plane
	public float maxHeartScale = 2.0f;
	public float minHeartScale = 1.0f;
	public float heartPeriod = 1.0f;
	public float heartScaleSpeed = 1.0f;
	
	// for scaling the light away and toward the heart
	public Transform childLight;
	public float lightScale = 2.0f;
	
	Vector3 originalScale;
	Vector3 originalLightPos;
	
	Transform thisTransform;
	public WaveField waveField;

	// Use this for initialization
	void Start () {
		
		thisTransform = transform;
		originalScale = thisTransform.localScale;
		originalLightPos = childLight.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		
		float time = heartPeriod * Mathf.PI * Time.realtimeSinceStartup;
	
		// go from [-1, 1] to [0, 1]
		float percentScale = Mathf.Sin(time) * 0.5f + 0.5f;
		float heartScale = minHeartScale + (maxHeartScale - minHeartScale) * percentScale;
		
		Vector3 newHeartScale = originalScale * heartScale;
		newHeartScale.z = originalScale.z;
		
		thisTransform.localScale = newHeartScale;
		
		childLight.transform.position = 
			Vector3.Lerp(originalLightPos, originalLightPos - Vector3.forward * lightScale, percentScale);
		
		UpdateHeartbeat();
	}
	
	float heartbeatTimer = 2.0f;
	void UpdateHeartbeat()
	{
		heartbeatTimer -= Time.deltaTime;
		//Debug.Log(heartbeatTimer);
		if (heartbeatTimer < 0)
		{
			heartbeatTimer += 2.0f;
			Vector3 pos3d = Camera.main.WorldToScreenPoint(Vector3.zero);
			Vector2 pos2d = new Vector2(pos3d.x, pos3d.y);
			int heartBeatStrength = 1 << 14; //16;
			waveField.SetPressure(pos2d, heartBeatStrength, Player.ColorCode.MAGENTA); //15);
		}
	}
}
