using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

	public GameObject atomPrefab;
	public GameObject meteorPrefab;
	public GameObject explosionPrefab;

	public int maxAtoms = 250;
	public int maxMeteors = 50;
	public int maxExplosions = 50;

	//Temp variables to control test level spawning
	public int TEMPspawnsPerFrame = 2;
	public int TEMPspawnsPerLevel = 5;

	private GravityController player;

	void Awake() {
		player = GameObject.FindGameObjectWithTag("Player").GetComponent<GravityController>();
	}

	void Start () {
		//Initialize object pools
		PooledObject.InitializePools<Atom>(atomPrefab, maxAtoms);
		PooledObject.InitializePools<Meteor>(meteorPrefab, maxMeteors);
		PooledObject.InitializePools<EffectControl>(explosionPrefab, maxExplosions);

		//Start level behaviour
		StartCoroutine(Level1Spawn());
	}
	


	/*
	 * API to get the list of objects to apply forces to
	 * Caches an array of activeBodies that can be provided to multiple sources
	 * If a Body has gone active/inactive, refreshActiveBodies boolean gets set and activeBodies is rebuilt
	 */


	/*
	 * Test level spawning function
	 */ 
	IEnumerator Level1Spawn() {
		float range = player.gravityDetectionRange + 10;
		Vector2 atomSpeed = new Vector2(-5f,0);
		Vector2 meteorSpeed = new Vector2(-10f,-10f);

		while (true) {
			yield return new WaitForSeconds(2f);
			
			PooledObject.Spawn<Atom>(new Vector3(range,Random.Range(-4f,4f),0), Quaternion.identity, atomSpeed);

			for (int i = 0; i < TEMPspawnsPerLevel; i++) {
				for (int j = 0; j < TEMPspawnsPerFrame; j++) {
					PooledObject.Spawn<Meteor>(new Vector3(Random.Range(-5f,5f) + range,range,0), Quaternion.identity, meteorSpeed);
					yield return new WaitForSeconds(.05f);
				}
				yield return new WaitForSeconds(.1f);
			}
		}
	}
}
