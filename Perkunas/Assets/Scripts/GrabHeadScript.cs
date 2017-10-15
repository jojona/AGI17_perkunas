using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabHeadScript : MonoBehaviour {
	/* *****************************************************************
	 * TODO: make sure this actually works
	 * *****************************************************************/
	//what this script does: once the box is grabbed, scale user up or down compared to the height of the controller at the time of grabbing
	public Transform t; //t is the transform to scale
	public GameObject g;
	public double startHeight;
	public double newHeight;
	private double oldScale;
	private double originalScale;

	// Use this for initialization
	void Start () {
		originalScale = t.transform.localScale.y;
	}
	
	// Update is called once per frame
	void Update () {
		if (g != null) {
			//get the heght of the controller given the old scale
			double y = transform.position.y;//is this right?
			y = oldScale * y / transform.localScale.y;

			//calculate how much we need to increase/decrease scale in order to make sure we are at the new controller height
			double newScale = y / startHeight;

			//update the scale in the transform
			t.transform.localScale.x = newScale;//is this correct?
		}
	}

	public double getScale() {
		return t.transform.localScale.y / originalScale;//return how much we have scaled up since game start
		//this is useful since it gives oltehr parts of the program a change behavioud depend scale good.
		//for instance, increase area of terrain modification
		//grab trees when large anough
	}

	public override void attach(GameObject G) {
		g = G;
		startHeight = g.transform.position.y;
	}

	public override void detach(Vector3 vel, Vector3 ang) {
		g = null;
	}
}
