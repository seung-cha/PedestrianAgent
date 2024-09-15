using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace PedestrianAgent
{
    /***
     * Graph that represents a set of other places (nodes) that an agent can visit.
     * Edge is directed.
     */
    public class AgentNetworkNode : MonoBehaviour
    {
        [SerializeField]
        List<AgentNetworkNode> adjacentNodes = new List<AgentNetworkNode>();

        public List<AgentNetworkNode> AdjacentNodes { get => adjacentNodes; }

        public float lateralLeft, lateralRight;

        private void Start()
        {
            // Add a collider so that it can be queried by raycast.
            gameObject.AddComponent<SphereCollider>().isTrigger = true;

            // Get rid of self loops
            adjacentNodes = adjacentNodes.Where((node) => node != this).ToList();
        }

        /**
         * Get a node visitable from this node.
         * 
         */
        public void GetRandomVisitableNode(AgentNetworkNode ancestor, List<AgentNetworkNode> arr, bool first = false)
        {
            arr.Add(this);
            int a = Random.Range(0, 20);
            // Select this node by 1/10 chance
            if(a == 0 && !first)
            {
                return;
            }
            else
            {
                if (adjacentNodes.Count == 0) return;

                int index = 0;

                // Try to get a random point, skip if the agent will move backwards.
                for(int i = 0; i < 100; i++)
                {
                    index = Random.Range(0, adjacentNodes.Count);

                    if (ancestor == null) break;

                    Vector3 v1 = this.transform.position - ancestor.transform.position;
                    Vector3 v2 = adjacentNodes[index].transform.position - this.transform.position;

                    if (Vector3.Dot(v1, v2) <= 0.3) break;

                }
                
                adjacentNodes[index].GetRandomVisitableNode(this, arr);
            }


        }


        private void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(this.transform.position, 1.0f);
           


            // Draw arrow to adjacent nodes
            foreach (var node in adjacentNodes)
            {
                Gizmos.color = Color.green;
                var dir = this.transform.position - node.transform.position;
                dir.Normalize();

                Gizmos.DrawLine(this.transform.position, node.transform.position);
                Gizmos.DrawLine(node.transform.position, node.transform.position + Quaternion.AngleAxis(45, Vector3.up) * dir);
                Gizmos.DrawLine(node.transform.position, node.transform.position + Quaternion.AngleAxis(-45, Vector3.up) * dir);

            }
        }
    }

}
