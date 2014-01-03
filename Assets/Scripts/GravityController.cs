using UnityEngine;
using System.Collections;

public class GravityController : GravityCenter {
	
	public float massMaxMultiplier = 10f;

	private float massMax;
	private float massMin;

	private GravityController_ParticleControl particles;

	private CameraController cameraControl;

	public override void Awake () {
		base.Awake ();
		massMax = r.mass *  massMaxMultiplier;
		massMin = r.mass * -massMaxMultiplier;
		particles = GetComponent<GravityController_ParticleControl>();
				
		cameraControl = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraController>();
	}

	void Start() {
		particles.UpdateParticleSystemRate(gravityForce/massMax);
	}

	public override void FixedUpdate() {
		base.FixedUpdate();

		Bounds bB = bounds;

		if(countAffectedBodies > 0) {
			for (int c = 0; c < countAffectedBodies; c++) {
				bB.Encapsulate(affectedBodies[c].GetComponent<Body>().GetBounds());
			}
		}
		else {
			bB.extents += bB.center;
			bB.center = transform.position;
		}

		cameraControl.UpdateViewport(bB);
	}

	public void AlterForce (float g) {
		//gravityForce = Mathf.Clamp(gravityForce + (g * massChangeRate * Time.deltaTime),massMin,massMax);
		gravityForce = Mathf.Clamp(g,massMin,massMax);
		particles.UpdateParticleSystemRate(gravityForce/massMax);
	}
	
	public override void AddMass (float m)
	{
		base.AddMass (m);
		massMax += m;
		massMin -= m;
	}

	public override void AddMass (float m, Bounds b)
	{
		base.AddMass (m, b);
		massMax += m;
		massMin -= m;
	}
}
