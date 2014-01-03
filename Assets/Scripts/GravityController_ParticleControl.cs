using UnityEngine;
using System.Collections;

[RequireComponent (typeof(GravityController))]
public class GravityController_ParticleControl : MonoBehaviour {

	public float growthSpeed  = 5f;

	public ParticleSystem gravityIn;
	public ParticleSystem gravityOut;
	
	private Vector3 gravityInMinScale = new Vector3(1.4f,1.4f,1.4f);
	private Vector3 gravityOutMinScale = new Vector3(.4f,.4f,.4f);
	private Vector3 gravityInMaxScale = new Vector3(1f,1f,1f);
	private Vector3 gravityOutMaxScale = new Vector3(1f,1f,1f);

	void Awake() {
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
		gravityOut.startSpeed = Mathf.Lerp(.05f, transform.localScale.x * -gravity, -gravity);
		gravityOut.transform.localScale = Vector3.Lerp(gravityOutMinScale,gravityOutMaxScale,-gravity);

		gravityIn.startSpeed = Mathf.Lerp(.05f, transform.localScale.x * gravity, gravity);
		gravityIn.transform.localScale = Vector3.Lerp(gravityInMinScale,gravityInMaxScale,gravity);
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
