using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimulationMoveCamera : MonoBehaviour
{
    public Animator anim;
    public DoorController DoorToLock;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            DoorToLock.isLocked = true;
            anim.SetTrigger("EndZoom");
        }
    }
}
