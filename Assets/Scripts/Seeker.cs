using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seeker : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        // Check if the colliding GameObject has the "Hider" tag
        if (other.gameObject.CompareTag("Hider"))
        {
            // Execute your desired action here
            other.gameObject.tag = "Touched";
            if(other.gameObject.name.Contains("(Clone)"))
            {
                other.gameObject.SetActive(false);
            }
        }
    }
}
