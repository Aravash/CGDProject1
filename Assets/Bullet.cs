using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    const float SPEED = 10f;
    [SerializeField] TrailRenderer trail;
    const float SPAWN_OFFSET = 0.5f;

    public void init(GameObject origin, Vector2 dir)
    {
        dir.Normalize();

        // place the bullet in front of the shooter
        Vector3 pos = origin.transform.position;
        pos.x += dir.x * SPAWN_OFFSET;
        pos.y += dir.y * SPAWN_OFFSET;
        gameObject.transform.position = pos;

        // Start the bullet's movement
        GetComponent<Rigidbody2D>().velocity = dir * SPEED;
    }
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //check if enemy
        if (collision.collider.GetComponent<enemyBehaviour>() != null)
        {
            collision.collider.GetComponent<enemyBehaviour>().kill();
        }
        //check if player
        if (collision.collider.GetComponent<Player>() != null)
        {
            collision.collider.GetComponent<Player>().kill();
        }

        // detach trail
        trail.transform.parent = null;
        trail.autodestruct = true;
        // destroy bullet
        Destroy(gameObject);
    }
}
