using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Raycast : MonoBehaviour
{
    public LineRenderer lineRenderer;
    public Camera mainCamera;
    public GameObject playerObject;
    public float maxDistance;
    private GameObject lastHitObject;
    private Transform Door;
    private Transform leftDoor;
    private Transform rightDoor;
    private Transform SlideDoor;
    private GameObject hider;
    private GameObject seeker;
    private bool isHidden = false;
    private Vector3 lastPosition;
    public GameObject menu;

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        mainCamera = Camera.main;

        lineRenderer.startWidth = 0.001f;
        lineRenderer.endWidth = 0.001f;
    }

    void Update()
    {
        Vector3 rayOrigin = mainCamera.transform.position - mainCamera.transform.up * 0.025f;
        Ray ray = new Ray(rayOrigin, mainCamera.transform.forward);

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, maxDistance))
        {
            DrawRay(ray.origin, hit.point);
            if (lastHitObject != hit.collider.gameObject)
            {
                RestoreOriginalColor();
                lastHitObject = hit.collider.gameObject;
            }

            if (hit.collider.CompareTag("Button"))
            {
                string objectName = hit.collider.gameObject.name;
                if (objectName == "Resume")
                {
                    // Use Y Button
                    // Kuei-Yu: js11
                    // Ryan: js20
                    // Android: js0
                    if (Input.GetAxis("js0") != 0 || Input.GetAxis("js11") != 0 || Input.GetAxis("js20") != 0 || Input.GetKeyDown(KeyCode.Y))
                    {
                        menu.SetActive(false);
                        CharacterMovement targetScript = playerObject.GetComponent<CharacterMovement>();
                        targetScript.enabled = true;
                    }
                }
                else if (objectName == "Exit")
                {
                    if (Input.GetAxis("js0") != 0 || Input.GetAxis("js11") != 0 || Input.GetAxis("js20") != 0 || Input.GetKeyDown(KeyCode.Y))
                    {
                        Application.Quit();
                    }
                }
            }

            if (!hit.collider.gameObject.GetComponent<Outline>() && !hit.collider.CompareTag("Ground") && !hit.collider.CompareTag("Untagged"))
            {
                var outline = hit.collider.gameObject.AddComponent<Outline>();
                outline.OutlineMode = Outline.Mode.OutlineAll;
                outline.OutlineColor = Color.yellow;
                outline.OutlineWidth = 10f;
            }

            // Use Y Button
            // Kuei-Yu: js11
            // Ryan: js20
            // Android: js0
            if (Input.GetAxis("js0") != 0 || Input.GetAxis("js11") != 0 || Input.GetAxis("js20") != 0 || Input.GetKeyDown(KeyCode.Y))
            {
                string gazedObjectName = hit.collider.gameObject.name;
                GameObject temp = GameObject.Find(gazedObjectName);

                // If object is a door (tagged as "Door")
                if (hit.collider.CompareTag("Door"))
                {
                    // Play object's audio source
                    AudioSource audioSourceToUse = temp.GetComponent<AudioSource>();
                    if (audioSourceToUse)
                    {
                        audioSourceToUse.Play();
                    }

                    Door = temp.transform.Find("Door").transform;
                    RotateDoors();
                }
                else if (hit.collider.CompareTag("Main Door"))
                {
                    AudioSource audioSourceToUse = temp.GetComponent<AudioSource>();
                    if (audioSourceToUse)
                    {
                        audioSourceToUse.Play();
                    }

                    leftDoor = temp.transform.Find("Left Door").transform;
                    rightDoor = temp.transform.Find("Right Door").transform;
                    MainDoors();
                }
                else if (hit.collider.CompareTag("Slide Door"))
                {
                    // Play object's audio source
                    AudioSource audioSourceToUse = temp.GetComponent<AudioSource>();
                    if (audioSourceToUse)
                    {
                        audioSourceToUse.Play();
                    }

                    SlideDoor = temp.transform.Find("Slide Door").transform;

                    SlideDoors();
                }
                else if (hit.collider.CompareTag("Hide Place"))
                {
                    Vector3 bedPosition = hit.collider.gameObject.transform.position;
                    Hide(bedPosition);
                }
                else if (hit.collider.CompareTag("Hider"))
                {
                    // Play object's audio source
                    // AudioSource audioSourceToUse = temp.GetComponent<AudioSource>();
                    // audioSourceToUse.Play();

                    // Modify transform to gazed object
                    hider = temp;

                    Hider();
                }
                else if (hit.collider.CompareTag("Seeker"))
                {
                    // Play object's audio source
                    // AudioSource audioSourceToUse = temp.GetComponent<AudioSource>();
                    // audioSourceToUse.Play();

                    // Modify transform to gazed object
                    seeker = temp;

                    // StartCoroutine(Seeker());
                    Seeker();
                }
            }
        }
        else
        {
            DrawRay(ray.origin, ray.origin + ray.direction * maxDistance);
            RestoreOriginalColor();
            lastHitObject = null;
        }
        // B Button on joystick to open menu
        // Kuei-Yu: js7
        // Ryan: js15
        // Android: js2
        if ((Input.GetAxisRaw("js2") != 0 || Input.GetAxisRaw("js7") != 0 || Input.GetAxisRaw("js15") != 0 || Input.GetKeyDown(KeyCode.B)) && !menu.activeSelf)
        {
            Vector3 menuPosition = mainCamera.transform.position + mainCamera.transform.forward * 0.1f;
            menu.transform.position = menuPosition;
            menu.transform.rotation = Camera.main.transform.rotation;
            menu.SetActive(true);
            CharacterMovement targetScript = playerObject.GetComponent<CharacterMovement>();
            targetScript.enabled = false;
        }
    }

    void RotateDoors()
    {
        Vector3 DoorRotate = new Vector3(0, 0, 90);
        if (Door.localRotation == Quaternion.Euler(-90, 0, 0))
        {
            Door.localRotation *= Quaternion.Euler(DoorRotate);
        }
        else
        {
            Door.localRotation *= Quaternion.Euler(-DoorRotate);
        }

        // start an audio source
        // AudioSource audioSource = GetComponent<AudioSource>();
        // audioSource.Play();
    }

    void MainDoors()
    {
        Vector3 DoorRotate = new Vector3(0, 0, 90);
        if (leftDoor.localRotation == Quaternion.Euler(-90, 0, 0))
        {
            leftDoor.localRotation *= Quaternion.Euler(-DoorRotate);
            rightDoor.localRotation *= Quaternion.Euler(DoorRotate);
        }
        else
        {
            leftDoor.localRotation *= Quaternion.Euler(DoorRotate);
            rightDoor.localRotation *= Quaternion.Euler(-DoorRotate);
        }

        // start an audio source
        // AudioSource audioSource = GetComponent<AudioSource>();
        // audioSource.Play();
    }

    void SlideDoors()
    {
        Vector3 DoorMove = new Vector3(0, 0, 2);
        if (SlideDoor.localPosition == new Vector3(0, 0, 0))
        {
            SlideDoor.localPosition -= DoorMove;
        }
        else
        {
            SlideDoor.localPosition += DoorMove;
        }
    }

    void Hide(Vector3 bedPosition)
    {
        if (!isHidden)
        {
            lastPosition = playerObject.transform.position;
            CharacterMovement targetScript = playerObject.GetComponent<CharacterMovement>();
            targetScript.enabled = false;
            Vector3 offset = new Vector3(0f, -0.5f, 0f);
            playerObject.transform.position = bedPosition + offset;
            isHidden = true;
        }
        else
        {
            CharacterMovement targetScript = playerObject.GetComponent<CharacterMovement>();
            targetScript.enabled = true;
            playerObject.transform.position = lastPosition;
            isHidden = false;
        }
    }

    void Hider()
    {
        // Disable the hider
        hider.SetActive(false);
    }

    void Seeker()
    {
        // test disable mesh renderer of seeker
        // seeker.GetComponent<MeshRenderer>().enabled = false;

        // Temporarily freeze movement of seeker for 5 seconds
        // disable MoveToPosition script
        StartCoroutine(waiter());

    }

    IEnumerator waiter()
    {
        seeker.GetComponent<UnityEngine.AI.NavMeshAgent>().enabled = false;
        yield return new WaitForSeconds(5);
        seeker.GetComponent<UnityEngine.AI.NavMeshAgent>().enabled = true;
    }

    void DrawRay(Vector3 start, Vector3 end)
    {
        lineRenderer.positionCount = 2;
        lineRenderer.SetPosition(0, start);
        lineRenderer.SetPosition(1, end);
    }

    private void RestoreOriginalColor()
    {
        if (lastHitObject != null)
            Destroy(lastHitObject.GetComponent<Outline>());
    }
}
