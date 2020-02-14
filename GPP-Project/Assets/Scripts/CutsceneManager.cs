using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneManager : MonoBehaviour
{
    public GameObject followCamera;
    public GameObject cutsceneCamera;

    public Transform focus;

    public Transform playerTarget;

    public Transform destination;

    private Doors doorScript;

    private void Awake()
    {
        if(focus.GetComponent<Doors>())
        {
            doorScript = focus.GetComponent<Doors>();
        }

        cutsceneCamera.SetActive(false);
    }

    public IEnumerator StartCutscene()
    {

        //reset door animation bool.
        doorScript.DoorAnimFinishedFalse();

        //deactivate player script and freeze player.
        FindObjectOfType<PlayerController>().FreezePlayer(true);
        FindObjectOfType<PlayerController>().enabled = false;

        cutsceneCamera.SetActive(true);

        cutsceneCamera.transform.position = followCamera.transform.position;
        cutsceneCamera.transform.rotation = followCamera.transform.rotation;

        followCamera.SetActive(false);

        while (Vector3.Distance(cutsceneCamera.transform.position, destination.position) > 0.2f)
        {
            cutsceneCamera.transform.position = Vector3.Slerp(cutsceneCamera.transform.position, destination.position, 0.02f);
            cutsceneCamera.transform.LookAt(focus.position);
            yield return null;
        }


        //deal with door animation.
        doorScript.SetDoorAnimator();

        while(!doorScript.GetDoorAnimFinished())
        {
            yield return null;
        }

        while (Vector3.Distance(cutsceneCamera.transform.position, followCamera.transform.position) > 0.2f)
        {
            cutsceneCamera.transform.position = Vector3.Slerp(cutsceneCamera.transform.position, followCamera.transform.position, 0.02f);
            cutsceneCamera.transform.LookAt(playerTarget.position);
            yield return null;
        }

        ResetCameras();
    }

    public void ResetCameras()
    {
        //activate player script and unfreeze player.
        FindObjectOfType<PlayerController>().enabled = true;
        FindObjectOfType<PlayerController>().FreezePlayer(false);


        followCamera.SetActive(true);
        cutsceneCamera.SetActive(false);
    }
}
