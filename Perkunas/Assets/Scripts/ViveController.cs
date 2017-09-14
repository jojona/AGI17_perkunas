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

		// 	Touchpad
		//		Location
		if (Controller.GetAxis () != Vector2.zero) {
			Debug.Log (gameObject.name + Controller.GetAxis ());
		}
		if (Controller.GetPress (SteamVR_Controller.ButtonMask.Touchpad)) {
			LaserPointer pointer = this.gameObject.GetComponent<LaserPointer> ();
			pointer.holdDown();
		}

		if (Controller.GetPressUp(SteamVR_Controller.ButtonMask.Touchpad))
		{
			LaserPointer pointer = this.gameObject.GetComponent<LaserPointer> ();
			pointer.release();
		}


		// 	Hair Trigger
		if (Controller.GetHairTriggerDown ()) {
			Debug.Log (gameObject.name + "Trigger Press");
			Grabber g = this.gameObject.GetComponent<Grabber> ();
			if (g != null) {
				g.grab ();
			}
		}
		if (Controller.GetHairTriggerUp()) {
			Debug.Log(gameObject.name + "Trigger Release");
			Grabber g = this.gameObject.GetComponent<Grabber> ();
			if (g != null) {
				g.ungrab (Controller.velocity, Controller.angularVelocity);
			}
		}
		if (Controller.GetHairTrigger ()) {
			Debug.Log (gameObject.name + "Trigger get");
		}

		// 	Grip button
		if (Controller.GetPressDown(SteamVR_Controller.ButtonMask.Grip)) {
			Debug.Log(gameObject.name + " Grip Press");

		}
		if (Controller.GetPressUp(SteamVR_Controller.ButtonMask.Grip)) {
			Debug.Log(gameObject.name + " Grip Release");
		}

	}
}
