using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroy : MonoBehaviour
{
    public float lifeTime = 3.0f;
    private void Start()
    {
        Destroy(gameObject, lifeTime);
    }
}
