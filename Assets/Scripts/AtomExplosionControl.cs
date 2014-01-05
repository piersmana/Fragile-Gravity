using UnityEngine;
using System.Collections;

public class AtomExplosionControl : MonoBehaviour {

	private ParticleSystem particles;

	void Awake() {
		particles = GetComponent<ParticleSystem>();
	}

	void Update() {
		if (particles.isPlaying == false)
			gameObject.SetActive(false);
	}

	public void PlayExplosion(Vector3 pos) {
		gameObject.SetActive(true);
		transform.position = pos;
		particles.Play();
	}
}
