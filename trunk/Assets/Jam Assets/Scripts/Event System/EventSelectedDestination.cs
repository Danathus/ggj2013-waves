using UnityEngine;
using System.Collections;

// =============================================================================
public class EventSelectedDestination : GameEvent {

#region Public Fields
	public Vector3 worldPos;
#endregion
	
	// -------------------------------------------------------------------------
	public EventSelectedDestination(Vector3 worldPos) {
		this.worldPos = worldPos;
	}
}
