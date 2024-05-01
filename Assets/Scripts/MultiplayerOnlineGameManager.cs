using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiplayerOnlineGameManager : MonoBehaviour
{
    public GameObject[] DefaultSection;

    void Start()
    {
        if (GetComponent<PhotonView>().IsMine)
        {
            for (int i = 0; i < DefaultSection.Length; i++)
            {
                DefaultSection[i].SetActive(true);
            }
        }
        else
        {
            Destroy(GetComponent<CharacterMovement>());
            Destroy(GetComponent<CharacterController>());
            Destroy(GetComponent<Rigidbody>());
            for (int i = 0; i < DefaultSection.Length; i++)
            {
                DefaultSection[i].SetActive(false);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
