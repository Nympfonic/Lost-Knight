using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPotionItem : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            Physics2D.IgnoreCollision(collision.collider, collision.otherCollider); // Ignore collisions with the player

            PlayerController.numOfHealthPotions++; // Increase player's health potion count by 1

            Destroy(gameObject); // Destroy the object once no longer needed
        }
        else if (collision.collider.CompareTag("Enemy")) 
        {
            Physics2D.IgnoreCollision(collision.collider, collision.otherCollider); // Ignore collisions with enemies
        }
    }
}
