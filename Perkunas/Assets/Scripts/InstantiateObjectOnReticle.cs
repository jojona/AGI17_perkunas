using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantiateObjectOnReticle: LaserPointer
{
   
	public List<GameObject> spawnAssets;
	public List<float> centerPosY;

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
		vec.y = centerPosY[index];

		GameObject spawnAsset = spawnAssets [index];

		Instantiate(spawnAsset, vec, spawnAsset.transform.rotation);
    }

	public override void increaseIndex ()
	{
		index = (index + 1) % spawnAssets.Count;
	}
}
