using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soul : MonoBehaviour
{
    private GameObject buttonSprite;

    private ParticleSystem ps;
    private ParticleSystem.Particle[] particles = new ParticleSystem.Particle[1000]; 

    private PlayerController pc;
    private bool playerInBounds = false;

    private SpriteRenderer sprite;

    private Light lightC;

    void Start()
    {
        ps = GetComponent<ParticleSystem>();

        sprite = GetComponent<SpriteRenderer>();

        buttonSprite = transform.Find("InteractionButton").gameObject;

        pc = GameObject.FindWithTag("Player").GetComponent<PlayerController>();

        lightC = GetComponent<Light>();
    }

    void Update()
    {
        if (ps.isPlaying)
        {
            int length = ps.GetParticles(particles);
            Vector3 playerPos = pc.gameObject.transform.position;

            for (int i = 0; i < length; i++)
            {
                particles[i].position = particles[i].position + (playerPos - particles[i].position) / (particles[i].remainingLifetime) * Time.deltaTime;
            }

            ps.SetParticles(particles, length);
        }

        if (playerInBounds && Input.GetKeyDown(KeyCode.E) && !pc.deathState)
        {
            if (PlayerController.numOfHearts < 6)
            {
                pc.SoulAcquired();

                InteractionPopup(false);

                ps.Play();
                lightC.intensity = Mathf.SmoothStep(lightC.intensity, 0f, 1f);
                sprite.color = new Color(1f, 1f, 1f, Mathf.SmoothStep(1f, 0f, 1f));

                if (ps.isStopped && sprite.color.a == 0)
                {
                    Destroy(gameObject);
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInBounds = true;

            InteractionPopup(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInBounds = false;

            InteractionPopup(false);
        }
    }

    private void InteractionPopup(bool popupEnabled)
    {
        if (popupEnabled == true)
        {
            buttonSprite.SetActive(true);
        }
        else
        {
            buttonSprite.SetActive(false);
        }
    }
}
