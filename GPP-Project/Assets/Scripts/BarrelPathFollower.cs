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

        public float timeToRotate = 0.05f;
        private float elapsedTime = 0;

        private bool hasRotated = false;

        private void Awake()
        {
            transform.rotation = Quaternion.Euler(0, -180, -90);
        }

        void Start()
        {
            if (pathCreator != null)
            {
                pathCreator.pathUpdated += OnPathChanged;
            }
        }

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
            else
            {
                //timeToSink = true;
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

