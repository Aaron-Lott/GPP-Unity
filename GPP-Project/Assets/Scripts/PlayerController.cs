using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public static PlayerController instance;

    //JUMPING VARIABLES.
    private bool isJumping = false;

    [HideInInspector]
    public bool canDoubleJump = false;

    private bool hasFlipped = false;

    [SerializeField]
    private Transform groundCheck0;
    [SerializeField]
    private Transform groundCheck1;
    [SerializeField]
    private float groundCheckRadius = 0.1f;
    [SerializeField]
    private LayerMask walkableLayer;

    private int extraJumpCount = 0;
    [SerializeField]
    private int ExtraJumps = 2;

    [Range(0.0f, 1.0f)]
    public float inAirSpeedMultiplier = 0.6f;
    

    [SerializeField]
    private float jumpForce = 500.0f;

    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 2.0f;

    //MOVEMENT VARIABLES    
    [SerializeField]
    private float movementSpeed = 6.0f;
    private float initialMovementSpeed;

    [Range(1.0f, 2.0f)]
    public float speedBoostMultiplier = 1.5f;

    private float powerUpDuration = 10.0f;
    private float startPowerUpTime;

    private Rigidbody rb;

    private Animator anim;

    [HideInInspector]
    public bool canForwardRoll = false;

    [SerializeField]
    private ParticleSystem speedBoostEffect;
    [SerializeField]
    private ParticleSystem doubleJumpEffect;
    [SerializeField]
    private ParticleSystem forwardRollEffect;

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

        initialMovementSpeed = movementSpeed;
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
            else if(!hasFlipped &&  canDoubleJump)
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
        Collider[] hitColliders = Physics.OverlapSphere(groundCheck0.position, groundCheckRadius, walkableLayer);
        //Collider[] hitColliders = Physics.OverlapCapsule(groundCheck0.position, groundCheck1.position, groundCheckRadius, walkableLayer);

        //checks for each object with walkable layer and returns true.
        foreach (Collider hit in hitColliders)
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

            //reduce extra jump count.
            if(extraJumpCount > 0)
            {
                extraJumpCount--;
            }
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
            if(canDoubleJump)
            {
                extraJumpCount = ExtraJumps;
            }
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
            rb.velocity = new Vector3((moveHorizontal * movementSpeed) * inAirSpeedMultiplier, rb.velocity.y, (moveVertical * movementSpeed) * inAirSpeedMultiplier);

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
                if((Input.GetKeyDown(KeyCode.LeftControl) || Input.GetKeyDown(KeyCode.Joystick1Button1)) && canForwardRoll)
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

    public void IncreaseMovementSpeed()
    {
        movementSpeed *= speedBoostMultiplier;
        speedBoostEffect.Play();
        anim.speed *= speedBoostMultiplier;
    }

    public void ResetMovementSpeed()
    {
        movementSpeed = initialMovementSpeed;
        speedBoostEffect.Stop();
        anim.speed /= speedBoostMultiplier;
    }

    public void CanDoubleJump()
    {
        canDoubleJump = true;
        doubleJumpEffect.Play();
    }

    public void ResetDoubleJump()
    {
        canDoubleJump = false;
        doubleJumpEffect.Stop();
    }

    public void CanForwardRoll()
    {
        canForwardRoll = true;
        forwardRollEffect.Play();
    }

    public void ResetForwardRoll()
    {
        canForwardRoll = false;
        forwardRollEffect.Stop();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(groundCheck0.position, groundCheckRadius);

        Gizmos.DrawWireSphere(groundCheck1.position, groundCheckRadius);
    }

    void FootR()
    {
        //function called on run animation event. ERRORS CAUSED WITHOUT IT.
    }

    void FootL()
    {
        //function called on run animation event. ERRORS CAUSED WITHOUT IT.
    }
}
