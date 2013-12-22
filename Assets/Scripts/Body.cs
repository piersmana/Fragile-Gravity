using UnityEngine;
using System.Collections;
[RequireComponent (typeof(Rigidbody2D))]
public abstract class Body : MonoBehaviour {
	
	public float hardness = 50;
	public float effectiveMass;
	public GameObject explosionPrefab;
	
	protected Rigidbody2D r;
	protected Transform t;

	public virtual void Awake() {
		r = rigidbody2D;
		t = transform;
		effectiveMass = r.mass;
	}

	public virtual void OnCollisionEnter2D (Collision2D coll) {
		//print ("Impact=" + (Mathf.Abs(Vector2.Dot (coll.contacts[0].normal,coll.relativeVelocity) * coll.rigidbody.mass)).ToString());
		if (Mathf.Abs(Vector2.Dot (coll.contacts[0].normal,coll.relativeVelocity) * coll.gameObject.GetComponent<Body>().effectiveMass) > hardness) {
			BlowUp(); //ADD: better control for destroy
		//	print ("Impact = " + (Mathf.Abs(Vector2.Dot (coll.contacts[0].normal,coll.relativeVelocity) * coll.rigidbody.mass)).ToString() + ", Hardness = " + hardness); //TEST: balance
		}
		//ADD: effects
	}

	public virtual void BlowUp() {
		Destroy (this.gameObject);
	}
}
