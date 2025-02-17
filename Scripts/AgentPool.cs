using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PedestrianAgent
{
    public class AgentPool : MonoBehaviour
    {
    /**
     * Per-scene based singleton instance.
     * Call Init() to explicitly load the pool with pedestrian game objects.
     * Assumes an instant will persist throughout the scene once Init() is called in that scene.
     * This should be loaded dynamically.
     */
        public static AgentPool Pool
        {
            get
            {
                if (_pool == null)
                {
                    GameObject obj = new GameObject("PedestrianAgentPool");
                    _pool = obj.AddComponent<AgentPool>();

                    return _pool;
                }
                else
                {
                    return _pool;
                }
            }
        }

        static AgentPool _pool;


        List<GameObject> objectVariation = new List<GameObject>();
        Queue<PedestrianAgent> poolQueue = new Queue<PedestrianAgent>();
        GameObject parent;


        void InstantiateAgents()
        {
            for (int i = 0; i < 50; i++)
            {
                // Construct agent.
                // Agent (empty gameobject)
                //  - Skin (variation)

                PedestrianAgent agent = new GameObject("agent").AddComponent<PedestrianAgent>();
                agent.transform.parent = parent.transform;

                GameObject obj = Instantiate(objectVariation[Random.Range(0, objectVariation.Count)]);
                obj.transform.parent = agent.transform;

                obj.transform.localPosition = Vector3.zero;
                agent.gameObject.SetActive(false);
                poolQueue.Enqueue(agent);
            }
        }


        public void Init(List<GameObject> objects)
        {
            parent = new GameObject("pool objects");
            parent.transform.parent = this.gameObject.transform;

            objectVariation = new List<GameObject>(objects);

            InstantiateAgents();
        }


        public PedestrianAgent GetAgent()
        {
            if(poolQueue.Count == 0)
            {
                InstantiateAgents();
            }

            PedestrianAgent agent = poolQueue.Dequeue();
            agent.gameObject.SetActive(true);
            return agent;
        }

        public void ReturnAgent(PedestrianAgent agent)
        {
            poolQueue.Enqueue(agent);
            agent.gameObject.SetActive(false);
        }

        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }

}
