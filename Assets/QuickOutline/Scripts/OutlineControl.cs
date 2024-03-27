using UnityEngine;

public class OutlineControl : MonoBehaviour
{
    private Outline outline;

    // Start is called before the first frame update
    void Start()
    {
        // Try to get the Outline component on the object
        outline = GetComponent<Outline>();
        // Initially disable the outline
        if (outline != null)
        {
            outline.enabled = false;
        }
    }

    // // When the pointer enters the collider of the object
    // void OnMouseEnter()
    // {
    //     if (outline != null)
    //     {
    //         outline.enabled = true;
    //     }
    // }

    // // When the pointer exits the collider of the object
    // void OnMouseExit()
    // {
    //     if (outline != null)
    //     {
    //         outline.enabled = false;
    //     }
    // }
}
