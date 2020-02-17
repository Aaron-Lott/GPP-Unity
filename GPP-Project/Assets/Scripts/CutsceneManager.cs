using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneManager : MonoBehaviour
{
    public GameObject followCamera;
    public GameObject cutsceneCamera;

    public Transform focus;

    public Transform playerTarget;
    private Animator playerAnim;

    public Transform cameraDestination;
    public Transform cameraDestination2;

    private Doors doorScript;

    private ElevatorButton elevatorScript;

    public Transform elevator;

    public enum cutsceneType{DOOR, ELEVATOR};

    public cutsceneType type;

    [Range(1.0f, 10.0f)]
    public float cameraSpeed = 5f;

    private void Awake()
    {
        if(focus.GetComponent<Doors>() && type == cutsceneType.DOOR)
        {
            doorScript = focus.GetComponent<Doors>();
        }
        else if(focus.GetComponent<ElevatorButton>() && type == cutsceneType.ELEVATOR)
        {
            elevatorScript = focus.GetComponent<ElevatorButton>();
        }

        cutsceneCamera.SetActive(false);

        playerAnim = playerTarget.GetComponent<Animator>();
    }

    public IEnumerator StartCutscene()
    {
        if(type == cutsceneType.DOOR)
        {
            //reset door animation bool.
            doorScript.DoorAnimFinishedFalse();
        }

        //deactivate player script and freeze player.

        FindObjectOfType<PlayerController>().FreezePlayer(true);
        FindObjectOfType<PlayerController>().enabled = false;

        cutsceneCamera.SetActive(true);

        cutsceneCamera.transform.position = followCamera.transform.position;
        cutsceneCamera.transform.rotation = followCamera.transform.rotation;

        followCamera.SetActive(false);

        Quaternion startRot = cutsceneCamera.transform.rotation;
        float y = 0f;
        float startDistance = Vector3.Distance(cutsceneCamera.transform.position, cameraDestination.position);

        while (Vector3.Distance(cutsceneCamera.transform.position, cameraDestination.position) > 0.2f)
        {
            cutsceneCamera.transform.position = Vector3.Lerp(cutsceneCamera.transform.position, cameraDestination.position, y += (cameraSpeed * 0.0001f));

            float x = 1 - Vector3.Distance(cutsceneCamera.transform.position, cameraDestination.position) / startDistance;
            float lerp =  1 / (1 + Mathf.Exp((-12f * (x - 0.5f))));
            cutsceneCamera.transform.rotation = Quaternion.Lerp(startRot, cameraDestination.transform.rotation, lerp);

            yield return null;
        }


        if(type == cutsceneType.DOOR)
        {
            //deal with door animation.
            doorScript.SetDoorAnimator();

            while (!doorScript.GetDoorAnimFinished())
            {
                yield return null;
            }
        }
        else
        { 
            Vector3 destination = new Vector3(focus.position.x, playerTarget.transform.position.y, focus.position.z - 1);
            while (Vector3.Distance(playerTarget.position, destination) > 0.2f)
            {
                playerTarget.LookAt(new Vector3(focus.position.x, playerTarget.position.y, focus.position.z));
                playerAnim.SetFloat("speed", 3);
                playerTarget.transform.position = Vector3.MoveTowards(playerTarget.transform.position, destination, 0.04f);
                yield return null;
            }

            playerAnim.SetFloat("speed", 0);
            playerAnim.SetTrigger("buttonPress");

            while(!elevatorScript.IsButtonPressed)
            {
                yield return null;
            }

            startRot = cutsceneCamera.transform.rotation;
            y = 0f;
            startDistance = Vector3.Distance(cutsceneCamera.transform.position, cameraDestination2.position);

            while (Vector3.Distance(cutsceneCamera.transform.position, cameraDestination2.transform.position) > 0.2f)
            {
                cutsceneCamera.transform.position = Vector3.Lerp(cutsceneCamera.transform.position, cameraDestination2.transform.position, y += (cameraSpeed * 0.0001f));

                float x = 1 - Vector3.Distance(cutsceneCamera.transform.position, cameraDestination2.transform.position) / startDistance;
                float lerp = 1 / (1 + Mathf.Exp((-12f * (x - 0.5f))));
                cutsceneCamera.transform.rotation = Quaternion.Lerp(startRot, cameraDestination2.transform.rotation, lerp);

                yield return null;
            }

            playerTarget.LookAt(new Vector3(elevator.position.x, playerTarget.position.y, elevator.position.z));


            destination = new Vector3(elevator.position.x, playerTarget.transform.position.y, elevator.position.z);

            while (Vector3.Distance(playerTarget.position, destination) > 0.2f)
            {
                playerAnim.SetFloat("speed", 3);
                playerTarget.transform.position = Vector3.MoveTowards(playerTarget.transform.position, destination, 0.05f);
                yield return null;
            }

            playerAnim.SetFloat("speed", 0);

            playerTarget.parent = elevator;

            destination = new Vector3(elevator.position.x, elevator.position.y - 7.25f, elevator.position.z);

            while (Vector3.Distance(elevator.position, destination) > 0.2f)
            {
                cutsceneCamera.transform.LookAt(elevator);
                elevator.transform.position = Vector3.MoveTowards(elevator.transform.position, destination, 0.05f);
                yield return null;
            }

            playerTarget.parent = null;
        }


        startRot = cutsceneCamera.transform.rotation;
        y = 0f;
        startDistance = Vector3.Distance(cutsceneCamera.transform.position, followCamera.transform.position);

        while (Vector3.Distance(cutsceneCamera.transform.position, followCamera.transform.position) > 0.2f)
        {
            cutsceneCamera.transform.position = Vector3.Lerp(cutsceneCamera.transform.position, followCamera.transform.position, y += (cameraSpeed * 0.0001f));

            float x = 1 - Vector3.Distance(cutsceneCamera.transform.position, followCamera.transform.position) / startDistance;
            float lerp = 1 / (1 + Mathf.Exp((-12f * (x - 0.5f))));
            cutsceneCamera.transform.rotation = Quaternion.Lerp(startRot, followCamera.transform.rotation, lerp);

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

    float GetLerpValue()
    {
        float startDistance = Vector3.Distance(cutsceneCamera.transform.position, cameraDestination.position);
        float x = 1 - Vector3.Distance(cutsceneCamera.transform.position, cameraDestination.position) / startDistance;
        return 1 / (1 + Mathf.Exp((-12f * (x - 0.5f))));
    }
}
