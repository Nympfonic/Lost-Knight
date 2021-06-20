using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level3_PuzzleRoomScript : MonoBehaviour
{
    //private Color[] colors = { Color.yellow, Color.magenta, Color.cyan };
    private Color Yellow = Color.yellow;
    private Color Magenta = Color.magenta;
    private Color Cyan = Color.cyan;

    public enum Colors { Yellow, Magenta, Cyan };
    public Colors DesiredColorForSetOne; // as Colors
    public Colors DesiredColorForSetTwo; // as Colors

    private Color actualDesiredColorForSetOne;
    private Color actualDesiredColorForSetTwo;

    public Color[] FirstSet_ColorContainer = new Color[3];
    public Color[] SecondSet_ColorContainer = new Color[3];


    // Update is called once per frame
    void Update()
    {
        // Update Desired Colors during run time.
        if (DesiredColorForSetOne == Colors.Yellow)
        {
            actualDesiredColorForSetOne = Color.yellow;
        }
        else if (DesiredColorForSetOne == Colors.Magenta)
        {
            actualDesiredColorForSetOne = Color.magenta;
        }
        else if (DesiredColorForSetOne == Colors.Cyan)
        {
            actualDesiredColorForSetOne = Color.cyan;
        }

        if (DesiredColorForSetTwo == Colors.Yellow)
        {
            actualDesiredColorForSetTwo = Color.yellow;
        }
        else if (DesiredColorForSetTwo == Colors.Magenta)
        {
            actualDesiredColorForSetTwo = Color.magenta;
        }
        else if (DesiredColorForSetTwo == Colors.Cyan)
        {
            actualDesiredColorForSetTwo = Color.cyan;
        }



        // Update Color Containers
        FirstSet_ColorContainer[0] = transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().color;
        FirstSet_ColorContainer[1] = transform.GetChild(0).gameObject.transform.GetChild(1).gameObject.GetComponent<SpriteRenderer>().color;
        FirstSet_ColorContainer[2] = transform.GetChild(0).gameObject.transform.GetChild(2).gameObject.GetComponent<SpriteRenderer>().color;

        SecondSet_ColorContainer[0] = transform.GetChild(1).gameObject.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().color;
        SecondSet_ColorContainer[1] = transform.GetChild(1).gameObject.transform.GetChild(1).gameObject.GetComponent<SpriteRenderer>().color;
        SecondSet_ColorContainer[2] = transform.GetChild(1).gameObject.transform.GetChild(2).gameObject.GetComponent<SpriteRenderer>().color;

        // Check color containers if they fit desired color.

        if (FirstSet_ColorContainer[0] == actualDesiredColorForSetOne &&
            FirstSet_ColorContainer[1] == actualDesiredColorForSetOne &&
            FirstSet_ColorContainer[2] == actualDesiredColorForSetOne)
        {
            Destroy(GameObject.Find("BoxStopper5"));
        }

        if (SecondSet_ColorContainer[0] == actualDesiredColorForSetTwo &&
            SecondSet_ColorContainer[1] == actualDesiredColorForSetTwo &&
            SecondSet_ColorContainer[2] == actualDesiredColorForSetTwo)
        {
            Destroy(GameObject.Find("RestOfBoxStoppers"));
        }

        // If so release first box.
    }
}
