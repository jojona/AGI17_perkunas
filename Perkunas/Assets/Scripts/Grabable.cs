using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grabable : MonoBehaviour {
	public GameObject attachedTo = null;
	// Use this for initialization
	void Start () {
		if (attachedTo != null) {
			this.gameObject.transform.parent = attachedTo.transform;
		}
	}
	
	// Update is called once per frame
	void Update () {

	}

	public void attach(GameObject g) {
		attachedTo = g;
		this.gameObject.transform.parent = g.transform;
	}

	public void detach() {
		attachedTo = null;
		this.gameObject.transform.parent = null;
	}
}
