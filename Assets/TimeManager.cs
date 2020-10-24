using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class TimeManager
{
    public static TimeManager Instance = new TimeManager();

    [SerializeField]
    private float currentPlayerSpeed;
    [SerializeField]
    private float currentGameSpeed = 0.1f;

    private float minGameSpeed = 0.1f;
    private float maxGameSpeed = 1f;
    //private float fixedDeltaTime;

    private GameObject player;

    public static TimeManager i
    {
        get { return Instance; }
    }

    private TimeManager()
    {
        player = GameObject.FindObjectOfType<Player>().gameObject;
    }

    public void ChangeTime()
    {
        currentPlayerSpeed = player.GetComponent<Rigidbody2D>().velocity.magnitude;

        currentPlayerSpeed = 
        float timeSpeed = (currentPlayerSpeed <= 0.5f) ? minGameSpeed: maxGameSpeed;

        currentGameSpeed = Mathf.Lerp(currentGameSpeed, timeSpeed, currentPlayerSpeed);
        //Time.fixedDeltaTime = Time.timeScale * 0.02f; //to fix laggy slow motion .02 is apparently standard
        //Time.fixedDeltaTime = this.fixedDeltaTime * Time.timeScale;

       
    }

    public Vector2 getTimeMultiplier()
    {
        currentGameSpeed = currentPlayerSpeed;

        return currentGameSpeed;

    }
}
