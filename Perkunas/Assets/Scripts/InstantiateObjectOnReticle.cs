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
		}
	}

	public override void release() {
		if (shouldSpawnTree) {
			SpawnTree();
		}

	}

    private void SpawnTree() {
        shouldSpawnTree = false;
        reticle.SetActive(false);
		laser.SetActive(false);
        Vector3 vec = hitPoint;
        vec.y = 1; // TODO based on asset size
        Quaternion quat = new Quaternion();

		Instantiate(spawnAsset, vec, quat);
    }
}
