using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]

public class AudioTester : MonoBehaviour
{
    public AudioClip soundToPlay;
    AudioSource audioSourceToUse;

    void Start()
    {
        audioSourceToUse = GetComponent<AudioSource>();
        InvokeRepeating("OpenDoor", 1.0f, 3.0f);    
    }
    
    void OpenDoor()
    {
        audioSourceToUse.PlayOneShot(soundToPlay);
    }
}