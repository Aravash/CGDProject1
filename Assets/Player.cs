using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private int max_ammo = 2;
    [SerializeField] private int current_ammo;
    private bool has_gun = true;
    private SpriteRenderer gun_sprite;

    const float MV_MAX_SPEED = 5f;
    const float MV_ACCEL = 0.9f;
    const float MV_FRICTION = 0.3f;
    const float RADIUS = 0.8f;
    Vector3 spawn_pos;
    Quaternion spawn_rot;

    // Use this for initialization
    void Start()
    {
        spawn_pos = gameObject.transform.position;
        spawn_rot = gameObject.transform.rotation;

        // Vector3 cam_pos = Camera.main.transform.position;
        // cam_pos.x = gameObject.transform.position.x;
        // cam_pos.y = gameObject.transform.position.y;
        // Camera.main.transform.position = cam_pos;
        gun_sprite = transform.GetChild(0).GetComponent<SpriteRenderer>();
        current_ammo = max_ammo;
    }

    // Update is called once per frame
    void Update()
    {
        if (LevelManager._i.getState() == LevelManager.LevelState.LS_WIN)
        {
            if (Input.anyKeyDown)
            {
                LevelManager._i.nextLevel();
            }
            return;
        }
        if (LevelManager._i.getState() == LevelManager.LevelState.LS_WAIT && Input.anyKeyDown)
        {
            LevelManager._i.startLevel();
        }
        if(Input.GetKeyDown("space"))
        {
            if (current_ammo > 0)
            {
                fireProjectile();
            }
            else if (has_gun)
            {
                throwGun();
            }
        }

        TimeManager.Instance.ChangeTime();
    }

    private void FixedUpdate()
    {
        // player can only move in the play state
        if (LevelManager._i.getState() == LevelManager.LevelState.LS_PLAY)
        {
            move();
        }
        else
        {
            gameObject.GetComponent<Rigidbody2D>().velocity *= 0;
        }

        // cam follow
        // Vector3 cam_pos = Camera.main.transform.position;
        // cam_pos.x = Mathf.SmoothStep(cam_pos.x, gameObject.transform.position.x, Time.fixedDeltaTime * 10);
        // cam_pos.y = Mathf.SmoothStep(cam_pos.y, gameObject.transform.position.y, Time.fixedDeltaTime * 10);
        // Camera.main.transform.position = cam_pos;
    }

    private void move()
    {
        applyFriction();

        // Fetch user directional input
        Vector2 wish_dir = new Vector2(0, 0);
        if (Input.GetKey("d"))
            wish_dir.x++;
        if (Input.GetKey("a"))
            wish_dir.x--;
        if (Input.GetKey("w"))
            wish_dir.y++;
        if (Input.GetKey("s"))
            wish_dir.y--;
        wish_dir.Normalize();

        // Convert input to movement
        Vector2 acceleration = wish_dir;
        acceleration.x *= MV_ACCEL;
        gameObject.GetComponent<Rigidbody2D>().velocity += acceleration;

        // if moving
        if (gameObject.GetComponent<Rigidbody2D>().velocity.magnitude > 0)
        {
            // Rotate the player to match their velocity
            Vector2 v = gameObject.GetComponent<Rigidbody2D>().velocity;
            float theta = Mathf.Atan2(v.y, v.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(theta + 90, Vector3.forward);
        }

        // Dilate time
        TimeManager.i.ChangeTime();
    }

    private void applyFriction()
    {
        Vector2 vel = gameObject.GetComponent<Rigidbody2D>().velocity;
        float speed = vel.magnitude;
        if (speed < 0.01)
        {
            vel.x = 0;
            vel.y = 0;
            gameObject.GetComponent<Rigidbody2D>().velocity = vel;
            return;
        }

        float drop = speed * MV_FRICTION;

        float newspeed = speed - drop;
        if (newspeed < 0)
            newspeed = 0;
        newspeed /= speed;

        vel.x *= newspeed;
        vel.y *= newspeed;
        gameObject.GetComponent<Rigidbody2D>().velocity = vel;
    }

    private void fireProjectile()
    {
        // create bullet
        GameObject projectile = Instantiate(Resources.Load("Prefabs/Bullet") as GameObject);
        projectile.GetComponent<Bullet>().init(gameObject, vecFromMyRot());

        changeAmmo(-1);
    }

    private void throwGun()
    {
        // create gun bullet
        GameObject projectile = Instantiate(Resources.Load("Prefabs/GunBullet") as GameObject);
        projectile.GetComponent<GunProjectile>().init(gameObject, vecFromMyRot());
        has_gun = false;
        gun_sprite.enabled = false;
    }
    
    // Create a vector from the player's rotation
    private Vector2 vecFromMyRot()
    {
        float theta = gameObject.transform.rotation.eulerAngles.z + 90;
        theta *= Mathf.Deg2Rad;
        return new Vector2(Mathf.Cos(theta), Mathf.Sin(theta));
    }

    public void kill()
    {
        // Prevent the level from resetting if the player has already won
        if (LevelManager._i.getState() == LevelManager.LevelState.LS_WIN)
        {
            return;
        }

        // Move player to start
        gameObject.transform.position = spawn_pos;
        gameObject.transform.rotation = spawn_rot;
        changeAmmo(2);
        getGun();

        // Reset all enemies
        LevelManager._i.countEnemies();
        EnemySpawn[] spawns = FindObjectsOfType<EnemySpawn>();
        foreach(EnemySpawn spawn in spawns)
        {
            spawn.activate();
        }

        // Clear all bullets
        Bullet[] bullets = FindObjectsOfType<Bullet>();
        foreach(Bullet bullet in bullets)
        {
            Destroy(bullet.gameObject);
        }
        
        // Clear all ammoboxes
        ammoBox[] ammoboxes = FindObjectsOfType<ammoBox>();
        foreach(ammoBox ammobox in ammoboxes)
        {
            Destroy(ammobox.gameObject);
        }
        
        // Clear all guns on floor
        GunDrop[] guns = FindObjectsOfType<GunDrop>();
        foreach(GunDrop gun in guns)
        {
            Destroy(gun.gameObject);
        }
    }

    public void changeAmmo(int change)
    {
        getGun();
        current_ammo += change;
        if (current_ammo > max_ammo) 
            current_ammo = max_ammo;
    }

    public void getGun()
    {
        if (has_gun) return;
        has_gun = true;
        gun_sprite.enabled = true;
    }

    public static float getTopSpeed()
    {
        float top = MV_ACCEL / MV_FRICTION;
        return top < MV_MAX_SPEED ? top : MV_MAX_SPEED;
    }
        private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.name == "AmmoBox(Clone)")
        {
            AudioSource source = GetComponent<AudioSource>();
            source.Play();
	    }
    }
    
    }
