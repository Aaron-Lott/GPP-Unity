using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damageable : MonoBehaviour
{
    public int health = 1;

    public void TakeDamage(int damageAmount, Vector3 spawnPos)
    {
        ParticleSystem ps = Instantiate(PlayerController.instance.damagePS, spawnPos, Quaternion.identity);

        if (GetComponent<Renderer>())
        {
            ps.GetComponent<ParticleSystemRenderer>().material = GetComponent<Renderer>().material;
        }

        health -= damageAmount;

        if(health <= 0)
        {
            health = 0;
        }
    }

    public void TakeDamage(int damageAmount)
    {
        health -= damageAmount;

        if (health <= 0)
        {
            health = 0;
        }
    }
}
