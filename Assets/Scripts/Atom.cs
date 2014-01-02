using UnityEngine;
using System.Collections;

public class Atom : Body {

	private GravityCenter parentBody;

	public override void Awake () {
		base.Awake();

		//r.AddForce(new Vector2(Random.Range(-100,100),Random.Range (-100,100))); //TEST: Add some initial chaos
	}
	
	public void AttachToBody(GravityCenter g) {

		r.isKinematic = true;
		t.parent = g.transform;
		gameObject.layer = LayerMask.NameToLayer("OwnedBodies");
		parentBody = g;
		parentBody.AddMass(r.mass,bounds);
		effectiveMass = parentBody.effectiveMass;

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

	public override void BlowUp() {
		Instantiate(explosionPrefab,t.position,t.localRotation);
		if (parentBody) {
			parentBody.AddMass(-r.mass);
		}
		base.BlowUp();
	}

	public override Bounds GetBounds ()
	{
		bounds = new Bounds(t.position,renderer.bounds.size);
		return base.GetBounds ();
	}
}
