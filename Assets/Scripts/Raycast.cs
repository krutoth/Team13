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
    private Quaternion lastRotation;
    public GameObject startMenu;
    public GameObject roleMenu;
    public GameObject endMenu;
    public GameObject menu;
    public TextMeshProUGUI circumstanceText;
    public TextMeshProUGUI countdownText;
    public TextMeshProUGUI statusText;
    private float timeRemaining;
    private bool countdownStarted = false;
    public GameObject Gate; // Main gate
    public GameObject playerVisual;
    public GameObject hiderVisual;
    public GameObject seekerVisual;
    private bool end = true;
    public int maxNPCs = 1; // The maximum number of NPCs allowed
    private Transform[] hidePlaces; // Array to store hide place positions
    private GameObject[] currentHider;

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        mainCamera = Camera.main;

        lineRenderer.startWidth = 0.001f;
        lineRenderer.endWidth = 0.001f;

        openMenu(startMenu);
        CharacterMovement targetScript = playerObject.GetComponent<CharacterMovement>();
        targetScript.enabled = false;
    }

    void Update()
    {
        currentHider = GameObject.FindGameObjectsWithTag("Hider");
        if (currentHider.Length == 0 && !end)
        {
            gameEnd(true);
        }

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
                else if (objectName == "Single") // Single game
                {
                    if (Input.GetAxis("js0") != 0 || Input.GetAxis("js11") != 0 || Input.GetAxis("js20") != 0 || Input.GetKeyDown(KeyCode.Y))
                    {
                        startMenu.SetActive(false);
                        openMenu(roleMenu);
                    }
                }
                else if (objectName == "Multiple") // Multiple game
                {
                    if (Input.GetAxis("js0") != 0 || Input.GetAxis("js11") != 0 || Input.GetAxis("js20") != 0 || Input.GetKeyDown(KeyCode.Y))
                    {
                        // Modified your multiplayer games starting setup here
                        startMenu.SetActive(false);
                        openMenu(roleMenu);
                    }
                }
                else if (objectName == "Hider" || objectName == "Seeker") // Set character as a hider / seeker
                {
                    if (Input.GetAxis("js0") != 0 || Input.GetAxis("js11") != 0 || Input.GetAxis("js20") != 0 || Input.GetKeyDown(KeyCode.Y))
                    {
                        foreach (Transform child in playerVisual.transform)
                        {
                            // Destroy the child object
                            Destroy(child.gameObject);
                        }
                        // Instantiate a copy of the objectToCopy
                        GameObject copiedObject = null;
                        if (objectName == "Hider")
                        {
                            copiedObject = Instantiate(hiderVisual);
                        }
                        else if (objectName == "Seeker")
                        {
                            copiedObject = Instantiate(seekerVisual);
                            // Find all GameObjects tagged with "Hide Place"
                            GameObject[] hidePlaceObjects = GameObject.FindGameObjectsWithTag("Hide Place");

                            // Initialize the array with the number of hide places found
                            hidePlaces = new Transform[hidePlaceObjects.Length];

                            // Fill the array with the positions of hide places
                            for (int i = 0; i < hidePlaceObjects.Length; i++)
                            {
                                hidePlaces[i] = hidePlaceObjects[i].transform;
                            }

                            // Start spawning NPCs at intervals
                            SpawnNPC();
                        }

                        // Set the parent of the copied object to the parentObject
                        copiedObject.transform.parent = playerVisual.transform;

                        // Optionally, reset the local position and rotation of the copied object
                        copiedObject.transform.localPosition = new Vector3(0f, -1.11f, 0f);
                        copiedObject.transform.localRotation = Quaternion.identity;
                        playerObject.tag = objectName;
                        roleMenu.SetActive(false);
                        StartCoroutine(hideTime());
                    }

                }
                else if (objectName == "Restart")
                {
                    if (Input.GetAxis("js0") != 0 || Input.GetAxis("js11") != 0 || Input.GetAxis("js20") != 0 || Input.GetKeyDown(KeyCode.Y))
                    {
                        playerObject.tag = "Untagged";
                        // Find all GameObjects with tags "Hider", "Seeker", or "Touched"
                        GameObject[] hiderToDelete = GameObject.FindGameObjectsWithTag("Hider");
                        GameObject[] seekerObjects = GameObject.FindGameObjectsWithTag("Seeker");
                        GameObject[] touchedObjects = GameObject.FindGameObjectsWithTag("Touched");

                        // Combine arrays
                        GameObject[] allObjects = new GameObject[hiderToDelete.Length + seekerObjects.Length + touchedObjects.Length];
                        hiderToDelete.CopyTo(allObjects, 0);
                        seekerObjects.CopyTo(allObjects, hiderToDelete.Length);
                        touchedObjects.CopyTo(allObjects, hiderToDelete.Length + seekerObjects.Length);

                        // Iterate through each object and destroy it
                        foreach (GameObject obj in allObjects)
                        {
                            // Check if the name contains "NPC"
                            if (obj.name.Contains("NPC"))
                            {
                                // Destroy the GameObject
                                Destroy(obj);
                            }
                        }
                        leftDoor = Gate.transform.Find("Left Door").transform;
                        rightDoor = Gate.transform.Find("Right Door").transform;
                        MainDoors();
                        endMenu.SetActive(false);
                        openMenu(startMenu);
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
                else if (hit.collider.CompareTag("Hide Place") && playerObject.tag == "Hider")
                {
                    Vector3 bedPosition = hit.collider.gameObject.transform.position;
                    Quaternion bedRotation = hit.collider.gameObject.transform.rotation;
                    Hide(bedPosition, bedRotation);
                }
                else if (hit.collider.CompareTag("Hider") && playerObject.tag == "Seeker")
                {
                    // Modify transform to gazed object
                    hider = temp;

                    Hider();
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
            openMenu(menu);
            CharacterMovement targetScript = playerObject.GetComponent<CharacterMovement>();
            targetScript.enabled = false;
        }
        // Countdown
        if (countdownStarted)
        {
            timeRemaining -= Time.deltaTime;

            if (timeRemaining <= 0f)
            {
                timeRemaining = 0f;
                countdownStarted = false;
            }

            UpdateCountdownDisplay();
        }
        // Got touched
        if (playerObject.tag == "Touched" && !end)
        {
            gameEnd(false);
        }
    }

    void openMenu(GameObject Menu)
    {
        Vector3 menuPosition = mainCamera.transform.position + mainCamera.transform.forward * 0.1f;
        Menu.transform.position = menuPosition;
        Menu.transform.rotation = Camera.main.transform.rotation;
        Menu.SetActive(true);
    }

    void gameEnd(bool win)
    {
        StopAllCoroutines();
        if (win)
        {
            statusText.text = "You Win!";
        }
        else
        {
            statusText.text = "You Lose!";
        }
        openMenu(endMenu);
        CharacterMovement targetScript = playerObject.GetComponent<CharacterMovement>();
        targetScript.enabled = false;
        timeRemaining = 0f;
        circumstanceText.text = "";
        countdownText.text = "";
        end = true;
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

    void Hide(Vector3 bedPosition, Quaternion bedRotation)
    {
        if (!isHidden)
        {
            lastPosition = playerObject.transform.position;
            lastRotation = playerObject.transform.rotation;
            CharacterMovement targetScript = playerObject.GetComponent<CharacterMovement>();
            targetScript.enabled = false;
            Vector3 offset = new Vector3(0.04f, 0.2f, 0.3f);
            playerObject.transform.position = bedPosition + offset;
            playerObject.transform.rotation = bedRotation;
            playerObject.transform.rotation *= Quaternion.Euler(0, 0, 90);
            isHidden = true;
        }
        else
        {
            CharacterMovement targetScript = playerObject.GetComponent<CharacterMovement>();
            playerObject.transform.rotation = lastRotation;
            targetScript.enabled = true;
            playerObject.transform.position = lastPosition;
            isHidden = false;
        }
    }

    void Hider()
    {
        // Disable the hider
        hider.SetActive(false);
        hider.tag = "Touched";
        hider.transform.parent.parent.tag = "Touched";
    }

    IEnumerator hideTime()
    {
        countdownStarted = true;
        timeRemaining = 30f;
        circumstanceText.text = "Time to hide...";
        if (playerObject.tag == "Hider")
        {
            CharacterMovement targetScript = playerObject.GetComponent<CharacterMovement>();
            targetScript.enabled = true;
            playerObject.transform.position = new Vector3(7, 1, 2);
        }
        else
        {
            playerObject.transform.position = new Vector3(-50, -2, 2);
        }
        yield return new WaitForSeconds(30);
        countdownText.text = "";
        StartCoroutine(startTime());
        if(playerObject.tag == "Hider")
        {
            GameObject NPC = Instantiate(seekerVisual.transform.parent.parent.gameObject, new Vector3(-50, -2, 2), Quaternion.identity);
            NPC.SetActive(true);
        }
    }

    IEnumerator startTime()
    {
        end = false;
        leftDoor = Gate.transform.Find("Left Door").transform;
        rightDoor = Gate.transform.Find("Right Door").transform;
        MainDoors();
        countdownStarted = true;
        timeRemaining = 120f;
        if (playerObject.tag == "Hider")
        {
            circumstanceText.text = "Time to survive...";
        }
        else
        {
            CharacterMovement targetScript = playerObject.GetComponent<CharacterMovement>();
            targetScript.enabled = true;
            circumstanceText.text = "Time to seek...";
        }
        yield return new WaitForSeconds(120);
        if (playerObject.tag == "Hider")
        {
            gameEnd(true);
        }
        else
        {
            gameEnd(false);
        }
    }
    void UpdateCountdownDisplay()
    {
        // Convert time remaining to seconds
        int seconds = Mathf.CeilToInt(timeRemaining);

        // Update the text to display the time remaining
        countdownText.text = "Time Remaining: " + seconds.ToString() + "s";
    }

    void SpawnNPC()
    {
        // Instantiate the NPC prefab
        GameObject[] NPC = new GameObject[maxNPCs];
        for (int i = 0; i < maxNPCs; i++)
        {
            // Randomly select a hide place
            Transform randomHidePlace = hidePlaces[Random.Range(0, hidePlaces.Length)];
            // Example: Instantiate hider GameObject and assign it to the array element
            NPC[i] = Instantiate(hiderVisual.transform.parent.parent.gameObject, randomHidePlace.position, Quaternion.identity);
            Vector3 offset = new Vector3(0.04f, 0.2f, 0.3f);
            NPC[i].name = "NPC " + i.ToString();
            NPC[i].transform.Find("Model").transform.Find("default").gameObject.name = i.ToString();
            NPC[i].transform.position = randomHidePlace.position + offset;
            NPC[i].transform.rotation = randomHidePlace.rotation;
            NPC[i].transform.rotation *= Quaternion.Euler(0, 0, 90);
            NPC[i].SetActive(true);
        }
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
