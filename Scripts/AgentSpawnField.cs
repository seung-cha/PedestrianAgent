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
        float removeDistance = 100.0f;

        [SerializeField]
        List<GameObject> prefabs = new List<GameObject>();

        [SerializeField]
        Camera cam;

        // Start is called before the first frame update
        void Start()
        {
            AgentPool.Pool.Init(prefabs);
            StartCoroutine(SpawnPedestrians());

            if(!cam)
            {
                Debug.LogWarning("Main camera not specified, using the main camera as the active camera.", this);
                cam = Camera.main;

                if(!cam)
                {
                    Debug.LogError("Could not assign the main camera. Make sure to assign camera manually.", this);
                }
            }

        }

        // Update is called once per frame
        void Update()
        {
        
        }

        IEnumerator SpawnPedestrians()
        {
            yield return new WaitForEndOfFrame();
            while(true)
            {
                List<Collider> colliders = new List<Collider>(Physics.OverlapSphere(this.transform.position, spawnRadius));

                foreach(var collider in colliders)
                {
                    Debug.Log(collider.name);

                    AgentNetworkNode node;
                    if(node = collider.GetComponent<AgentNetworkNode>())
                    {
                        if(Random.value <= 0.8f)
                        {
                            Vector3 vpPos = this.cam.WorldToViewportPoint(node.transform.position);

                            if (vpPos.x > 1.0f || vpPos.x < 0.0f || vpPos.y > 1.0f || vpPos.y < 0.0f)
                            {
                                PedestrianAgent agent = AgentPool.Pool.GetAgent();
                                agent.removeDistance = removeDistance;
                                agent.cam = cam;
                                agent.currentNode = node;
                                agent.transform.position = collider.transform.position;
                            }
                        }
                    }
                }




                yield return new WaitForSeconds(2.0f);
            }

        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(this.transform.position, spawnRadius);
        }
    }
}

