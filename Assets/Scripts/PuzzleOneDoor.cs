using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleOneDoor : MonoBehaviour
{
    private Color[] colors = { Color.yellow, Color.magenta, Color.cyan };
    //private Color doorColour = Color.magenta;
    private Color doorColour;

    private GameObject puzzle1Button1;
    private GameObject puzzle1Button2;
    private GameObject puzzle1Button3;

    // Update is called once per frame

    private void Start()
    {
        int r = Random.Range(1, 4);
        if (r == 1)
        {
            doorColour = Color.magenta;
        }
        else if (r == 2)
        {
            doorColour = Color.yellow;
        }
        else
        {
            doorColour = Color.cyan;
        }

        //Debug.Log(doorColour);
        SetDoorColourIndicator();
    }

    private void SetDoorColourIndicator()
    {
        GameObject.Find("p1-Indicator").GetComponent<SpriteRenderer>().color = doorColour;
    }

    void Update()
    {
        // declares all puzzle buttons related to the door.
        puzzle1Button1 = GameObject.Find("p1-Button");
        puzzle1Button2 = GameObject.Find("p1-Button (1)");
        puzzle1Button3 = GameObject.Find("p1-Button (2)");

        // Check if all buttons are the same colour as the door. If yes, open door.
        if (puzzle1Button1.GetComponent<SpriteRenderer>().color == doorColour &&
            puzzle1Button2.GetComponent<SpriteRenderer>().color == doorColour &&
            puzzle1Button3.GetComponent<SpriteRenderer>().color == doorColour)
        {
            gameObject.GetComponent<DoorController>().isLocked = false;
        }
        else
        {
            gameObject.GetComponent<DoorController>().isLocked = true;
        }
    }
}
