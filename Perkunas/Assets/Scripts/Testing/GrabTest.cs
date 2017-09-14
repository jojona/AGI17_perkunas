using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabTest : MonoBehaviour {

	public Vector3 velocity;
	public float timeTilMove = 0.0f;
	public float timeTilGrab = 0.0f;
	public float timeTilRelease = 100.0f;
	public float timeTilStop = 100.0f;
	public Vector3 releaseSpeed;
	public Vector3 releaseAngularVelocity;
	private bool hasGrabed;
	private bool done1 = false;
	private bool done2 = false;
	private float startTime;
	// Use this for initialization
	void Start () {
		startTime = UnityEngine.Time.fixedTime;
	}
	
	// Update is called once per frame
	void Update () {
		float dTime = UnityEngine.Time.fixedTime - startTime;
		if (!hasGrabed && dTime >= timeTilGrab) {
			Debug.Log ("test script grabbed");
			GetComponent<Grabber>().grab ();
			hasGrabed = true;
		}
		if (dTime >= timeTilMove && dTime < timeTilStop) {
			transform.Translate (velocity * UnityEngine.Time.deltaTime);
		} else if (dTime >= timeTilStop && !done2) {
			Debug.Log ("test script stopped after " + dTime + " seconds");
			done2 = true;
		}
		if (hasGrabed && !done1 && dTime > timeTilRelease) {
			GetComponent<Grabber>().ungrab (releaseSpeed, releaseAngularVelocity);
			done1 = true;
			Debug.Log ("test script released");
		}
	}
}
