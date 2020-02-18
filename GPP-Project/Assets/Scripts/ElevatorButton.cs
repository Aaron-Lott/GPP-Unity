using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorButton : MonoBehaviour
{
    public CutsceneManager cutscene;

    private Animator anim;

    public GameObject buttonUI;

    public bool IsButtonPressed { get; set; }

    private void Start()
    {
        anim = GetComponent<Animator>();
        buttonUI.SetActive(false);
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.GetComponent<PlayerController>() &&Input.GetButtonDown("Action"))
        {
            StartCoroutine(cutscene.StartCutscene());
            buttonUI.SetActive(false);
            GetComponent<BoxCollider>().enabled = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        buttonUI.SetActive(true);
    }

    private void OnTriggerExit(Collider other)
    {
        buttonUI.SetActive(false);
    }



    public void StartButtonAnimation()
    {
        anim.SetTrigger("buttonPress");
    }

    public void SetButtonIsPressedTrue()
    {
        IsButtonPressed = true;
    }
}
