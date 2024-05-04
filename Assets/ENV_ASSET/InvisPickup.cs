using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvisPickup : MonoBehaviour
{
    public bool invisRespawn;
    public float invisRespawnTime = 3f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Collision detected for InvisPickup");
        // For testing, compare Capsule tag. For game, compare Clone tag
        if (other.gameObject.CompareTag("Hider")) 
        {
            // For test, change color to red
            // gameObject.GetComponent<Renderer>().material.color = Color.red;

            // Disable mesh
            gameObject.GetComponent<MeshRenderer>().enabled = false;

            // Disable collider
            gameObject.GetComponent<Collider>().enabled = false;

            // Disable mesh of Hider avatar (Child "clone" of collided gameObject)

            GameObject model = other.gameObject.transform.GetChild(1).gameObject;
            GameObject avatar = model.transform.GetChild(0).gameObject;

            avatar.GetComponent<MeshRenderer>().enabled = false;

            if (invisRespawn)
            {
                Invoke("InvisRespawn", invisRespawnTime);
            }
        }
    }

    void InvisRespawn()
    {
        gameObject.GetComponent<MeshRenderer>().enabled = true;
        gameObject.GetComponent<Collider>().enabled = true;
    }
}
