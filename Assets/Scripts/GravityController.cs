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

	public void Update() {

		Bounds bB = bounds;
		bB.extents += bB.center;
		bB.center = t.position;
		
		for (int i = manager.maxAtoms - 1; i >= 0; i--) {
			if (manager.availableAtoms[i].g.activeSelf) 
				bB.Encapsulate(manager.availableAtoms[i].GetBounds());
			else 
				manager.availableAtoms[i].g.SetActive(false);
		}

		cameraControl.UpdateViewport(bB);
	}

	public void AlterForce (float g) {
		//gravityForce = Mathf.Clamp(gravityForce + (g * massChangeRate * Time.deltaTime),massMin,massMax);
		gravityForce = Mathf.Clamp(g,massMin,massMax); //TODO: make this a % change
		particles.UpdateParticleSystemRate(gravityForce/massMax);
	}
	
	public override void AddMass (float m, Bounds b)
	{
		base.AddMass(m, b);
		massMax += m;
		massMin -= m;
	}
	
	public override void AddMass (float m)
	{
		base.AddMass(m);
		massMax += m;
		massMin -= m;
	}
}
