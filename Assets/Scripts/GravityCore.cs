using UnityEngine;
using System.Collections;
[RequireComponent (typeof(GravitySource))]
public class GravityCore : MonoBehaviour {

	private GravitySource grav;

	void Awake() {
		grav = GetComponent<GravitySource>();
	}

	public void AddChildBody(Attachable b) {
		grav.r.mass += b.r.mass;
	}
	
	public void RemoveChildBody(Attachable b) {
		grav.r.mass -= b.r.mass;
	}
}
