using UnityEngine;
using System.Collections;

public class Meteor : Body {

	private TrailRenderer trail;

	public override void Awake () {
		trail = GetComponent<TrailRenderer>();

		base.Awake();

		//r.AddForce(new Vector2(Random.Range(-100,100),Random.Range (-100,100))); //TEST: Add some initial chaos
	}
		
	public override void OnTriggerEnter2D (Collider2D coll) {
		if (!coll.collider2D.isTrigger) RemoveBody ();
		base.OnTriggerEnter2D(coll);
	}

	public void EnableBody(Vector3 pos, Vector2 force) {
		base.EnableBody();
		t.position = pos;
		r.AddForce(force);
		trail.enabled = true;
	}

	public override void RemoveBody() {
		//Instantiate(explosionPrefab,t.position,t.localRotation);
		BodyManager.Instance.SpawnMeteorExplosion(t.position);
		r.isKinematic = true;
		r.velocity = Vector2.zero;
		StartCoroutine(WaitForTrailFade(trail.time));
	}

	IEnumerator WaitForTrailFade(float t) {
		yield return new WaitForSeconds(t * 1.5f);
		r.isKinematic = false;
		trail.enabled = false;
		base.RemoveBody();
	}

	public override Bounds GetBounds ()
	{
		bounds = renderer.bounds;
		return base.GetBounds();
	}
}
