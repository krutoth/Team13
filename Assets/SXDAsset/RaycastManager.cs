// RaycastManager.cs
using UnityEngine;
using UnityEngine.UI;

public class RaycastManager : MonoBehaviour
{
    public LayerMask interactableLayer; 
    public Camera vrCamera; 
    public float maxRayDistance = 100f; // Max distance for the raycast.
    public Color highlightColor = Color.red; // Color to highlight the buttons.
    public GameObject SettingsMenu; // Make sure this is the Settings Menu GameObject
    public SettingsMenuController settingsMenuController;

    private GameObject currentSelectedObject;
    private Button highlightedButton; // Currently highlighted button
    private Color originalColor; // Store the original color of the button
    private GameObject objectToManipulate; // The object that will be copied or cut.

    void Update()
    {
        RaycastForInteractable();

        // Check for interaction input if we are not interacting with buttons
        // X Button
        if ((Input.GetButtonDown("js2") || Input.GetButtonDown("js24")) && currentSelectedObject && highlightedButton == null)
        {
            InteractableMenu menu = currentSelectedObject.GetComponent<InteractableMenu>();
            if (menu)
            {
                ToggleMenu(menu);
            }
        }

        // New placement logic
        // A Button
        if ((Input.GetButtonDown("js10") || Input.GetButtonDown("js21")) && objectToManipulate != null)
        {
            // Logic to place the object at the desired location
            // For example, let's place it in front of the camera.
            Vector3 positionInFrontOfCamera = vrCamera.transform.position + vrCamera.transform.forward * 2f; // 2 meters in front of the camera
            objectToManipulate.transform.position = positionInFrontOfCamera;
            
            // Reactivate the object if it was cut (inactive)
            if (!objectToManipulate.activeSelf)
            {
                objectToManipulate.SetActive(true);
            }

            // Clear the reference to avoid repeated placement
            objectToManipulate = null;
        }

        // New logic for showing settings menu with "js3" button
        // OK button
        if (Input.GetButtonDown("js0") || Input.GetButtonDown("js17"))
        {
            // Toggle the settings menu visibility
            if (SettingsMenu != null)
            {
                bool isActive = SettingsMenu.activeSelf;
                SettingsMenu.SetActive(!isActive);

                if (!isActive) // If we are showing the menu
                {
                    // Calculate a new position in front of the camera, slightly lowered and to the left
                    Vector3 forwardOffset = vrCamera.transform.forward * 6f; // 2 meters in front of the camera
                    Vector3 downwardOffset = vrCamera.transform.up * -1.3f; // 0.5 meters down
                    Vector3 leftwardOffset = vrCamera.transform.right * -2f; // Slightly to the left
                    Vector3 positionInFrontOfCamera = vrCamera.transform.position + forwardOffset + downwardOffset + leftwardOffset;

                    // Set the menu to face the user. The forward vector of the menu should directly oppose the camera's forward vector
                    Quaternion rotationFacingUser = Quaternion.Euler(0f, vrCamera.transform.eulerAngles.y, 0f);

                    // Update the position and rotation of the settings menu
                    SettingsMenu.transform.position = positionInFrontOfCamera;
                    SettingsMenu.transform.rotation = rotationFacingUser;
                }
            }
        }

        // Check if 'js0' is pressed for teleportation
        // Y Button
        if (Input.GetButtonDown("js3") || Input.GetButtonDown("js20"))
        {
            TeleportToRaycastHit();
        }
    }

    // Modified CopyObject method
    private void CopyObject(GameObject objectToCopy)
    {
        // Instantiate and copy the object
        GameObject copiedObject = Instantiate(objectToCopy, objectToCopy.transform.position, objectToCopy.transform.rotation);
        copiedObject.name = objectToCopy.name + "_copy";
        
        // Store the copied object for potential manipulation
        objectToManipulate = copiedObject;
    }

    // Modified CutObject method
    private void CutObject(GameObject objectToCut)
    {
        // Deactivate the object to simulate 'cutting'
        objectToCut.SetActive(false);
        
        // Store the cut object for potential manipulation
        objectToManipulate = objectToCut;
    }

