using UnityEngine;

public class TranslateOnCommand : MonoBehaviour
{
    public float speed = 5f;
    private Outline outline;

    void Start()
    {
        outline = GetComponent<Outline>();
    }

    void Update()
    {
        if (outline.enabled && Input.GetButton("js2"))
        {
            transform.Translate(Vector3.right * speed * Time.deltaTime);
        }
    }
}
