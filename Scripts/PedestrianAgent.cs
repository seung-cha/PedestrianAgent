using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PedestrianAgent
{
    public class PedestrianAgent : MonoBehaviour
    {

        public AgentNetworkNode currentNode;
        public AgentNetworkNode nextNode;

        private Vector3 localGoalPos;

        public Animator animator;

        public Camera cam;
        public float removeDistance = 100.0f;

        [SerializeField]
        List<AgentNetworkNode> path = new List<AgentNetworkNode>();

        [SerializeField]
        private float speed;

        float tolDist = 1;

        int index = 0;
        bool move = true;
        
        // Start is called before the first frame update
        void Start()
        {
            animator = GetComponentInChildren<Animator>();

            tolDist = Random.Range(0.1f, 3.0f);


            animator.SetFloat("Walk", 1.0f);
            speed = Random.Range(1.3f, 2f);

            // Highly unlikely that there will be only one node returned.
            // If there is only one node, don't make the agent move.
            path = AgentNetworkGraph.Network.GetRandomForwardPath(currentNode);

            if(path.Count == 1)
            {
                move = false;
                animator.SetFloat("Walk", 0.0f);
            }
            else
            {
                nextNode = path[++index];
                transform.forward = nextNode.transform.position - transform.position;
            }


        }

        // Update is called once per frame
        void Update()
        {
            // Check if agent is being rendered. Otherwise remove the agent.
            if(Vector3.Distance(this.transform.position, this.cam.transform.position) >= removeDistance)
            {
                Vector3 vpPos = this.cam.WorldToViewportPoint(this.transform.position);

                if(vpPos.x > 1.0f || vpPos.x < 0.0f || vpPos.y > 1.0f || vpPos.y < 0.0f)
                {
                    AgentPool.Pool.ReturnAgent(this);
                }

            }

            if(move)
            {
                transform.Translate(Vector3.forward * Time.deltaTime * speed, Space.Self);
            }

            if(index == path.Count - 1 && Vector3.Distance(transform.position, nextNode.transform.position) <= tolDist)
            {
                move = false;
                animator.SetFloat("Walk", 0.0f);
            }
            else
            {

                if(Vector3.Distance(transform.position, nextNode.transform.position) <= tolDist)
                {            
                    if(nextNode.NodeActive)
                    {
                        move = true;
                        animator.SetFloat("Walk", 1.0f);

                        currentNode = nextNode;

                        if(index + 1 < path.Count)
                        {
                            nextNode = path[++index];

                            Vector3 fwd = nextNode.transform.position - currentNode.transform.position;
                            this.transform.forward = fwd;
                        }
      
                    }
                    else
                    {
                        move = false;
                        animator.SetFloat("Walk", 0.0f);
                    }
                }
            }
        }

    }

}
