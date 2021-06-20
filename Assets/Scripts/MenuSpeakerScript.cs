using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuSpeakerScript : MonoBehaviour
{
    private AudioSource audioSource;

    // ALL SoundFX here ...
    public AudioClip MouseClickOnButtonFX;



    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayClickSound()
    {
        audioSource.clip = MouseClickOnButtonFX;
        audioSource.Play();
    }
}
