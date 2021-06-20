using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTrigger : MonoBehaviour
{
    public Animator animator;
    private GameObject cam;
    private bool isTriggered = false;

    void Awake()
    {
        cam = GameObject.FindGameObjectWithTag("MainCamera");
        animator = cam.GetComponent<Animator>();
    }

    private void Update()
    {
        if (isTriggered)
        {
            gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            animator.SetTrigger("Zoom");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            animator.SetTrigger("Unzoom");
            isTriggered = true;
        }
    }
}