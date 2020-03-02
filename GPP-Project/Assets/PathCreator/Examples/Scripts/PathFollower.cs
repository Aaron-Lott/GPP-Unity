using UnityEngine;

namespace PathCreation.Examples
{
    // Moves along a path at constant speed.
    // Depending on the end of path instruction, will either loop, reverse, or stop at the end of the path.
    public class PathFollower : MonoBehaviour
    {
        public PathCreator pathCreator;
        public EndOfPathInstruction endOfPathInstruction;
        private float speed = 8;
        float distanceTravelled;

        private bool facingForward = true;

        void Start() {
            if (pathCreator != null)
            {
                pathCreator.pathUpdated += OnPathChanged;
            }
        }

        void Update()
        {
            if (pathCreator != null)
            {
                PlayerController.instance.GetComponent<Animator>().SetFloat("speed", speed * Mathf.Abs(Input.GetAxis("Horizontal")));

                distanceTravelled += Input.GetAxis("Horizontal") * speed * Time.deltaTime;
                transform.position = pathCreator.path.GetPointAtDistance(distanceTravelled, endOfPathInstruction);

                Quaternion rot = pathCreator.path.GetRotationAtDistance(distanceTravelled, endOfPathInstruction);

                if(Input.GetAxis("Horizontal") < 0)
                {
                    //turn around
                    facingForward = false;
                }
                else if (Input.GetAxis("Horizontal") > 0)
                {
                    facingForward = true;
                }

                if(facingForward)
                {
                    transform.rotation = Quaternion.Slerp(transform.rotation, rot * Quaternion.Inverse(Quaternion.Euler(0, 0, -90)), 0.2f);
                }
                else
                {
                    transform.rotation = Quaternion.Slerp(transform.rotation, rot * Quaternion.Inverse(Quaternion.Euler(0, 180, -90)), 0.2f);
                }
            }
        }

        void OnPathChanged() {
            distanceTravelled = pathCreator.path.GetClosestDistanceAlongPath(transform.position);
        }
        
        public void SetDistanceTravelled(float newDist)
        {
            distanceTravelled = newDist;
        }

        public bool IsFacingForward()
        {
            return facingForward;
        }
    }
}