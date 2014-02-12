using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GravityCenter : Body {

	public float gravityForce;
	public float gravityDetectionRangeMultiplier = 10f; //TODO: clean up old range/force detection, refactor

	public float gravityDetectionRange;

	//public float rotationSpeed = .025f;

	protected int gravityLayermask;

	protected BodyManager manager;

	public override void Awake () {
		base.Awake();
		gravityForce = r.mass;
		gravityDetectionRange = t.localScale.x * gravityDetectionRangeMultiplier;

		manager = GameObject.FindGameObjectWithTag("GameController").GetComponent<BodyManager>();

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

		for (int i = manager.maxAtoms - 1; i >= 0; i--) {
			if (manager.availableAtoms[i].g.activeSelf) {					
				Vector2 delta = new Vector2(t.position.x - manager.availableAtoms[i].t.position.x, t.position.y - manager.availableAtoms[i].t.position.y);
				manager.availableAtoms[i].r.AddForce(delta.normalized * gravityForce * manager.availableAtoms[i].r.mass / delta.sqrMagnitude);
			}
			else 
				manager.availableAtoms[i].g.SetActive(false);
		}
	}

	public override void OnCollisionEnter2D (Collision2D coll) {
		base.OnCollisionEnter2D(coll);
	}

	public virtual void AddBody(Body b) {
		//affectedBodies.Add(b);
	}

	public virtual void RemoveBody(Body b) {
		//affectedBodies.Remove(b);
	}
	
	public virtual void AddMass(float m, Bounds b) {
		
		r.mass += m;
		effectiveMass = r.mass;
		bounds.Encapsulate(b);
		
		//TODO: condition for growth
	}
	
	public virtual void AddMass(float m) {
		
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
