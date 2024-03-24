using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    private GameObject obj;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Interact()
    {
        // Debug.Log("Interacting with " + gameObject.name);
        // move gameObject upward
        obj = gameObject;
        obj.transform.position += new Vector3(0, 1, 0);
    }
}
