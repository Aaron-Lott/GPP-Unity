using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrelDestroyer : MonoBehaviour
{
    private MovingBarrel barrel;

    private float lifeTimeAfterSink = 10;

    private Vector3 collisionPoint;

    private float tiltMultiplier = 1.6f;

    private void Start()
    {
        if (transform.parent.GetComponent<MovingBarrel>())
        {
            barrel = transform.parent.GetComponent<MovingBarrel>();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {

        if (collision.gameObject != PlayerController.instance.gameObject)
        {
            Debug.Log("hit");
            barrel.timeToSink = true;
            Destroy(gameObject, lifeTimeAfterSink);
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject == PlayerController.instance.gameObject)
        {
            foreach (ContactPoint contact in collision.contacts)
            {      
               collisionPoint = transform.InverseTransformPoint(contact.point);

                if(collisionPoint.y != 0)
                {
                    Quaternion newRot = transform.rotation * Quaternion.Euler(collisionPoint.y * tiltMultiplier, 0, 0);
                    transform.rotation = newRot;
                }
            }
        }
    }
}
