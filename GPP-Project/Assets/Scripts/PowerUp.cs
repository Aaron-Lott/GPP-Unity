using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    public float rotationSpeed = 50.0f;
    public float frequency = 1.0f;
    public float amplitude = .01f;

    public GameObject collectEffect;

    private float powerUpDuration = 15.0f;
    private float elapsedTimeDuration;

    private bool hasPowerUp = false;

    private bool collected = false;

    private float respawnTime = 16.0f;
    private float elapsedTimeRespawn;

    public bool canRespawn = true;


    private void Update()
    {
        transform.Rotate(rotationSpeed * Time.deltaTime, rotationSpeed * Time.deltaTime, rotationSpeed * Time.deltaTime);
        transform.position += new Vector3(0, Mathf.Sin(Time.fixedTime * Mathf.PI * frequency) * amplitude, 0);

        if(hasPowerUp)
        {
            elapsedTimeDuration += Time.deltaTime;

            if(elapsedTimeDuration > powerUpDuration)
            {
                ResetPowerUp();
                hasPowerUp = false;
            }
        }
        else
        {
            elapsedTimeDuration = 0;
        }

        if(collected)
        {
            elapsedTimeRespawn += Time.deltaTime;

            if(elapsedTimeRespawn > respawnTime)
            {
                EnablePowerUp();
                collected = false;
            }
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject == PlayerController.instance.gameObject)
        {
            DisablePowerUp();
            Instantiate(collectEffect, transform.position, Quaternion.identity);
            PickUp();

            collected = true;
            elapsedTimeRespawn = 0;
        }
    }

    void DisablePowerUp()
    {
        hasPowerUp = true;

        foreach (Transform child in transform)
        {
            if (child.GetComponent<ParticleSystem>())
            {
                child.GetComponent<ParticleSystem>().Stop();
            }
        }

        if (GetComponent<Renderer>())
        {
            GetComponent<Renderer>().enabled = false;
        }

        if (GetComponent<BoxCollider>())
        {
            GetComponent<BoxCollider>().enabled = false;
        }
    }

    void EnablePowerUp()
    {
        foreach (Transform child in transform)
        {
            if (child.GetComponent<ParticleSystem>())
            {
                child.GetComponent<ParticleSystem>().Play();
            }
        }

        if (GetComponent<Renderer>())
        {
            GetComponent<Renderer>().enabled = true;
        }

        if (GetComponent<BoxCollider>())
        {
            GetComponent<BoxCollider>().enabled = true;
        }
    }

    public virtual void PickUp()
    {

    }

    public virtual void ResetPowerUp()
    {

    }
}
