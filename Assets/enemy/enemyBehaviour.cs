using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class enemyBehaviour : MonoBehaviour
{ [SerializeField] private bool showDebugLine = true;
    
   [SerializeField]
   private Transform player;

   private Transform pointA;
   private Transform pointB;
   [SerializeField] 
   private float shoot_timer_length = 3f;
   private float shoot_timer;

   [SerializeField] 
   private float movespeed = 1f;
   private bool had_LOS = false;
   private Vector2 last_player_pos;

   // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        if(asleep)
        {
            // AI is disabled during pre-game
            return;
        }

        if (hasPlayerLOS())
        {
            had_LOS = true;
            last_player_pos = player.position;
            chase();
        }
        else if (had_LOS)
        {
            hunt();
        }
        else patrol();
    }
    bool hasPlayerLOS()
    {
        //set layermask to enemy layer
        int layerMask = 1 << 8;
        //set mask to check for everything but enemies
        layerMask = ~layerMask;

        RaycastHit2D hit;
        if (hit = Physics2D.Linecast(transform.position, player.position, layerMask))
        {
            if(showDebugLine)Debug.DrawLine(transform.position, hit.point, Color.white, 2.5f);
            
            if (hit.collider.CompareTag("Player"))
            {
                return true;
            }
        }
        
        return false;
    }
    void chase()
    {
        //move toward the player, but not too close!
        if (Vector2.Distance(transform.position, player.position) >= 2)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.position,
                movespeed * Time.deltaTime);
        }
        //rotate to look at the player
        transform.right = player.position - transform.position;
        
        //shoot when possible
        if (shoot_timer <= 0f)
        {
            shoot();
            shoot_timer = shoot_timer_length;
        }
        else shoot_timer -= Time.deltaTime;
    }
    void hunt()
    {
        if (Vector2.Distance(transform.position, last_player_pos) >= 0)
        {
            transform.position = Vector2.MoveTowards(transform.position, last_player_pos,
                movespeed * Time.deltaTime);
        }
    }
    void patrol()
    {
        //coming soon(TM)
    }
    void shoot()
    {
        Debug.Log("i want to shoot");
        /*float theta = gameObject.transform.rotation.eulerAngles.z + 90;
        theta *= Mathf.Deg2Rad;
        Vector2 dir = new Vector2(Mathf.Cos(theta), Mathf.Sin(theta));*/
        GameObject projectile = Instantiate(Resources.Load("Prefabs/Bullet") as GameObject);
        projectile.GetComponent<Bullet>().init(gameObject, transform.right);
    }

    // external interface funcs
    public void kill()
    {
        Destroy(gameObject);
        EnemyTracker._i.enemyDeath();
    }
    private bool asleep = true;
    public void wake()
    {
        asleep = false;
    }
}