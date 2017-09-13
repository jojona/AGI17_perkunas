using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour {

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

		// Touchpad location
		if (Controller.GetAxis () != Vector2.zero) {
			Debug.Log (gameObject.name + Controller.GetAxis ());
		}

		// Hair Trigger
		if (Controller.GetHairTriggerDown ()) {
			Debug.Log (gameObject.name + "Trigger Press");
		}
		if (Controller.GetHairTriggerUp()) {
			Debug.Log(gameObject.name + "Trigger Press");
		}

		// Grip button
		if (Controller.GetPressDown(SteamVR_Controller.ButtonMask.Grip))
		{
			Debug.Log(gameObject.name + " Grip Press");
		}
		if (Controller.GetPressUp(SteamVR_Controller.ButtonMask.Grip))
		{
			Debug.Log(gameObject.name + " Grip Release");
		}

	}
}
