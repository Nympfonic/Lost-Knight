using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordTrigger : MonoBehaviour
{
    private float offsetX;
    private BoxCollider2D box2D;
    private PlayerController pc;

    private void Start()
    {
        box2D = GetComponent<BoxCollider2D>();
        offsetX = box2D.offset.x;
        pc = transform.parent.GetComponent<PlayerController>();
    }

    private void Update()
    {
        if (!transform.parent.GetComponent<SpriteRenderer>().flipX)
        {
            box2D.offset = new Vector2(offsetX, box2D.offset.y);
        }
        else
        {
            box2D.offset = new Vector2(-offsetX, box2D.offset.y);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy") && !pc.hasAttacked)
        {
            Vector3 hitDirection = new Vector3(collision.transform.position.x - transform.position.x, 0, 0);
            hitDirection = hitDirection.normalized;

            if (collision.GetComponent<BossController>() != null)
            {
                collision.GetComponent<BossController>().TakeDamage(pc.attackDamage);
            }
            else
            {
                collision.GetComponent<AIController>().TakeDamage(pc.attackDamage, hitDirection);
            }

            pc.hasAttacked = true;
        }

        //    if (enemiesToDamage[i].tag == "Enemy")
        //    {
        //       enemiesToDamage[i].SendMessage("TakeDamage", attackDamage);
        //    }
    }
}
