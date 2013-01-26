using UnityEngine;
using System.Collections;

public class Heart : MonoBehaviour {
	
	// for scaling the heart in the xz plane
	public float maxHeartScale = 2.0f;
	public float minHeartScale = 1.0f;
	public float heartPeriod = 1.0f;
	public float heartScaleSpeed = 1.0f;
	
	// for scaling the light away and toward the heart
	public float maxLightScale = 2.0f;
	public float minLightScale = 1.0f;
	public float lightScalePeriod = 1.0f;
	
	Vector3 originalScale;
	
	
	
	Transform thisTransform;

	// Use this for initialization
	void Start () {
		
		thisTransform = transform;
		originalScale = thisTransform.localScale;
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
	}
}
