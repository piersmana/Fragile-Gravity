using UnityEngine;
using System.Collections;
[RequireComponent (typeof(GravitySource))]
public class GravityController : MonoBehaviour {
		
	public float massMaxMultiplier = 10f;
		
	public float massChangeRate = 10f;
	
	public float dragThreshold = 5f;
	public float dragSensitivity = 2f;

	private GravityController_ParticleControl particles;

	[HideInInspector]
	public Transform t;
	[HideInInspector]
	public GameObject g;
	[HideInInspector]
	public Rigidbody2D r;
		
	private GravitySource grav;

	void Awake() {
		t = transform;
		g = gameObject;
		r = rigidbody2D;

		grav = GetComponent<GravitySource>();

		particles = GetComponent<GravityController_ParticleControl>();
	}

	void Start() {
		particles.UpdateParticleSystemRate(grav.gravityForce/(r.mass *  massMaxMultiplier));
		StartCoroutine(CheckStartDrag());
	}

	void AlterForce (float f) {
		float massMax = r.mass *  massMaxMultiplier;
		grav.gravityForce = Mathf.Clamp(r.mass * -massMax, r.mass *  massMax, grav.gravityForce + f);
		particles.UpdateParticleSystemRate(grav.gravityForce/massMax);
	}

	//Input
	
	IEnumerator CheckStartDrag() {
		while (true) {
			if (Input.GetMouseButton(0)) {
				yield return StartCoroutine(DragAdjustGravity());
			}
			yield return null;
		}
	}
	
	IEnumerator DragAdjustGravity() {
		Vector3 gravityScreenCenter = Camera.main.WorldToScreenPoint(t.position);
		float startDistance = Vector3.Distance(gravityScreenCenter, Input.mousePosition);
		
		while (Input.GetMouseButton(0)) {
			float delta = Vector3.Distance(gravityScreenCenter,Input.mousePosition) - startDistance;
			if (Mathf.Abs(delta) >= dragThreshold) {
				break;
			}
			yield return null;
		}
		
		while (Input.GetMouseButton(0)) {
			float delta = Vector3.Distance(gravityScreenCenter,Input.mousePosition) - startDistance;
			//print (delta);
			//Tweak gravity manipulation to have sensitivity based on setting + screen size
			AlterForce(delta * dragSensitivity);
			yield return null;
		}
		
		yield return null;
	}
}
