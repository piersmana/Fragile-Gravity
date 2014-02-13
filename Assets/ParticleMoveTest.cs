using UnityEngine;
using System.Collections;

public class ParticleMoveTest : MonoBehaviour {

	public Vector2 forces = new Vector2(20f,0);

	void Start() {
		StartCoroutine("TestMotion");
	}

	void FixedUpdate() {
		
		rigidbody2D.AddForce(forces);
	}

	IEnumerator TestMotion() {
		while (true) {
			yield return new WaitForSeconds(1f);
			forces = -forces;
			yield return new WaitForSeconds(1f);
		}
	}
}
