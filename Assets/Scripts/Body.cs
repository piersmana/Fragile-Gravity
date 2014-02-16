using UnityEngine;
using System.Collections;
[RequireComponent (typeof(Rigidbody2D))]
public abstract class Body : PooledObject {
	
	public float hardness = 10;
	public float hardnessDamageThreshold = 5;
	public float hardnessMax = 100;
	public float hardnessGrowthRate = .2f; //TODO: Add growth function

	public float effectiveMass;

	protected Bounds bounds;
	protected float initHardness;
	
	//private static List<Body> _activeBodies;
	private static Body[] activeBodies;
	private static bool refreshActiveBodies;

	protected override void Awake() {
		base.Awake();

		effectiveMass = r.mass;
		bounds = n.bounds;
		initHardness = hardness;

		activeBodies = new Body[0];
	}

	public static Body[] GetActiveBodies() {
		/*if (refreshActiveBodies) {
			_activeBodies.Clear();
			for (int i = 0; i < availableAtoms.Length; i++) {
				if (availableAtoms[i].g.activeSelf)
					_activeBodies.Enqueue(availableAtoms[i]);
			}
			for (int i = 0; i < availableMeteors.Length; i++) {
				if (availableMeteors[i].g.activeSelf)
					_activeBodies.Enqueue(availableMeteors[i]);
			}
			activeBodies = _activeBodies.ToArray();
			refreshActiveBodies = false;
		}
		*/
		return activeBodies;

	}

	protected virtual void OnCollisionEnter2D (Collision2D coll) {
		//print ("Impact=" + (Mathf.Abs(Vector2.Dot (coll.contacts[0].normal,coll.relativeVelocity) * coll.rigidbody.mass)).ToString());
		if (coll.contacts.Length > 0) { //BUG: third collision with no contacts being generated!
			DamageOnCollision(coll);
		//	print ("Impact = " + (Mathf.Abs(Vector2.Dot (coll.contacts[0].normal,coll.relativeVelocity) * coll.rigidbody.mass)).ToString() + ", Hardness = " + hardness); //TEST: balance
		}
	}
	
//	protected virtual void OnTriggerEnter2D (Collider2D coll) {
//		if (coll.name == "CameraRange") {
//			//TODO: Add to camera watch list
//		}
//	}
	
	protected virtual void OnTriggerExit2D (Collider2D coll) {
		if (coll.name == "GravityRange") {
			//TODO: Remove from camera watch list
		}

		if (coll.name == "CameraRange") {
			Reclaim();
		}
	}

	protected void DamageOnCollision(Collision2D coll) {
		float impact = Mathf.Abs(Vector2.Dot (coll.contacts[0].normal,coll.relativeVelocity) * coll.gameObject.GetComponent<Body>().effectiveMass);
		if (impact >= hardnessDamageThreshold) {
			hardness -= impact;
			if (hardness <= 0) {
				Reclaim();
			}
		}
	}

	protected override void Initialize(Vector3 pos, Quaternion rot, Vector2 velocity) {
		t.position = pos;
		t.localRotation = rot;
		r.velocity = velocity;
	}

	protected override void Terminate() {
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
