using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;

public class SpikeTrap : MonoBehaviour
{
    public int DamageToPlayer = 1;
    public bool isEnd = false;
    public Animator anim;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (isEnd)
            {
                StartCoroutine(FadeOut_NextScene());
            }
            else
            {
                Vector3 hitDirection = collision.transform.position - transform.position;
                hitDirection = hitDirection.normalized;

                collision.GetComponent<PlayerController>().TakeDamage(DamageToPlayer, hitDirection);
            }
            
        } else
        {
            if (isEnd)
            {
                Destroy(collision.gameObject);
            }
        }
    }

    IEnumerator FadeOut_NextScene()
    {
        // Fade Out Animation
        anim.SetTrigger("FadeOut");

        // Wait Animation Duration Time.
        yield return new WaitForSeconds(0.5f);

        // Load Next Scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
