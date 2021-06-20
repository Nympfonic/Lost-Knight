using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapRoom : MonoBehaviour
{
    private BoxCollider2D box2d;
    public GameObject LockedDoor;
    public Animator ld_anim;
    private AudioSource audioSource;
    private bool audioHasPlayed = false;

    // Start is called before the first frame update
    void Start()
    {
        box2d = GetComponent<BoxCollider2D>();
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (audioHasPlayed)
        {
            double audioTime = audioSource.time;
            if (audioTime > 7)
            {
                StartCoroutine(FadeOutMusic(audioSource, 1f));
                audioHasPlayed = false;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        LockedDoor.transform.position = new Vector3(95.5f, 25.5f, 0f);
        
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        ld_anim.SetTrigger("activateTrap");
        audioSource.Play();
        audioHasPlayed = true;
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
}
