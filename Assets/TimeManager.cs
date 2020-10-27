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
    private float timeSpeed;

    public bool playerAction = false;
    //private float fixedDeltaTime;


    Vector2 dir;

    public static TimeManager i
    {
        get { return Instance; }
    }

    private TimeManager()
    {
        //currentGameSpeed = minGameSpeed;
    }
    public void ChangeTime()
    {
        currentPlayerSpeed = Player.FindObjectOfType<Player>().GetComponent<Rigidbody2D>().velocity.magnitude;

        float interp = currentPlayerSpeed / Player.getTopSpeed();
        interp *= interp;
        //timeSpeed = (currentPlayerSpeed > minGameSpeed) ? maxGameSpeed : minGameSpeed;
        currentGameSpeed = Mathf.Lerp(minGameSpeed, maxGameSpeed, interp);


        if (playerAction && currentPlayerSpeed < minGameSpeed)
        {
            currentGameSpeed = maxGameSpeed;
            //Debug.Log("SHOULD WORK!");
        }
        
       
    }
    public float getTimeMultiplier()
    {
        if(LevelManager._i.getState() == LevelManager.LevelState.LS_WIN)
        {
            return 1;
        }
        return currentGameSpeed;
    }
}
