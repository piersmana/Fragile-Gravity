using UnityEngine;
using System.Collections;

public class GravityCenter : Body {

	public static int maxAtoms = 1000;

	public float gravityForce;
	public float gravityDetectionRangeMultiplier = 15f;

	public float gravityDetectionRange;

	protected Collider2D[] affectedBodies;
	protected int gravityLayermask;

	public override void Awake () {
		base.Awake();
		gravityForce = r.mass;
		gravityDetectionRange = t.localScale.magnitude * gravityDetectionRangeMultiplier;
		affectedBodies = new Collider2D[maxAtoms];
		gravityLayermask = 1 << LayerMask.NameToLayer("FreeBodies");

	}

	void FixedUpdate () {

		int countAffectedBodies = Physics2D.OverlapCircleNonAlloc(new Vector2(t.position.x,t.position.y),gravityDetectionRange,affectedBodies,gravityLayermask);
		Transform bT;
		Rigidbody2D bR;

		if(countAffectedBodies > 0) {
			for (int c = 0; c < countAffectedBodies; c++) {
				bT = affectedBodies[c].transform;
				bR = affectedBodies[c].rigidbody2D;

				Vector2 delta = new Vector2(t.position.x - bT.position.x, t.position.y - bT.position.y);
				bR.AddForce(delta.normalized * gravityForce * bR.mass / delta.sqrMagnitude);
			}
		}

	}

	public override void OnCollisionEnter2D (Collision2D coll) {
		base.OnCollisionEnter2D(coll);
	}

	public virtual void AddMass(float m) {
		r.mass += m;
		effectiveMass = r.mass;

		//ADD: condition for growth
	}	

	//ADD: destruction condition and effects

	void OnDrawGizmos() {
		Gizmos.color = Color.green;
		Gizmos.DrawWireSphere(transform.position,gravityDetectionRange);
	}
}
