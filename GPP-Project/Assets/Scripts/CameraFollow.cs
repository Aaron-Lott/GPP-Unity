using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{

    private Transform target;
    private Vector3 offset;
    private Vector3 refVector;


    void Start()
    {
        target = PlayerController.instance.transform;
        offset = new Vector3(transform.position.x, -4, transform.position.z * 0.25f);
    }

    void Update()
    {
        transform.position = target.position -  new Vector3(0, offset.y, offset.z);
    }
}
