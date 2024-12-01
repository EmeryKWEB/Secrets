using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyVision : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision == collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Enemy sees " + collision);
            GameController.instance.LoseGame("You were seen!");
        }

    }

}
