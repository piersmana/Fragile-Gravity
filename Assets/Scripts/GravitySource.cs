using UnityEngine;
using System.Collections;

public class GravitySource : MonoBehaviour {
	public float gravityForce;
	
	protected int gravityLayermask;
	
	[HideInInspector]
	public Transform t;
	[HideInInspector]
	public GameObject g;
	[HideInInspector]
	public Rigidbody2D r;
	
	void Awake() {
		t = transform;
		g = gameObject;
		r = rigidbody2D;
		
		if (r != null)
			gravityForce = r.mass;
		
		//TODO: Replace with camera lookat
	}
	
	void FixedUpdate () {
		Body[] activeBodies = Body.GetActiveBodies();
		Body cachedBody;
		
		for (int i = activeBodies.Length - 1; i >= 0; i--) {
			cachedBody = activeBodies[i];
			Vector2 delta = new Vector2(t.position.x - cachedBody.t.position.x, t.position.y - cachedBody.t.position.y);
			cachedBody.r.AddForce(delta.normalized * gravityForce * cachedBody.r.mass / delta.sqrMagnitude);
		}
	}

	//TODO: destruction condition and effects
}
