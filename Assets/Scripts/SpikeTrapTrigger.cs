using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeTrapTrigger : MonoBehaviour
{
    private bool isTriggered = false;

    public GameObject trap; // Can select a game object as the trap
    private Animator animator;

    private void Awake()
    {
        if (trap != null)
        {
            animator = trap.GetComponent<Animator>();
        }
    }

    void Update()
    {
        if (trap != null)
        {
            // If the trap is triggered, move the trap towards targetVector
            if (isTriggered)
            {
                isTriggered = false;
                animator.SetTrigger("Active");
            }

            // Disable script once targetVector has been reached
            /*if ()
            {
                gameObject.SetActive(false);
            }*/
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        /* If the collider is the player and the 
          trap has not been triggered yet then trigger */
        if (collision.CompareTag("Player") && !isTriggered)
        {
            isTriggered = true;
        }
    }
}
