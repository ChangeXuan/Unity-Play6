using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvatarSys : MonoBehaviour {

	private GameObject girlSource;
	private Transform girlSourceTrans;
	private GameObject girlTarger;
	private Dictionary<string,Dictionary<string,SkinnedMeshRenderer>> girlData = new Dictionary<string, Dictionary<string, SkinnedMeshRenderer>>();
	private Transform[] girlHips;
	private Dictionary<string,SkinnedMeshRenderer> girlSmr = new Dictionary<string, SkinnedMeshRenderer>();
	private string[,] girlStr = new string[,]{ { "eyes","1" }, { "hair","1" }, { "top","1" }, { "pants","1" }, { "shoes","1" }, { "face","1" } };

	private GameObject boySource;
	private Transform boySourceTrans;
	private GameObject boyTarger;
	private Dictionary<string,Dictionary<string,SkinnedMeshRenderer>> boyData = new Dictionary<string, Dictionary<string, SkinnedMeshRenderer>>();
	private Transform[] boyHips;
	private Dictionary<string,SkinnedMeshRenderer> boySmr = new Dictionary<string, SkinnedMeshRenderer>();
	private string[,] boyStr = new string[,]{ { "eyes","1" }, { "hair","1" }, { "top","1" }, { "pants","1" }, { "shoes","1" }, { "face","1" } };

	private bool nowPeople = false;
	// Use this for initialization
	void Start () {
		initGirl ();
		initBoy ();
	}

	void initGirl() {
		InstantiateGirl ();
		saveData (girlSourceTrans,girlData,girlTarger,girlSmr);
		initGirlAvatar ();
	}

	void initBoy() {
		InstantiateBoy ();
		saveData (boySourceTrans, boyData, boyTarger, boySmr);
		initBoyAvatar ();
		boyTarger.SetActive (false);
	}

	void Update() {
//		if (Input.GetMouseButtonDown (0)) {
//			int num = Random.Range (1, 7);
//			changeMesh ("top", num.ToString());
//		}
		if (Input.GetMouseButtonDown (1)) {
			nowPeople = !nowPeople;
			boyTarger.SetActive (nowPeople);
			girlTarger.SetActive (!nowPeople);
		}
		if (Input.GetMouseButtonDown (2)) {
			int num = Random.Range (1, 7);
			if (nowPeople) {
				changeMesh ("top", num.ToString (), boyData, boyHips, boySmr);
			} else {
				changeMesh ("hair", num.ToString (),girlData,girlHips,girlSmr);
			}

		}

	}

	void InstantiateGirl() {
		girlSource = Instantiate (Resources.Load ("FemaleModel")) as GameObject;
		girlSourceTrans = girlSource.transform;
		girlSource.SetActive (false);

		girlTarger = Instantiate (Resources.Load ("FemaleTarger")) as GameObject;
		girlHips = girlTarger.GetComponentsInChildren<Transform> ();
	}

	void InstantiateBoy() {
		boySource = Instantiate (Resources.Load ("MaleModel")) as GameObject;
		boySourceTrans = boySource.transform;
		boySource.SetActive (false);

		boyTarger = Instantiate (Resources.Load ("MaleTarget")) as GameObject;
		boyHips = boyTarger.GetComponentsInChildren<Transform> ();
	}

	void saveData(Transform souceTran,Dictionary<string,Dictionary<string,SkinnedMeshRenderer>> data,
		GameObject target,Dictionary<string,SkinnedMeshRenderer> smr) {
		if (souceTran == null) {
			return;
		}
		SkinnedMeshRenderer[] parts = souceTran.GetComponentsInChildren<SkinnedMeshRenderer> ();
		foreach (var part in parts) {
			string[] names = part.name.Split ('-');
			if (!data.ContainsKey (names [0])) {
				GameObject partGo = new GameObject ();
				partGo.name = names[0];
				partGo.transform.parent = target.transform;

				smr.Add (names [0], partGo.AddComponent<SkinnedMeshRenderer>());
				data.Add (names [0], new Dictionary<string, SkinnedMeshRenderer> ());
			}
			data [names [0]].Add (names [1], part);
		}
	}

	void changeMesh(string part,string num,Dictionary<string,Dictionary<string,SkinnedMeshRenderer>> data,
		Transform[] hips,Dictionary<string,SkinnedMeshRenderer> smr) {
		SkinnedMeshRenderer skm = data [part] [num];
		List<Transform> bones = new List<Transform> ();
		foreach (var trans in skm.bones) {
			foreach (var bone in hips) {
				if (bone.name == trans.name) {
					bones.Add (bone);
					break;
				}	
			}
		}
		smr [part].bones = bones.ToArray();
		smr [part].materials = skm.materials;
		smr [part].sharedMesh = skm.sharedMesh;
	}

	void initGirlAvatar() {
		int len = girlStr.GetLength (0);//row
		for (int i = 0; i < len; i++) {
			changeMesh (girlStr [i, 0], girlStr [i, 1],girlData,girlHips,girlSmr);
		}
	}

	void initBoyAvatar() {
		int len = boyStr.GetLength (0);//row
		for (int i = 0; i < len; i++) {
			changeMesh (boyStr [i, 0], boyStr [i, 1],boyData,boyHips,boySmr);
		}
	}
}
