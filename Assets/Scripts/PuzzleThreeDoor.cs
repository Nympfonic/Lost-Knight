using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleThreeDoor : MonoBehaviour
{

    private GameObject enemy1;
    private GameObject enemy2;
    private GameObject enemy3;

    // Start is called before the first frame update
    void Start()
    {
        enemy1 = GameObject.Find("Chicken");
        enemy2 = GameObject.Find("Chicken (1)");
        enemy3 = GameObject.Find("Chicken (2)");

    }

    // Update is called once per frame
    void Update()
    {
        if (enemy1 == null &&
            enemy2 == null &&
            enemy3 == null )
        {
            gameObject.GetComponent<DoorController>().isLocked = false;
        }
        else
        {
            gameObject.GetComponent<DoorController>().isLocked = true;
        }
    }
}
