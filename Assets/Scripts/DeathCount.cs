using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeathCount : MonoBehaviour
{
    [HideInInspector] public static int deaths = 0;
    private Text deathText;

    private void Start()
    {
        deathText = GetComponent<Text>();
    }

    private void Update()
    {
        deathText.text = "Deaths: " + deaths;
    }
}
