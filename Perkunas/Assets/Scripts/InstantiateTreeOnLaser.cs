using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantiateTreeOnLaser : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
        laser = Instantiate(laserPrefab);
        laserTransform = laser.transform;

        reticle = Instantiate(teleportReticlePrefab);
    }

    private SteamVR_TrackedObject trackedObj;
    public Object Tree;
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
    private bool shouldSpawnTree;

    private SteamVR_Controller.Device Controller
    {
        get { return SteamVR_Controller.Input((int)trackedObj.index); }
    }

    void Awake()
    {
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
    void Update()
    {
        if (Controller.GetPress(SteamVR_Controller.ButtonMask.Touchpad))
        {
            RaycastHit hit;

            if (Physics.Raycast(trackedObj.transform.position, transform.forward, out hit, 100, teleportMask)) {
                hitPoint = hit.point;
                ShowLaser(hit);
                reticle.SetActive(true);
                reticle.transform.position = hitPoint + teleportReticleOffset;
                shouldSpawnTree = true;
            }

        }
        else
        {
            laser.SetActive(false);
            reticle.SetActive(false);
        }

        if (Controller.GetPressUp(SteamVR_Controller.ButtonMask.Touchpad) && shouldSpawnTree)
        {
            SpawnTree();
        }
    }

    private void SpawnTree()
    {
        shouldSpawnTree = false;
        reticle.SetActive(false);
        Vector3 vec = hitPoint;
        vec.y = 1;
        Quaternion quat = new Quaternion();

        Instantiate(Tree, vec, quat);
    }
}
