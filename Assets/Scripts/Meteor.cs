using UnityEngine;
using System.Collections;

public class Meteor : Body {

	private ParticleSystem particles;

	protected override void Awake () {
		particles = particleSystem;
		particles.enableEmission = true;

		base.Awake();
	}
		
	protected void OnTriggerEnter2D (Collider2D coll) {
		if (!coll.collider2D.isTrigger) 
			StartCoroutine(WaitForTrailFade(particles.startLifetime * 1.1f));
//		base.OnTriggerEnter2D(coll);
	}

	protected override void OnTriggerExit2D (Collider2D coll) {
		if (coll.name == "CameraRange") {
			StartCoroutine(WaitForTrailFade(particles.startLifetime * 1.1f));
		}
	} 

	protected override void Initialize(Vector3 pos, Quaternion rot, Vector2 velocity) {
		StartCoroutine(WaitForTrailLead(particles.startLifetime * 1.1f));
		base.Initialize(pos, rot, velocity);
		//StartCoroutine(WaitForTrailFade(particles.startLifetime));
	}

	protected override void Terminate() {
		base.Terminate();
//		StartCoroutine(WaitForTrailFade(particles.startLifetime * 1.1f));
	}

	IEnumerator WaitForTrailFade(float secs) {
		EffectControl.Spawn<EffectControl>(t.position, t.localRotation);
		r.isKinematic = true;
		r.velocity = Vector2.zero;
		yield return new WaitForSeconds(secs);
		particles.enableEmission = false;
		Reclaim();
	}

	IEnumerator WaitForTrailLead(float secs) {
		r.isKinematic = false;
		yield return new WaitForSeconds(secs);
		particles.enableEmission = true;
	}

	public override Bounds GetBounds ()
	{
		bounds = renderer.bounds;
		return base.GetBounds();
	}
}
