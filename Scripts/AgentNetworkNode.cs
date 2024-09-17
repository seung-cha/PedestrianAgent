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

        public bool NodeActive = true;
        public float SpawnWidth = 1.0f;

        private void Start()
        {
            // Add a collider so that it can be queried by raycast.
            gameObject.AddComponent<SphereCollider>().isTrigger = true;

            // Get rid of self loops
            adjacentNodes = adjacentNodes.Where((node) => node != this).ToList();

            AgentNetworkGraph.Network.InitNode(this, adjacentNodes);
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(this.transform.position, SpawnWidth);
           


            // Draw arrow to adjacent nodes
            foreach (var node in adjacentNodes)
            {
                Gizmos.color = Color.green;
                var dir = this.transform.position - node.transform.position;
                dir.Normalize();

                Gizmos.DrawLine(this.transform.position, node.transform.position);
                //Gizmos.DrawLine(node.transform.position, node.transform.position + Quaternion.AngleAxis(45, Vector3.up) * dir);
                //Gizmos.DrawLine(node.transform.position, node.transform.position + Quaternion.AngleAxis(-45, Vector3.up) * dir);
            }
        }
    }

}
