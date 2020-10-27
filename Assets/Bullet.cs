using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    const float SPEED = 10f;
    [SerializeField] TrailRenderer trail;
    const float SPAWN_OFFSET = 0.5f;
    Vector2 true_velocity;

    public void init(GameObject origin, Vector2 dir)
    {
        dir.Normalize();

        // place the bullet in front of the shooter
        Vector3 pos = origin.transform.position;
        pos.x += dir.x * SPAWN_OFFSET;
        pos.y += dir.y * SPAWN_OFFSET;
        gameObject.transform.position = pos;

        true_velocity = dir * SPEED;
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
        else
        {
            // Only spawn smoke if the bullet didn't hit the player
            // This check prevents the smoke from lingering into the next round
            GameObject explosion = Instantiate(Resources.Load("Prefabs/BulletSmoke") as GameObject);
            explosion.GetComponent<Transform>().position = transform.position;
        }

        // detach trail
        trail.transform.parent = null;
        trail.autodestruct = true;

        // destroy bullet
        //Destroy(gameObject);
        StartCoroutine("DestroyBullet");
    }

    public IEnumerator DestroyBullet()
    {
        gameObject.GetComponent<BoxCollider2D>().enabled = false;
        gameObject.GetComponent<SpriteRenderer>().enabled = false;
        yield return new WaitForSecondsRealtime(.6f);
        Destroy(gameObject);
    }

    private void FixedUpdate()
    {
        Vector2 vel = true_velocity;
        vel.Normalize();
        vel *= SPEED;
        vel *= TimeManager.Instance.getTimeMultiplier();
        GetComponent<Rigidbody2D>().velocity = vel;
    }
}
