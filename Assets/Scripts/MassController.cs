using UnityEngine;
using System.Collections;

public class MassController : MonoBehaviour {

	public float massChangeRate = 5f;
	public float massMax = 100f;
	public float massMin = -100f;

	private GravityCenter massSource;

	void Awake () {
		massSource = GetComponent<GravityCenter>();
	}

	void Update () {
		float g = Input.GetAxis("GravityShift");
		if(g != 0)
			massSource.gravityForce = Mathf.Clamp(massSource.gravityForce + (g * massChangeRate * Time.deltaTime),massMin,massMax);
	}
}
