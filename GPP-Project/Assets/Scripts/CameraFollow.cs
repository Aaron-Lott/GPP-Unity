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


    public float smoothTime = 0.15f;

    public float collisionSmoothTime = 0.5f;

    [HideInInspector]
    public bool rotateWithInput = true;

    public float rotationSpeed = 4.0f;

    Quaternion turnAngleHorizontal;

    void Start()
    {
        maxOffset = offset * 2;
        minOffset = offset;
    }

    void LateUpdate()
    {
        if (rotateWithInput)
        {
            turnAngleHorizontal = Quaternion.AngleAxis(Input.GetAxis("RightStickX") * rotationSpeed, Vector3.up);

            //Quaternion turnAngleVertical = Quaternion.AngleAxis(-Input.GetAxis("RightStickY") * rotationSpeed, target.right);
        }

        if(Input.GetAxis("RightStickY") > 0.1f)
        {
           offset = Vector3.Slerp(offset, maxOffset, Input.GetAxis("RightStickY"));
        }
        else if(Input.GetAxis("RightStickY") < -0.1f)
        {
            offset = Vector3.Slerp(offset, minOffset, Mathf.Abs(Input.GetAxis("RightStickY")));
        }

        offset = turnAngleHorizontal * offset;

        //transform.position = Vector3.Slerp(transform.position, target.position - offset, smoothTime);
        //transform.position = Vector3.Slerp(transform.position, target.position, collisionSmoothTime);


        transform.LookAt(target.position);

        RayCastFunc();

    }

    bool RayCastFunc()
    {
        Vector3 direction = transform.position - target.position;
        RaycastHit hit;

        if(Physics.Raycast(target.position, direction, out hit, direction.magnitude))
        {
            Debug.DrawRay(target.position, direction, Color.yellow);
            transform.position = target.position;
            return true;
        }
        else
        {
            transform.position = Vector3.Slerp(transform.position, target.position - offset, smoothTime);
        }

        return false;
    }
}

