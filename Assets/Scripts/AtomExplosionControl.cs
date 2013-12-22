using UnityEngine;
using System.Collections;

public class AtomExplosionControl : MonoBehaviour {

	private ParticleSystem particles;

	void Awake() {
		particles = GetComponent<ParticleSystem>();
	}

	void Update() {
		if (particles.isPlaying == false)
			Destroy(this.gameObject);
	}
}
