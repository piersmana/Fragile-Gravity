using UnityEngine;
using System.Collections;

public class Atom : Body {

	private GravityCenter parentBody;

	protected override void Awake () {
		base.Awake();
		bodyType = GameManager.BodyTypes.Atom;
		//r.AddForce(new Vector2(Random.Range(-100,100),Random.Range (-100,100))); //TEST: Add some initial chaos
	}
	
	public void AttachToBody(GravityCenter g) {

		r.isKinematic = true;
		t.parent = g.transform;
		gameObject.layer = LayerMask.NameToLayer("OwnedBodies");
		parentBody = g;
		parentBody.AddChildBody(this);
		effectiveMass = parentBody.effectiveMass;

		//TODO: Effects
		//TODO: Start hardening subroutine

	}
	
	protected override void OnCollisionEnter2D (Collision2D coll) {
		base.OnCollisionEnter2D(coll);
	}

	void OnCollisionStay2D (Collision2D coll) {
		GravityCenter g = coll.transform.root.GetComponent<GravityCenter>();
		if (g != null) {
			AttachToBody (g);
		}
	}

	public override void InitializeBody(Vector3 pos, Vector2 force) {
		base.InitializeBody(pos, force);
	}

	public override void TerminateBody() {
		//Instantiate(explosionPrefab,t.position,t.localRotation);
		GameManager.Instance.SpawnEffect(t.position, Quaternion.identity, GameManager.BodyTypes.Atom);
		if (parentBody) {
			r.isKinematic = false;
			parentBody.RemoveChildBody(this);
			t.parent = null;
			parentBody = null;
			effectiveMass = r.mass;
			gameObject.layer = LayerMask.NameToLayer("FreeBodies");
		}
		base.TerminateBody();
	}

	public override Bounds GetBounds ()
	{
		bounds = renderer.bounds;
		return base.GetBounds();
	}
}
