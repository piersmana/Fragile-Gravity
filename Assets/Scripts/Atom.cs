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

		//TODO: Effects
		//TODO: Start hardening subroutine

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

	public void EnableBody(Vector3 pos, Vector2 force) {
		base.EnableBody();
		t.position = pos;
		r.AddForce(force);
	}

	public override void RemoveBody() {
		//Instantiate(explosionPrefab,t.position,t.localRotation);
		BodyManager.Instance.SpawnAtomExplosion(t.position);
		if (parentBody) {
			r.isKinematic = false;
			parentBody.AddMass(-r.mass);
			t.parent = null;
			parentBody = null;
			effectiveMass = r.mass;
			gameObject.layer = LayerMask.NameToLayer("FreeBodies");
		}
		base.RemoveBody();
	}

	public override Bounds GetBounds ()
	{
		bounds = renderer.bounds;
		return base.GetBounds();
	}
}
