using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

// Implement Raycast. The raycast should acts as a forward vector
// positioned at the center of the screen, serving as a pointer to select
// various objects in the scene.
public class VRLook : MonoBehaviour
{
    Ray ray;
    RaycastHit hit;
    Camera cam;
    // Add a line renderer to visualize the raycast
    [SerializeField] LineRenderer lineRenderer;
    public CharacterMovement characterMovement;
    float oldSpeed;
    bool menuOpen = false;
    // Keep track of current menu object
    GameObject prevMenu = null;

    // Interactable currentObj = null;
    GameObject currentObj = null;

    GraphicRaycaster gr;
    PointerEventData pointerData;


    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
        oldSpeed = characterMovement.speed;
        gr = GetComponent<GraphicRaycaster>();
    }

    // Update is called once per frame
    void Update()
    {
        
        StartRaycast();
        // StartRaycastUI();
        
        // StartRaycastUI();
        if (menuOpen)
        {
            // Disable Character movement by changing the speed to 0 in CharacterMovement.cs
            characterMovement.speed = 0;

        }
        else
        {
            // Enable Character movement by changing the speed to 1 in CharacterMovement.cs
            characterMovement.speed = oldSpeed;
        }
    }

    // Implement the Physics Raycast function for objects
    void StartRaycast()
    {
        
        // Create a ray from the camera to the center of the screen
        // ray = cam.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        // offset the ray below camera to avoid clipping
        ray = new Ray(cam.transform.position, cam.transform.forward);
        // ray = new Ray(cam.transform.position + cam.transform.forward * 0.1f, cam.transform.forward);


        // Draw a line from the camera to object/end of screen, do not redraw if aiming in same direction
        if (Physics.Raycast(ray, out hit))
        {
            // Draw a line from the camera to the object hit by the raycast
            // offset the line below camera to avoid clipping, horizontally lower than the camera
            // lineRenderer.SetPosition(0, cam.transform.position);
            lineRenderer.SetPosition(0, cam.transform.position + cam.transform.forward * 0.1f);
            lineRenderer.SetPosition(1, hit.point);

            // Check if object hit by raycast is interactable (The assignment objects)
            // Assignment Obj: Cube1, Cube2, Cube3, Sphere1, Sphere2, Sphere3

            // Interactable obj = hit.collider.GetComponent<Interactable>();
            string gazedObjectName = hit.collider.gameObject.name;
            GameObject temp = GameObject.Find(gazedObjectName);
            
            // if (obj) 
            // {
            //     currentObj = obj;

            // }
            // else 
            // {
            //     currentObj = null;
            // }

            if (temp)
            {
                currentObj = temp;
            }
            else
            {
                currentObj = null;
            }

            // Open menu
            if (Input.GetButton("js1") || Input.GetButton("js3") || 
                Input.GetButton("js24") || Input.GetKeyDown(KeyCode.X) && 
                currentObj)
            {
                // disable Character movement by changing the speed to 0 in CharacterMovement.cs
                // characterMovement.speed = 0;

                if (!menuOpen)
                {
                    // Enable Menu child object
                    currentObj.transform.GetChild(0).gameObject.SetActive(true);
                    menuOpen = true;
                    prevMenu = currentObj;
                }
                else
                {
                    // Disable Menu child object
                    prevMenu.transform.GetChild(0).gameObject.SetActive(false);
                    currentObj.transform.GetChild(0).gameObject.SetActive(true);
                    prevMenu = currentObj;
                    // menuOpen = false;
                }
                
                // Enable Menu child object
                // currentObj.transform.GetChild(0).gameObject.SetActive(true);

            }

            // Activate buttons when raycast points at them
            if (Input.GetButton("js1") || Input.GetButton("js3") || 
                Input.GetButton("js24") || Input.GetKeyDown(KeyCode.X))
            {
                pointerData = new PointerEventData(EventSystem.current);


                switch (gazedObjectName)
                {
                    case "cpyBtn":

                        break;
                    case "cutBtn":

                        break;
                    case "exitBtn":

                        break;
                }
            }
        }
        else
        {
            currentObj = null;
        }

        

        // Change button color when raycast points at them
        
        
    }



    // Implement the Graphics Raycast function for UI elements
    void StartRaycastUI()
    {
        // Create a ray from the camera to the center of the screen
        // ray = cam.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        ray = new Ray(cam.transform.position, cam.transform.forward);

        // Create a list of Raycast Results
        List<RaycastResult> results = new List<RaycastResult>();

        // Create a new Pointer Event Data
        PointerEventData ped = new PointerEventData(EventSystem.current);
        // ped.position = Input.mousePosition;

        // Raycast using the Graphics Raycaster and the Pointer Event Data
        gr.Raycast(ped, results);

        string gazedObjectName = hit.collider.gameObject.name;
        GameObject temp = GameObject.Find(gazedObjectName);

        if (temp)
        {
            currentObj = temp;
        }
        else
        {
            currentObj = null;
        }

        // Loop through the Raycast Results
        for (int i = 0; i < results.Count; i++)
        {
            // Debug.Log("Hit " + results[i].gameObject.name);
            // Check if the X button is pressed
            if (Input.GetButton("js1") || Input.GetButton("js3") || 
                Input.GetButton("js24") || Input.GetKeyDown(KeyCode.X))
            {
                switch (gazedObjectName)
                {
                    case "cpyBtn":
                        break;
                    case "cutBtn":

                        break;
                    case "exitBtn":

                        break;
                }

            }
        }
    }


}
