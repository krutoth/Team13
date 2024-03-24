using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutlineHover : MonoBehaviour
{
    public Outline outline;
    public Transform reticlePointer;
    // public GameObject character;

    // Start is called before the first frame update
    void Start()
    {
        outline.OutlineColor = Color.green;
        outline.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        if (Physics.Raycast(reticlePointer.position, reticlePointer.forward, out hit))
        {
            if (hit.collider.gameObject == gameObject)
            {
                outline.enabled = true;
            }
            else
            {
                outline.enabled = false;
            }
        }
        else
        {
            outline.enabled = false;
        }
    }
}
