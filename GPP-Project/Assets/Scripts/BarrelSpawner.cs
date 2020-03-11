using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrelSpawner : MonoBehaviour
{
    private float elapsedTime = 0;
    public float spawnTime = 3f;

    private Animator anim;

    public string animParamter;

    public Transform barrel;
    public GameObject movingBarrel;

    public ParticleSystem gooSplash;

    public bool timeToSpawn = false;

    public Transform barrelSpline;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        if(timeToSpawn)
        {
            elapsedTime += Time.deltaTime;
        }

        if(elapsedTime > spawnTime)
        {
            anim.SetTrigger(animParamter);
            elapsedTime = 0;
        }
    }

    public void SpawnMovingBarrel()
    {
       GameObject newBarrel = Instantiate(movingBarrel, barrel.position, Quaternion.identity);
        newBarrel.transform.parent = barrelSpline;
    }

    public void PlayGooSplash()
    {
        gooSplash.Play();
    }
}
