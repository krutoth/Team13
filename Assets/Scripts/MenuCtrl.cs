using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuCtrl : MonoBehaviour
{
    public GameObject character;
    // Start is called before the first frame update
    void Start()
    {
        transform.LookAt(character.transform);
        // Reverse it
        transform.Rotate(0, 180, 0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
