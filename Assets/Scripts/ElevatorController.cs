using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorController : MonoBehaviour
{
    private bool isActivated = false, detectedPlayer = false, isTriggered = false;
    private float delay = 0;
    public float delayTime = 1.5f;
    private Animator animator;
    private AudioSource audioSource;
    public GameObject[] elevatorDoors;
    
    void Awake()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (detectedPlayer && !isTriggered)
        {
            if (delay >= delayTime)
            {
                isActivated = true;
                isTriggered = true;

                foreach (GameObject door in elevatorDoors)
                {
                    door.GetComponent<DoorController>().isLocked = true;
                }

                audioSource.Play();

                AudioSource levelMusic = GameObject.Find("Music").GetComponent<AudioSource>();
                StartCoroutine(FadeOutMusic(levelMusic, 2f));
            }

            while (delay < delayTime)
            {
                delay += Time.deltaTime;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            detectedPlayer = true;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (isActivated)
        {
            isActivated = false;
            collision.gameObject.transform.SetParent(transform);
            animator.SetTrigger("Active");
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            detectedPlayer = false;
            collision.gameObject.transform.SetParent(null);

            if (delay < delayTime)
            {
                delay = 0;
                isTriggered = false;
            }
        }
    }

    private IEnumerator FadeOutMusic(AudioSource audioSource, float fadeTime)
    {
        float startVolume = audioSource.volume;

        while (audioSource.volume > 0)
        {
            audioSource.volume -= startVolume * Time.deltaTime / fadeTime;

            yield return null;
        }

        audioSource.Stop();
        audioSource.volume = startVolume;
    }

    private void StopAnimation()
    {
        animator.SetTrigger("Disable");

        foreach (GameObject door in elevatorDoors)
        {
            door.GetComponent<DoorController>().isLocked = false;
        }

        StartCoroutine(FadeOutMusic(audioSource, 1f));
    }
}
