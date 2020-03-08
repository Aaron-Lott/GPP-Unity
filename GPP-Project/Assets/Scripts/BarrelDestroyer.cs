using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrelDestroyer : MonoBehaviour
{
    private MovingBarrel barrel;

    private float lifeTimeAfterSink = 10;

    private void Start()
    {
        if(transform.parent.GetComponent<MovingBarrel>())
        barrel = transform.parent.GetComponent<MovingBarrel>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject != PlayerController.instance.gameObject)
        {
            barrel.timeToSink = true;
            Destroy(gameObject, lifeTimeAfterSink);
        }
    }
}
