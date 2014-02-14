using UnityEngine;
using System.Collections;

public class EffectControl : MonoBehaviour {
		
	[HideInInspector]
	public Transform t;
	[HideInInspector]
	public Renderer n;
	[HideInInspector]
	public GameObject g;

	private ParticleSystem particles;

	void Awake() {
		t = transform;
		n = renderer;
		g = gameObject;

		particles = particleSystem;
	}

	void Update() {
		if (particles.isPlaying == false)
			GameManager.Instance.ReclaimEffect(this);
	}

	public void InitializeEffect(Vector3 pos, Quaternion rot) {
		t.position = pos;
		t.localRotation = rot;
		particles.Play();
	}
}
