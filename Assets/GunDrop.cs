using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunDrop : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<Player>().getGun();
            Destroy(gameObject);
        }
    }
}
