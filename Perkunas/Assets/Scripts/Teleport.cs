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

				// TODO Cap it to end of map after map scene is created
				shouldTeleport = true;
			} else {
				reticle.SetActive (false);
			}
		}
	}

	public override void release() {
		if (shouldTeleport) {
			TeleportPlayer();
			reticle.SetActive(false);
			laser.SetActive (false);
		}
	}

	private void TeleportPlayer() {
		shouldTeleport = false;
		Vector3 difference = cameraRigTransform.position - headTransform.position;
		difference.y = 0;
		cameraRigTransform.position = hitPoint + difference;
	}

	public override void increaseIndex ()
	{
		index = 0;
	}
}
