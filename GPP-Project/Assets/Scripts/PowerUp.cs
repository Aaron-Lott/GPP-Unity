using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    public enum powerUpType {doubleJump, forwardRoll, speedBoost};

    public powerUpType powerUp = powerUpType.doubleJump;

    public float rotationSpeed = 50.0f;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject == PlayerController.instance.gameObject)
        {
            //stop playing particle system.

            foreach(Transform child in transform)
            {
                if(child.GetComponent<ParticleSystem>())
                {
                    child.GetComponent<ParticleSystem>().Stop();
                }
            }

            if (GetComponent<Renderer>())
            {
                GetComponent<Renderer>().enabled = false;
            }

            if(GetComponent<BoxCollider>())
            {
                GetComponent<BoxCollider>().enabled = false;
            }

            switch (powerUp)
            {
                case powerUpType.doubleJump:
                    PlayerController.instance.canDoubleJump = true;
                    break;

                case powerUpType.forwardRoll:
                    PlayerController.instance.canForwardRoll = true;
                    break;

                case powerUpType.speedBoost:
                    PlayerController.instance.IncreaseMovementSpeed();
                    break;

                default:
                    break;
            }
        }
    }

    private void Update()
    {
        //constant rotation.
        transform.Rotate(rotationSpeed * Time.deltaTime, rotationSpeed * Time.deltaTime, rotationSpeed * Time.deltaTime); 
    }


}
