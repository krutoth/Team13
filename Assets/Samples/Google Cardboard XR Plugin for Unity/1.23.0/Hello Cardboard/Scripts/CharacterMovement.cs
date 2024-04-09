// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using Unity.Netcode;

// public class CharacterMovement : NetworkBehaviour //MonoBehaviour
// {
//     CharacterController charCntrl;
//     [Tooltip("The speed at which the character will move.")]
//     public float speed = 5f;
//     [Tooltip("The camera representing where the character is looking.")]
//     public GameObject cameraObj;
//     [Tooltip("Should be checked if using the Bluetooth Controller to move. If using keyboard, leave this unchecked.")]
//     public bool joyStickMode;

//     // Start is called before the first frame update
//     private void Start()
//     {
//         if (!IsOwner) return;
//         charCntrl = GetComponent<CharacterController>();
//     }

//     // Update is called once per frame
//     private void Update()
//     {
//         if (!IsOwner) return;
//         //Get horizontal and Vertical movements
//         float horComp = Input.GetAxis("Horizontal");
//         float vertComp = Input.GetAxis("Vertical");

//         if (!joyStickMode)
//         {
//             horComp = Input.GetAxis("Vertical");
//             vertComp = Input.GetAxis("Horizontal") * -1;
//         }

//         Vector3 moveVect = Vector3.zero;

//         //Get look Direction
//         Vector3 cameraLook = cameraObj.transform.forward;
//         cameraLook.y = 0f;
//         cameraLook = cameraLook.normalized;

//         Vector3 forwardVect = cameraLook;
//         Vector3 rightVect = Vector3.Cross(forwardVect, Vector3.up).normalized * -1;

//         moveVect += rightVect * horComp;
//         moveVect += forwardVect * vertComp;

//         moveVect *= speed;
     

//         charCntrl.SimpleMove(moveVect);


//     }
// }


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class CharacterMovement : NetworkBehaviour //MonoBehaviour
{
    CharacterController charCntrl;
    [Tooltip("The speed at which the character will move.")]
    public float speed = 5f;
    [Tooltip("Jump height")]
    public float jumpHeight = 2f;
    [Tooltip("The camera representing where the character is looking.")]
    public GameObject cameraObj;
    [Tooltip("Should be checked if using the Bluetooth Controller to move. If using keyboard, leave this unchecked.")]
    public bool joyStickMode;
    [Tooltip("Gravity value")]
    public float gravityValue = -30f;

    private Vector3 playerVelocity;
    private bool groundedPlayer;

    // Start is called before the first frame update
    private void Start()
    {
        if (!IsOwner) return;
        charCntrl = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    private void Update()
    {
        if (!IsOwner) return;
        groundedPlayer = charCntrl.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }

        float horComp = Input.GetAxis("Horizontal");
        float vertComp = Input.GetAxis("Vertical");

        if (!joyStickMode)
        {
            horComp = Input.GetAxis("Vertical");
            vertComp = Input.GetAxis("Horizontal") * -1;
        }

        Vector3 move = Vector3.zero;

        Vector3 cameraLook = cameraObj.transform.forward;
        cameraLook.y = 0; // This ensures the character doesn't tilt upwards/downwards.
        cameraLook = cameraLook.normalized;

        Vector3 forwardVect = cameraLook;
        Vector3 rightVect = Vector3.Cross(forwardVect, Vector3.up).normalized * -1;

        move += rightVect * horComp;
        move += forwardVect * vertComp;

        move *= speed;

        // Make the character move
        charCntrl.Move(move * Time.deltaTime);

        // Align character's rotation with the camera's rotation on the y-axis
        Quaternion cameraRotation = Quaternion.Euler(0, cameraObj.transform.eulerAngles.y, 0);
        transform.rotation = Quaternion.Slerp(transform.rotation, cameraRotation, Time.deltaTime * 10);

        // Jump logic
        if (Input.GetButtonDown("Jump") && groundedPlayer)
        {
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -1.3f * gravityValue);
        }

        // Apply gravity
        playerVelocity.y += gravityValue * Time.deltaTime;
        charCntrl.Move(playerVelocity * Time.deltaTime);
    }

}
