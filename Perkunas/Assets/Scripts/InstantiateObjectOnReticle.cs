using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This class will spawn object based on where the user points the laser.
public class InstantiateObjectOnReticle: LaserPointer
{
   	
   	// List of prefabs to spawn
	public List<GameObject> spawnAssets;

	// List of height offset for each prefab
	public List<float> centerPosY;

	// List of prefabs to show above the reticle for each object
	public List<GameObject> spawnIndicator;

    private bool shouldSpawnTree;

	private GameObject indicatorPrefab;

	// Update the laser position based on superclass
	public override void holdDown() {
		if (Raycast ()) {
			shouldSpawnTree = true;
		} else {
			reticle.SetActive (false);
			laser.SetActive (false);
		}
	}

	// Call spawn function and remove laser and reticle
	public override void release() {
		if (shouldSpawnTree) {
			SpawnTree();
			reticle.SetActive(false);
			laser.SetActive(false);
		}

	}

	// Spawn the object
    private void SpawnTree() {
        shouldSpawnTree = false;
        Vector3 vec = hitPoint;
		vec.y += centerPosY[index];

		GameObject spawnAsset = spawnAssets [index];

		Instantiate(spawnAsset, vec, spawnAsset.transform.rotation);
    }

	public override void increaseIndex ()
	{
		Destroy(indicatorPrefab);
		index = (index + 1) % spawnAssets.Count;
		indicatorPrefab = Instantiate(spawnIndicator[index], new Vector3(0, centerPosY[index]+0.5f, 0) + hitPoint, spawnIndicator[index].transform.rotation) as GameObject;
		indicatorPrefab.transform.parent = reticle.transform.GetChild(1).transform;
		indicatorPrefab.transform.localScale *= 0.5f;

	}
}
