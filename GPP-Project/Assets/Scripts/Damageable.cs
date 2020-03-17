using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damageable : MonoBehaviour
{
    public int health = 1;

    private float invinsibilityTime = 1.0f;
    private float elapsedInvinsTime = 0;

    public void TakeDamage(int damageAmount, Vector3 spawnPos)
    {
        ParticleSystem ps = Instantiate(PlayerController.instance.damagePS, spawnPos, Quaternion.identity);

        if (GetComponent<Renderer>())
        {
            ps.GetComponent<ParticleSystemRenderer>().material = GetComponent<Renderer>().material;
        }
        else if (GetComponentInChildren<SkinnedMeshRenderer>()) // EXCLUSIVE FOR PLAYER CONTROLLER.
        {

            ps.GetComponent<ParticleSystemRenderer>().material = GetComponentInChildren<SkinnedMeshRenderer>().material;
        }


        health -= damageAmount;

        OnDamage();

        if (health <= 0)
        {
            health = 0;
        }
    }

    public void TakeDamage(int damageAmount)
    {
        health -= damageAmount;

        OnDamage();

        if (health <= 0)
        {
            health = 0;
        }

        elapsedInvinsTime = 0;
    }

    public virtual void OnDamage()
    {
        //FUNCTION CALLED FROM DAMAGEABLE CHILDREN.
    }
}
