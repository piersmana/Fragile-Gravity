using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

	private static GameManager instance;

	public static GameManager Instance
	{
		get {return instance;}
	}

	public enum BodyTypes {Meteor, Atom, GravityCenter, GravityController, Unknown};

	//Temp variables to control test level spawning
	public int TEMPspawnsPerFrame = 2;
	public int TEMPspawnsPerLevel = 5;

	//Atoms
	public GameObject atomPrefab;
	
	public GameObject atomExplosionPrefab;
	
	public int maxAtoms = 250;
	public int maxAtomExplosions = 50;
	
	public Atom[] availableAtoms;
	private EffectControl[] availableAtomExplosions;

	//Meteors
	public GameObject meteorPrefab;
	
	public GameObject meteorExplosionPrefab;
	
	public int maxMeteors = 50;
	public int maxMeteorExplosions = 50;
	
	public Meteor[] availableMeteors;
	private EffectControl[] availableMeteorExplosions;

	//GravityCenters
	//TODO: GravityCenters

	public Vector3 hiddenPosition = new Vector3(99999f,99999f,0);

	private Transform bodyHeap;
	private Transform effectHeap;

	private Queue<Body> _activeBodies;
	private Body[] activeBodies;
	private bool refreshActiveBodies;

	private GravityController player;

	void Awake() {
		instance = this;
		player = GameObject.FindGameObjectWithTag("Player").GetComponent<GravityController>();

		bodyHeap = new GameObject().transform;
		bodyHeap.name = "Bodies";
		effectHeap = new GameObject().transform;
		effectHeap.name = "Effects";
		bodyHeap.parent = effectHeap.parent = this.transform;

		_activeBodies = new Queue<Body>();
		refreshActiveBodies = true;
	}

	void Start () {
		//Initialize object pools
		availableAtoms = new Atom[maxAtoms];
		for (int i = 0; i < maxAtoms; i++) {
			availableAtoms[i] = (Instantiate(atomPrefab,hiddenPosition,Quaternion.identity) as GameObject).GetComponent<Atom>();
			availableAtoms[i].g.SetActive(false);
			availableAtoms[i].t.parent = bodyHeap;
		}

		availableAtomExplosions = new EffectControl[maxAtomExplosions];
		for (int i = 0; i < maxAtomExplosions; i++) {
			availableAtomExplosions[i] = (Instantiate(atomExplosionPrefab,hiddenPosition,Quaternion.identity) as GameObject).GetComponent<EffectControl>();
			availableAtomExplosions[i].g.SetActive(false);
			availableAtomExplosions[i].t.parent = effectHeap;
		}

		availableMeteors = new Meteor[maxMeteors];
		for (int i = 0; i < maxMeteors; i++) {
			availableMeteors[i] = (Instantiate(meteorPrefab,hiddenPosition,Quaternion.identity) as GameObject).GetComponent<Meteor>();
			availableMeteors[i].g.SetActive(false);
			availableMeteors[i].t.parent = bodyHeap;
		}
		
		availableMeteorExplosions = new EffectControl[maxMeteorExplosions];
		for (int i = 0; i < maxMeteorExplosions; i++) {
			availableMeteorExplosions[i] = (Instantiate(meteorExplosionPrefab,hiddenPosition,Quaternion.identity) as GameObject).GetComponent<EffectControl>();
			availableMeteorExplosions[i].g.SetActive(false);
			availableMeteorExplosions[i].t.parent = effectHeap;
		}

		//Start level behaviour
		StartCoroutine(Level1Spawn());
	}
	
	public void SpawnBody(Vector3 pos, Vector3 force, BodyTypes bodytype) {
		Body[] bodyQueue;

		switch (bodytype) {
		case BodyTypes.Meteor:
			bodyQueue = availableMeteors;
			break;
		case BodyTypes.Atom:
			bodyQueue = availableAtoms;
			break;
		default:
			bodyQueue = availableMeteors;
			break;
		}

		for (int i = 0; i < bodyQueue.Length; i++) {
			if (!bodyQueue[i].g.activeSelf) {
				bodyQueue[i].g.SetActive(true);
				bodyQueue[i].InitializeBody(pos,force);
				refreshActiveBodies = true;
				break;
			}
		}
	}

	public void SpawnEffect(Vector3 pos, Quaternion rot, BodyTypes bodytype) {
		EffectControl[] effectQueue;
		
		switch (bodytype) {
		case BodyTypes.Meteor:
			effectQueue = availableMeteorExplosions;
			break;
		case BodyTypes.Atom:
			effectQueue = availableAtomExplosions;
			break;
		default:
			effectQueue = availableMeteorExplosions;
			break;
		}

		for (int i = 0; i < effectQueue.Length; i++) {
			if (!effectQueue[i].g.activeSelf) {
				effectQueue[i].g.SetActive(true);
				effectQueue[i].InitializeEffect(pos, rot);
				break;
			}
		}
	}
	
	public void ReclaimBody(Body body) {
		body.g.SetActive(false);
		body.t.parent = bodyHeap;
		refreshActiveBodies = true;
	}

	public void ReclaimEffect(EffectControl effect) {
		effect.g.SetActive(false);
		effect.t.parent = effectHeap;
	}

	/*
	 * API to get the list of objects to apply forces to
	 * Caches an array of activeBodies that can be provided to multiple sources
	 * If a Body has gone active/inactive, refreshActiveBodies boolean gets set and activeBodies is rebuilt
	 */
	public Body[] GetActiveBodies() {
		if (refreshActiveBodies) {
			_activeBodies.Clear();
			for (int i = 0; i < availableAtoms.Length; i++) {
				if (availableAtoms[i].g.activeSelf)
					_activeBodies.Enqueue(availableAtoms[i]);
			}
			for (int i = 0; i < availableMeteors.Length; i++) {
				if (availableMeteors[i].g.activeSelf)
					_activeBodies.Enqueue(availableMeteors[i]);
			}
			activeBodies = _activeBodies.ToArray();
			refreshActiveBodies = false;
		}

		return activeBodies;
	}

	/*
	 * Test level spawning function
	 */ 
	IEnumerator Level1Spawn() {
		float range = player.gravityDetectionRange + 10;
		Vector2 atomSpeed = new Vector2(-200f,0);
		Vector2 meteorSpeed = new Vector2(-8f,-8f);

		while (true) {
			yield return new WaitForSeconds(2f);
			
			SpawnBody(new Vector3(range,Random.Range(-4f,4f),0), atomSpeed, BodyTypes.Atom);

			for (int i = 0; i < TEMPspawnsPerLevel; i++) {
				for (int j = 0; j < TEMPspawnsPerFrame; j++) {
					SpawnBody(new Vector3(Random.Range(-5f,5f) + range,range,0), meteorSpeed, BodyTypes.Meteor);
					yield return new WaitForSeconds(.05f);
				}
				yield return new WaitForSeconds(.1f);
			}
		}
	}
}
