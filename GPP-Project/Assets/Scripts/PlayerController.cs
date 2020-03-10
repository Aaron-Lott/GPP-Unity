using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public static PlayerController instance;

    public CameraFollow cameraFollow;

    [HideInInspector]
    public Rigidbody rb;

    private Animator anim;

    private CameraShake cameraShake;

    //JUMPING VARIABLES.
    private bool isJumping = false;

    [HideInInspector]
    public bool canDoubleJump = false;

    private bool hasFlipped = false;

    [HideInInspector]
    public bool isGrounded = true;

    [SerializeField]
    private Transform groundCheck;

    [SerializeField]
    private float groundCheckRadius = 0.2f;
    [SerializeField]
    private LayerMask walkableLayer;

    private int extraJumpCount = 0;
    [SerializeField]
    private int ExtraJumps = 2;

    [Range(1.0f, 2.0f)]
    public float doubleJumpModifier = 1.2f;

    [Range(0.0f, 1.0f)]
    public float inAirSpeedMultiplier = 0.75f;
    

    [SerializeField]
    private float jumpForce = 500.0f;

    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 2.0f;

    //MOVEMENT VARIABLES    
    [SerializeField]
    private float movementSpeed = 8.0f;
    private float initialMovementSpeed;

    [Range(1.0f, 2.0f)]
    public float speedBoostMultiplier = 1.5f;

    private float powerUpDuration = 10.0f;
    private float startPowerUpTime;

    [HideInInspector]
    public bool canForwardRoll = false;

    [HideInInspector]
    public bool hasSpeedBoost = false;

    public ParticleSystem warpEffect;

    [SerializeField]
    private ParticleSystem speedBoostEffect;
    [SerializeField]
    private ParticleSystem doubleJumpEffect;
    [SerializeField]
    private ParticleSystem forwardRollEffect;

    //ATTACK VARIABLES
    public int damageAmount = 1;

    public int hitForce = 1000;
    private float forceRadius = 3;

    private bool wieldingWeapon = false;

    public ParticleSystem damagePS;

    [SerializeField]
    private Transform hitCheckL;
    [SerializeField]
    private Transform hitCheckR;
    [SerializeField]
    private Transform hitCheckWeapon;


    [SerializeField]
    private float hitCheckRadius = .75f;

    public bool enableCameraShake = true;

    [HideInInspector]
    public float originalCapsuleHeight;
    [HideInInspector]
    public Vector3 originalCapsuleCenter;

    //POWERUP VARIABLES
    [HideInInspector]
    public DoubleJump doubleJump;
    [HideInInspector]
    public ForwardRoll forwardRoll;
    [HideInInspector]
    public SpeedBoost speedBoost;

    [HideInInspector]
    public Vector3 movement;


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
        rb.interpolation = RigidbodyInterpolation.Interpolate;

        originalCapsuleHeight = GetComponent<CapsuleCollider>().height;
        originalCapsuleCenter = GetComponent<CapsuleCollider>().center;

        if(GetComponent<PathCreation.Examples.PathFollower>())
        {
            GetComponent<PathCreation.Examples.PathFollower>().enabled = false;
        }
    }

    private void Update()
    {
        CheckIfJumping();
    }

    void FixedUpdate()
    {
        JumpManager();

        MovementManager();

        AttackAnimationManager();
    }

    public void CheckIfJumping()
    {
        if (Input.GetButtonDown("Jump"))
        {
            isJumping = true;
        }
        else
        {
            isJumping = false;
        }
    }

    public void JumpManager()
    {
        if (isJumping)
        {
            //reset landing boolean.
            //anim.SetBool("isLanding", false);


            if (isGrounded || extraJumpCount > 0)
            {
                Jump(jumpForce);
            }

            if (isGrounded)
            {
                //only set just animation on the first jump.
                anim.SetTrigger("jump");
            }
            else
            {

                if(!hasFlipped && canDoubleJump)
                {
                    //set extra jumps to a flip animation.

                    anim.SetTrigger("flip");
                    hasFlipped = true;

                    //add extra force on second jump
                    rb.velocity = rb.velocity * doubleJumpModifier;
                }
            }
        }

        if(!isGrounded)
        {
            //slow player's movement down if in the air.
            rb.velocity = new Vector3(movement.x * inAirSpeedMultiplier, rb.velocity.y, movement.z * inAirSpeedMultiplier);

            //check if player is falling without having jumped.
            if (rb.velocity.y < 0f)
            {
                anim.SetFloat("speed", 0);
                anim.SetBool("isFalling", true);
            }
        }
        else
        {
            //reset flip boolean.
            hasFlipped = false;
            anim.SetBool("isFalling", false);
        }

        anim.SetBool("isLanding", IsLanding());

        JumpImprover();
    }

    private void OnTriggerEnter(Collider other)
    {
        isGrounded = true;     
    }

    private void OnTriggerStay(Collider other)
    {
        isGrounded = true;
    }


    private void OnTriggerExit(Collider other)
    {
        isGrounded = false;
    }



    /*public bool IsGrounded()
    {

        Collider[] hitColliders = Physics.OverlapSphere(groundCheck.position, groundCheckRadius, walkableLayer);

        //checks for each object with walkable layer and returns true.
        foreach (Collider hit in hitColliders)
        {
            if (hit)
            {
                return true;
            }
        }

        return false;
    }*/

    private bool IsLanding()
    {
        Collider[] hitColliders = Physics.OverlapSphere(groundCheck.position, groundCheckRadius * 2, walkableLayer);

        foreach (Collider hit in hitColliders)
        {
            if (hit && rb.velocity.y < 0)
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
        if(isGrounded)
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

        Vector3 fromCameraToMe = transform.position - cameraFollow.transform.position;
        fromCameraToMe.y = 0;

        fromCameraToMe.Normalize();

        //deals with rotation.
        movement =  (fromCameraToMe * moveVertical + cameraFollow.transform.right * moveHorizontal) * movementSpeed;

        if (movement != Vector3.zero)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(movement), 0.2f);
        }

        if (isGrounded)
        {

            anim.SetFloat("speed", Mathf.Round(rb.velocity.magnitude));

            rb.velocity = new Vector3(movement.x, rb.velocity.y, movement.z);
        }

        //check if player is moving along horizontal axis.
        if (rb.velocity.x != 0 || rb.velocity.z != 0)
        {
            if(isGrounded)
            {
                //check for input / if the player can roll.
                if (Input.GetButtonDown("Roll") && canForwardRoll)
                {
                    anim.SetTrigger("roll");
                }
            }

            if(hasSpeedBoost)
            {
                warpEffect.Play();
                speedBoostEffect.Play();
            }
        }
        else
        {
            warpEffect.Stop();
            speedBoostEffect.Stop();
        }
    }
    private void AttackAnimationManager()
    {

        if (Input.GetButtonDown("AttackL") && anim.GetLayerWeight(1) == 0 && anim.GetLayerWeight(3) == 0)
        {
            int randAnim = Random.Range(1, 7);
            anim.SetTrigger("attack");

            if (!wieldingWeapon)
            {
                //select random attack from blend tree and set blend parameter.
                anim.SetFloat("Blend", randAnim);
                anim.SetLayerWeight(1, 1.0f);
                StartCoroutine(AttackingUnarmed());
            }
            else
            {
                //select random attack from blend tree and set blend parameter.
                anim.SetFloat("2ArmedBlend", randAnim);
                anim.SetLayerWeight(3, 1.0f);
                StartCoroutine(Attacking2Armed());
            }
        }
        else if (Input.GetButtonDown("WieldWeapon"))
        {
            wieldingWeapon = !wieldingWeapon;
            int weight = 0;

            if(wieldingWeapon)
            {
                weight = 1;
            }
            else
            {
                weight = 0;
            }

            anim.SetLayerWeight(2, weight);
        }

        
    }

    IEnumerator AttackingUnarmed()
    {
        yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(1).length);
        anim.SetLayerWeight(1, 0.0f);
    }

    IEnumerator Attacking2Armed()
    {
        yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(2).length);
        anim.SetLayerWeight(3, 0.0f);
    }

    void HitL()
    {
        CameraShake();

        Collider[] hitColliders = Physics.OverlapSphere(hitCheckL.position, hitCheckRadius);

        foreach (Collider hit in hitColliders)
        {
            if (hit)
            {
                if (hit.GetComponent<Damageable>())
                {
                    hit.GetComponent<Damageable>().TakeDamage(damageAmount, hit.transform.position);
                }

                if (hit.GetComponent<Rigidbody>() && hit.gameObject != gameObject)
                {
                    ApplyHitForce(hit);
                }
            }
        }
    }

    void HitR()
    {
        CameraShake();

        Collider[] hitColliders = Physics.OverlapSphere(hitCheckR.position, hitCheckRadius);

        foreach (Collider hit in hitColliders)
        {
            if (hit)
            {
                if (hit.GetComponent<Damageable>())
                {
                    hit.GetComponent<Damageable>().TakeDamage(damageAmount, hitCheckL.transform.position);                  
                }

                if(hit.GetComponent<Rigidbody>() && hit.gameObject != gameObject)
                {
                    ApplyHitForce(hit);
                }
            }
        }
    }

    void HitWeapon()
    {
        CameraShake();

        /*Collider[] hitColliders = Physics.OverlapSphere(hitCheckWeapon.position, hitCheckRadius);

        foreach (Collider hit in hitColliders)
        {
            if (hit)
            {
                if (hit.GetComponent<Damageable>())
                {
                    hit.GetComponent<Damageable>().TakeDamage(damageAmount, hitCheckL.transform.position);
                }

                if (hit.GetComponent<Rigidbody>() && hit.gameObject != gameObject)
                {
                    ApplyHitForce(hit);
                }
            }
        }*/
    }

    void StartHit()
    {
        cameraShake = Camera.main.GetComponent<CameraShake>();
        cameraShake.StartCoroutine(cameraShake.ZoomOut(50, 70));
    }

    void StartRoll()
    {
        cameraShake = Camera.main.GetComponent<CameraShake>();
        cameraShake.StartCoroutine(cameraShake.ZoomOut(50, 70, 0.5f));
    }

    void CameraShake()
    {
        if (enableCameraShake && Camera.main)
        {
            cameraShake = Camera.main.GetComponent<CameraShake>();
            cameraShake.StartCoroutine(cameraShake.Shake(0.1f, 0.3f));
        }
    }

    void ApplyHitForce(Collider hit)
    {
        hit.GetComponent<Rigidbody>().AddForce((hit.transform.position - transform.position) * hitForce);
    }

    public void FreezePlayer(bool freeze)
    {
        if(freeze)
        {
            rb.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotation;

            if(anim.isInitialized)
            {
                anim.Rebind();
            }
        }
        else
        {
            rb.constraints &= ~RigidbodyConstraints.FreezePosition;
        }
    }

    public void ResetAnim()
    {
        if (anim.isInitialized)
        {
            anim.Rebind();
        }
    }


    public void ApplySpeedBoost()
    {
        hasSpeedBoost = true;
        movementSpeed *= speedBoostMultiplier;
        anim.speed *= speedBoostMultiplier;
    }

    public void ResetSpeedBoost()
    {
        hasSpeedBoost = false;
        movementSpeed = initialMovementSpeed;
        warpEffect.Stop();
        speedBoostEffect.Stop();
        anim.speed /= speedBoostMultiplier;
    }

    public void CanDoubleJump()
    {
        canDoubleJump = true;
    }

    public void ResetDoubleJump()
    {
        canDoubleJump = false;
        doubleJumpEffect.Stop();
    }

    public void CanForwardRoll()
    {
        canForwardRoll = true;
    }

    public void ResetForwardRoll()
    {
        SetColliderSize(originalCapsuleCenter.y, originalCapsuleHeight);
        canForwardRoll = false;
        forwardRollEffect.Stop();
    }

    public void PlayDoubleJumpEffect()
    {
        doubleJumpEffect.Play();
    }

    public void PlayForwardRollEffect()
    {
        forwardRollEffect.Play();
    }

    public void StartButtonAnimation()
    {
        FindObjectOfType<ElevatorButton>().StartButtonAnimation();
    }

    public void SetColliderSize(float center, float height)
    {
        GetComponent<CapsuleCollider>().center = new Vector3(0, center, 0);
        GetComponent<CapsuleCollider>().height = height;
    }

    public void FollowerGrounded()
    {
        if(isGrounded && rb.velocity.y < 0)
        {
            rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
        }
    }

    private void OnDrawGizmos()
    {
       Gizmos.color = Color.red;
        //Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        Gizmos.DrawWireSphere(hitCheckR.position, hitCheckRadius);
        Gizmos.DrawWireSphere(hitCheckL.position, hitCheckRadius);
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
