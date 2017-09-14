using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : LaserPointer {

	private bool shouldTeleport; 

	public override void holdDown() {
		if (Raycast ()) {
			shouldTeleport = true;
		} else {
			laser.SetActive(false);

			// Do a parabolic curve instead of straigt teleport line
			float angle = 90f - Vector3.Angle (Vector3.up, transform.forward);
			if (angle < 90 || angle > 0) {
				float force = 25f; // TODO distance to end of map

				float d = force * Mathf.Sin (angle); // TODO throwinglocation is the foot

				hitPoint = trackedObj.transform.position + d * (transform.forward - Vector3.Dot (transform.forward, Vector3.up) * Vector3.up);
				hitPoint.y = 0;
				reticle.SetActive (true);
				reticle.transform.position = hitPoint + teleportReticleOffset;

				// TODO Cap it to end of map

				// TODO Draw "Curve laser"

				// TODO if location ok
				shouldTeleport = true;
			}
		}
	}

	public override void release() {
		if (shouldTeleport) {
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
