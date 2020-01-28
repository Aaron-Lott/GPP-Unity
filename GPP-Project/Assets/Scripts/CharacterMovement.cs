using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{

    public static CharacterMovement instance;

    //JUMPING VARIABLES.
    private bool isJumping = false;

    private bool hasFlipped = false;

    [SerializeField]
    private Transform groundCheck;
    [SerializeField]
    private float groundCheckRadius = 0.1f;
    [SerializeField]
    private LayerMask walkableLayer;

    private int extraJumpCount;
    [SerializeField]
    private int ExtraJumps = 1;

    private float slowAirMovement = 0.6f;
    

    [SerializeField]
    private float jumpForce = 200.0f;

    private float fallMultiplier = 2.5f;
    private float lowJumpMultiplier = 2.0f;

    //MOVEMENT VARIABLES    
    [SerializeField]
    private float movementSpeed = 10.0f;

    private Rigidbody rb;

    private Animator anim;

    private void Awake()
    {
        if(instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();

        extraJumpCount = ExtraJumps;
    }

    private void Update()
    {
        if(Input.GetButtonDown("Jump"))
        {
            isJumping = true;
        }
        else
        {
            isJumping = false;
        }
    }

    void FixedUpdate()
    {
        if(isJumping)
        {
            //reset landing boolean.

            anim.SetBool("isLanding", false);

            if (IsGrounded() || extraJumpCount > 0)
            {
                Jump(jumpForce);
            }

            if (IsGrounded())
            {

                //only set just animation on the first jump.
                anim.SetTrigger("jump");
            }
            else if(!hasFlipped)
            {
                //set extra jumps to a flip animation.
                anim.SetTrigger("flip");
                hasFlipped = true;
            }
        }

        JumpImprover();

        MovementManager();
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
            extraJumpCount = ExtraJumps;
        }
    }

    private void MovementManager()
    {

        float moveHorizontal = Input.GetAxisRaw("Horizontal");
        float moveVertical = Input.GetAxisRaw("Vertical");

        //deals with rotation.
        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);

        if(movement != Vector3.zero)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(movement), 0.2f);
        }

        if (IsGrounded())
        {
            rb.velocity = new Vector3(movementSpeed * moveHorizontal, rb.velocity.y, movementSpeed * moveVertical);

            //check if the player is about to hit the ground at negative velocity and play land animation if true.
            if (rb.velocity.y < -1.0)
            {
                anim.SetBool("isLanding", true);

                //reset flip boolean.

                hasFlipped = false;
            }
        }
        else
        {
            //slow player's movement down if in the air.
            rb.velocity = new Vector3((Input.GetAxisRaw("Horizontal") * movementSpeed) * slowAirMovement, rb.velocity.y, (Input.GetAxisRaw("Vertical") * movementSpeed) * slowAirMovement);

            //check if player is falling without having jumped.
            if(rb.velocity.y < -1.0)
            {
                //anim.SetTrigger("falling");
            }
        }

        //check if player is moving along horizontal axis and set run animation.
        if (rb.velocity.x != 0 || rb.velocity.z != 0)
        {
            if(IsGrounded())
            {
                anim.SetBool("isRunning", true);

                //roll foward if player is running and presses ctrl or button B on Xbox controller.
                if(Input.GetKeyDown(KeyCode.LeftControl) || Input.GetKeyDown(KeyCode.Joystick1Button1))
                {
                    anim.SetTrigger("roll");
                }
            }
        }
        else
        {
            anim.SetBool("isRunning", false);
        }
    }

    void FootR()
    {
        //function called on run animation event. ERRORS ARE CAUSED WITHOUT IT.
    }

    void FootL()
    {
        //function called on run animation event. ERRORS ARE CAUSED WITHOUT IT.
    }

}
