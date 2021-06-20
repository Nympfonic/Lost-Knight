using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserBullet : MonoBehaviour
{
    public float moveSpeed = 7f;

    private Rigidbody2D rb;

    public GameObject target;
    private Vector2 moveDirection;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        target = GameObject.FindGameObjectWithTag("Player");
        moveDirection = (target.transform.position - transform.position).normalized * moveSpeed;
        rb.velocity = new Vector2(moveDirection.x, moveDirection.y);
        Destroy(gameObject, 5f);

        // Set Rotation
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy") ||
            collision.gameObject.CompareTag("Platform") ||
            collision.gameObject.CompareTag("CameraBounds") ||
            collision.gameObject.CompareTag("Item") || 
            collision.gameObject.CompareTag("Projectile"))
        {
            // Do nothing.
        }
        else if (collision.gameObject.CompareTag("Player"))
        {
            //Debug.Log("Hit Player!");
            //Destroy(gameObject);

            // Deal damage to player.
            PlayerController pc = collision.gameObject.GetComponent<PlayerController>();
            Vector3 hitDirection = collision.transform.position - transform.position;
            hitDirection = hitDirection.normalized;

            pc.TakeDamage(1, hitDirection);

            // Delete Bullet.
            Destroy(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
