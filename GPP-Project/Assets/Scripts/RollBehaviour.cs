using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollBehaviour : StateMachineBehaviour
{
    private float originalCapsuleHeight;
    private Vector3 originalCapsuleCener;

    private Rigidbody rb;

    private float forceStrength = 250;


    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        rb = animator.GetComponent<Rigidbody>();

        originalCapsuleHeight = animator.GetComponent<CapsuleCollider>().height;
        originalCapsuleCener = animator.GetComponent<CapsuleCollider>().center;

        //set collider to be smaller during roll.
        animator.GetComponent<CapsuleCollider>().center = new Vector3(0, 0.5f, 0);
        animator.GetComponent<CapsuleCollider>().height = 1.0f;

    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //apply force during roll.
        Vector3 force = forceStrength * animator.transform.forward;
        rb.AddForce(force);
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //set collider to be the original size after roll.
        animator.GetComponent<CapsuleCollider>().center = originalCapsuleCener;
        animator.GetComponent<CapsuleCollider>().height = originalCapsuleHeight;
    }

}
