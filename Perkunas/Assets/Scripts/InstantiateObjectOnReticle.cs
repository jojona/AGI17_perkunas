using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantiateObjectOnReticle: LaserPointer
{
   
	public GameObject spawnAsset;

    private bool shouldSpawnTree;


	public override void holdDown() {
		if (Raycast ()) {
			shouldSpawnTree = true;
		} else {
			reticle.SetActive (false);
			laser.SetActive (false);
		}
	}

	public override void release() {
		if (shouldSpawnTree) {
			SpawnTree();
			reticle.SetActive(false);
			laser.SetActive(false);
		}

	}

    private void SpawnTree() {
        shouldSpawnTree = false;
        Vector3 vec = hitPoint;
        vec.y = 1; // TODO based on asset size
        Quaternion quat = new Quaternion();

		Instantiate(spawnAsset, vec, quat);
    }
}
