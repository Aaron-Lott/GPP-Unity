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

    private int extraJumpCount;
    [SerializeField]
    private int maxExtraJumpCount = 1;
    

    [SerializeField]
    private float jumpForce = 200.0f;
    [SerializeField]
    private float fallMultiplier = 2.5f;
    [SerializeField]
    private float lowJumpMultiplier = 2.0f;

    //MOVEMENT VARIABLES    
    [SerializeField]
    private float movementSpeed = 10.0f;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        extraJumpCount = maxExtraJumpCount;
    }

    void FixedUpdate()
    {
        if(IsGrounded() || extraJumpCount > 0)
        {
            if(Input.GetButtonDown("Jump"))
            {
                Jump(jumpForce);
            }
        }

        JumpImprover();

        MovementManager();

        Debug.Log(extraJumpCount);
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

    private void Jump(float force)
    {
        if(rb)
        {
            rb.velocity = new Vector3(rb.velocity.x, force, rb.velocity.z) * Time.deltaTime;
            extraJumpCount--;
        }
    }

    private void JumpImprover()
    {

        //improves quality of jump.
        if (rb)
        {
            if (rb.velocity.y < 0)
            {
                rb.velocity += Vector3.up * Physics.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
            }
            else if(rb.velocity.y > 0 && !Input.GetButton("Jump"))
            {
                rb.velocity += Vector3.up * Physics.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
            }
        }


        //resets player double jump.
        if(IsGrounded())
        {
            extraJumpCount = maxExtraJumpCount;
        }
    }

    private void MovementManager()
    {
        
        rb.velocity = new Vector3(movementSpeed * Input.GetAxisRaw("Horizontal"), rb.velocity.y, movementSpeed * Input.GetAxisRaw("Vertical"));     
    }

}
