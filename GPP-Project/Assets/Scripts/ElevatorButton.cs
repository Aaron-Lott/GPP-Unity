using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorButton : MonoBehaviour
{
    public CutsceneManager cutscene;

    private Animator anim;

    public bool IsButtonPressed { get; set; }

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<PlayerController>())
        {
            StartCoroutine(cutscene.StartCutscene());
            GetComponent<BoxCollider>().enabled = false;
        }
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
