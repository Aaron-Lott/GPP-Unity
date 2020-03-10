using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnTrigger : MonoBehaviour
{
    private RespawnPanel respawnPanel;

    private Animator panelAnim;

    private void Start()
    {
        respawnPanel = FindObjectOfType<RespawnPanel>();
        panelAnim = respawnPanel.GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject == PlayerController.instance.gameObject && !respawnPanel.hasTriggered)
        {
            panelAnim.SetTrigger("blackout");
            respawnPanel.hasTriggered = true;
        }
    }
}
