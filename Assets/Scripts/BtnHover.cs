using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BtnHover : MonoBehaviour
{
    public GameObject character;
    public Transform rp;
    // public 
    
    // Button from canvas panel
    // public GameObject btn;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // While character is looking at the button, change color to yellow
        RaycastHit hit;
        if (Physics.Raycast(rp.position, rp.forward, out hit))
        {
            if (hit.collider.gameObject == gameObject)
            {
                // btn.GetComponent<Renderer>().material.color = Color.yellow;
                // Debug.Log("Looking at button");
            }
            else
            {
                // btn.GetComponent<Renderer>().material.color = Color.white;
            }
        }
        else
        {
            // btn.GetComponent<Renderer>().material.color = Color.white;
        }
    }
}
