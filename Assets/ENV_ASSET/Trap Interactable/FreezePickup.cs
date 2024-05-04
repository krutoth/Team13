using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezePickup : MonoBehaviour
{
    public bool freezeRespawn;
    public float freezeRespawnTime = 3f;

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
        Debug.Log("Collision detected for FreezePickup");
        // For testing, compare Capsule tag. For game, compare Clone tag
        if (other.gameObject.CompareTag("Seeker")) 
        {
            // For test, change color to red
            // gameObject.GetComponent<Renderer>().material.color = Color.red;

            // Disable mesh
            gameObject.GetComponent<MeshRenderer>().enabled = false;

            // Disable collider
            gameObject.GetComponent<Collider>().enabled = false;

            // Stop Seeker from moving for 5 seconds for both CharacterMovement.cs and MoveToPosition.cs
            if (other.gameObject.GetComponent<CharacterMovement>() != null)
            {
                other.gameObject.GetComponent<CharacterMovement>().speed = 0;
            }

            // FOR NPC SEEKER, NOT WORKING
            // if (other.gameObject.GetComponent<MoveToPosition>() != null)
            // {
            //     // other.gameObject.GetComponent<MoveToPosition>().agent.SetDestination(other.gameObject.transform.position);
            //     other.gameObject.GetComponent<MoveToPosition>().enabled = false;
            // }

            // FOR NPC SEEKER, NOT WORKING
            // GameObject seekerObj = GameObject.Find("V3(Clone)");
            // seekerObj.GetComponent<MoveToPosition>().enabled = false;

            if (freezeRespawn)
            {
                Invoke("Respawn", freezeRespawnTime);
            }
        }
    }

    void Respawn()
    {
        // yield return new WaitForSeconds(6);
        gameObject.GetComponent<MeshRenderer>().enabled = true;
        gameObject.GetComponent<Collider>().enabled = true;
    }
}
