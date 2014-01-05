using UnityEngine;
using System.Collections;
[RequireComponent (typeof(Rigidbody2D))]
public abstract class Body : MonoBehaviour {
	
	public float hardness = 50;
	public float effectiveMass;
	
	protected Rigidbody2D r;
	protected Transform t;
	protected Renderer n;
	protected GameObject g;
	protected Bounds bounds;

	public virtual void Awake() {
		r = rigidbody2D;
		t = transform;
		n = renderer;
		g = gameObject;
		effectiveMass = r.mass;
		bounds = n.bounds;
	}

	public virtual void OnCollisionEnter2D (Collision2D coll) {
		//print ("Impact=" + (Mathf.Abs(Vector2.Dot (coll.contacts[0].normal,coll.relativeVelocity) * coll.rigidbody.mass)).ToString());
		if (coll.contacts.Length > 0 && Mathf.Abs(Vector2.Dot (coll.contacts[0].normal,coll.relativeVelocity) * coll.gameObject.GetComponent<Body>().effectiveMass) > hardness) { //BUG: third collision with no contacts being generated!
			RemoveBody(); //ADD: better control for destroy
		//	print ("Impact = " + (Mathf.Abs(Vector2.Dot (coll.contacts[0].normal,coll.relativeVelocity) * coll.rigidbody.mass)).ToString() + ", Hardness = " + hardness); //TEST: balance
		}
		//ADD: effects
	}

	public virtual void EnableBody() {
		g.SetActive(true);
	}

	public virtual void RemoveBody() {
		t.position = BodyManager.Instance.hiddenPosition;
		g.SetActive(false);
	}

	public virtual Bounds GetBounds() {
		return bounds;
	}

	void OnDrawGizmos() {
		Gizmos.color = Color.blue;
		Gizmos.DrawWireCube(bounds.center,bounds.size);
	}
}
