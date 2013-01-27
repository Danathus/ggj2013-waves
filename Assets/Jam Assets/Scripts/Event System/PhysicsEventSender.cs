using UnityEngine;
using System.Collections;

public class PhysicsEventSender : MonoBehaviour {
	
	void OnTriggerEnter(Collider other) {
		
		EventTriggerEnter triggerEvent = new EventTriggerEnter();
		triggerEvent.self = this.collider;
		triggerEvent.other = other;
		
		EventMessenger.Instance.Raise(triggerEvent);	
	}
}
