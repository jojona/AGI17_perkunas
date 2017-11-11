using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : LaserPointer {

	private bool shouldTeleport; 

	// Draw the reticle and update the teleport location
	public override void holdDown() {
		if (Raycast ()) {
			// Raycast from superclass to get teleport location
			shouldTeleport = true;
		} else {
			// Do a parabolic curve instead of straigt teleport line
			laser.SetActive(false);
			float angle = 90f - Vector3.Angle (Vector3.up, transform.forward);
			if (angle < 90 && angle > 0) {
				float force = 25f;

				float d = force * Mathf.Sin (Mathf.PI*angle/180);

				hitPoint = trackedObj.transform.position + d * (transform.forward - Vector3.Dot (transform.forward, Vector3.up) * Vector3.up);
				hitPoint.y = 0;
				reticle.SetActive (true);

				float heightmapPosX = hitPoint.x / grabTerrain.terrainWidth * terrain.terrainData.heightmapWidth;
				float heightmapPosZ = hitPoint.z / grabTerrain.terrainWidth * terrain.terrainData.heightmapWidth;
				float y = terrain.terrainData.GetHeight ((int)heightmapPosX, (int)heightmapPosZ);
				hitPoint = hitPoint + new Vector3 (0, y, 0);
				reticle.transform.position = hitPoint + teleportReticleOffset;

				shouldTeleport = true;
			} else {
				reticle.SetActive (false);
			}
		}
	}

	// Relese the button
	public override void release() {
		if (shouldTeleport) {
			TeleportPlayer();
			reticle.SetActive(false);
			laser.SetActive (false);
		}
	}

	// Move the player to the new location
	private void TeleportPlayer() {
		shouldTeleport = false;
		Vector3 difference = cameraRigTransform.position - headTransform.position;
		difference.y = -0.1f;
		cameraRigTransform.position = hitPoint + difference;
	}

	// Unused
	public override void increaseIndex ()
	{
		index = 0;
	}
}
