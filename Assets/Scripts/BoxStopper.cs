using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxStopper : MonoBehaviour
{

    private BoxCollider2D boxCollider;
    private bool isSolved;

    // Start is called before the first frame update
    void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        
    }

    // Update is called once per frame
    void Update()
    {
        isSolved = GameObject.Find("DoorTrigger").GetComponent<PuzzleDoorTrigger>().isSolved;

        if (isSolved)
        {
            boxCollider.enabled = false;
        }
        else
        {
            boxCollider.enabled = true;
        }
    }
}
