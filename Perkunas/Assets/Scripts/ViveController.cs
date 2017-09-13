using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViveController : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}

	private SteamVR_TrackedObject trackedObj;

	private SteamVR_Controller.Device Controller
	{
		get { return SteamVR_Controller.Input((int)trackedObj.index); }
	}

	void Awake() {
		trackedObj = GetComponent<SteamVR_TrackedObject> ();
	}

	// Update is called once per frame
	void Update () {

		// Controller inputs

		// 	Touchpad location
		if (Controller.GetAxis () != Vector2.zero) {
			Debug.Log (gameObject.name + Controller.GetAxis ());
		}

		// 	Hair Trigger
		if (Controller.GetHairTriggerDown ()) {
			Debug.Log (gameObject.name + "Trigger Press");
		}
		if (Controller.GetHairTriggerUp()) {
			Debug.Log(gameObject.name + "Trigger Press");
		}
		if (Controller.GetHairTrigger ()) {
			Debug.Log (gameObject.name + "Trigger get");
		}

		// 	Grip button
		if (Controller.GetPressDown(SteamVR_Controller.ButtonMask.Grip)) {
			Debug.Log(gameObject.name + " Grip Press");
			Grabber g = this.gameObject.getComponent<Grabber> ();
			if (g != null) {
				g.grab ();
			}
		}
		if (Controller.GetPressUp(SteamVR_Controller.ButtonMask.Grip)) {
			Debug.Log(gameObject.name + " Grip Release");
			Grabber g = this.gameObject.getComponent<Grabber> ();
			if (g != null) {
				g.ungrab ();
			}
		}

	}
}
