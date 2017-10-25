using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class setOnFire : MonoBehaviour {

	public GameObject child;
	private string tagString;

	// Use this for initialization
	void Start () {
		tagString = "Seed";
		child = transform.GetChild (3).gameObject;
		child.SetActive (true);
	}
	
	// Update is called once per frame
	public void changeFire() {
		child.SetActive (false);
		child = transform.GetChild (0).gameObject;
		child.SetActive(true); 
	}
}
