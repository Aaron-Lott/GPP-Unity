using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    private bool timeToShake = false;
    private float initialView;

    public bool slowMotion = true;

    private void Start()
    {
        initialView = Camera.main.fieldOfView;
    }

    public IEnumerator Shake(float duration, float magnitude)
    {
        timeToShake = true;

        if (slowMotion)
        {
            Time.timeScale = 1;
            Time.fixedDeltaTime = 0.02f;
        }

        Vector3 initialPos = transform.localPosition;

        float elapsedTime = 0.0f;

        while (elapsedTime < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            transform.localPosition = new Vector3(x, y, initialPos.z);

            elapsedTime += Time.deltaTime;

            yield return null;
        }

        timeToShake = false;
        transform.localPosition = initialPos;

        yield return null;
    }

    public IEnumerator ZoomOut(float magnitude, int maxFieldOfView)
    {
         if (slowMotion)
         {
             Time.timeScale = 0.5f;
             Time.fixedDeltaTime = 0.02f * Time.timeScale;
         }

         while (!timeToShake)
         {
             if(Camera.main.fieldOfView < maxFieldOfView)
             {
                 Camera.main.fieldOfView += (Time.deltaTime * magnitude);
             }

             yield return null;
         }

         Camera.main.fieldOfView = initialView;
         

        yield return null;
    }

    public IEnumerator ZoomOut(float magnitude, int maxFieldOfView, float duration)
    {
        
        float elapsedTime = 0.0f;

        while (elapsedTime < duration)
        {
            if (Camera.main.fieldOfView < maxFieldOfView)
            {
                Camera.main.fieldOfView += (Time.deltaTime * magnitude);
            }

            elapsedTime += Time.deltaTime;


            yield return null;
        }

        elapsedTime = 0.0f;

        while(elapsedTime < duration / 1.5f)
        {
            if (Camera.main.fieldOfView > initialView)
            {
                Camera.main.fieldOfView -= (Time.deltaTime * magnitude);
            }

            elapsedTime += Time.deltaTime;

            yield return null;
        }

        Camera.main.fieldOfView = initialView;
        

        yield return null;
    }
    
}
