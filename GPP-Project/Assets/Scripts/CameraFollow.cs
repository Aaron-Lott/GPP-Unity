﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{

    public Transform target;
    public Vector3 offset; //new Vector3(0, -2, 5);
    private Vector3 vectorRef;


    public float smoothTime = 0.15f;

    [HideInInspector]
    public bool rotateWithInput = true;

    public float rotationSpeed = 4.0f;

    Quaternion turnAngleHorizontal;

    void Start()
    {
    }

    void LateUpdate()
    {
        if(rotateWithInput)
        {
            turnAngleHorizontal = Quaternion.AngleAxis(Input.GetAxis("RightStickX") * rotationSpeed, Vector3.up);

            //Quaternion turnAngleVertical = Quaternion.AngleAxis(-Input.GetAxis("RightStickY") * rotationSpeed, target.right);
        }


        offset = turnAngleHorizontal * offset;

        transform.LookAt(target.position);

        transform.position = Vector3.Slerp(transform.position, target.position - offset, smoothTime);
    }
}
