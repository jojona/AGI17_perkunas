using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class LaserPointer : MonoBehaviour {

	protected SteamVR_TrackedObject trackedObj;

	public GameObject laserPrefab;
	public GameObject teleportReticlePrefab;
	public Transform cameraRigTransform;
	public Transform headTransform;
	public Vector3 teleportReticleOffset;

	protected GameObject laser;
	protected Transform laserTransform;
	protected Vector3 hitPoint;
	public LayerMask teleportMask;


	protected GameObject reticle;

	protected SteamVR_Controller.Device Controller
	{
		get { return SteamVR_Controller.Input((int)trackedObj.index); }
	}


	// Use this for initialization
	void Start() {
		laser = Instantiate(laserPrefab);
		laserTransform = laser.transform;

		reticle = Instantiate(teleportReticlePrefab);
	}

	void Awake()
	{
		trackedObj = GetComponent<SteamVR_TrackedObject>();
	}

	protected void ShowLaser(RaycastHit hit)
	{
		laser.SetActive(true);
		laserTransform.position = Vector3.Lerp(trackedObj.transform.position, hitPoint, .5f);
		laserTransform.LookAt(hitPoint);
		laserTransform.localScale = new Vector3(laserTransform.localScale.x, laserTransform.localScale.y,
			hit.distance);
	}

	protected bool Raycast() {
		RaycastHit hit;

		if (Physics.Raycast (trackedObj.transform.position, transform.forward, out hit, 100, teleportMask)) {
			hitPoint = hit.point;
			ShowLaser (hit);
			reticle.SetActive (true);
			reticle.transform.position = hitPoint + teleportReticleOffset;
			return true;
		} else {
			reticle.SetActive (false);
			laser.SetActive (false);
			return false;
		}
	}

	public abstract void holdDown();
	public abstract void release();
}
