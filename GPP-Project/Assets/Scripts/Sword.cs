using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{
    private CameraShake cameraShake;

    public bool enableCameraShake = true;

    public Transform hitCheck;

    public int damageAmount = 1;

    private float hitCheckRadius = 2.0f;

    public int hitForce = 1500;

    public void Hit()
    {
        CameraShake();

        Collider[] hitColliders = Physics.OverlapSphere(hitCheck.position, hitCheckRadius);

        foreach (Collider hit in hitColliders)
        {
            if (hit)
            {
                if (hit.GetComponent<Damageable>() && !hit.GetComponent<PlayerController>())
                {
                    hit.GetComponent<Damageable>().TakeDamage(damageAmount, hitCheck.transform.position);
                }

                if (hit.GetComponent<Rigidbody>() && hit.gameObject != gameObject)
                {
                    ApplyHitForce(hit);
                }
            }
        }
    }

    void CameraShake()
    {
        if (enableCameraShake && Camera.main)
        {
            cameraShake = Camera.main.GetComponent<CameraShake>();
            cameraShake.StartCoroutine(cameraShake.Shake(0.1f, 0.3f));
        }
    }

    void ApplyHitForce(Collider hit)
    {
        hit.GetComponent<Rigidbody>().AddForce((hit.transform.position - transform.position) * hitForce);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(hitCheck.position, hitCheckRadius);
    }
}
