using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorSwitch : Damageable
{
    public CutsceneManager cutsceneManager;

    private bool coroutineFinished;

    private void Update()
    {
        if(health <= 0)
        {
            GetComponent<Animator>().SetTrigger("swing");
        }
    }

    public void StartCutscene()
    {
        if(!coroutineFinished)
        {
            cutsceneManager.StartCoroutine(cutsceneManager.StartCutscene());
            coroutineFinished = true;
        }
    }
}
