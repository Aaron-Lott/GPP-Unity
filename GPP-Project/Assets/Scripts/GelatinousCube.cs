using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GelatinousCube : Damageable
{
    public int cubeDeathCount = 2;

    private int maxForce = 6;
    private int minForce = 2;

    private bool shakeRoutineCalled = false;

    private float startScale;

    public Transform groundCheck;
    private float groundCheckRadius = 0.1f;
    public LayerMask walkableLayer;

    private int sizeDivider = 2;

    private float minSize = 1.8f;

    public bool randomPatrolOnDeath = false;

    private int newHealth;

    private Rigidbody rb;

    private float elapsedTime = 0;
    private float jumpInterval = 0;
    private int minJumpTime = 1;
    private int maxJumpTime = 4;

    private int damageAmount = 1;

    public Transform[] patrolPoints;

    [HideInInspector]
    public float engageDistance = 15;
    
    private enum EnemyStates {PATROLLING, COMBAT_ENGAGED};
    private EnemyStates states = EnemyStates.PATROLLING;

    public enum PatrolEnd {STOP, REVERSE, LOOP, UNORDERED};
    public PatrolEnd patrol = PatrolEnd.STOP;

    int patrolPointIndex = 0;
    private bool indexIncrease = false;
    private bool isMovingForward = true;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        startScale = transform.localScale.y;
        shakeRoutineCalled = false;

        newHealth = health - 1;

        jumpInterval = Random.Range(minJumpTime, maxJumpTime);
    }

    private void Update()
    {
        if (Vector3.Distance(transform.position, PlayerController.instance.transform.position) < engageDistance)
        {
            states = EnemyStates.COMBAT_ENGAGED;
        }
        else
        {
            states = EnemyStates.PATROLLING;
        }
    }

    private void FixedUpdate()
    {
        if (states == EnemyStates.PATROLLING)
        {
            Patrolling();
        }
        else if (states == EnemyStates.COMBAT_ENGAGED)
        {
            EngageCombat();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject == PlayerController.instance.gameObject)
        {
            PlayerController.instance.PlayerTakeDamage(damageAmount, new Vector3(collision.transform.position.x, collision.transform.position.y + 2, collision.transform.position.z));
        }
    }

    private bool IsGrounded()
    {
        Collider[] hitColliders = Physics.OverlapSphere(groundCheck.position, groundCheckRadius, walkableLayer);

        foreach (Collider hit in hitColliders)
        {
            if (hit)
            {
                return true;
            }
        }
        return false;
    }

    void EngageCombat()
    {
        if (IsGrounded())
        {
            elapsedTime += Time.deltaTime;

            if (elapsedTime > jumpInterval)
            {
                const int moveForce = 500;
                const int jumpForce = 300;
                Jump(PlayerController.instance.transform.position, moveForce, jumpForce, true);
            }
        }

        LookAtTarget(PlayerController.instance.transform.position);
    }

    void LookAtTarget(Vector3 target)
    {
        var lookPos = target - transform.position;
        lookPos.y = 0;

        if (lookPos != Vector3.zero)
        {
            var rotation = Quaternion.LookRotation(-lookPos);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 2);
        }
        
    }

    void Patrolling()
    {
        foreach(Transform point in patrolPoints)
        {
            point.parent = null;
        }

        if(patrolPoints.Length > patrolPointIndex)
        {
            LookAtTarget(patrolPoints[patrolPointIndex].position);
           
            if (IsGrounded())
            {
                elapsedTime += Time.deltaTime;

                if (elapsedTime > jumpInterval)
                {
                    switch (patrol)
                    {
                        case PatrolEnd.STOP:
                            break;

                        case PatrolEnd.LOOP:

                            if (!indexIncrease)
                            {
                                patrolPointIndex++;
                                indexIncrease = true;
                            }

                            if (patrolPointIndex == patrolPoints.Length)
                            {
                                patrolPointIndex = 0;
                            }

                            break;

                        case PatrolEnd.REVERSE:

                            if (!indexIncrease)
                            {
                                if(isMovingForward)
                                {
                                    patrolPointIndex++;
                                }
                                else
                                {
                                    patrolPointIndex--;
                                }

                                indexIncrease = true;
                            }

                            if (patrolPointIndex == patrolPoints.Length - 1)
                            {
                                isMovingForward = false;
                            }
                            else if(patrolPointIndex == 0)
                            {
                                isMovingForward = true;
                            }

                            break;

                        case PatrolEnd.UNORDERED:
                            if (!indexIncrease)
                            {
                                int randNum = Random.Range(0, 2) * 2 - 1;
                                patrolPointIndex += randNum;
                                indexIncrease = true;
                            }

                            if (patrolPointIndex == patrolPoints.Length)
                            {
                                patrolPointIndex = 0;
                            }

                            break;                                                  
                    }

                    if (patrolPointIndex > patrolPoints.Length - 1)
                    {
                        patrolPointIndex = 0;
                    }
                    else if (patrolPointIndex < 0)
                    {
                        patrolPointIndex = patrolPoints.Length - 1;
                    }

                    const int moveForce = 800;
                    const int jumpForce = 300;

                    Jump(patrolPoints[patrolPointIndex].position, moveForce, jumpForce, false);
                }
                }
            }
        
    }

    void Jump(Vector3 destination, int moveForce, int jumpForce, bool isRandom)
    {
        float newMoveForce = moveForce;
        float newJumpForce = jumpForce;

        Vector3 distToDestination = (destination - transform.position).normalized;

        if(isRandom)
        {
            newMoveForce = Random.Range(0.5f, 1.5f) * moveForce;
            newJumpForce = Random.Range(0.5f, 1.5f) * jumpForce;
        }

        rb.velocity = new Vector3(distToDestination.x * newMoveForce, newJumpForce, distToDestination.z * moveForce) * Time.deltaTime;
        jumpInterval = Random.Range(minJumpTime, maxJumpTime);

        if (!shakeRoutineCalled)
        {
            StartCoroutine(ShakeScale());
        }
    }

    public override void OnDamage()
    {
        if(!shakeRoutineCalled)
        {
            StartCoroutine(ShakeScale());
        }

        if(health <= 0)
        {
            //checks if cube is too small to split into more.
            if(transform.localScale.y > minSize)
            {
                for (int i = 0; i < cubeDeathCount; i++)
                {
                    GameObject newCube = Instantiate(gameObject, transform.position, Quaternion.identity);
                    newCube.transform.localScale = gameObject.transform.localScale / sizeDivider;

                    if(randomPatrolOnDeath)
                    {
                        newCube.GetComponent<GelatinousCube>().patrol = (PatrolEnd)Random.Range(0, 4);
                    }

                    newCube.GetComponent<GelatinousCube>().health = newHealth;

                    Rigidbody newRB = newCube.GetComponent<Rigidbody>();

                    newRB.mass = rb.mass / sizeDivider;

                    Vector3 multiplierForce = transform.position - PlayerController.instance.transform.position;

                    newRB.velocity = new Vector3(Random.Range(minForce, maxForce) * multiplierForce.x,
                        Random.Range(minForce, maxForce) * multiplierForce.y, Random.Range(minForce, maxForce) * multiplierForce.z);
                }
            }

            Destroy(gameObject);
        }
    }

    public IEnumerator ShakeScale()
    {
        shakeRoutineCalled = true;

        float minScale = startScale * 0.75f;
        float maxScale = startScale * 1.25f;
        Vector3 scaleVector;

        float t = 0;

        float lerpValue = 0.1f;
        float lerpAdd = lerpValue;

        for(int i = 0; i < 3; i++)
        {
            while (transform.localScale.y < maxScale)
            {
                scaleVector = new Vector3(minScale, maxScale, minScale);

                transform.localScale = Vector3.Lerp(transform.localScale, scaleVector, t += lerpValue);
                yield return null;
            }

            t = 0;

            lerpValue += lerpAdd;

            while (transform.localScale.y > minScale)
            {
                scaleVector = new Vector3(maxScale, minScale, maxScale);

                transform.localScale = Vector3.Lerp(transform.localScale, scaleVector, t += lerpValue);

                yield return null;
            }

            t = 0;

            lerpValue += lerpAdd;

            while (transform.localScale.y < startScale)
            {
                scaleVector = Vector3.one * startScale;

                transform.localScale = Vector3.Lerp(transform.localScale, scaleVector, t += lerpValue);

                yield return null;
            }
        }

        indexIncrease = false;
        shakeRoutineCalled = false;
        elapsedTime = 0;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }
}
