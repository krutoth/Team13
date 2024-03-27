using UnityEngine;
using UnityEngine.UI;
using TMPro; // Namespace for TextMeshPro

public class SettingsMenuController : MonoBehaviour
{
    public Raycaster raycaster; // Add a reference to the Raycaster script

    public CharacterMovement characterMovementScript; // Reference to the CharacterMovement script
    public RaycastManager raycastManager; // Reference to the RaycastManager

    // The actual ray lengths you want to use
    private float[] rayLengthOptions = { 5f, 50f, 1000f };
    // The corresponding text values you want to display
    private string[] rayLengthDisplayOptions = { "1m", "10m", "50m" };

    // Options for speed and raycast length
    private float[] speedOptions = { 50f, 10f, 2f };
    private int currentSpeedIndex = 1; // Start with Medium speed

    // Options for raycast length and corresponding display strings
    private float[] raycastLengthOptions = { 5f, 50f, 1000f };
    private string[] raycastLengthDisplay = { "1m", "10m", "50m" };
    private int currentRaycastLengthIndex = 0; // Start with 5f, which is displayed as "1m"

    // UI buttons
    public Button resumeButton;
    public Button speedButton;
    public Button raycastLengthButton;
    public Button quitButton;

    private void Awake()
    {
        // Hide the settings menu at the start
        gameObject.SetActive(false);
    }

    void Start()
    {
        // Attach button listeners
        resumeButton.onClick.AddListener(() => ToggleMenu(false));
        speedButton.onClick.AddListener(ToggleSpeed);
        raycastLengthButton.onClick.AddListener(ToggleRaycastLength); // Make sure you have this method implemented
        quitButton.onClick.AddListener(QuitGame);

        // Initialize button texts
        UpdateSpeedButtonText();
        UpdateRaycastLengthButtonText();
        // Initialize other button texts if necessary
    }

    void ToggleMenu(bool show)
    {
        gameObject.SetActive(show);
    }

    public void ToggleSpeed()
    {
        // Cycle through the speed options
        currentSpeedIndex = (currentSpeedIndex + 1) % speedOptions.Length;
        characterMovementScript.speed = speedOptions[currentSpeedIndex];
        UpdateSpeedButtonText();
    }

    void UpdateSpeedButtonText()
    {
        string speedText = GetSpeedText(currentSpeedIndex);
        speedButton.GetComponentInChildren<TextMeshProUGUI>().text = $"Speed: {speedText}";
    }

    string GetSpeedText(int index)
    {
        switch (index)
        {
            case 0: return "High";
            case 1: return "Medium";
            case 2: return "Low";
            default: return "Unknown";
        }
    }

    public void ToggleRaycastLength()
    {
        // Cycle through the raycast length options
        currentRaycastLengthIndex = (currentRaycastLengthIndex + 1) % raycastLengthOptions.Length;
        // Update the RaycastManager's maxRayDistance with the new value
        if (raycastManager != null)
        {
            raycastManager.maxRayDistance = raycastLengthOptions[currentRaycastLengthIndex];
        }
        // Update the raycaster's ray length
        if (raycaster != null)
        {
            raycaster.rayLength = rayLengthOptions[currentRaycastLengthIndex];
        }
        
        // Update the button text
        UpdateRaycastLengthButtonText();
    }

    void UpdateRaycastLengthButtonText()
    {
        // Set the button's TextMeshPro text to the corresponding display string
        raycastLengthButton.GetComponentInChildren<TextMeshProUGUI>().text = $"Raycast Length: {raycastLengthDisplay[currentRaycastLengthIndex]}";
    }

    public void QuitGame()
    {
        // Quit the application
        Application.Quit();
    }
}
