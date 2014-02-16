using UnityEngine;
using System.Collections;

public class EffectControl : PooledObject {
		
	private ParticleSystem particles;

	protected override void Awake() {
		base.Awake();
		particles = particleSystem;
	}

	void Update() {
		if (particles.isPlaying == false)
			Reclaim();
	}

	protected override void Initialize(Vector3 pos, Quaternion rot, Vector2 velocity) {
		t.position = pos;
		t.localRotation = rot;
		r.velocity = velocity;
		particles.Play();
	}

	protected override void Terminate() {}
}
