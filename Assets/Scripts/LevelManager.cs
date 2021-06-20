using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    private GameObject player;

    public Vector3 playerStartPos;

    void Awake()
    {
        player = GameObject.FindWithTag("Player");

        player.transform.position = playerStartPos;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
