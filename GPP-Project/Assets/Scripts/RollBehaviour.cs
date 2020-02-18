using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollBehaviour : StateMachineBehaviour
{
    private Rigidbody rb;

    private float forceStrength = 250;

    private PlayerController player;


    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player = animator.GetComponent<PlayerController>();
        rb = animator.GetComponent<Rigidbody>();

        //set collider to be smaller during roll.
        player.SetColliderSize(0.5f, 1.0f);
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //apply force during roll.
        Vector3 force = forceStrength * animator.transform.forward;
        rb.AddForce(force);
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player.SetColliderSize(player.originalCapsuleCenter.y, player.originalCapsuleHeight);
    }

}
