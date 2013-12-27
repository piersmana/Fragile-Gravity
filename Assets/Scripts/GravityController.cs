using UnityEngine;
using System.Collections;

public class GravityController : GravityCenter {
	
	public float massMaxMultiplier = 10f;

	private float massMax;
	private float massMin;

	public override void Awake () {
		base.Awake ();
		massMax = r.mass *  massMaxMultiplier;
		massMin = r.mass * -massMaxMultiplier;
	}

	void Update () {
		//float g = Input.GetAxis("GravityShift");
		//if(g != 0)
			
	}

	public void AlterForce (float g) {
		//gravityForce = Mathf.Clamp(gravityForce + (g * massChangeRate * Time.deltaTime),massMin,massMax);
		gravityForce = Mathf.Clamp(g,massMin,massMax);
	}

	public override void AddMass (float m)
	{
		base.AddMass (m);
		massMax += m;
		massMin -= m;
	}
}
