using UnityEngine;
using System.Collections;

public class BodyManager : MonoBehaviour {

	private static BodyManager instance;

	public static BodyManager Instance
	{
		get {return instance;}
	}

	public int TEMPspawnsPerFrame = 2;
	public int TEMPspawnsPerLevel = 5;

	//Atoms
	public GameObject atomPrefab;
	
	public GameObject atomExplosionPrefab;
	
	public int maxAtoms = 250;
	public int maxAtomExplosions = 50;
	
	public Atom[] availableAtoms;
	private ExplosionControl[] availableAtomExplosions;

	//Meteors
	public GameObject meteorPrefab;
	
	public GameObject meteorExplosionPrefab;
	
	public int maxMeteors = 50;
	public int maxMeteorExplosions = 50;
	
	public Meteor[] availableMeteors;
	private ExplosionControl[] availableMeteorExplosions;

	public Vector3 hiddenPosition = new Vector3(99999f,99999f,0);
	
	private GravityController player;

	void Awake() {
		instance = this;
		player = GameObject.FindGameObjectWithTag("Player").GetComponent<GravityController>();
	}

	void Start () {
		availableAtoms = new Atom[maxAtoms];
		for (int i = 0; i < maxAtoms; i++) {
			availableAtoms[i] = (Instantiate(atomPrefab,hiddenPosition,Quaternion.identity) as GameObject).GetComponent<Atom>();
			availableAtoms[i].gameObject.SetActive(false);
		}
		
		availableAtomExplosions = new ExplosionControl[maxAtomExplosions];
		for (int i = 0; i < maxAtomExplosions; i++) {
			availableAtomExplosions[i] = (Instantiate(atomExplosionPrefab,hiddenPosition,Quaternion.identity) as GameObject).GetComponent<ExplosionControl>();
			availableAtomExplosions[i].gameObject.SetActive(false);
		}

		availableMeteors = new Meteor[maxMeteors];
		for (int i = 0; i < maxMeteors; i++) {
			availableMeteors[i] = (Instantiate(meteorPrefab,hiddenPosition,Quaternion.identity) as GameObject).GetComponent<Meteor>();
			availableMeteors[i].gameObject.SetActive(false);
		}
		
		availableMeteorExplosions = new ExplosionControl[maxMeteorExplosions];
		for (int i = 0; i < maxMeteorExplosions; i++) {
			availableMeteorExplosions[i] = (Instantiate(meteorExplosionPrefab,hiddenPosition,Quaternion.identity) as GameObject).GetComponent<ExplosionControl>();
			availableMeteorExplosions[i].gameObject.SetActive(false);
		}

		StartCoroutine(Level1Spawn());
	}
	
	public void SpawnAtom(Vector3 pos, Vector3 force) {
		for (int i = 0; i < maxAtoms; i++) {
			if (!availableAtoms[i].gameObject.activeSelf) {
				availableAtoms[i].EnableBody(pos,force);
				break;
			}
		}
	}

	public void SpawnMeteor(Vector3 pos, Vector3 force) {
		for (int i = 0; i < maxMeteors; i++) {
			if (!availableMeteors[i].gameObject.activeSelf) {
				availableMeteors[i].EnableBody(pos,force);
				break;
			}
		}
	}
	
	public void SpawnAtomExplosion(Vector3 pos) {
		for (int i = 0; i < maxAtomExplosions; i++) {
			if (!availableAtomExplosions[i].gameObject.activeSelf) {
				availableAtomExplosions[i].PlayExplosion(pos);
				break;
			}
		}
	}

	public void SpawnMeteorExplosion(Vector3 pos) {
		for (int i = 0; i < maxMeteorExplosions; i++) {
			if (!availableMeteorExplosions[i].gameObject.activeSelf) {
				availableMeteorExplosions[i].PlayExplosion(pos);
				break;
			}
		}
	}

	IEnumerator Level1Spawn() {
		float range = player.gravityDetectionRange + 10;
		Vector2 atomSpeed = new Vector2(-200f,0);
		Vector2 meteorSpeed = new Vector2(-.8f,-.8f);

		while (true) {
			yield return new WaitForSeconds(2f);
			
			SpawnAtom(new Vector3(range,Random.Range(-4f,4f),0), atomSpeed);

			for (int i = 0; i < TEMPspawnsPerLevel; i++) {
				for (int j = 0; j < TEMPspawnsPerFrame; j++) {
					SpawnMeteor(new Vector3(Random.Range(-4f,4f) + range/2,range/2,0), meteorSpeed);
					yield return new WaitForSeconds(.05f);
				}
				yield return new WaitForSeconds(.1f);
			}
		}
	}
}