    private void TeleportToRaycastHit()
    {
        Ray ray = new Ray(vrCamera.transform.position, vrCamera.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, maxRayDistance))
        {
            // Teleport the player to the hit location, with some offset if needed
            // For instance, this offsets the player's position slightly above the hit point to avoid clipping into the ground.
            Vector3 teleportPosition = hit.point + new Vector3(0, 1.6f, 0); // Adjust the Y offset as needed
            
            // Assuming you have a reference to the player or the object you wish to move. 
            // This might be the VR camera's parent object if the VR setup involves a player container for movement.
            vrCamera.transform.parent.position = teleportPosition;
        }
    }

    private void RaycastForInteractable()
    {
        Ray ray = new Ray(vrCamera.transform.position, vrCamera.transform.forward);
        RaycastHit hit;

        // Perform the raycast
        if (Physics.Raycast(ray, out hit, maxRayDistance, interactableLayer))
        {
            // Attempt to get a Button component from the hit object
            Button button = hit.transform.GetComponent<Button>();
            if (button != null)
            {
                HighlightButton(button);

                // B Button
                // Determine if we've hit the 'Copy' or 'Cut' button
                if (button.CompareTag("CopyButton") && (Input.GetButtonDown("js5") || Input.GetButtonDown("js15")))
                {
                    // If we hit the 'Copy' button, we want to copy the table, which is the parent object
                    CopyObject(hit.transform.parent.parent.parent.gameObject);
                }
                else if (button.CompareTag("CutButton") && (Input.GetButtonDown("js5") || Input.GetButtonDown("js15")))
                {
                    // If we hit the 'Cut' button, we want to cut the table, which is the parent object
                    CutObject(hit.transform.parent.parent.parent.gameObject);
                }
                    // Check if the 'SpeedButton' is hit and 'js10' is pressed
                else if (button.CompareTag("SpeedButton") && (Input.GetButtonDown("js5") || Input.GetButtonDown("js15")))
                {
                    // Call the method to handle speed change
                    settingsMenuController.ToggleSpeed();
                }
                // Check if the 'Raycast Length' button is hit and 'js10' is pressed
                if (button.CompareTag("RaycastLengthButton") && (Input.GetButtonDown("js5") || Input.GetButtonDown("js15")))
                {
                    settingsMenuController.ToggleRaycastLength();
                }
                if (button.CompareTag("QuitButton") && (Input.GetButtonDown("js5") || Input.GetButtonDown("js15")))
                {
                    settingsMenuController.QuitGame();
                }                                
            }
            else
            {
                // If the hit object is not a button, remove the highlight.
                RemoveHighlight();
            }

            // Store the current object as the selected one for other interactions.
            currentSelectedObject = hit.transform.gameObject;

            // B Button
            // If we hit the 'Exit' button, trigger the menu to hide
            if (hit.transform.CompareTag("ExitButton") && (Input.GetButtonDown("js5") || Input.GetButtonDown("js15")))
            {
                InteractableMenu menu = hit.transform.GetComponentInParent<InteractableMenu>();
                if (menu)
                {
                    menu.HideMenu();
                }
            }
        }
        else
        {
            // If the ray doesn't hit anything, remove the highlight and clear the selection.
            RemoveHighlight();
            currentSelectedObject = null;
        }
    }

    private void HighlightButton(Button button)
    {
        // If it's a new button, change the previous button's color back.
        if (highlightedButton != button)
        {
            if (highlightedButton != null)
            {
                // Reset the previous highlighted button's color.
                highlightedButton.image.color = originalColor;
            }

            // Highlight the new button.
            originalColor = button.image.color; // Remember the original color.
            button.image.color = highlightColor;
            highlightedButton = button;
        }
    }

    private void RemoveHighlight()
    {
        if (highlightedButton != null)
        {
            highlightedButton.image.color = originalColor; // Reset the button's color.
            highlightedButton = null;
        }
    }

    // This new method is used to toggle the visibility of the menu
    private void ToggleMenu(InteractableMenu menu)
    {
        if (menu.menuCanvas.activeSelf)
        {
            menu.HideMenu();
        }
        else
        {
            menu.ShowMenu();
        }
    }

}


