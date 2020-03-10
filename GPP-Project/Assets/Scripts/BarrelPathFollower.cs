using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace PathCreation.Examples
{

    public class BarrelPathFollower : MonoBehaviour
    {
        public PathCreator pathCreator;
        public EndOfPathInstruction endOfPathInstruction;
        private float speed = 5;
        float distanceTravelled;

        [HideInInspector]
        public bool timeToSink;

        private float sinkSpeed = 2;

        // Start is called before the first frame update
        void Start()
        {
            if (pathCreator != null)
            {
                pathCreator.pathUpdated += OnPathChanged;
            }
        }

        // Update is called once per frame
         void Update()
        {
            if (!timeToSink)
            {
                if (pathCreator != null)
                {

                    distanceTravelled += speed * Time.deltaTime;

                    Vector3 splinePos = pathCreator.path.GetPointAtDistance(distanceTravelled, endOfPathInstruction);

                    transform.position = new Vector3(splinePos.x, transform.position.y, splinePos.z);

                    Quaternion splineRot = pathCreator.path.GetRotationAtDistance(distanceTravelled);

                    transform.rotation = splineRot;
                }
            }
            else
            {
                transform.position -= new Vector3(0, sinkSpeed, 0) * Time.deltaTime;

                if (transform.childCount <= 0)
                {
                    Destroy(gameObject);
                }
            }
        }

        void OnPathChanged()
        {
            distanceTravelled = pathCreator.path.GetClosestDistanceAlongPath(transform.position);
        }

        public void SetDistanceTravelled(float newDist)
        {
            distanceTravelled = newDist;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject == PlayerController.instance.gameObject)
            {
                other.transform.parent = transform;
                PlayerController.instance.rb.interpolation = RigidbodyInterpolation.None;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject == PlayerController.instance.gameObject)
            {
                other.transform.parent = null;
                PlayerController.instance.rb.interpolation = RigidbodyInterpolation.Interpolate;
            }
        }


    }
}

