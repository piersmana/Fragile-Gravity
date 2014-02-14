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

	[HideInInspector]
	public GameManager.BodyTypes bodyType = GameManager.BodyTypes.Unknown;

	protected Bounds bounds;
	protected float initHardness;

	protected virtual void Awake() {
		r = rigidbody2D;
		t = transform;
		n = renderer;
		g = gameObject;
		effectiveMass = r.mass;
		bounds = n.bounds;
		initHardness = hardness;
	}

	protected virtual void OnCollisionEnter2D (Collision2D coll) {
		//print ("Impact=" + (Mathf.Abs(Vector2.Dot (coll.contacts[0].normal,coll.relativeVelocity) * coll.rigidbody.mass)).ToString());
		if (coll.contacts.Length > 0) { //BUG: third collision with no contacts being generated!
			DamageOnCollision(coll);
		//	print ("Impact = " + (Mathf.Abs(Vector2.Dot (coll.contacts[0].normal,coll.relativeVelocity) * coll.rigidbody.mass)).ToString() + ", Hardness = " + hardness); //TEST: balance
		}
	}
	
	protected virtual void OnTriggerEnter2D (Collider2D coll) {
		if (coll.name == "CameraRange") {
			//TODO: Add to camera watch list
		}
	}
	
	protected virtual void OnTriggerExit2D (Collider2D coll) {
		if (coll.name == "GravityRange") {
			//TODO: Remove from camera watch list
		}

		if (coll.name == "CameraRange") {
			TerminateBody();
		}
	}

	protected void DamageOnCollision(Collision2D coll) {
		float impact = Mathf.Abs(Vector2.Dot (coll.contacts[0].normal,coll.relativeVelocity) * coll.gameObject.GetComponent<Body>().effectiveMass);
		if (impact >= hardnessDamageThreshold) {
			hardness -= impact;
			if (hardness <= 0) {
				TerminateBody();
			}
		}
	}

	public virtual void InitializeBody(Vector3 pos, Vector2 force) {
		t.position = pos;
		r.AddForce(force);
	}

	public virtual void TerminateBody() {
		hardness = initHardness;
		GameManager.Instance.ReclaimBody(this);
	}

	public virtual Bounds GetBounds() {
		return bounds;
	}

//	void OnDrawGizmos() {
//		Gizmos.color = Color.blue;
//		Gizmos.DrawWireCube(bounds.center,bounds.size);
//	}
}
