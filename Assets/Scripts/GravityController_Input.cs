using UnityEngine;
using System.Collections;

[RequireComponent (typeof(GravityController))]
public class GravityController_Input : MonoBehaviour {

	public float massChangeRate = 10f;

	public float dragThreshold = 5f;
	public float dragSensitivity = 2f;

	private GravityController g;
	private Transform t;

	void Awake () {
		g = GetComponent<GravityController>();
		t = transform;
	}

	void Start() {
		StartCoroutine(CheckStartDrag());
	}

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
		float startGravityForce = g.gravityForce;

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
			g.AlterForce(startGravityForce + (delta/dragSensitivity));
			yield return null;
		}

		yield return null;
	}
}
