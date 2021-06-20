using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleOneButton : MonoBehaviour
{
    private SpriteRenderer sr;
    private Color[] colors = { Color.yellow, Color.magenta, Color.cyan };
    public int buttonID;
    private int ArrayIndex;
    private bool playerInbound = false;

    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        ArrayIndex = buttonID;
    }

    private void changeColor()
    {
        if (ArrayIndex + 1 > colors.Length - 1)
        {
            ArrayIndex = 0;
            sr.color = colors[ArrayIndex];
        }
        else
        {
            ArrayIndex++;
            sr.color = colors[ArrayIndex];
        }
    }

    private void Update()
    {
        switch (ArrayIndex)
        {
            case 0:
                sr.color = colors[ArrayIndex];
                break;

            case 1:
                sr.color = colors[ArrayIndex];
                break;

            case 2:
                sr.color = colors[ArrayIndex];
                break;
        }

        if (playerInbound && Input.GetKeyDown(KeyCode.E))
        {
            changeColor();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInbound = true;

            if (buttonID == 0)
            {
                GameObject.Find("HintIconE_1").SendMessage("ButtonSwitch");
            }
            else if (buttonID == 1)
            {
                GameObject.Find("HintIconE_2").SendMessage("ButtonSwitch");
            }
            else if (buttonID == 2)
            {
                GameObject.Find("HintIconE_3").SendMessage("ButtonSwitch");
            }
            
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInbound = false;

            if (buttonID == 0)
            {
                GameObject.Find("HintIconE_1").SendMessage("ButtonSwitch");
            }
            else if (buttonID == 1)
            {
                GameObject.Find("HintIconE_2").SendMessage("ButtonSwitch");
            }
            else if (buttonID == 2)
            {
                GameObject.Find("HintIconE_3").SendMessage("ButtonSwitch");
            }

        }
    }
}
