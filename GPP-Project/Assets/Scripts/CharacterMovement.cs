using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{

    //JUMPING VARIABLES.
    [SerializeField]
    private Transform groundCheck;
    [SerializeField]
    private float groundCheckRadius = 0.1f;
    [SerializeField]
    private LayerMask walkableLayer;

    [SerializeField]
    private float jumpForce = 5.0f;

    void Start()
    {
        
    }

    void FixedUpdate()
    {
        if(IsGrounded())
        {
            if(Input.GetAxis("Jump") > 0)
            {
                JumpHandler();
            }
        }
    }

    private bool IsGrounded()
    {
        Collider[] hitColliders = Physics.OverlapSphere(groundCheck.position, groundCheckRadius, walkableLayer);

        //checks for each object with walkable layer and returns true.
        foreach(Collider hit in hitColliders)
        {
            if (hit)
            {
                return true;
            }
        }

        return false;
    }

    private void JumpHandler()
    {
        Rigidbody rb = gameObject.GetComponent<Rigidbody>();

        rb.AddForce(new Vector3(rb.velocity.x, jumpForce, rb.velocity.z));
    }

}
