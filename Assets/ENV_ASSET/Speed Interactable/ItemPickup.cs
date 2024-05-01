using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        // Debug.Log("Collision detected test");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // When the player collides with the item, the item will be picked up
    void OnCollisionEnter(Collision other)
    {
        Debug.Log("Collision detected");
    }
}
