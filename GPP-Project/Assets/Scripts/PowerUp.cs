using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    public enum powerUpType {doubleJump, forwardRoll, speedBoost};

    public powerUpType powerUp = powerUpType.doubleJump;

    public float rotationSpeed = 50.0f;

    public GameObject speedBoostPS;
    public GameObject doubleJumpPS;
    public GameObject forwardRollPS;

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
                    Instantiate(doubleJumpPS, transform.position, Quaternion.identity);
                    PlayerController.instance.canDoubleJump = true;
                    break;

                case powerUpType.forwardRoll:
                    PlayerController.instance.canForwardRoll = true;
                    Instantiate(forwardRollPS, transform.position, Quaternion.identity);
                    break;

                case powerUpType.speedBoost:
                    PlayerController.instance.IncreaseMovementSpeed();
                    Instantiate(speedBoostPS, transform.position, Quaternion.identity);
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
