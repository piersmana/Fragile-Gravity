using UnityEngine;
using System.Collections;

public class GravityCenter : Body {

	public static int maxAtoms = 1000;

	public float gravityForce;
	public float gravityDetectionRangeMultiplier = 25f;

	public float gravityDetectionRange;

	//public float rotationSpeed = .025f;

	protected Collider2D[] affectedBodies;
	protected int countAffectedBodies = 0;
	protected int gravityLayermask;

	public override void Awake () {
		base.Awake();
		gravityForce = r.mass;
		gravityDetectionRange = t.localScale.x * gravityDetectionRangeMultiplier;
		affectedBodies = new Collider2D[maxAtoms];
		gravityLayermask = 1 << LayerMask.NameToLayer("FreeBodies");

		Body[] childBodies = GetComponentsInChildren<Body>();
		foreach (Body child in childBodies) {
			if (child.gameObject != gameObject) {
				bounds.Encapsulate(child.GetBounds());
			}
		}
	}

	//public virtual void Update () {
		//t.RotateAround(t.position,Vector3.forward,rotationSpeed);
	//}

	public virtual void FixedUpdate () {

		countAffectedBodies = Physics2D.OverlapCircleNonAlloc(new Vector2(t.position.x,t.position.y),gravityDetectionRange,affectedBodies,gravityLayermask);
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

	public virtual void AddMass(float m, Bounds b) {
		r.mass += m;
		effectiveMass = r.mass;
		bounds.Encapsulate(b);

		//ADD: condition for growth
	}	

	public override Bounds GetBounds() {
		bounds.center = n.bounds.center;
		return base.GetBounds();
	}

	//ADD: destruction condition and effects

	void OnDrawGizmos() {
		Gizmos.color = Color.green;
		Gizmos.DrawWireSphere(transform.position,gravityDetectionRange);
		Gizmos.color = Color.cyan;
		Gizmos.DrawWireCube(bounds.center,bounds.size);
	}
}
