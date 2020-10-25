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

        timeSpeed = (currentPlayerSpeed > minGameSpeed) ? maxGameSpeed : minGameSpeed;

        currentGameSpeed = Mathf.Lerp(currentGameSpeed, timeSpeed, currentGameSpeed);
    }

    public float getTimeMultiplier()
    {
        Debug.Log("HAVE DETECTED A CHANGE!");
        return currentGameSpeed;

    }
}
