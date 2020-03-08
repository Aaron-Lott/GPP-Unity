using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingBarrel : MonoBehaviour
{
    private Rigidbody rb;

    public float moveSpeed;

    public enum moveDirection {X_AXIS, Y_AXIS, Z_AXIS};

    public moveDirection direction;

    private Vector3 moveVec;

    [HideInInspector]
    public bool timeToSink;

    void Start()
    {
        rb = GetComponentInChildren<Rigidbody>();


        switch(direction)
        {
            case moveDirection.X_AXIS:
                moveVec = new Vector3(moveSpeed, 0, 0);
                rb.constraints = RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezePositionZ;
                break;

            case moveDirection.Y_AXIS:
                moveVec = new Vector3(0, moveSpeed, 0);
                rb.constraints = RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ;
                break;

            case moveDirection.Z_AXIS:
                moveVec = new Vector3(0, 0, moveSpeed);
                rb.constraints = RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionY;
                break;

            default:
                break;
        }
    }

    void Update()
    {
        if(!timeToSink)
        {
            transform.position += moveVec * Time.deltaTime;
        }
        else
        {
            if(rb != null)
            {
                rb.constraints = RigidbodyConstraints.None;
                rb.useGravity = false;
            }


            transform.position += new Vector3(0, -2, 0) * Time.deltaTime;

            if(transform.childCount <= 0)
            {
                Destroy(gameObject);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject == PlayerController.instance.gameObject)
        {
            other.transform.parent = transform;
            PlayerController.instance.rb.interpolation = RigidbodyInterpolation.None;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == PlayerController.instance.gameObject)
        {
            other.transform.parent = null;
            PlayerController.instance.rb.interpolation = RigidbodyInterpolation.Interpolate;
        }
    }
}
