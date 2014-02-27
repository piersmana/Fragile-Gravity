using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public enum BodyType {Meteor, Atom, GravityCenter};

public class PooledObject : MonoBehaviour {
	static Dictionary<BodyType, PooledObject[]> availablePool;

	static Transform pooledHeap;

	static List<GameObject> activeObjects;
	static bool initialized = false;
	
	public static Vector3 hiddenPosition = new Vector3(99999f,99999f,0);

	[HideInInspector]
	public Transform t;
	[HideInInspector]
	public GameObject g;
	
	void Awake() {
		t = transform;
		g = gameObject;
	}
	
	public static void InitializePool(BodyType type, GameObject objectPrefab, int maxPool){
		if (!initialized) {
			availablePool = new Dictionary<BodyType, PooledObject[]>();
			activeObjects = new List<GameObject>();
			pooledHeap = new GameObject().transform;
			initialized = true;
		}

		if (!availablePool.ContainsKey(type)) {
			PooledObject[] tempPool =  new PooledObject[maxPool];
			for (int i = 0; i < maxPool; i++) {
				tempPool[i] = (Instantiate(objectPrefab,hiddenPosition,Quaternion.identity) as GameObject).GetComponent<PooledObject>();
				tempPool[i].g.SetActive(false);
				tempPool[i].t.parent = pooledHeap;
			}
			availablePool.Add(type,tempPool);
		}
	}

	public static GameObject Spawn(BodyType type) {
		PooledObject[] currentPool = availablePool[type];
		GameObject currGO;
		for (int i = 0; i < currentPool.Length; i++) {
			currGO = currentPool[i].g;
			if (!currGO.activeSelf) {
				currGO.SetActive(true);
				activeObjects.Add(currGO);
				return currGO;
			}
		}
		return null;
	}

	public void Reclaim() {
		g.SetActive(false);
		activeObjects.Remove(this.g);
		t.parent = pooledHeap;
	}
}
