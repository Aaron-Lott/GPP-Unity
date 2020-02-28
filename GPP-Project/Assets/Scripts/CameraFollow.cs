using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{

    public Transform target;
    public Vector3 offset; //new Vector3(0, -2, 5);
    private Vector3 vectorRef;

    private Vector3 maxOffset;
    private Vector3 minOffset;


    private RaycastHit cameraHit;
    private float cameraMaxHitDistance;

    public float paddingSize;

    public float smoothTime = 0.15f;

    public float collisionSmoothTime = 0.01f;

    private float collisionSmoothPerc = 0;

    [HideInInspector]
    public bool rotateWithInput = true;

    public float rotationSpeed = 4.0f;

    Quaternion turnAngleHorizontal;

    private bool isColliding = false;

    public bool smoothCollisionOn;

    void Start()
    {
        maxOffset = offset * 2;
        minOffset = offset;
        cameraMaxHitDistance = offset.z;
    }

    void LateUpdate()
    {
        if (rotateWithInput)
        {
            turnAngleHorizontal = Quaternion.AngleAxis(Input.GetAxis("RightStickX") * rotationSpeed, Vector3.up);
        }

        //handling zooming in and out.
        if(Input.GetAxis("RightStickY") > 0.1f)
        {
           offset = Vector3.Slerp(offset, maxOffset, Input.GetAxis("RightStickY"));
        }
        else if(Input.GetAxis("RightStickY") < -0.1f)
        {
            offset = Vector3.Slerp(offset, minOffset, Mathf.Abs(Input.GetAxis("RightStickY")));
        }

        offset = turnAngleHorizontal * offset;

        transform.LookAt(target.position);

        if(RayCastFunc())
        {
            Vector3 padding = (transform.position - target.position).normalized;
            padding *= paddingSize;

            if(smoothCollisionOn)
            {
                collisionSmoothPerc += collisionSmoothTime;
                transform.position = Vector3.Lerp(transform.position, cameraHit.point + padding, collisionSmoothPerc * Time.deltaTime);
            }
            else
            {
                transform.position = cameraHit.point + padding;
            }
            
        }
        else
        {
            collisionSmoothPerc = 0;
        }
        
        transform.position = Vector3.Lerp(transform.position, target.position - offset, smoothTime);
        
    }

    bool RayCastFunc()
    {
        Vector3 direction = transform.position - target.position;

        if(Physics.Raycast(target.position, direction, out cameraHit, cameraMaxHitDistance))
        {
            Debug.DrawRay(target.position, direction, Color.yellow);
            return true;
        }

        return false;
    }
}

