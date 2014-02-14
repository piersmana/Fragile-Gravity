using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GravityCenter : Body {

	public float gravityForce;
	public float gravityDetectionRangeMultiplier = 10f; //TODO: clean up old range/force detection, refactor

	public float gravityDetectionRange;

	//public float rotationSpeed = .025f;

	protected int gravityLayermask;

	protected override void Awake () {
		base.Awake();
		gravityForce = r.mass;
		gravityDetectionRange = t.localScale.x * gravityDetectionRangeMultiplier;

		Body[] childBodies = GetComponentsInChildren<Body>();
		foreach (Body child in childBodies) {
			if (child.gameObject != gameObject) {
				bounds.Encapsulate(child.GetBounds());
			}
		}
		//TODO: Replace with camera lookat

		bodyType = GameManager.BodyTypes.GravityCenter;
	}

	//public virtual void Update () {
		//t.RotateAround(t.position,Vector3.forward,rotationSpeed);
	//}

	protected virtual void FixedUpdate () {
		Body[] activeBodies = GameManager.Instance.GetActiveBodies();
		Body cachedBody;

		for (int i = activeBodies.Length - 1; i >= 0; i--) {
			cachedBody = activeBodies[i];
			Vector2 delta = new Vector2(t.position.x - cachedBody.t.position.x, t.position.y - cachedBody.t.position.y);
			cachedBody.r.AddForce(delta.normalized * gravityForce * cachedBody.r.mass / delta.sqrMagnitude);
		}
	}

	protected override void OnCollisionEnter2D (Collision2D coll) {
		base.OnCollisionEnter2D(coll);
	}

	public virtual void AddChildBody(Body b) {
		//affectedBodies.Add(b);
		AddMass(b.r.mass);
		bounds.Encapsulate(b.GetBounds());
	}

	public virtual void RemoveChildBody(Body b) {
		//affectedBodies.Remove(b);
		AddMass(-b.r.mass);
	}
		
	protected virtual void AddMass(float m) {
		
		r.mass += m;
		effectiveMass = r.mass;
		
		//TODO: condition for growth
	}

	public override Bounds GetBounds() {
		bounds.center = n.bounds.center;
		return base.GetBounds();
	}

	//TODO: destruction condition and effects

//	void OnDrawGizmos() {
//		Gizmos.color = Color.green;
//		Gizmos.DrawWireSphere(transform.position,gravityDetectionRange);
//		Gizmos.color = Color.cyan;
//		Gizmos.DrawWireCube(bounds.center,bounds.size);
//	}
}
