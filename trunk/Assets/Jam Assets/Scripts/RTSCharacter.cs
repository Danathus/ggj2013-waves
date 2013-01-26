using UnityEngine;
using System.Collections;

[RequireComponent (typeof(NavMeshAgent))]
public class RTSCharacter : MonoBehaviour {
	
#region Public Fields
	public ParticleSystem clickEffect;
#endregion
	
#region Private Fields
	NavMeshAgent agent;
	Transform effectParent;
#endregion

	// -------------------------------------------------------------------------
	void Start () {
	
		agent = GetComponent<NavMeshAgent>();
		effectParent = GameObject.FindGameObjectWithTag("DynamicObjectRoot").transform as Transform;
	}
	
	// -------------------------------------------------------------------------
	void Update () {
	
		if (Input.GetMouseButtonDown(1)) {
			
			Ray mouseRay = Camera.mainCamera.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit = new RaycastHit();
			
			if (Physics.Raycast(mouseRay, out hit)) {
				
				agent.destination = hit.point;
				ParticleSystem effect = (ParticleSystem)
					Instantiate(clickEffect, hit.point, Quaternion.identity);
				
				effect.transform.parent = effectParent;
				Destroy(effect.gameObject, 2.0f);
				
				// Raise an event so that anyone who is interested may react
				EventMessenger.Instance.Raise(new EventSelectedDestination(hit.point));
			}
		}
		
		if (agent.hasPath) {
			
			animation.CrossFade("walk");
		} else {
			animation.CrossFade("idle");
		}
	}
}
