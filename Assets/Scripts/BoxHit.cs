using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxHit : MonoBehaviour
{
    // gameObject

    // Player
    public GameObject Player;

    public float magnitude = 1f;
    public float distance = 1f;

    private Rigidbody2D rb2d;

    private void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        float dist = Vector3.Distance(Player.transform.position, gameObject.transform.position);
        // if player is in range and input is left click then send box flying.
        if (dist < distance && Player.GetComponent<SpriteRenderer>().sprite.name == "knight_attack_6")
        {
            Vector3 hitDirection = new Vector3(transform.position.x - Player.transform.position.x, 0, 0);
            hitDirection = hitDirection.normalized;
            rb2d.AddForce(hitDirection * magnitude);
        }
    }
}
