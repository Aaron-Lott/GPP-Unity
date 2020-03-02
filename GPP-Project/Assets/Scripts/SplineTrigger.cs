using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplineTrigger : MonoBehaviour
{
    public GameObject player;
    public Transform playerTarget;

    public GameObject splineCamera;
    public GameObject followCamera;

    public Transform cameraTarget;
    private Vector3 initCameraTargetPos;

    public bool isEntrance = true;

    private float cameraSpeed = 5f;

    private bool inPosition = false;

    private void Awake()
    {
        player.GetComponent<PathCreation.Examples.PathFollower>().enabled = !isEntrance;
        splineCamera.SetActive(false);
    }

    private void Start()
    {
        initCameraTargetPos = cameraTarget.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        player.GetComponent<Rigidbody>().velocity = Vector3.zero;
        player.GetComponent<PlayerController>().enabled = !isEntrance;
        player.transform.rotation = Quaternion.Euler(0, 0, 0);
        player.GetComponent<PlayerController>().ResetAnim();
        player.GetComponent<PathCreation.Examples.PathFollower>().enabled = isEntrance;
        player.GetComponent<PathCreation.Examples.PathFollower>().SetDistanceTravelled(0);

        if(isEntrance)
        {
            SplineCameraActive(true);
            cameraTarget.parent = player.transform;

            StartCoroutine(StartSpline());
        }
        else
        {
            SplineCameraActive(false);
            cameraTarget.parent = null;
            cameraTarget.transform.position = initCameraTargetPos;
        }
    }

    public IEnumerator StartSpline()
    {
        splineCamera.transform.position = followCamera.transform.position;
        splineCamera.transform.rotation = followCamera.transform.rotation;

        Quaternion startRot = splineCamera.transform.rotation;
        float y = 0f;
        float startDistance = Vector3.Distance(splineCamera.transform.position, cameraTarget.position);

        while (Vector3.Distance(splineCamera.transform.position, cameraTarget.position) > 0.2f)
        {
            splineCamera.transform.position = Vector3.Lerp(splineCamera.transform.position, cameraTarget.position, y += (cameraSpeed * 0.0001f));

            float x = 1 - Vector3.Distance(cameraTarget.transform.position, cameraTarget.position) / startDistance;
            float lerp = 1 / (1 + Mathf.Exp((-12f * (x - 0.5f))));
            splineCamera.transform.rotation = Quaternion.Lerp(startRot, cameraTarget.transform.rotation, lerp);

            yield return null;
        }

        inPosition = true;
    }

    private void Update()
    {
        if(inPosition)
        {
            splineCamera.transform.position = Vector3.Lerp(splineCamera.transform.position, cameraTarget.position, 0.1f);
        }
    }

    private void SplineCameraActive(bool active)
    {
        splineCamera.SetActive(active);
        followCamera.SetActive(!active);
    }
}
