using UnityEngine;
using System.Collections;

public class GravityController_ParticleControl : MonoBehaviour {

	public float growthSpeed  = 5f;

	public ParticleSystem gravityIn;
	public ParticleSystem gravityOut;

	void Awake() {

	}

	public void UpdateParticleSystemRate(float gravity) {
		if (gravity >= 0f) {
			gravityOut.emissionRate = 0;
			gravityIn.emissionRate = Mathf.Floor(gravity);
		}
		else {
			gravityIn.emissionRate = 0;
			gravityOut.emissionRate = Mathf.Floor(-gravity);
		}
	}

	public IEnumerator UpdateParticleSystemSize(float scale) {
		float startTime = Time.time;
		float startScale = transform.localScale.x;
		float currentScale;
		do {
			currentScale = Mathf.Lerp(startScale,scale,(Time.time - startTime) * scale/startScale/growthSpeed);
			//transform.localScale = new Vector3(currentScale,currentScale,currentScale);
			gravityIn.startSpeed = gravityOut.startSpeed = currentScale * .8f;
			yield return null;
		} while (currentScale < scale);
		yield return null;
	}


//	IEnumerator Start() {
//		yield return new WaitForSeconds(2f);
//		UpdateParticleSystemRate(100);
//		yield return new WaitForSeconds(2f);
//		StartCoroutine(UpdateParticleSystemSize(2f));
//		UpdateParticleSystemRate(200);
//
//		yield return null;
//	}
}
