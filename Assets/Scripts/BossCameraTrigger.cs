using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossCameraTrigger : MonoBehaviour
{
    public Animator animator;
    private Animator bossAnim, bossNameAnim, bossHealthAnim;
    private AudioSource[] music;
    private BossController bossController;
    private GameObject boss, cam;
    private GameObject[] bossDoors;
    private bool isTriggered = false, uiFadeOut = false;
    public bool bossFightOver = false;

    private double introMusicStartTime;

    void Awake()
    {
        // Initialisation
        cam = GameObject.FindGameObjectWithTag("MainCamera");
        animator = cam.GetComponent<Animator>();
        boss = GameObject.Find("Boss");
        bossAnim = boss.GetComponent<Animator>();
        bossNameAnim = GameObject.Find("BossName").GetComponent<Animator>();
        bossHealthAnim = GameObject.Find("BossHealthBar").GetComponent<Animator>();
        bossController = boss.GetComponent<BossController>();
        bossDoors = GameObject.FindGameObjectsWithTag("BossDoor");
        music = GetComponents<AudioSource>();
    }

    private void Update()
    {
        if (bossController.deathState && !uiFadeOut)
        {
            uiFadeOut = true;
            bossNameAnim.SetTrigger("FadeOut"); // Play the Boss name fade out animation
            bossHealthAnim.SetTrigger("FadeOut"); // Play the Boss health bar fade out animation

            //double musicLoopEndTime = AudioSettings.dspTime + 3;
            //endMusicLoop = true;
            StartCoroutine(FadeOutMusic(music[1], 5f));
            StartCoroutine(FadeOutMusic(music[0], 5f));
        }

        /*if (endMusicLoop)
        {
            FadeOutMusic();
        }*/
        
        // Once no longer needed, disable game object
        if (bossFightOver)
        {
            // Unlock all Boss doors
            foreach (GameObject door in bossDoors)
            {
                door.GetComponent<DoorController>().isLocked = false;
            }

            //gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !bossController.deathState && !isTriggered)
        {
            animator.SetTrigger("Zoom"); // Play the camera zoom animation
            bossAnim.SetTrigger("Intro"); // Play the Boss intro animation
            bossNameAnim.SetTrigger("FadeIn"); // Play the Boss name fade in animation
            bossHealthAnim.SetTrigger("FadeIn"); // Play the Boss health bar fade in animation
            

            introMusicStartTime = AudioSettings.dspTime + 2;
            music[0].PlayScheduled(introMusicStartTime); // Play the intro Boss music 1 second later
            double introMusicDuration = (double)music[0].clip.samples / music[0].clip.frequency; // Calculate length of intro music 
            music[0].SetScheduledEndTime(introMusicStartTime + introMusicDuration);
            music[1].PlayScheduled(introMusicStartTime + introMusicDuration);

            // Lock all Boss doors
            foreach (GameObject door in bossDoors)
            {
                door.GetComponent<DoorController>().isLocked = true;
            }

            isTriggered = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // Check that the Boss is dead first
        if (collision.CompareTag("Player") && bossController.deathState)
        {
            animator.SetTrigger("Unzoom"); // Play the zoom out animation
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
}