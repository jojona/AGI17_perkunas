using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour {

	// Use this for initialization
	void Start () {
		laser = Instantiate(laserPrefab);
		laserTransform = laser.transform;

		reticle = Instantiate(teleportReticlePrefab);
	}

	private SteamVR_TrackedObject trackedObj;

	public GameObject laserPrefab;
	private GameObject laser;
	private Transform laserTransform;
	private Vector3 hitPoint;

	public Transform cameraRigTransform; 
	public GameObject teleportReticlePrefab;
	private GameObject reticle;
	public Transform headTransform; 
	public Vector3 teleportReticleOffset; 
	public LayerMask teleportMask; 
	private bool shouldTeleport; 

	private SteamVR_Controller.Device Controller {
		get { return SteamVR_Controller.Input((int)trackedObj.index); }
	}

	void Awake() {
		trackedObj = GetComponent<SteamVR_TrackedObject>();
	}

	private void ShowLaser(RaycastHit hit)
	{
		laser.SetActive(true);
		laserTransform.position = Vector3.Lerp(trackedObj.transform.position, hitPoint, .5f);
		laserTransform.LookAt(hitPoint); 
		laserTransform.localScale = new Vector3(laserTransform.localScale.x, laserTransform.localScale.y,
			hit.distance);
	}
	
	// Update is called once per frame
	void Update () {
		if (Controller.GetPress(SteamVR_Controller.ButtonMask.Touchpad)) {
			RaycastHit hit;

			if (Physics.Raycast (trackedObj.transform.position, transform.forward, out hit, 100, teleportMask)) {
				hitPoint = hit.point;
				ShowLaser (hit);
				reticle.SetActive (true);
				reticle.transform.position = hitPoint + teleportReticleOffset;
				shouldTeleport = true;
			} else {
				// Do a parabolic curve instead of straigt teleport line
				float angle = 90 - Vector3.Angle (Vector3.up, transform.forward);
				float force = 10f; // TODO distance to end of map

				float d = force * Mathf.Sin (angle);

				hitPoint = trackedObj.transform.position + d * (transform.forward - Vector3.Dot (transform.forward, Vector3.up) * Vector3.up);
				hitPoint.y = 0;
				reticle.SetActive (true);
				reticle.transform.position = hitPoint + teleportReticleOffset;

				// TODO Cap it to end of map

				// TODO Draw "Curve laser"

				// TODO if location ok
				shouldTeleport = true;
			}
		} else {
			laser.SetActive(false);
			reticle.SetActive(false);
		}

		if (Controller.GetPressUp(SteamVR_Controller.ButtonMask.Touchpad) && shouldTeleport) {
			TeleportPlayer();
		}
	}

	private void TeleportPlayer() {
		shouldTeleport = false;
		reticle.SetActive(false);
		Vector3 difference = cameraRigTransform.position - headTransform.position;
		difference.y = 0;
		cameraRigTransform.position = hitPoint + difference;
	}
}
