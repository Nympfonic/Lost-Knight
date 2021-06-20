using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleDoorTrigger : MonoBehaviour
{

    [HideInInspector] public bool isSolved;

    private void Start()
    {
        GetComponentInParent<DoorController>().isLocked = true;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Box"))
        {
            GetComponentInParent<DoorController>().isLocked = false;
            isSolved = true;
        }
        else
        {
            GetComponentInParent<DoorController>().isLocked = true;
            isSolved = false;
        }
    }

}
