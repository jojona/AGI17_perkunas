using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This class handles button input from the Vive controllers
public class ViveController : MonoBehaviour {

	// Use this for initialization
	void Start () {
		GetComponent<InstantiateObjectOnReticle> ().increaseIndex ();
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

		// 	Touchpad - Spawn objects or teleport depending on the "LaserPointer" subclass on the controller
		//		Location
		if (Controller.GetAxis () != Vector2.zero) {
			//Debug.Log (gameObject.name + Controller.GetAxis ());
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


		// 	Hair Trigger - Grabs objects with the grabbable script
		if (Controller.GetHairTriggerDown ()) {
			//Debug.Log (gameObject.name + "Trigger Press");
			Grabber g = this.gameObject.GetComponent<Grabber> ();
			if (g != null) {
				g.grab ();
			}
		}
		if (Controller.GetHairTriggerUp()) {
			//Debug.Log(gameObject.name + "Trigger Release");
			Grabber g = this.gameObject.GetComponent<Grabber> ();
			if (g != null) {
				g.ungrab (Controller.velocity*7, Controller.angularVelocity);
			}
		}
		if (Controller.GetHairTrigger ()) {
			//Debug.Log (gameObject.name + "Trigger get");
		}

		// 	Grip button - Increase the index for the controller, this changes the object to spawn on the spawnobject controller
		if (Controller.GetPressDown(SteamVR_Controller.ButtonMask.Grip)) {
			//Debug.Log(gameObject.name + " Grip Press");

			GetComponent<InstantiateObjectOnReticle> ().increaseIndex ();

		}
		if (Controller.GetPressUp(SteamVR_Controller.ButtonMask.Grip)) {
			//Debug.Log(gameObject.name + " Grip Release");
		}

	}
}
