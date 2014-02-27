using UnityEngine;
using System.Collections;
[RequireComponent (typeof(PooledObject))]
public class Meteor : Body {

	private ParticleSystem particles;

	private PooledObject pooled;
		
	protected override void Awake() {
		base.Awake();

		pooled = GetComponent<PooledObject>();

		particles = particleSystem;
		particles.enableEmission = true;
	}
		
	protected void OnTriggerEnter2D (Collider2D coll) {
		if (!coll.collider2D.isTrigger) 
			StartCoroutine(WaitForTrailFade(particles.startLifetime * 1.1f));
	}

	protected  void OnTriggerExit2D (Collider2D coll) {
		if (coll.name == "CameraRange") {
			StartCoroutine(WaitForTrailFade(particles.startLifetime * 1.1f));
		}
	} 

	public override void Initialize(Vector3 pos, Quaternion rot, Vector2 velocity) {
		t.position = pos;
		StartCoroutine(WaitForTrailLead(particles.startLifetime * 1.1f));
		r.velocity = velocity;
	}

	IEnumerator WaitForTrailFade(float secs) {
		//EffectControl.Spawn<EffectControl>(t.position, t.localRotation);
		r.isKinematic = true;
		r.velocity = Vector2.zero;
		yield return new WaitForSeconds(secs);
		particles.enableEmission = false;
		Terminate();
	}

	IEnumerator WaitForTrailLead(float secs) {
		r.isKinematic = false;
		yield return new WaitForSeconds(secs);
		particles.enableEmission = true;
	}

	void Terminate() {
		pooled.Reclaim();
	}
}
