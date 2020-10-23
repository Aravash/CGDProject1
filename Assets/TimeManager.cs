using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public static TimeManager Instance { get; private set; }

    [SerializeField]
    private float currentPlayerSpeed;
    [SerializeField]
    private float currentGameSpeed;

    private float minGameSpeed = 0.1f;
    private float maxGameSpeed = 1f;


    private GameObject player;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        player = FindObjectOfType<Player>().gameObject;
        //Time.timeScale = gameSpeed;
     
    }

    void Update()
    {
        currentPlayerSpeed = player.GetComponent<Rigidbody2D>().velocity.magnitude;
        currentGameSpeed = Time.timeScale;
        ChangeTime();
    }

    public void ChangeTime()
    {
        float timeSpeed = (currentPlayerSpeed >= 0.3f) ? maxGameSpeed: minGameSpeed;
        Time.timeScale = Mathf.Lerp(Time.timeScale, timeSpeed, currentPlayerSpeed);
        Time.fixedDeltaTime = Time.timeScale * 0.02f;
    }
}
