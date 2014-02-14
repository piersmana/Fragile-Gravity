using UnityEngine;
using System.Collections;

public class Meteor : Body {

	private ParticleSystem particles;

	protected override void Awake () {
		particles = particleSystem;
		particles.enableEmission = false;

		base.Awake();

		bodyType = GameManager.BodyTypes.Meteor;
	}
		
	protected override void OnTriggerEnter2D (Collider2D coll) {
		if (!coll.collider2D.isTrigger) TerminateBody ();
		base.OnTriggerEnter2D(coll);
	}

	public override void InitializeBody(Vector3 pos, Vector2 force) {
		r.isKinematic = false;
		base.InitializeBody(pos, force);
		StartCoroutine(WaitForTrailFade(particles.startLifetime));
	}

	public override void TerminateBody() {
		GameManager.Instance.SpawnEffect(t.position, t.localRotation, GameManager.BodyTypes.Meteor);
		r.isKinematic = true;
		r.velocity = Vector2.zero;
		StartCoroutine(WaitForTrailFade(particles.startLifetime * 1.1f));
	}

	IEnumerator WaitForTrailFade(float t) {
		yield return new WaitForSeconds(t);
		particles.enableEmission = !particles.enableEmission;
		if (particles.enableEmission == false) {base.TerminateBody();}
	}

	public override Bounds GetBounds ()
	{
		bounds = renderer.bounds;
		return base.GetBounds();
	}
}
