using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    public enum powerUpType {doubleJump, forwardRoll, speedBoost};

    public powerUpType powerUp = powerUpType.doubleJump;

    public float rotationSpeed = 50.0f;
    public float frequency = 1.0f;
    public float amplitude = .01f;

    public GameObject speedBoostPS;
    public GameObject doubleJumpPS;
    public GameObject forwardRollPS;

    public float powerUpDuration = 10.0f;

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
                    PlayerController.instance.CanDoubleJump();
                    Instantiate(doubleJumpPS, transform.position, Quaternion.identity);
                    Invoke("ResetDoubleJump", powerUpDuration);
                    break;

                case powerUpType.forwardRoll:
                    PlayerController.instance.CanForwardRoll();
                    Instantiate(forwardRollPS, transform.position, Quaternion.identity);
                    Invoke("ResetForwardRoll", powerUpDuration);
                    break;

                case powerUpType.speedBoost:
                    PlayerController.instance.IncreaseMovementSpeed();
                    Instantiate(speedBoostPS, transform.position, Quaternion.identity);
                    Invoke("ResetSpeedBoost", powerUpDuration);
                    break;

                default:
                    break;
            }
        }
    }

    private void Update()
    {
        transform.Rotate(rotationSpeed * Time.deltaTime, rotationSpeed * Time.deltaTime, rotationSpeed * Time.deltaTime);
        transform.position += new Vector3 (0, Mathf.Sin(Time.fixedTime * Mathf.PI * frequency) * amplitude, 0);
    }

    void ResetDoubleJump()
    {
        PlayerController.instance.ResetDoubleJump();
    }

    void ResetForwardRoll()
    {
        PlayerController.instance.ResetForwardRoll();
    }

    void ResetSpeedBoost()
    {
        PlayerController.instance.ResetMovementSpeed();
    }
}
