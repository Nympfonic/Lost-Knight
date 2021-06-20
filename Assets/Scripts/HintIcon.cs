using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HintIcon : MonoBehaviour
{

    // OffImg - Image
    // OnImg - Image
    // isOn - Bool

    public Sprite OffImg;
    public Sprite OnImg;
    private bool isOn = false;
    private SpriteRenderer sr;


    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isOn)
        {
            sr.sprite = OnImg;
        }
        else
        {
            sr.sprite = OffImg;
        }
    }

    void ButtonSwitch()
    {
        isOn = !isOn;
    }

}
