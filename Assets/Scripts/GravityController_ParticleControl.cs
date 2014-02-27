using UnityEngine;
using System.Collections;

[RequireComponent (typeof(GravitySource))]
public class GravityController_ParticleControl : MonoBehaviour {

	public float growthSpeed  = 5f;

	private ParticleSystem gravityParticleSystem;
	
	private Vector3 gravityInMinScale = new Vector3(.8f,.8f,.8f);
	private Vector3 gravityOutMinScale = new Vector3(1.2f,1.2f,1.2f);
	private Vector3 gravityNormalScale = new Vector3(1f,1f,1f);

	void Awake() {
		gravityParticleSystem = GameObject.Find("GravityForceParticles").particleSystem;
		UpdateParticleSystemRate(0);
	}

//Old particle modifier that modifiers number of particles based on gravity force
//	public void UpdateParticleSystemRate(float gravity) {
//		if (gravity >= 0f) {
//			gravityOut.emissionRate = 0;
//			gravityIn.emissionRate = Mathf.Floor(gravity);
//		}
//		else {
//			gravityIn.emissionRate = 0;
//			gravityOut.emissionRate = Mathf.Floor(-gravity);
//		}
//	}

	public void UpdateParticleSystemRate(float gravity) {

		gravityParticleSystem.startSpeed = transform.localScale.x * gravity; 

		if (gravityParticleSystem.startSpeed == 0) 
			gravityParticleSystem.startSpeed = 0.00001f;

		if (gravity >= 0) {
			gravityParticleSystem.transform.localScale = Vector3.Lerp(gravityInMinScale, gravityNormalScale, gravity);
		}
		else if (gravity < 0) {
			gravityParticleSystem.transform.localScale = Vector3.Lerp(gravityOutMinScale, gravityNormalScale, -gravity);
		}
	}

	public IEnumerator UpdateParticleSystemSize(float scale) {
		float startTime = Time.time;
		float startScale = transform.localScale.x;
		float currentScale;
		do {
			currentScale = Mathf.Lerp(startScale,scale,(Time.time - startTime) * scale/startScale/growthSpeed);
			//transform.localScale = new Vector3(currentScale,currentScale,currentScale);
			//gravityIn.startSpeed = gravityOut.startSpeed = currentScale * .8f;
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
