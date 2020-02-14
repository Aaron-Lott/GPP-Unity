using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Doors : MonoBehaviour
{
    //to be called during door animations.
    private bool animFinished;

    private int animNumber = 0;

    public void DoorAnimFinishedTrue()
    {
        animFinished = true;
    }

    public void DoorAnimFinishedFalse()
    {
        animFinished = false;
    }

    public bool GetDoorAnimFinished()
    {
        return animFinished;
    }

    public void SetDoorAnimator()
    {
        if(GetComponent<Animator>())
        {
            animNumber++;
            GetComponent<Animator>().SetInteger("doorState", animNumber);
        }
    }
}
