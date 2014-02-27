using UnityEngine;
using System.Collections;
public class Hardness : MonoBehaviour {
	
	public float hardness = 10;
	public float hardnessDamageThreshold = 0;
//	public float hardnessMax = 100;
//	public float hardnessGrowthRate = .2f; //TODO: Add growth function
	
	protected float initHardness;
		
	void Awake() {
		initHardness = hardness;
	}

/*	
 * void OnCollisionEnter2D (Collision2D coll) {
		//print ("Impact=" + (Mathf.Abs(Vector2.Dot (coll.contacts[0].normal,coll.relativeVelocity) * coll.rigidbody.mass)).ToString());
		if (coll.contacts.Length > 0) { //BUG: third collision with no contacts being generated!
			DamageOnCollision(coll);
		}
	}
*/
			
	public void DamageOnCollision(Collision2D coll) {
		float impact = Mathf.Abs(Vector2.Dot (coll.contacts[0].normal,coll.relativeVelocity) * coll.gameObject.GetComponent<Body>().effectiveMass);
		if (impact >= hardnessDamageThreshold) {
			hardness -= impact;
		}
	}

	public void Reset() {
		hardness = initHardness;
	}

}
