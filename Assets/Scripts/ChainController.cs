using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChainController : MonoBehaviour
{
    private delegate void ChainPull(GameObject target);
    private ChainPull chainPull;

    private bool oneTimeUse = true;
    private bool isPulled = false;

    public GameObject target;
    public TargetType targetType;

    private Animator animator;
    private bool animFinished;

    public enum TargetType
    {
        Door,
        Bridge
    }

    private BoxCollider2D box2D;
    private bool playerInBounds = false;

    private GameObject buttonSprite;

    void Start()
    {
        animator = GetComponent<Animator>();

        box2D = GetComponent<BoxCollider2D>();

        buttonSprite = transform.Find("InteractionButton").gameObject;

        switch (targetType)
        {
            case (TargetType.Door):
                chainPull = Door;
                break;

            case (TargetType.Bridge):
                chainPull = Bridge;
                break;
        }
    }

    void Update()
    {
        if (target != null)
        {
            if (playerInBounds && Input.GetKeyDown(KeyCode.E) && !isPulled)
            {
                chainPull(target);
                animator.Play("Chain");
                isPulled = true;
            }

            if (isPulled && animFinished)
            {
                if (!oneTimeUse)
                {
                    isPulled = false;
                }
                else
                {
                    box2D.enabled = false;
                    buttonSprite.SetActive(false);
                }
            }
        }
        else
        {
            Debug.Log("Target GameObject not found");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInBounds = true;

            InteractionPopup(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInBounds = false;

            InteractionPopup(false);
        }
    }

    private void InteractionPopup(bool popupEnabled)
    {
        if (popupEnabled == true)
        {
            buttonSprite.SetActive(true);
        }
        else
        {
            buttonSprite.SetActive(false);
        }
    }

    private void Door(GameObject target)
    {
        DoorController dc = target.GetComponent<DoorController>();

        dc.isLocked = !dc.isLocked; // toggle door lock status
    }

    private void Bridge(GameObject target)
    {
        BridgeController bc = target.GetComponent<BridgeController>();

        bc.isLowered = true; // bridges can only be lowered
    }

    public void AnimFinished()
    {
        animFinished = true;
    }
}
