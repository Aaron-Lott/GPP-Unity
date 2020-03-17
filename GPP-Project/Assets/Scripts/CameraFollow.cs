using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{

    public Transform target;
    public Vector3 offset; //new Vector3(0, -2, 5);
    private Vector3 vectorRef;

    private RaycastHit cameraHit;
    private float cameraMaxHitDistance;

    private float paddingSize = -1;

    public float smoothTime = 0.15f;

    public float collisionSmoothTime = 0.01f;

    private float collisionSmoothPerc = 0;

    private float axisSnapValue = 0;
    private float axisSnapSpeed = 0.03f;

    private Vector3 initialOffset;

    [HideInInspector]
    public bool rotateWithInput = true;

    public float rotationSpeed = 4.0f;

    Quaternion turnAngleHorizontal;

    private bool isColliding = false;

    public bool smoothCollisionOn;

    private float zoomValue = 1;

    private const float initZoom = 1;
    private const float maxZoom = 2;
    private const float minZoom = 0.5f;

    void Start()
    {
        cameraMaxHitDistance = offset.z;
        axisSnapValue = offset.z / 2;

        initialOffset = offset;
    }

    void LateUpdate()
    {
        if (rotateWithInput)
        {
            turnAngleHorizontal = Quaternion.AngleAxis(Input.GetAxis("RightStickX") * rotationSpeed, Vector3.up);
        }

        if(Input.GetButtonDown("RightStickY"))
        {
            ZoomHandler();
        }

        if (!PlayerController.instance.LockIntoCombat())
        {
            offset = turnAngleHorizontal * offset;
            transform.LookAt(target.position);
            SnapCameraAxis();
        }
        else
        {
            offset = initialOffset;
            transform.LookAt(PlayerController.instance.FindClosestEnemy());
        }

        transform.position = Vector3.Lerp(transform.position, target.position - (offset * zoomValue), smoothTime);

        ColliderHandler();
        
    }

    void ColliderHandler()
    {
        if (IsColliding())
        {
            Vector3 padding = (transform.position - target.position).normalized;
            padding *= paddingSize;

            if (smoothCollisionOn)
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
    }

    bool IsColliding()
    {
        Vector3 direction = transform.position - target.position;

        if(Physics.Raycast(target.position, direction, out cameraHit, cameraMaxHitDistance))
        {
            if(!cameraHit.collider.isTrigger)
            {
                Debug.DrawRay(target.position, direction, Color.yellow);
                return true;
            }
        }

        return false;
    }

    void SnapCameraAxis()
    {
        //PlayerController.instance.rb.velocity == Vector3.zero
        if (Input.GetAxis("RightStickX") == 0 && PlayerController.instance.rb.velocity.magnitude <= 0.1f)
        {
            Vector3 newPos;

            if (offset.z < -axisSnapValue)
            {
                newPos = new Vector3(0, offset.y, -initialOffset.z);
                offset = Vector3.MoveTowards(offset, newPos, axisSnapSpeed);
            }
            else if (offset.z > axisSnapValue)
            {
                newPos = new Vector3(0, offset.y, initialOffset.z);
                offset = Vector3.MoveTowards(offset, newPos, axisSnapSpeed);
            }
            else if (offset.x < -axisSnapValue)
            {
                newPos = new Vector3(-initialOffset.z, offset.y, 0);
                offset = Vector3.MoveTowards(offset, newPos, axisSnapSpeed);
            }
            else if (offset.x > axisSnapValue)
            {
                newPos = new Vector3(initialOffset.z, offset.y, 0);
                offset = Vector3.MoveTowards(offset, newPos, axisSnapSpeed);
            }
        }
    }

    void ZoomHandler()
    {
        switch (zoomValue)
        {
            case initZoom:
                zoomValue = maxZoom;
                break;

            case maxZoom:
                zoomValue = minZoom;
                break;

            case minZoom:
                zoomValue = initZoom;
                break;

            default:
                break;
        }
    }
}

