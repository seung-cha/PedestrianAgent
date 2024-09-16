using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Singleton to store pedestrian network as an undirected graph.
/// Should be created dynamically.
/// </summary>

namespace PedestrianAgent
{
    public class AgentNetworkGraph : MonoBehaviour
    {
        public static AgentNetworkGraph Network
        {
            get
            {
                if(_network == null)
                {
                    GameObject obj = new GameObject("Pedestrian Agent Network");
                    _network = obj.AddComponent<AgentNetworkGraph>();
                }

                return _network;
            }
        }

        private static AgentNetworkGraph _network = null;

        public Dictionary<AgentNetworkNode, List<AgentNetworkNode>> graph { get => _graph; }
        private Dictionary<AgentNetworkNode, List<AgentNetworkNode>> _graph = new Dictionary<AgentNetworkNode, List<AgentNetworkNode>>();

        // Start is called before the first frame update
        void Start()
        {
           

        }

        public List<AgentNetworkNode> GetAdjacentNodes(AgentNetworkNode key)
        {
            return graph[key];
        }

        /// <param name="key"></param>
        /// <param name="nodes"></param>
        public void InitNode(AgentNetworkNode key, List<AgentNetworkNode> nodes)
        {
            foreach(var node in nodes)
            {
                // Create an undirected graph.
                InsertNode(key, node);
                InsertNode(node, key);
            }

        }

        private void InsertNode(AgentNetworkNode key, AgentNetworkNode node)
        {
            if (!_graph.ContainsKey(key)) _graph.Add(key, new List<AgentNetworkNode>());

            if (!_graph[key].Contains(node)) _graph[key].Add(node);
        }

        
        public List<AgentNetworkNode> GetRandomForwardPath(AgentNetworkNode startingNode, int minNodes = 5)
        {
            List<AgentNetworkNode> list = new List<AgentNetworkNode>();
            _GetRandomForwardPath(ref list, startingNode, null, minNodes);

            return list;
        }

        private void _GetRandomForwardPath(ref List<AgentNetworkNode> list, AgentNetworkNode current, AgentNetworkNode prev, int minNodeCount)
        {
            list.Add(current);
            int a = Random.Range(0, 20);
            // Terminate here by 1/20 chance
            if (list.Count >= minNodeCount && a == 0)
            {
                return;
            }
            else
            {
                if (graph[current].Count == 0) return;

                int index = 0;
                bool validSelection = false;

                // Try to get a random point, agent should only move forward or rotate 90 deg but not go backwards.
                for (int i = 0; i < 100; i++)
                {
                    index = Random.Range(0, graph[current].Count);

                    if (prev == null)
                    {
                        validSelection = true;
                        break;
                    }

                    Vector3 v1 = (current.transform.position - prev.transform.position).normalized;
                    Vector3 v2 = (graph[current][index].transform.position - current.transform.position).normalized;
            
                    // At best agent can make a 90 deg turn.
                    if (Vector3.Dot(v1, v2) >= -0.2f)
                    {
                        validSelection = true;
                        break;
                    }
                }

                if (!validSelection) return;

                _GetRandomForwardPath(ref list, graph[current][index], current, minNodeCount);

            }
        }


        private void OnDrawGizmosSelected()
        {
            foreach(var key in graph.Keys)
            {
                foreach (var node in graph[key])
                {
                    Gizmos.color = Color.red;
                    var dir = key.transform.position - node.transform.position;
                    dir.Normalize();

                    Gizmos.DrawLine(key.transform.position, node.transform.position);
                    Gizmos.DrawLine(node.transform.position, node.transform.position + Quaternion.AngleAxis(15, Vector3.up) * dir);
                    Gizmos.DrawLine(node.transform.position, node.transform.position + Quaternion.AngleAxis(-15, Vector3.up) * dir);
                }
            }
        }
    }

}
