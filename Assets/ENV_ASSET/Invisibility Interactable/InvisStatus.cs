using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvisStatus : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (gameObject.GetComponent<MeshRenderer>().enabled == false)
        {
            StartCoroutine(waitTime());
        }
    }

    IEnumerator waitTime()
    {
        yield return new WaitForSeconds(10);
        
        // Enable mesh
        gameObject.GetComponent<MeshRenderer>().enabled = true;
    }
}
