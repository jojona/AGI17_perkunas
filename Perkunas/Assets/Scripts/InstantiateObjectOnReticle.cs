using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantiateObjectOnReticle: LaserPointer
{
   
	public List<GameObject> spawnAssets;
	public List<float> centerPosY;
	public List<GameObject> spawnIndicator;

    private bool shouldSpawnTree;

	private GameObject indicatorPrefab;

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
		Destroy(indicatorPrefab);
		index = (index + 1) % spawnAssets.Count;
		indicatorPrefab = Instantiate(spawnIndicator[index], new Vector3(0, centerPosY[index]+0.5f, 0) + hitPoint, spawnIndicator[index].transform.rotation) as GameObject;
		indicatorPrefab.transform.parent = reticle.transform.GetChild(1).transform;
		indicatorPrefab.transform.localScale *= 0.5f;

	}
}
