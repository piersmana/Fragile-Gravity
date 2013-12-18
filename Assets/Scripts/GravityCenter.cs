using UnityEngine;
using System.Collections;

public class GravityCenter : MonoBehaviour {

	public float gravityForce = 5;
	public int maxVoxels = 1000;
	public float gravityDetectionRange = 100f;

	private Collider2D[] affectedCubes;
	private int gravityLayermask;

	void Awake () {
	
		affectedCubes = new Collider2D[maxVoxels];
		gravityLayermask = 1 << LayerMask.NameToLayer("FreeVoxels");

	}

	void FixedUpdate () {

		int countAffectedCubes = Physics2D.OverlapCircleNonAlloc(new Vector2(transform.position.x,transform.position.y),gravityDetectionRange,affectedCubes,gravityLayermask);
		Transform cube;

		if(countAffectedCubes > 0) {
			for (int c = 0; c < countAffectedCubes; c++) {
				cube = affectedCubes[c].transform;
				if(cube.name != "GravityCenter") {
					Vector2 delta = new Vector2(transform.position.x - cube.position.x, transform.position.y - cube.position.y);
					cube.rigidbody2D.AddForce(delta.normalized * gravityForce * cube.rigidbody2D.mass / delta.sqrMagnitude);
				}
			}
		}

	}

//	void OnCollisionEnter2D (Collision2D coll) {
//		if(coll.relativeVelocity.magnitude < 5) {
//			coll.gameObject.GetComponent<VoxelMass>().AttachToPlanet(this.transform);
//		}
//	}
}
