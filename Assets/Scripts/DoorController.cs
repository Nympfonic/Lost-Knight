using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    public bool isLocked;
    private SpriteRenderer sr;
    private BoxCollider2D box2d;
    private AudioSource audioSource;
    private bool hasPlayedSound = false;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        box2d = GetComponent<BoxCollider2D>();
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (isLocked) // if door is 
        {
            sr.color = new Color(1f, 0f, 0f, 1f); // turn red.
            box2d.enabled = true; // enable box collider.
        }
        else
        {
            sr.color = new Color(0f, 1f, 0f, 1f); // turn green.
            box2d.enabled = false; // disable box collider.
            PlaySound();
        }
    }

    private void PlaySound()
    {
        if (audioSource != null && !hasPlayedSound)
        {
            audioSource.Play();
            hasPlayedSound = true;
        }
    }
}
