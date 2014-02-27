using UnityEngine;
using System.Collections;
[RequireComponent (typeof(Rigidbody2D))]
public abstract class Body : MonoBehaviour {

	public float effectiveMass;
	
	//private static List<Body> _activeBodies;
	private static Body[] activeBodies;
	private static bool initialized = false;
	
	[HideInInspector]
	public Transform t;
	[HideInInspector]
	public GameObject g;
	[HideInInspector]
	public Rigidbody2D r;
	
	protected virtual void Awake() {
		if (!initialized) {
			activeBodies = new Body[0];
			initialized = true;
		}

		t = transform;
		g = gameObject;
		r = rigidbody2D;

		effectiveMass = r.mass;
	}

	public static Body[] GetActiveBodies() {
		/*if (refreshActiveBodies) {
			_activeBodies.Clear();
			for (int i = 0; i < availableAtoms.Length; i++) {
				if (availableAtoms[i].g.activeSelf)
					_activeBodies.Enqueue(availableAtoms[i]);
			}
			for (int i = 0; i < availableMeteors.Length; i++) {
				if (availableMeteors[i].g.activeSelf)
					_activeBodies.Enqueue(availableMeteors[i]);
			}
			activeBodies = _activeBodies.ToArray();
			refreshActiveBodies = false;
		}
		*/
		return activeBodies;

	}

//	protected virtual void OnTriggerEnter2D (Collider2D coll) {
//		if (coll.name == "CameraRange") {
//			//TODO: Add to camera watch list
//		}
//	}
	
//	protected void OnTriggerExit2D (Collider2D coll) {
//		if (coll.name == "GravityRange") {
//			//TODO: Remove from camera watch list
//		}
//
//		if (coll.name == "CameraRange") {
//			//Reclaim();
//		}
//	}
	public abstract void Initialize(Vector3 pos, Quaternion rot, Vector2 velocity);
}
