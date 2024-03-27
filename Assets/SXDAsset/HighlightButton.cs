// HighlightButton.cs
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HighlightButton : MonoBehaviour
{
    private Color originalColor;
    public Color highlightColor = Color.red;
    
    private Button button;

    private void Start()
    {
        button = GetComponent<Button>();
        originalColor = button.image.color; // Store the original color of the button
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        button.image.color = highlightColor; // Change color on hover
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        button.image.color = originalColor; // Change color back on exit
    }
}
