using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadLevel : MonoBehaviour
{
    [SerializeField] private int sceneIndex = 1;
    private Animator transitionAnim;
    public AsyncOperation asyncOperation;

    private void Awake()
    {
        transitionAnim = GameObject.Find("FadeTransition").GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            transitionAnim.SetTrigger("FadeOut");
            asyncOperation = SceneManager.LoadSceneAsync(sceneIndex);
            asyncOperation.allowSceneActivation = false;
        }
    }
}
