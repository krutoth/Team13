using UnityEngine;

public class ChangeColorOnCommand : MonoBehaviour
{
    private Color originalColor;
    public Color changeToColor = Color.blue;
    private bool isOriginalColor = true;
    private Outline outline;
    private Renderer renderer;

    void Start()
    {
        renderer = GetComponent<Renderer>();
        originalColor = renderer.material.color;
        outline = GetComponent<Outline>();
    }

    void Update()
    {
        if (outline.enabled && Input.GetButtonDown("js2"))
        {
            renderer.material.color = isOriginalColor ? changeToColor : originalColor;
            isOriginalColor = !isOriginalColor;
        }
    }
}
