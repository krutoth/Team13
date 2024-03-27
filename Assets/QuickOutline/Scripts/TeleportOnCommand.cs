using UnityEngine;

public class TeleportOnCommand : MonoBehaviour
{
    public Transform playerTransform;
    private Outline outline;
    private Rigidbody playerRigidbody;

    void Start()
    {
        outline = GetComponent<Outline>();
        playerRigidbody = playerTransform.GetComponent<Rigidbody>();

        if (outline == null)
        {
            Debug.LogError("Outline component not found on the sphere!", this);
        }
        if (playerTransform == null)
        {
            Debug.LogError("Player Transform is not assigned in the TeleportOnCommand script.", this);
        }
        if (playerRigidbody == null)
        {
            Debug.LogError("Rigidbody component not found on the player!", this);
        }
    }

    void Update()
    {
        if (outline != null && outline.enabled)
        {
            if (Input.GetButtonDown("js3"))
            {
                Debug.Log("Teleport button pressed and sphere is highlighted.", this);
                playerTransform.position = transform.position; // Teleport the player
                gameObject.SetActive(false); // Disable the sphere

                // If there's a rigidbody, wake it up
                if (playerRigidbody != null)
                {
                    playerRigidbody.WakeUp();
                }
            }
        }
    }
}
