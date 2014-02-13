using UnityEngine;
using System.Collections;
[RequireComponent (typeof(Rigidbody2D))]
public abstract class Body : MonoBehaviour {
	
	public float hardness = 10;
	public float hardnessDamageThreshold = 5;
	public float hardnessMax = 100;
	public float hardnessGrowthRate = .2f; //TODO: Add growth function

	public float effectiveMass;

	[HideInInspector]
	public Rigidbody2D r;
	[HideInInspector]
	public Transform t;
	[HideInInspector]
	public Renderer n;
	[HideInInspector]
	public GameObject g;

	protected Bounds bounds;
	protected float initHardness;

	public virtual void Awake() {
		r = rigidbody2D;
		t = transform;
		n = renderer;
		g = gameObject;
		effectiveMass = r.mass;
		bounds = n.bounds;
		initHardness = hardness;
	}

	public virtual void OnCollisionEnter2D (Collision2D coll) {
		//print ("Impact=" + (Mathf.Abs(Vector2.Dot (coll.contacts[0].normal,coll.relativeVelocity) * coll.rigidbody.mass)).ToString());
		if (coll.contacts.Length > 0) { //BUG: third collision with no contacts being generated!
			float impact = Mathf.Abs(Vector2.Dot (coll.contacts[0].normal,coll.relativeVelocity) * coll.gameObject.GetComponent<Body>().effectiveMass);
			if (impact >= hardnessDamageThreshold) {
				hardness -= impact;
				if (hardness <= 0) {
					RemoveBody();
				}
			}
		//	print ("Impact = " + (Mathf.Abs(Vector2.Dot (coll.contacts[0].normal,coll.relativeVelocity) * coll.rigidbody.mass)).ToString() + ", Hardness = " + hardness); //TEST: balance
		}
		//TODO: effects
	}
	
	public virtual void OnTriggerEnter2D (Collider2D coll) {
		if (coll.name == "CameraRange") {
			coll.transform.root.GetComponent<GravityCenter>().AddBody(this);
		}
	}
	
	public virtual void OnTriggerExit2D (Collider2D coll) {
		if (coll.name == "GravityRange") {
			coll.transform.root.GetComponent<GravityCenter>().RemoveBody(this);
		}

		if (coll.name == "CameraRange") {
			RemoveBody();
		}
	}

	public virtual void EnableBody() {
		g.SetActive(true);
	}

	public virtual void RemoveBody() {
		//t.position = BodyManager.Instance.hiddenPosition;
		g.SetActive(false);
		hardness = initHardness;
	}

	public virtual Bounds GetBounds() {
		return bounds;
	}

//	void OnDrawGizmos() {
//		Gizmos.color = Color.blue;
//		Gizmos.DrawWireCube(bounds.center,bounds.size);
//	}
}
