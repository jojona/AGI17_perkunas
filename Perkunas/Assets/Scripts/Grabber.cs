using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grabber : MonoBehaviour {
	//information of what we will grab next
	LinkedList<Collision> colliders = new LinkedList<Collision>();
	GameObject grabbed = null;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		//we may or may not need a more sophisticated way of interacting with the controllers.
		//is there any reason we may want to react to a grab by anything other than the vive controllers?
		//if(Input.
	}


	//these two functions track what objects are currently colliding with the grabber
	void OnCollisionEnter (Collision col)
	{
		colliders.AddFirst (col);
	}

	void OnCollisionExit (Collision col) {
		colliders.Remove (col);
	}

	public void grab() {
		if (grabbed == null) {
			bool noGrab = true;
			//iterate through all colliders, use the first suitable
			foreach(Collision c in colliders) {
				Grabable g = c.gameObject.GetComponent<Grabable> ();
				if (g != null) {
					grabbed = c.gameObject;
					g.attach (this.gameObject);
					Debug.Log("grabbed " + grabbed.ToString();
					noGrab = false;
					break;
				}
			}
			if(noGrab)
				Debug.Log ("nothing to grab");
		} else {
			Debug.log("Tried to grab something while holding something");
		}
	}

	public void ungrab() {
		if (grabbed != null) {	
									Debug.Log("Ungrabbing " + grabbed.ToString();
			grabbed.GetComponent<Grabable> ().detach ();
			grabbed = null;

		} else {
			Debug.Log ("nothing to ungrab");
		}
	}
}
