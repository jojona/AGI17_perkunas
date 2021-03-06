﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabHeadScript : Grabable {
	/* *****************************************************************
	* TODO: make sure this actually works
	* *****************************************************************/
	//what this script does: once the box is grabbed, scale user up or down compared to the height of the controller at the time of grabbing
	public Transform t; //t is the transform to scale
	private GameObject g;
	private float startHeight;
	private float oldScale;
	private float originalScale;

	// Use this for initialization
	void Start () {
		originalScale = t.localScale.y;
	}

	// Update is called once per frame
	void Update () {
		if (g != null) {
			//get the heght of the controller given the old scale
			float y = g.transform.position.y * oldScale / t.localScale.y;//is this right?
			y = y / startHeight;

			//calculate how much we need to increase/decrease scale in order to make sure we are at the new controller height
			float newScale = oldScale + (y * oldScale - oldScale) * 3.0f;
			newScale = newScale < 3f ? 3f : newScale;
			newScale = newScale > 11.5f ? 11.5f : newScale;			

			//update the scale in the transform
			t.localScale = new Vector3(newScale,newScale,newScale);//is this correct?
		}
	}

	public float getScale() {
		return t.localScale.y / originalScale;//return how much we have scaled up since game start
		//this is useful since it gives other parts of the program a change behavioud depend scale good.
		//for instance, increase area of terrain modification
		//grab trees when large anough
	}

	//attach head to controller
	public override void attach(GameObject G) {
		g = G;
		startHeight = g.transform.position.y;
		oldScale = t.localScale.y;
		Debug.Log("Grabbed the head at height " + t.position.y);
   	}

	//detach head from controller
	public override void detach(GameObject G, Vector3 vel, Vector3 ang) {
		g = null;
       	Debug.Log("released the head at height " + t.position.y);
	}
}