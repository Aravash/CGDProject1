using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class enemyBehaviour : MonoBehaviour
{
    [SerializeField] private bool showDebugLine = true;
    
    [SerializeField]
    private Transform player;
    
    [SerializeField] 
    private float shoot_timer_length = 3f;
    private float shoot_timer;
    
    [SerializeField] 
    private float help_timer_length = 2f;
    private float help_timer;
    
    [SerializeField] 
    private float movespeed = 1f;
    private bool had_LOS = false;
    private Vector2 last_player_pos;
    private Vector2 patrol_pos;
    bool alerted = false;
    
    const int ASSIST_SEARCH_RESOLUTION = 10;

    enum EnemyStates
    {
        ES_ASLEEP,
        ES_IDLE,
        ES_PATROL,
        ES_ATTACK, // Has LOS
        ES_FOLLOW, // Had LOS
        ES_HELP // Nearby Enemy has LOS to player
    }

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        switch(getState())
        {
            case EnemyStates.ES_ASLEEP:
                return;
            case EnemyStates.ES_ATTACK:
                alertNearby();
                chase();
                break;
            case EnemyStates.ES_FOLLOW:
                alertNearby();
                hunt();
                break;
            case EnemyStates.ES_PATROL:
                patrol();
                break;
            case EnemyStates.ES_HELP:
                helpAlly();
                break;
        }
    }

    /**************
     * Behaviours *
     **************/
    void chase()
    {
        // Save data
        had_LOS = true;
        last_player_pos = player.position;

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

        // Gone to last seen pos, and cannot find
        if((Vector2)transform.position == last_player_pos)
        {
            // No clue where the player has gone
            had_LOS = false;
            // Will help other nearby enemies
            alerted = true;
        }
    }
    void patrol()
    {
        transform.position = Vector2.MoveTowards(transform.position, patrol_pos,
            movespeed * Time.deltaTime);
    }

    void helpAlly()
    {
        // This is more complex, so we don't want to do it every frame
        if (help_timer <= 0)
        {
            help_timer -= Time.deltaTime;
            return;
        }

        help_timer = help_timer_length;

        // find all nearby allies that are in combat
        enemyBehaviour[] allies = FindObjectsOfType<enemyBehaviour>();
        List<enemyBehaviour> alliesInCombat = new List<enemyBehaviour>();
        foreach (enemyBehaviour ally in allies)
        {
            if (checkLOS(ally.transform.position) && ally.isInCombat())
            {
                alliesInCombat.Add(ally);
            }
        }
        if(alliesInCombat.Count == 0)
        {
            // Shouldn't happen, but just in case
            return;
        }

        // find nearest ally
        enemyBehaviour nearestAlly = null;
        float dist = Mathf.Infinity;
        foreach (enemyBehaviour ally in alliesInCombat)
        {
            // compare the distance of this ally to the current closest found
            float new_dist = (ally.transform.position - transform.position).magnitude;
            if (new_dist < dist)
            {
                nearestAlly = ally;
                dist = new_dist;
            }
        }

        // Head towards the closest point we can to make LOS with player
        for(int i = 0; i < ASSIST_SEARCH_RESOLUTION; ++i)
        {
            // Scan up the friend's player-LOS to find the nearest point without an obstruction
            Vector3 check_pos = Vector3.Lerp(nearestAlly.getLastSeenPos(), nearestAlly.transform.position, (float)i / ASSIST_SEARCH_RESOLUTION);
            if(checkLOS(check_pos, true))
            {
                // Found an unobstructed route to our ally's player-LOS

                last_player_pos = check_pos;
                had_LOS = true;
                break;
            }
        }
    }

    /*************
     * Utilities *
     *************/
    EnemyStates getState()
    {
        // AI is disabled during pre-game
        if (asleep)
            return EnemyStates.ES_ASLEEP;

        // I can see the player!
        if (checkLOS(player.position))
            return EnemyStates.ES_ATTACK;

        if (had_LOS)
            return EnemyStates.ES_FOLLOW;

        if (alerted)
            return EnemyStates.ES_HELP;

        return EnemyStates.ES_PATROL;
    }

    bool checkLOS(Vector3 endPos, bool debug = false)
    {
        // Only cast on the default layer (excludes player, bullets, and enemies)
        int layer_mask = 1;
        // if the linecast didn't hit anything, then there was no obstruction
        //return !Physics2D.Linecast(transform.position, endPos, layer_mask);
        RaycastHit2D hit;
        hit = Physics2D.Linecast(transform.position, endPos, layer_mask);
        if(debug)
            Debug.DrawLine(transform.position, endPos, Color.white, 2.5f);
        return hit.collider == null;
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

    void alertNearby()
    {
        enemyBehaviour[] allies = FindObjectsOfType<enemyBehaviour>();
        foreach (enemyBehaviour ally in allies)
        {
            if (checkLOS(ally.transform.position) && !ally.isInCombat())
            {
                ally.alert();
            }
        }
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

    public void setPatrolPos(Vector2 change)
    {
        patrol_pos = change;
    }

    // For communicating with other enemies
    public bool isInCombat()
    {
        return had_LOS;
    }
    public Vector2 getLastSeenPos()
    {
        return last_player_pos;
    }
    public void alert()
    {
        if(getState() == EnemyStates.ES_IDLE || getState() == EnemyStates.ES_PATROL)
        {
            alerted = true;
        }
    }
}