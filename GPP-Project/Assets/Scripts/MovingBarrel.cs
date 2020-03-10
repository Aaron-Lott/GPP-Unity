using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingBarrel : MonoBehaviour
{
    public float moveSpeed;

    public enum moveDirection {X_AXIS, Y_AXIS, Z_AXIS};

    public moveDirection direction;

    private Vector3 moveVec;

    private float sinkSpeed = 2;

    [HideInInspector]
    public bool timeToSink;

    void Start()
    {
        switch(direction)
        {
            case moveDirection.X_AXIS:
                moveVec = new Vector3(1, 0, 0);
                break;

            case moveDirection.Y_AXIS:
                moveVec = new Vector3(0, 1, 0);
                break;

            case moveDirection.Z_AXIS:
                moveVec = new Vector3(0, 0, 1);
                break;

            default:
                break;
        }
    }

    void Update()
    {

        if(!timeToSink)
        {
            transform.position += moveVec * moveSpeed * Time.deltaTime;         
        }
        else
        {
            transform.position -= new Vector3(0, sinkSpeed, 0) * Time.deltaTime;

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
