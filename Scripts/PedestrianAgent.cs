using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PedestrianAgent
{
    public class PedestrianAgent : MonoBehaviour
    {

        public AgentNetworkNode currentNode;
        public AgentNetworkNode nextNode;

        public Animator animator;

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
            currentNode.GetRandomVisitableNode(null, path);
            nextNode = path[++index];


        }

        // Update is called once per frame
        void Update()
        {

            if(move)
            {
                transform.Translate(Vector3.forward * Time.deltaTime * speed, Space.Self);
            }

            if(Vector3.Distance(transform.position, nextNode.transform.position) <= tolDist)
            {            
               
                currentNode = nextNode;

                if(index + 1 < path.Count)
                {
                    nextNode = path[++index];

                    Vector3 fwd = nextNode.transform.position - currentNode.transform.position;
                    this.transform.forward = fwd;
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
