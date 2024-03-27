using UnityEngine;

public class RotateOnCommand : MonoBehaviour
{
    public float speed = 90f; // Degrees per second
    private Outline outline;

    void Start()
    {
        outline = GetComponent<Outline>();
    }

    void Update()
    {
        if (outline.enabled && Input.GetButton("js2"))
        {
            transform.Rotate(Vector3.up, speed * Time.deltaTime);
        }
    }
}
