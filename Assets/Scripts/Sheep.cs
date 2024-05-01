using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sheep : MonoBehaviour
{
    public UnityEngine.AI.NavMeshAgent agent;
    public GameObject player;
    public float distance = 4.0f;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        float dist = Vector3.Distance(player.transform.position, transform.position);

        if (dist < distance)
        {
            Vector3 direction = transform.position - player.transform.position;
            Vector3 newPos = transform.position + direction;
            agent.SetDestination(newPos);
        }
    }
}
