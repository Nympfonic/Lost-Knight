using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadPlayers : MonoBehaviour
{
    public GameObject ThingToEnable;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            LoadOtherPlayers();
            this.gameObject.SetActive(false);
        }
    }

    void LoadOtherPlayers()
    {
        ThingToEnable.SetActive(true);
    }
}
