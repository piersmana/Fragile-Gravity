using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class ObjectInitialization {
	public BodyType type;
	public GameObject prefab;
	public int maxObjects;
}

public class GameManager : MonoBehaviour {

	public ObjectInitialization[] initializationArray;

	//Temp variables to control test level spawning
	public int TEMPspawnsPerFrame = 2;
	public int TEMPspawnsPerLevel = 5;

	//private GravityController player;

	void Awake() {
		//player = GameObject.FindGameObjectWithTag("Player").GetComponent<GravityController>();
	}

	void Start () {
		//Initialize object pools
		foreach (ObjectInitialization init in initializationArray) {
			PooledObject.InitializePool(init.type, init.prefab, init.maxObjects);
		}
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
		float range = 25;
		Vector2 atomSpeed = new Vector2(-5f,0);
		Vector2 meteorSpeed = new Vector2(-10f,-10f);

		while (true) {
			yield return new WaitForSeconds(2f);
			
			PooledObject.Spawn(BodyType.Atom).GetComponent<Body>().Initialize(new Vector3(range,Random.Range(-4f,4f),0), Quaternion.identity, atomSpeed);

			for (int i = 0; i < TEMPspawnsPerLevel; i++) {
				for (int j = 0; j < TEMPspawnsPerFrame; j++) {
					PooledObject.Spawn(BodyType.Meteor).GetComponent<Body>().Initialize(new Vector3(Random.Range(-5f,5f) + range,range,0), Quaternion.identity, meteorSpeed);
					yield return new WaitForSeconds(.05f);
				}
				yield return new WaitForSeconds(.1f);
			}
		}
	}
}
