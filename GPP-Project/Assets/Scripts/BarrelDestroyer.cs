using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrelDestroyer : MonoBehaviour
{    
    private PathCreation.Examples.BarrelPathFollower barrelFollower;

    private float lifeTimeAfterSink = 10;

    private Vector3 collisionPoint;

    private float tiltMultiplier = 0.6f;

    private void Start()
    {
        if (transform.parent.GetComponent<PathCreation.Examples.BarrelPathFollower>())
        {
            barrelFollower = transform.parent.GetComponent<PathCreation.Examples.BarrelPathFollower>();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject != PlayerController.instance.gameObject)
        {
            barrelFollower.timeToSink = true;

            Destroy(gameObject, lifeTimeAfterSink);
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        ManageRotation(collision);
    }

    private void ManageRotation(Collision collision)
    {
        if (collision.gameObject == PlayerController.instance.gameObject)
        {
            foreach (ContactPoint contact in collision.contacts)
            {
                collisionPoint = transform.InverseTransformPoint(contact.point);

                Quaternion newRot = transform.rotation * Quaternion.Euler(0, 0, -collisionPoint.y * tiltMultiplier);
                transform.rotation = newRot;

            }
        }
    }
}
