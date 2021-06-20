using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeTransitionAnimEventHandler : MonoBehaviour
{
    private LoadLevel loadLevel;

    private void Awake()
    {
        loadLevel = GameObject.Find("LoadLevelTrigger").GetComponent<LoadLevel>();
    }

    private void FadeOutAnimFinished()
    {
        loadLevel.asyncOperation.allowSceneActivation = true;
    }
}
