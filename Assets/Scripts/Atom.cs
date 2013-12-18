using UnityEngine;
using System.Collections;

public class Atom : MonoBehaviour {

	private bool attachedToPlanet = false;

	void Awake () {

		attachedToPlanet = false;
	
	}
	
	public void AttachToPlanet(Transform planet) {

		print ("attached");
		this.rigidbody2D.isKinematic = true;
		this.transform.parent = planet;
		this.gameObject.layer = LayerMask.NameToLayer("PlayerPlanet");
		attachedToPlanet = true;

	}
	
	void OnCollisionEnter2D (Collision2D coll) {
		print (coll.gameObject.name + " " + coll.relativeVelocity.magnitude);
		if(coll.relativeVelocity.magnitude < 20) {
			AttachToPlanet(coll.transform);
		}
	}
}
