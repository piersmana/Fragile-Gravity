using UnityEngine;
using System.Collections;

public class GravityController : GravityCenter {
	
	public float massMaxMultiplier = 10f;

	private float massMax;
	private float massMin;

	private GravityController_ParticleControl particles;

	private CameraController cameraControl;

	protected override void Awake () {
		base.Awake ();
		massMax = r.mass *  massMaxMultiplier;
		massMin = r.mass * -massMaxMultiplier;
		particles = GetComponent<GravityController_ParticleControl>();
				
		cameraControl = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraController>();
	}

	void Start() {
		particles.UpdateParticleSystemRate(gravityForce/massMax);
	}

	protected void Update() {
		Body[] activeBodies = Body.GetActiveBodies();

		Bounds bB = bounds;
		bB.extents += bB.center;
		bB.center = t.position;

		for (int i = activeBodies.Length - 1; i >= 0; i--) {
			//if (activeBodies[i].bodyType != GameManager.BodyTypes.Meteor)
				bB.Encapsulate(activeBodies[i].GetBounds());
				//TODO: Remove, instead use camera watch list
		}

		cameraControl.UpdateViewport(bB);
	}

	public void AlterForce (float g) {
		//gravityForce = Mathf.Clamp(gravityForce + (g * massChangeRate * Time.deltaTime),massMin,massMax);
		gravityForce = Mathf.Clamp(g,massMin,massMax); //TODO: make this a % change
		particles.UpdateParticleSystemRate(gravityForce/massMax);
	}
		
	protected override void AddMass (float m)
	{
		base.AddMass(m);
		massMax += m;
		massMin -= m;
	}
}
