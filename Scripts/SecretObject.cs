using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecretObject : MonoBehaviour
{
    public string secretMessage = "Secret Message";
    public AudioClip secretPickupSound;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            //store secret message in inventory
            GameController.instance.PlaySound(secretPickupSound);
            collision.GetComponent<Inventory>().AddSecret(secretMessage, gameObject);
            Destroy(gameObject);
        }
    }

}
