// InteractableMenu.cs
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InteractableMenu : MonoBehaviour
{
    public GameObject menuCanvas;
    public Button copyButton;
    public Button cutButton;
    public Button exitButton;

    private void Awake()
    {
        // Initially hide the menu
        menuCanvas.SetActive(false);

        // Setup button listeners
        copyButton.onClick.AddListener(CopyObject);
        cutButton.onClick.AddListener(CutObject);
        exitButton.onClick.AddListener(HideMenu);
    }

    // Call this from the raycast script when the object is selected
    public void ShowMenu()
    {
        menuCanvas.SetActive(true);
        // Disable character movement here
    }

    public void HideMenu()
    {
        menuCanvas.SetActive(false);
        // Enable character movement here
    }

    private void CopyObject()
    {
        Debug.Log("Copy functionality here");
    }

    private void CutObject()
    {
        Debug.Log("Cut functionality here");
    }
}