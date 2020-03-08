﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrelSpawner : MonoBehaviour
{
    private float elapsedTime = 0;
    private float spawnTime = 3f;

    private Animator anim;

    public Transform barrel;
    public GameObject movingBarrel;

    public ParticleSystem gooSplash;

    public bool timeToSpawn = false;

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
            anim.SetTrigger("trigger");
            elapsedTime = 0;
        }
    }

    public void SpawnMovingBarrel()
    {
        Instantiate(movingBarrel, barrel.position, Quaternion.identity);
    }

    public void PlayGooSplash()
    {
        gooSplash.Play();
    }
}