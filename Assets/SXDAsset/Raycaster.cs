// Raycaster.cs
using UnityEngine;

public class Raycaster : MonoBehaviour
{
    public float rayLength = 10f; 
    public LayerMask interactableLayer; 
    public float rayStartDistance = 0.05f; // Distance from the camera to avoid clipping
    public float rayStartHeight = -0.1f; // Height offset to start the ray below the camera

    private LineRenderer lineRenderer; // This will visually represent the ray in the scene

    void Start()
    {
        // Add or retrieve a LineRenderer component
        lineRenderer = gameObject.GetComponent<LineRenderer>();
        if (lineRenderer == null)
        {
            lineRenderer = gameObject.AddComponent<LineRenderer>();
        }

        // Set up the LineRenderer appearance
        lineRenderer.startWidth = 0.01f; // Start width of the line
        lineRenderer.endWidth = 0.01f; // End width of the line
        lineRenderer.positionCount = 2; // The line will consist of two points
        lineRenderer.material = new Material(Shader.Find("Unlit/Color"));
        lineRenderer.material.color = Color.red; // This makes the line red, adjust as needed
        lineRenderer.alignment = LineAlignment.TransformZ; // Maintain consistent width in VR

        // Enable the line renderer by default
        lineRenderer.enabled = true;
    }

    void Update()
    {
        // Calculate the start position of the ray to be below the camera
        Vector3 rayStart = transform.position + (transform.forward * rayStartDistance) + (transform.up * rayStartHeight);
        Ray ray = new Ray(rayStart, transform.forward);
        RaycastHit hit;

        // Set the start position of the line to the calculated start position
        lineRenderer.SetPosition(0, rayStart);

        if (Physics.Raycast(ray, out hit, rayLength, interactableLayer))
        {
            // If the ray hits an interactable object, set the line end to the hit point
            lineRenderer.SetPosition(1, hit.point);
            // Implement interaction logic here for when the object is hit
        }
        else
        {
            // If no object is hit, set the line to the maximum length
            lineRenderer.SetPosition(1, rayStart + transform.forward * rayLength);
        }
    }
}
