using UnityEngine;
using System.Collections;
[RequireComponent (typeof(PooledObject))]
[RequireComponent (typeof(Hardness))]
[RequireComponent (typeof(Attachable))]
public class Atom : Body {

	private PooledObject pooled;
	private Hardness hardness;
	private Attachable attachable;
	
	protected override void Awake() {
		base.Awake();

		pooled = GetComponent<PooledObject>();
		hardness = GetComponent<Hardness>();
		attachable = GetComponent<Attachable>();
	}
	
	void OnCollisionEnter2D (Collision2D coll) {
		//print ("Impact=" + (Mathf.Abs(Vector2.Dot (coll.contacts[0].normal,coll.relativeVelocity) * coll.rigidbody.mass)).ToString());
		if (coll.contacts.Length > 0) { //BUG: third collision with no contacts being generated!
			hardness.DamageOnCollision(coll);
			if (hardness.hardness <= 0)
				Terminate();
		}
	}
	
	void OnCollisionStay2D (Collision2D coll) {
		GravityCore g = coll.transform.root.GetComponent<GravityCore>();
		if (g != null) {
			attachable.AttachToCore(g);
		}
	}
	
	public override void Initialize(Vector3 pos, Quaternion rot, Vector2 velocity) {
		t.position = pos;
		t.localRotation = rot;
		r.velocity = velocity;
	}
	
	void Terminate() {
		hardness.Reset();
		attachable.Reset();
		pooled.Reclaim();
	}
}
