using UnityEngine;
using System.Collections;

public class Heart : MonoBehaviour {
	
	// for scaling the heart in the xz plane
	[HideInInspector]
	public float maxHeartScale = 2.0f; // changed to 1.2 in data
	[HideInInspector]
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

	static public float maxHealth = 3.0f;
	public float health = maxHealth;
	public float regenRate = 0.1f; // hp per second
	public float hitDamage = 1.0f; // amount of damage done per hit

	// Use this for initialization
	void Start () {
		
		thisTransform = transform;
		originalScale = thisTransform.localScale;
		originalLightPos = childLight.transform.position;
	}
	
	// Update is called once per frame
	public bool dead = false;
	void Update () {		
		// update minheartscale based on health
		minHeartScale = 0.1f + 0.9f*(health / maxHealth);
		maxHeartScale = minHeartScale + 0.2f;
		//Debug.Log (minHeartScale + ", " + maxHeartScale);
		//
		
		if (dead)
		{
			minHeartScale = maxHeartScale = 0.0f;
		}
		
		float time = heartPeriod * Mathf.PI * Time.realtimeSinceStartup;
	
		// go from [-1, 1] to [0, 1]
		float percentScale = Mathf.Sin(time) * 0.5f + 0.5f;
		float heartScale = minHeartScale + (maxHeartScale - minHeartScale) * percentScale;
		
		Vector3 newHeartScale = originalScale * heartScale;
		newHeartScale.z = originalScale.z;
		
		thisTransform.localScale = newHeartScale;
		
		childLight.transform.position = 
			Vector3.Lerp(originalLightPos, originalLightPos - Vector3.forward * lightScale, percentScale);

		if (dead)
		{
			return;
		}

		UpdateHeartbeat();
		UpdateHealth();
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

	void UpdateHealth()
	{
		health += Time.deltaTime * regenRate;
		health = Mathf.Min(health, maxHealth);
		//Debug.Log(health);
	}

	public void GetHurt()
	{
		health -= hitDamage;
		if (health <= 0)
		{
			health = 0;
			Die();
		}
	}
	
	public void Die()
	{
		Vector3 pos3d = Camera.main.WorldToScreenPoint(Vector3.zero);
		Vector2 pos2d = new Vector2(pos3d.x, pos3d.y);
		int heartBeatStrength = 1 << 20; //16;
		waveField.SetPressure(pos2d, heartBeatStrength, Player.ColorCode.MAGENTA); //15);
		
		dead = true;
	}
}
