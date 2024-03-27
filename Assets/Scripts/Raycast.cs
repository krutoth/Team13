using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Raycast : MonoBehaviour
{
    public LineRenderer lineRenderer;
    public Camera mainCamera;
    public GameObject playerObject;
    public float maxDistance; // 最大射線距離
    private GameObject lastHitObject; // 追蹤上一次擊中的物體
    public Transform leftDoor;
    public Transform rightDoor;
    private bool isOpenedDoor = false;
    public Transform leftSlideDoor;
    public Transform rightSlideDoor;
    private bool isSlidedDoor = false;
    private bool isHidden = false;
    private Vector3 lastPosition;

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        mainCamera = Camera.main;

        // 設定射線寬度
        lineRenderer.startWidth = 0.05f;
        lineRenderer.endWidth = 0.05f;
    }

    void Update()
    {
        // 計算射線起點，將其設定在相機下方位置
        Vector3 rayOrigin = mainCamera.transform.position - mainCamera.transform.up * 0.5f;
        Ray ray = new Ray(rayOrigin, mainCamera.transform.forward);

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, maxDistance))
        {
            DrawRay(ray.origin, hit.point);
            // 如果擊中的物體與上一次不同，則恢復
            if (lastHitObject != hit.collider.gameObject)
            {
                RestoreOriginalColor();
                lastHitObject = hit.collider.gameObject;
            }

            // 更改擊中物體的外框
            if (!hit.collider.gameObject.GetComponent<Outline>() && !hit.collider.CompareTag("Ground"))
            {
                var outline = hit.collider.gameObject.AddComponent<Outline>();
                outline.OutlineMode = Outline.Mode.OutlineAll;
                outline.OutlineColor = Color.yellow;
                outline.OutlineWidth = 5f;
            }

            // Use X on keyboard or js11 on joystick to interact with objects
            if (Input.GetAxis("js11") != 0 || Input.GetKeyDown("js24") || Input.GetKeyDown(KeyCode.X))
            {
                if (hit.collider.CompareTag("Door"))
                {
                    RotateDoors();
                    if (isOpenedDoor)
                    {
                        hit.collider.isTrigger = true;
                    }
                    else
                    {
                        hit.collider.isTrigger = false;
                    }
                }
                else if (hit.collider.CompareTag("Slide Door"))
                {
                    SlideDoors();
                    if (isSlidedDoor)
                    {
                        hit.collider.isTrigger = true;
                    }
                    else
                    {
                        hit.collider.isTrigger = false;
                    }
                }
                else if (hit.collider.CompareTag("Hide Place"))
                {
                    Vector3 bedPosition = hit.collider.gameObject.transform.position;
                    Hide(bedPosition);
                }
            }
        }
        else
        {
            DrawRay(ray.origin, ray.origin + ray.direction * maxDistance);
            // 如果射線未擊中任何物體，則恢復
            RestoreOriginalColor();
            lastHitObject = null;
        }
    }

    void RotateDoors()
    {
        Vector3 leftDoorMove = new Vector3(-1, 0, 1);
        Vector3 rightDoorMove = new Vector3(1, 0, 1);
        Vector3 leftDoorRotate = new Vector3(0, 90, 0);
        Vector3 rightDoorRotate = new Vector3(0, -90, 0);
        if (!isOpenedDoor)
        {
            leftDoor.localPosition += leftDoorMove;
            leftDoor.localRotation *= Quaternion.Euler(leftDoorRotate);
            rightDoor.localPosition += rightDoorMove;
            rightDoor.localRotation *= Quaternion.Euler(rightDoorRotate);
            isOpenedDoor = true;
        }
        else
        {
            leftDoor.localPosition -= leftDoorMove;
            leftDoor.localRotation *= Quaternion.Euler(-leftDoorRotate);
            rightDoor.localPosition -= rightDoorMove;
            rightDoor.localRotation *= Quaternion.Euler(-rightDoorRotate);
            isOpenedDoor = false;
        }
    }
    void SlideDoors()
    {
        Vector3 leftDoorMove = new Vector3(-2, 0, 0);
        Vector3 rightDoorMove = new Vector3(2, 0, 0);
        if (!isSlidedDoor)
        {
            leftSlideDoor.localPosition += leftDoorMove;
            rightSlideDoor.localPosition += rightDoorMove;
            isSlidedDoor = true;
        }
        else
        {
            leftSlideDoor.localPosition -= leftDoorMove;
            rightSlideDoor.localPosition -= rightDoorMove;
            isSlidedDoor = false;
        }
    }

    void Hide(Vector3 bedPosition)
    {
        if (!isHidden)
        {
            lastPosition = playerObject.transform.position;
            CharacterMovement targetScript = playerObject.GetComponent<CharacterMovement>();
            targetScript.enabled = false;
            Vector3 offset = new Vector3(0f, -3f, 0f);
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

    void DrawRay(Vector3 start, Vector3 end)
    {
        // 設定射線起點和終點
        lineRenderer.positionCount = 2;
        lineRenderer.SetPosition(0, start);
        lineRenderer.SetPosition(1, end);
    }

    // 恢復
    private void RestoreOriginalColor()
    {
        if (lastHitObject != null)
            Destroy(lastHitObject.GetComponent<Outline>());
    }
}
