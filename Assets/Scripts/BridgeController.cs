using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BridgeController : MonoBehaviour
{
    public bool isLowered;
    public AnimType curAnimType;

    private Animator animator;
    private BoxCollider2D box2D;

    void Start()
    {
        animator = GetComponent<Animator>();
        box2D = GetComponent<BoxCollider2D>();
    }

    void Update()
    {
        if (isLowered) // if bridge is 
        {
            switch (curAnimType)
            {
                case (AnimType.None):
                    break;

                case (AnimType.Lower):
                    animator.Play("LowerBridge");
                    break;

                case (AnimType.Extend):
                    animator.Play("ExtendBridge");
                    break;
            }
        }
    }

    public enum AnimType
    {
        None,
        Lower,
        Extend
    }

}
