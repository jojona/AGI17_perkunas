using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class setOnFire : MonoBehaviour {

	private GameObject child;
	private string tagString;
	private bool live;
	private bool notFire;

	// Use this for initialization
	void Start () {
		tagString = "Seed";
		child = transform.GetChild (3).gameObject;
		live = true;
		notFire = true;
	}

	public bool isLive(){
		return live;
	}


	public bool isNotOnFire(){
		return notFire;
	}


		

	public void setDead(){
		live = false;
	}

	// Update is called once per frame
	public void changeFire() {
		//child.SetActive (false);
		child = transform.GetChild (0).gameObject;
		//child.SetActive(true); 
	}

	public GameObject getChild(){
		return child;
	}

	public void StartFire(){
		if (live) {
			child.SetActive (true);
			notFire = false;
			gameObject.GetComponent<TreeLife> ().setFire ();
			transform.GetChild (5).gameObject.GetComponent<propagationOfFire> ().propagationFire();
			transform.GetChild(5).gameObject.SetActive(false);
		}
	}

	public void removeFire(){
		child.SetActive (false);
		notFire = true;
		if (live) {
			transform.GetChild (5).gameObject.SetActive (true);
		}
	}
}
