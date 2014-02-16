using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public abstract class PooledObject : MonoBehaviour {

	protected static Dictionary<Type, PooledObject[]> availablePool;

	protected static Transform pooledHeap;

	protected static List<PooledObject> _activeObjects;
	protected static PooledObject[] activeObjects;
	protected static bool refreshActiveObjects = false;
	
	public static Vector3 hiddenPosition = new Vector3(99999f,99999f,0);

	[HideInInspector]
	public Rigidbody2D r;
	[HideInInspector]
	public Transform t;
	[HideInInspector]
	public Renderer n;
	[HideInInspector]
	public GameObject g;
	
	public static void InitializePools<T>(GameObject objectPrefab, int maxPool) where T : PooledObject{
		if (!refreshActiveObjects) {
			availablePool = new Dictionary<Type, PooledObject[]>();
			_activeObjects = new List<PooledObject>(maxPool);
			pooledHeap = new GameObject().transform;
			refreshActiveObjects = true;
		}

		if (!availablePool.ContainsKey(typeof(T))) {
			PooledObject[] tempPool =  new PooledObject[maxPool];
			for (int i = 0; i < maxPool; i++) {
				tempPool[i] = (Instantiate(objectPrefab,hiddenPosition,Quaternion.identity) as GameObject).GetComponent<PooledObject>();
				tempPool[i].g.SetActive(false);
				tempPool[i].t.parent = pooledHeap;
			}
			availablePool.Add(typeof(T),tempPool);
		}
	}

	protected virtual void Awake() {
		r = rigidbody2D;
		t = transform;
		n = renderer;
		g = gameObject;
	}

	public static PooledObject Spawn<T>(Vector3 pos = default(Vector3), Quaternion rot = default(Quaternion), Vector2 velocity = default(Vector2)) where T : PooledObject {
		PooledObject[] currentPool = availablePool[typeof(T)];
		for (int i = 0; i < currentPool.Length; i++) {
			if (!currentPool[i].g.activeSelf) {
				currentPool[i].g.SetActive(true);
				currentPool[i].Initialize(pos, rot, velocity);
				refreshActiveObjects = true;
				return currentPool[i];
			}
		}
		return currentPool[0];
	}

	public void Reclaim() {
		Terminate ();
		g.SetActive(false);
		refreshActiveObjects = false;
		t.parent = pooledHeap;
	}

	protected abstract void Initialize(Vector3 pos, Quaternion rot, Vector2 velocity = default(Vector2));
	protected abstract void Terminate();
}
