using UnityEngine;
using System.Collections;

public class Atom : Body {

	private GravityCenter parentBody;

	public override void Awake () {
		base.Awake();

		r.AddForce(new Vector2(Random.Range(-100,100),Random.Range (-100,100))); //TEST: Add some initial chaos
	}
	
	public void AttachToBody(GravityCenter g) {

		r.isKinematic = true;
		t.parent = g.transform;
		gameObject.layer = LayerMask.NameToLayer("OwnedBodies");
		parentBody = g;
		parentBody.AddMass(r.mass);

		//ADD: Effects
		//ADD: Start hardening subroutine

	}
	
	public override void OnCollisionEnter2D (Collision2D coll) {
		base.OnCollisionEnter2D(coll);
	}

	void OnCollisionStay2D (Collision2D coll) {
		GravityCenter g = coll.transform.root.GetComponent<GravityCenter>();
		if (g != null) {
			AttachToBody (g);
		}
	}

	void OnDestroy() { //ADD: better destruction control
		if (parentBody) {
			parentBody.AddMass(-r.mass);
		}
	}
}
