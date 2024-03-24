using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Unity VR Reticle Input
// This script is used to enable and disable the Outline.cs script component to control the highlight effects on objects
// When the reticle pointer points at any interactable object, the object will be highlighted with an outline color on each object
// When the reticle pointer is not pointing at any interactable object, the outline will be disabled
public class ReticleInput : MonoBehaviour
{
    // public Outline outline;
    // public CharacterController controller;
    public GameObject character;
    public Transform reticlePointer;

    // current object color
    private Color cubeOriginalColor;

    private void Start()
    {
        // outline = GetComponent<Outline>();
        // outline.enabled = false;
        // Cube 3 original color
        cubeOriginalColor = GameObject.Find("Cube3").GetComponent<Renderer>().material.color;
        // controller = GetComponent<CharacterController>();
    }

    /*Translation: When the pointer points at Cube1 and the button “X” is pressed, the cube
    should move in any one direction on the x-axis. Cube1 should be continuously moved
    while the pointer points at the cube and the button is pressed and held. Otherwise, the
    movement of Cube1 should be stopped.*/
    /*Rotation: When the pointer points at Cube2 and the button “X” is pressed, the cube
    should be rotated in any one direction. Cube2 should be continuously rotated while the
    pointer points at the cube and the button is pressed and held. Otherwise, the rotation of
    Cube2 should be stopped*/
    /*Change Color: When the pointer points at Cube3 and the button “X” is pressed, the
    color of the cube should be changed to a different color (doesn’t matter which color if
    it is different from the original color). When the button is pressed again, the color of
    the cube should be changed to the original color. */
    /*Teleportation: When the pointer points at any sphere and the button “Y” is pressed, the
    user should be teleported to the sphere location and the sphere should disappear.*/
 

    public void Update()
    {
        // Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));

        RaycastHit hit;
        // Perform the raycast
        if (Physics.Raycast(reticlePointer.position, reticlePointer.forward, out hit))
        {
            string gazedObjectName = hit.collider.gameObject.name;
            //Debug.Log("Gazed object name: " + gazedObjectName);
            GameObject temp = GameObject.Find(gazedObjectName);

            if (Input.GetButton("js1") || Input.GetButton("js3") || Input.GetButton("js24") || Input.GetKeyDown(KeyCode.X))
            {
                switch (gazedObjectName)
                {
                    case "Cube1":
                        // move Cube1 in any one direction on the x-axis
                        temp.transform.Translate(Vector3.right * Time.deltaTime);

                        break;
                    case "Cube2":
                        // rotate Cube2 in any one direction
                        temp.transform.Rotate(Vector3.up * Time.deltaTime * 100);
                        break;
                    case "Cube3":
                        // change color of Cube3 and revert back to original color if clicked again

                        if (temp.GetComponent<Renderer>().material.color == cubeOriginalColor)
                        {
                            temp.GetComponent<Renderer>().material.color = Color.red;
                        }
                        else
                        {
                            temp.GetComponent<Renderer>().material.color = cubeOriginalColor;
                        }

                        // Time delay for button press due to faulty controller
                        Invoke("ResetColor", 1);
                        
                        break;
                }
            }
            else if (Input.GetButton("js0") || Input.GetButton("js2") || Input.GetButton("js5") || Input.GetKeyDown(KeyCode.Y))
            {
            // For all spheres, destroy the sphere
            // After sphere is destroyed, teleport the user to the sphere location
                switch (gazedObjectName)
                {
                    case "Sphere1":

                        // break;

                    case "Sphere2":
                        // Destroy(gameObject);
                        // break;
                        
                    case "Sphere3":
                        // Get sphere location
                        // Destroy sphere
                        // Teleport user to sphere location

                        Vector3 sphereLocation = temp.transform.position;
                        Destroy(temp);
                        // Destroy(gameObject);

                        // Disable character controller component
                        // Set transform of the player
                        // Enable character controller component

                        // CharacterController controller = GetComponent<CharacterController>();
                        // controller.enabled = false;
                        character.transform.position = sphereLocation;
                        // controller.enabled = true;
                        break;
                }
            }
            
        }

    }
    
}
