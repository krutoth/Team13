using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MoveToPosition : MonoBehaviour
{
    public NavMeshAgent agent;
    public Transform target;

    public GameObject hider;


    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.SetDestination(target.position);
    }

    // Update is called once per frame
    void Update()
    {
        // If Seeker Destination not target, set after waitTime
        if (agent.destination != target.position)
        {
            StartCoroutine(waitTime());
        }

        agent.SetDestination(target.position);

        // If Hider is invisible, stop Seeker from moving for 10 seconds
        // Child of Character object, then child of model object
        GameObject model = hider.transform.GetChild(1).gameObject;
        GameObject avatar = model.transform.GetChild(0).gameObject;

        if (avatar.GetComponent<MeshRenderer>().enabled == false)
        {
            agent.SetDestination(gameObject.transform.position);
            StartCoroutine(waitTime());
        }
    }

    IEnumerator waitTime()
    {
        yield return new WaitForSeconds(11);

    }
}
