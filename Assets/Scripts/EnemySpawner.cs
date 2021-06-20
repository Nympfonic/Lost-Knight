using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemy;
    public Vector3 enemyPosition;
    private bool isTriggered;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !isTriggered)
        {
            isTriggered = true;
            SpawnEnemy(enemy, enemyPosition);

            gameObject.SetActive(false);
        }
    }

    private void SpawnEnemy(GameObject enemy, Vector3 enemyPos)
    {
        Instantiate(enemy, enemyPos, Quaternion.identity);
    }
}
