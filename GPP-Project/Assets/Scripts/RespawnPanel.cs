using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnPanel : MonoBehaviour
{
    [HideInInspector]
    public bool hasTriggered = false;

    private Animator playerAnim;

    private void Start()
    {
        playerAnim = PlayerController.instance.GetComponent<Animator>();
    }

    public void Respawn() //called in blackout panel animation.
    {
        Respawner[] respawners = FindObjectsOfType<Respawner>();

        foreach (Respawner obj in respawners)
        {
            if (obj.isActive)
            {
                PlayerController.instance.transform.position = obj.transform.position;
            }
        }

        playerAnim.SetTrigger("takeDamage");
        hasTriggered = false;
    }
}
