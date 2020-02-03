using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{

    private Transform target;
    private Vector3 refVector;

    void Start()
    {
        target = PlayerController.instance.transform;
    }

    void Update()
    {
        Vector3 offset = target.position - transform.position;
        Debug.Log(offset);

        transform.position = Vector3.SmoothDamp(transform.position, offset, ref  refVector, 0.15f);
    }
}
