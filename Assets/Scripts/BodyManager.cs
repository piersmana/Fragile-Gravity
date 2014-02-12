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

	public GameObject atomPrefab;

	public GameObject atomExplosionPrefab;

	public int maxAtoms = 250;
	public int maxAtomExplosions = 50;

	public Vector3 hiddenPosition = new Vector3(99999f,99999f,0);

	public Atom[] availableAtoms;
	private AtomExplosionControl[] availableAtomExplosions;
	
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

		availableAtomExplosions = new AtomExplosionControl[maxAtomExplosions];
		for (int i = 0; i < maxAtomExplosions; i++) {
			availableAtomExplosions[i] = (Instantiate(atomExplosionPrefab,hiddenPosition,Quaternion.identity) as GameObject).GetComponent<AtomExplosionControl>();
			availableAtomExplosions[i].gameObject.SetActive(false);
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

	public void SpawnAtomExplosion(Vector3 pos) {
		for (int i = 0; i < maxAtomExplosions; i++) {
			if (!availableAtomExplosions[i].gameObject.activeSelf) {
				availableAtomExplosions[i].PlayExplosion(pos);
				break;
			}
		}
	}

	IEnumerator Level1Spawn() {
		float range = player.gravityDetectionRange + 10;
		Vector2 atomSpeed = new Vector2(-100f,0);
		
		SpawnAtom(new Vector3(5,0,0), Vector2.zero);
		SpawnAtom(new Vector3(-5,0,0), Vector2.zero);
		SpawnAtom(new Vector3(0,5,0), Vector2.zero);
		SpawnAtom(new Vector3(0,-5,0), Vector2.zero);
		SpawnAtom(new Vector3(3.54f,3.54f,0), Vector2.zero);
		SpawnAtom(new Vector3(3.54f,-3.54f,0), Vector2.zero);
		SpawnAtom(new Vector3(-3.54f,3.54f,0), Vector2.zero);
		SpawnAtom(new Vector3(-3.54f,-3.54f,0), Vector2.zero);
		
		yield return new WaitForSeconds(3f);
		
		for (int i = 0; i < TEMPspawnsPerLevel; i++) {
			for (int j = 0; j < TEMPspawnsPerFrame; j++) {
				SpawnAtom(new Vector3(range,Random.Range(-4f,4f),0), atomSpeed);
			}
			yield return new WaitForSeconds(.05f);
		}
		
		yield return new WaitForSeconds(3f);
		
		for (int i = 0; i < TEMPspawnsPerLevel; i++) {
			for (int j = 0; j < TEMPspawnsPerFrame; j++) {
				SpawnAtom(new Vector3(range,Random.Range(-4f,4f),0), atomSpeed);
			}
			yield return new WaitForSeconds(.05f);
		}
	}
}
