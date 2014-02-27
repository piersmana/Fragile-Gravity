using UnityEngine;
using System.Collections;
[RequireComponent (typeof(Rigidbody2D))]
public class Attachable : MonoBehaviour {

	private GravityCore parentBody;
		
	[HideInInspector]
	public Transform t;
	[HideInInspector]
	public GameObject g;
	[HideInInspector]
	public Rigidbody2D r;
	
	void Awake() {
		t = transform;
		g = gameObject;
		r = rigidbody2D;
	}
	
	public void AttachToCore(GravityCore g) {
		r.isKinematic = true;
		t.parent = g.transform;
		gameObject.layer = LayerMask.NameToLayer("OwnedBodies");
		parentBody = g;
		parentBody.AddChildBody(this);
		//effectiveMass = parentBody.effectiveMass;
		
		//TODO: Effects
		//TODO: Start hardening subroutine
		
	}

	public void DetachFromCore() {
		if (parentBody) {
			r.isKinematic = false;
			parentBody.RemoveChildBody(this);
			t.parent = null;
			parentBody = null;
			//effectiveMass = r.mass;
			gameObject.layer = LayerMask.NameToLayer("FreeBodies");
		}
	}

	public void Reset() {
		DetachFromCore();
	}
}
