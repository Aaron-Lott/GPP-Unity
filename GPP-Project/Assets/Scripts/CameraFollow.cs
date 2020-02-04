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
    }

    void Update()
    {

    }
}
