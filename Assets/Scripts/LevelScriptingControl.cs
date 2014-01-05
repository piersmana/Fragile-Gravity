using UnityEngine;
using System.Collections;

public class LevelScriptingControl : MonoBehaviour {

	public int spawnsPerFrame = 2;
	public int spawnsPerLevel = 100;

	public GameObject atomPrefab;
	public GameObject clusterPrefab;
	public GameObject planetPrefab;

	private GravityController player;

	void Awake() {
		player = GameObject.FindGameObjectWithTag("Player").GetComponent<GravityController>();
	}

	// Use this for initialization
	void Start () {
		StartCoroutine(Level1Spawn());
	}


	IEnumerator Level1Spawn() {
		float range = player.gravityDetectionRange + 10;
		Rigidbody2D curAtom;
		Vector2 atomSpeed = new Vector2(-1000f,0);

		yield return new WaitForSeconds(2f);
		
		for (int i = 0; i < spawnsPerLevel; i++) {
			for (int j = 0; j < spawnsPerFrame; j++) {
			curAtom = (Instantiate(atomPrefab, new Vector3(range,Random.Range(3f,10f),0), Quaternion.identity) as GameObject).rigidbody2D;
			curAtom.AddForce(atomSpeed);
			}
			yield return null;
		}
	}
}
