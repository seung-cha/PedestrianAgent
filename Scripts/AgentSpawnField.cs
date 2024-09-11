using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PedestrianAgent
{
    public class AgentSpawnField : MonoBehaviour
    {
        [SerializeField]
        float spawnRadius;

        [SerializeField]
        List<GameObject> prefabs = new List<GameObject>(); 

        // Start is called before the first frame update
        void Start()
        {
            AgentPool.Pool.Init(prefabs);

            StartCoroutine(SpawnPedestrians());
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        IEnumerator SpawnPedestrians()
        {
            while(true)
            {
                List<Collider> colliders = new List<Collider>(Physics.OverlapSphere(this.transform.position, spawnRadius));

                foreach(var collider in colliders)
                {
                    Debug.Log(collider.name);

                    AgentNetworkNode node;
                    if(node = collider.GetComponent<AgentNetworkNode>())
                    {
                        if(Random.value <= 0.5f)
                        {
                            PedestrianAgent agent = AgentPool.Pool.GetAgent();
                            agent.currentNode = node;
                            agent.transform.position = collider.transform.position;
                        }
                    }
                }




                yield return new WaitForSeconds(8.0f);
            }

        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(this.transform.position, spawnRadius);
        }
    }
}

