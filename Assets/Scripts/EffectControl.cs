using UnityEngine;
using System.Collections;

public class EffectControl : MonoBehaviour {
		
	private ParticleSystem particles;
	
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
		particles = particleSystem;
	}

	void Update() {
		if (particles.isPlaying == false){}
		//	Reclaim();
	}

	protected void Initialize(Vector3 pos, Quaternion rot, Vector2 velocity) {
		t.position = pos;
		t.localRotation = rot;
		r.velocity = velocity;
		particles.Play();
	}

	protected void Terminate() {}
}
