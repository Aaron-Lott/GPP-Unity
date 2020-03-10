using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Respawner : MonoBehaviour
{
    [HideInInspector]
    public bool isActive = false;

    private Respawner[] respawners;

    private float boxSize = 10;

    private void Start()
    {
        respawners = FindObjectsOfType<Respawner>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject == PlayerController.instance.gameObject)
        {
            foreach(Respawner obj in respawners)
            {
                obj.isActive = false;
            }

            isActive = true;
        }
    }
}
