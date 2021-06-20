using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossLaser : MonoBehaviour
{
    private BossController bossController;

    void Awake()
    {
        bossController = transform.parent.GetComponent<BossController>();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerController pc = collision.GetComponent<PlayerController>();
            Vector3 hitDirection = new Vector3(collision.transform.position.x - transform.position.x, 0, 0);
            hitDirection = hitDirection.normalized;

            pc.invulnerableTimer = 1f;
            pc.TakeDamage(1, hitDirection);
            pc.invulnerableTimer = 0.5f;
        }
    }
}
