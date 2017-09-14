using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flame : MonoBehaviour {


	private SteamVR_TrackedObject trackedObj;


	void Awake() {
		trackedObj = GetComponent<SteamVR_TrackedObject>();
	}


	private SteamVR_Controller.Device Controller {
		get { return SteamVR_Controller.Input((int)trackedObj.index); }
	}

	// Use this for initialization
	void Start () {
		GetComponent<ParticleSystem> ().Clear();
		GetComponent<ParticleSystem> ().Pause ();
	}
	
	// Update is called once per frame
	void Update () {
		if (false) {
			if (Controller.GetPress (SteamVR_Controller.ButtonMask.Grip)) {
				GetComponent<ParticleSystem> ().Play ();
			} 	
		}
	}
}
